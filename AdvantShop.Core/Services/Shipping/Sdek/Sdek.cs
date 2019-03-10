//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;

namespace AdvantShop.Shipping.Sdek
{
    [ShippingKey("Sdek")]
    public class Sdek : BaseShippingWithCargo
    {
        private readonly string _authLogin;
        private readonly string _authPassword;
        private readonly string _tariff;
        private readonly string _cityFrom;
        private readonly int _deliveryNote;
        //тип наценки
        private readonly string _typeAdditionalPrice;
        private readonly float _additionalPrice;

        private readonly SdekApiService _sdekApiService;

        public float AdditionalPrice { get; set; }

        public Sdek(ShippingMethod method, PreOrder preOrder) : base(method, preOrder)
        {
            _authLogin = _method.Params.ElementOrDefault(SdekTemplate.AuthLogin);
            _authPassword = _method.Params.ElementOrDefault(SdekTemplate.AuthPassword);
            _tariff = _method.Params.ElementOrDefault(SdekTemplate.Tariff);
            _cityFrom = _method.Params.ElementOrDefault(SdekTemplate.CityFrom);
            _additionalPrice = _method.Params.ElementOrDefault(SdekTemplate.AdditionalPrice).TryParseFloat();
            _typeAdditionalPrice = _method.Params.ElementOrDefault(SdekTemplate.TypeAdditionPrice);
            _deliveryNote = _method.Params.ElementOrDefault(SdekTemplate.DeliveryNote).TryParseInt();

            _sdekApiService = new SdekApiService(_authLogin, _authPassword, _tariff, _cityFrom, _additionalPrice,
                                                 _defaultLength, _defaultWidth, _defaultHeight, _defaultWeight, _deliveryNote);
        }

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            if (string.IsNullOrEmpty(_preOrder.CityDest) || string.IsNullOrEmpty(_cityFrom))
                return new List<BaseShippingOption>();

            var sdekCityToId = SdekService.GetSdekCityId(_preOrder.CityDest, _preOrder.RegionDest);
            var sdekCityFromId = SdekService.GetSdekCityId(_cityFrom, string.Empty);
            var dateExecute = DateTime.Now.Date.ToString("yyyy-MM-dd");

            var sdekTariff = _sdekApiService.Tariffs.FirstOrDefault(item => string.Equals(item.TariffId.ToString(), _tariff));
            var goodsWeight = _preOrder.Items.Sum(item => item.Weight > 0 ? item.Weight * item.Amount : _defaultWeight * item.Amount);

            if (sdekTariff.WeightLimitation.HasValue && sdekTariff.WeightLimitation.Value < goodsWeight)
            {
                return new List<SdekOption>();
            }

            var points = PreparePoint(sdekCityToId, goodsWeight);

            if (sdekTariff == null || (sdekTariff.Mode.EndsWith("-С") && (points == null || points.Count == 0)))
            {
                return new List<SdekOption>();
            }

            var jsonData = JsonConvert.SerializeObject(
                new
                {
                    version = "1.0",
                    dateExecute = dateExecute,
                    authLogin = _authLogin,
                    secure = SdekService.GetMd5Hash(MD5.Create(), dateExecute + "&" + _authPassword),
                    senderCityId = sdekCityFromId.ToString(),
                    receiverCityId = sdekCityToId.ToString(),
                    tariffId = sdekTariff.TariffId,
                    goods = sdekTariff.MergeToOnePlace ? PrepareGoods(true) : PrepareGoods(false)
                });

            var responseResult = _sdekApiService.GetCalculatedPrice(jsonData);

            var sdekAnswer = JsonConvert.DeserializeObject<SdekResponse>(responseResult);
            if (sdekAnswer.Error == null)
            {
                var shippingOption = new SdekOption(_method)
                {
                    Rate = _typeAdditionalPrice == "Fixed" ? sdekAnswer.Result.PriceByCurrency + _additionalPrice : sdekAnswer.Result.PriceByCurrency + (sdekAnswer.Result.PriceByCurrency / 100) * _additionalPrice,
                    DeliveryTime = (sdekAnswer.Result.DeliveryPeriodMin > 0 ? sdekAnswer.Result.DeliveryPeriodMin.ToString() + "-" : string.Empty) 
                        + sdekAnswer.Result.DeliveryPeriodMax.ToString() + " дн.",
                    ShippingPoints = sdekTariff.Mode.EndsWith("-Д") ? null : points,
                    TariffId = sdekAnswer.Result.TariffId.ToString(),
                    DisplayCustomFields = !sdekTariff.Mode.EndsWith("-Д"),
                    HideAddressBlock = !sdekTariff.Mode.EndsWith("-Д")
                };
                return new List<SdekOption>() { shippingOption };
            }

            if (sdekAnswer.Error.Any(x => x.Code == 3) || SettingsMain.LogAllErrors)
            {
                var errors = sdekAnswer.Error.Aggregate("", (current, error) => current + error.Code + " " + error.Text);
                Debug.Log.Error(new Exception("Sdek: " + errors + ". jsonData: " + jsonData));
            }

            return new List<SdekOption>();
        }

