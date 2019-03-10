//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using AdvantShop.Configuration;

namespace AdvantShop.Shipping.Edost
{
    [ShippingKey("Edost")]
    public class Edost : BaseShippingWithCargoAndCache
    {
        private const string Url = "http://www.edost.ru/edost_calc_kln.php";
        private readonly string _shopId;
        private readonly string _password;
        private readonly float _rate;

        public Edost(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {
            _shopId = _method.Params.ElementOrDefault(EdostTemplate.ShopId);
            _password = _method.Params.ElementOrDefault(EdostTemplate.Password);
            _rate = _method.Params.ElementOrDefault(EdostTemplate.Rate).TryParseFloat();
        }

        private string GetParam(string destination)
        {
            var items = _preOrder.Items.Select(item => new Measure { XYZ = new[] { item.Height, item.Width, item.Length }, Amount = item.Amount }).ToList();
            var dimensions = MeasureHelper.GetDimensions(items);

            var length = dimensions[0];
            var width = dimensions[1];
            var height = dimensions[2];
            var totalPrice = _preOrder.Items.Sum(item => item.Price * item.Amount);

            var a = new StringBuilder();
            a.Append("to_city=" + destination.Replace("ё", "е"));
            a.Append("&zip=" + _preOrder.ZipDest);
            a.Append("&weight=" + _preOrder.Items.Sum(item => item.Weight * item.Amount).ToString("F3"));
            a.Append("&strah=" + (_rate != 0 ? totalPrice / _rate : totalPrice).ToString("F2"));
            a.Append("&id=" + _shopId);
            a.Append("&p=" + _password);
            a.Append("&ln=" + length);
            a.Append("&wd=" + width);
            a.Append("&hg=" + height);
            return a.ToString();
        }

        private IEnumerable<EdostTarif> FillTarif(XDocument doc)
        {
            var tarifs = new List<EdostTarif>();
            foreach (var el in doc.Root.Elements("tarif"))
            {
                var idEl = el.Element("id");
                var priceEl = el.Element("price");
                var priceCashEl = el.Element("pricecash");
                var priceTransferEl = el.Element("transfer");
                var nameEl = el.Element("name");
                var pickpointMapEl = el.Element("pickpointmap");
                var companyEl = el.Element("company");
                var dayEl = el.Element("day");

                if (idEl == null || priceEl == null || nameEl == null || companyEl == null)
                    continue;

                var item = new EdostTarif
                {
                    Id = idEl.Value.TryParseInt(),
                    Price = priceEl.Value.TryParseFloat() * _rate,
                    PriceCash = priceCashEl != null ? priceCashEl.Value.TryParseFloat() * _rate : 0,
                    PriceTransfer = priceTransferEl != null ? priceTransferEl.Value.TryParseFloat() * _rate : 0,
                    Name = nameEl != null ? nameEl.Value : string.Empty,
                    Company = companyEl != null ? companyEl.Value : string.Empty,
                    PickpointMap = pickpointMapEl != null ? pickpointMapEl.Value : string.Empty,
                    Day = dayEl != null ? dayEl.Value : string.Empty
                };

                if (pickpointMapEl != null)
                    tarifs.Insert(0, item);
                else
                    tarifs.Add(item);
            }
            return tarifs;
        }

        private List<EdostOffice> FillOffice(XDocument doc)
        {
            var offices = new List<EdostOffice>();
            foreach (var el in doc.Root.Elements("office"))
            {
                foreach (var tarif in el.Element("to_tarif").Value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var item = new EdostOffice
                    {
                        Id = el.Element("id").Value.TryParseInt(),
                        Name = el.Element("name").Value,
                        Address = el.Element("address").Value,
                        Tel = el.Element("tel").Value,
                        Scheldule = el.Element("schedule").Value,
                        TarifId = tarif.TryParseInt()
                    };
                    offices.Add(item);
                }
            }
            return offices;
        }


        //1 - успех
        //2 - доступ к расчету заблокирован
        //3 - неверные данные магазина (пароль или идентификатор)
        //4 - неверные входные параметры
        //5 - неверный город или страна
        //6 - внутренняя ошибка сервера расчетов
        //7 - не заданы компании доставки в настройках магазина
        //8 - сервер расчета не отвечает
        //9 - превышен лимит расчетов за день
        //11 - не указан вес
        //12 - не заданы данные магазина (пароль или идентификатор)
        private static void GetErrorEdost(string str, string postData)
        {
            var error = "";
            var log = true;
            switch (str)
            {
                case "2":
                    error = "доступ к расчету заблокирован";
                    break;

                case "3":
                    error = "неверные данные магазина (пароль или идентификатор)";
                    break;

                case "4":
                    error = "неверные входные параметры. postData: " + postData;
                    break;

                case "5":
                    log = false;
                    error = "неверный город или страна. postData: " + postData;
                    break;

                case "6":
                    error = "внутренняя ошибка сервера расчетов";
                    break;

                case "7":
                    error = "не заданы компании доставки в настройках магазина";
                    break;

                case "8":
                    error = "сервер расчета не отвечает";
                    break;

                case "9":
                    error = "превышен лимит расчетов за день";
                    break;

                case "11":
                    log = false;
                    error = "не указан вес";
                    break;

                case "12":
                    error = "не заданы данные магазина (пароль или идентификатор)";
                    break;
            }

            if (log || SettingsMain.LogAllErrors)
                Debug.Log.Error("Edost: " + error);
        }

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            var result = _calc(_preOrder.CityDest);
            if (!result.Any() && !string.IsNullOrWhiteSpace(_preOrder.RegionDest))
            {
                result = _calc(_preOrder.CityDest + " (" + _preOrder.RegionDest + ")");
            }
            if (!result.Any() && !string.IsNullOrWhiteSpace(_preOrder.RegionDest))
            {
                result = _calc(_preOrder.RegionDest);
            }
            if (!result.Any() && !string.IsNullOrWhiteSpace(_preOrder.CountryDest))
            {
                result = _calc(_preOrder.CountryDest);
            }
            return result;
        }