        private List<SdekGoods> PrepareGoods(bool mergeToSingleGood)
        {
            var goods = new List<SdekGoods>();

            if (mergeToSingleGood)
            {
                var items = _preOrder.Items.Select(item => new Measure {XYZ = new[] {item.Height, item.Width, item.Length}, Amount = item.Amount}).ToList();
                var dimensions = MeasureHelper.GetDimensions(items);

                //сдек в см
                var length = dimensions[0]/10;
                var width = dimensions[1]/10;
                var height = dimensions[2]/10;

                goods.Add(new SdekGoods
                {
                    Length =
                        length > 0
                            ? length.ToString(CultureInfo.InvariantCulture)
                            : (_defaultLength/10).ToString(CultureInfo.InvariantCulture),
                    Width =
                        width > 0
                            ? width.ToString(CultureInfo.InvariantCulture)
                            : (_defaultWidth/10).ToString(CultureInfo.InvariantCulture),
                    Height =
                        height > 0
                            ? height.ToString(CultureInfo.InvariantCulture)
                            : (_defaultHeight/10).ToString(CultureInfo.InvariantCulture),
                    Weight =
                        _preOrder.Items.Sum(item => item.Weight > 0 ? item.Weight*item.Amount : _defaultWeight*item.Amount).ToInvariantString(),
                });
            }
            else
            {
                foreach (var item in _preOrder.Items)
                {
                    //сдек в см
                    var lengthT = item.Length/10.0f;
                    var widthT = item.Width/10.0f;
                    var heightT = item.Height/10.0f;

                    for (var index = 0; index < item.Amount; index++)
                    {
                        goods.Add(new SdekGoods
                        {
                            Weight =
                                item.Weight == 0
                                    ? _defaultWeight.ToString(CultureInfo.InvariantCulture)
                                    : item.Weight.ToString(CultureInfo.InvariantCulture),
                            Length =
                                lengthT > 0
                                    ? lengthT.ToString(CultureInfo.InvariantCulture)
                                    : (_defaultLength/10).ToString(CultureInfo.InvariantCulture),
                            Width =
                                widthT > 0
                                    ? widthT.ToString(CultureInfo.InvariantCulture)
                                    : (_defaultWidth/10).ToString(CultureInfo.InvariantCulture),
                            Height =
                                heightT > 0
                                    ? heightT.ToString(CultureInfo.InvariantCulture)
                                    : (_defaultHeight/10).ToString(CultureInfo.InvariantCulture)
                        });
                    }
                }
            }
            return goods;
        }

 private List<BaseShippingPoint> PreparePoint(int sdekCityToId, float goodsWeight)
        {
            return CacheManager.Get("Sdek_PreparePoint_" + sdekCityToId, 2, () =>
            {
                var listShippingPoints = new List<BaseShippingPoint>();

                foreach (var sdekPoint in _sdekApiService.GetListOfCityPoints(sdekCityToId, false)
                    .Where(item => 
                        (!item.WeightLimit.WeightMax.HasValue || item.WeightLimit.WeightMax >= goodsWeight) && 
                        (!item.WeightLimit.WeightMin.HasValue || item.WeightLimit.WeightMin <= goodsWeight))
                    .OrderBy(x => x.Address))
                {
                    listShippingPoints.Add(new BaseShippingPoint
                    {
                        Id = sdekPoint.Code.GetHashCode(),
                        Code = sdekPoint.Code,
                        Address = sdekPoint.Address,
                        Description = sdekPoint.Note + " " + sdekPoint.WorkTime
                    });
                }

                return listShippingPoints;
            });
        }


        #region Sdek Api Methods

        public SdekStatusAnswer SendNewOrders(Order order, int tariffId)
        {
            SdekParamsSendOrder parametrs = new SdekParamsSendOrder(_method);
            return _sdekApiService.SendNewOrders(order, tariffId, parametrs);
        }

        public void ReportOrderStatuses()
        {
            _sdekApiService.ReportOrderStatuses();
        }

        public SdekStatusAnswer ReportOrderStatuses(Order order)
        {
            return _sdekApiService.ReportOrderStatuses(order);
        }

        public SdekStatusAnswer ReportOrdersInfo(Order order)
        {
            return _sdekApiService.ReportOrdersInfo(order);
        }

        public void ReportOrdersInfo()
        {
            _sdekApiService.ReportOrdersInfo();
        }

        public string PrintFormOrder(Order order)
        {
            return _sdekApiService.PrintFormOrder(order);
        }
        
        public SdekStatusAnswer DeleteOrder(Order order)
        {
            return _sdekApiService.DeleteOrder(order);
        }

        public SdekStatusAnswer CallCustomer(Order order)
        {
            return _sdekApiService.CallCustomer(order);
        }
        
        public SdekStatusAnswer CallCourier(DateTime date, DateTime timeBegin, DateTime timeEnd, string cityName,
            string street, string house, string flat, string phone, string name, string weight)
        {
            return _sdekApiService.CallCourier(date, timeBegin, timeEnd, cityName, street, house, flat, phone, name, weight);
        }

        #endregion
    }
}