        private IEnumerable<BaseShippingOption> _calc(string destination)
        {
            string postData = GetParam(destination);

            ServicePointManager.Expect100Continue = false;
            var request = WebRequest.Create(Url);
            request.Method = "POST";

            byte[] byteArray = Encoding.GetEncoding("windows-1251").GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            request.Timeout = 3000;

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }
            using (var response = request.GetResponse())
            {
                using (var dataStream = response.GetResponseStream())
                {
                    if (dataStream == null) return new List<BaseShippingOption>();
                    using (var reader = new StreamReader(dataStream))
                    {
                        string responseFromServer = reader.ReadToEnd();
                        return ParseAnswer(responseFromServer, postData);
                    }
                }
            }
        }

        private IEnumerable<BaseShippingOption> ParseAnswer(string responseFromServer, string postData)
        {
            if (responseFromServer.IsNullOrEmpty())
                return new List<BaseShippingOption>();

            var shippingOptions = new List<EdostOption>();

            var doc = XDocument.Parse(responseFromServer);

            if (doc.Root == null)
                return shippingOptions;

            var status = doc.Root.Element("stat");
            if (status != null && status.Value != "1")
            {
                GetErrorEdost(status.Value, postData);
                return shippingOptions;
            }

            var tarifs = FillTarif(doc);
            var offices = FillOffice(doc);

            foreach (var tarif in tarifs)
            {
                var shippingOption = CreateOption(tarif, offices);
                shippingOptions.Add(shippingOption);
            }

            return shippingOptions;
        }


        private EdostOption CreateOption(EdostTarif tarif, IEnumerable<EdostOffice> offices)
        {
            var office = offices.Any(x => x.TarifId == tarif.Id);
            var point = tarif.PickpointMap.IsNotEmpty();
            if (tarif.PriceCash != 0 || tarif.PriceTransfer != 0)
            {
                if (office)
                {
                    return new EdostCashOnDeliveryBoxberryOption(_method, tarif, offices) { DeliveryId = tarif.Id, Name = tarif.Company + (string.IsNullOrWhiteSpace(tarif.Name) ? string.Empty : " (" + tarif.Name + ")") };
                }
                else if (point)
                {
                    return new EdostCashOnDeliveryPickPointOption(_method, tarif) { DeliveryId = tarif.Id, Name = tarif.Company + (string.IsNullOrWhiteSpace(tarif.Name) ? string.Empty : " (" + tarif.Name + ")") };
                }
                else
                {
                    return new EdostCashOnDeliveryOption(_method, tarif) { DeliveryId = tarif.Id, Name = tarif.Company + (string.IsNullOrWhiteSpace(tarif.Name) ? string.Empty : " (" + tarif.Name + ")") };
                }
            }
            if (office)
            {
                return new EdostBoxberryOption(_method, tarif, offices) { DeliveryId = tarif.Id, Name = tarif.Company + (string.IsNullOrWhiteSpace(tarif.Name) ? string.Empty : " (" + tarif.Name + ")") };
            }
            if (point)
            {
                return new EdostPickPointOption(_method, tarif) { DeliveryId = tarif.Id, Name = tarif.Company + (string.IsNullOrWhiteSpace(tarif.Name) ? string.Empty : " (" + tarif.Name + ")") };
            }
            return new EdostOption(_method, tarif) { DeliveryId = tarif.Id, Name = tarif.Company + (string.IsNullOrWhiteSpace(tarif.Name) ? string.Empty : " (" + tarif.Name + ")") };
        }

        protected override int GetHashForCache()
        {
            string postData = GetParam(_preOrder.CityDest + _preOrder.RegionDest + _preOrder.CountryDest);
            var hash = _method.ShippingMethodId ^ postData.GetHashCode();
            return hash;
        }
    }
}