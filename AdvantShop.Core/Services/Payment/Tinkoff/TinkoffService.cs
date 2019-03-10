//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Taxes;

namespace AdvantShop.Core.Services.Payment.Tinkoff
{
    // Документация: https://oplata.tinkoff.ru/landing/develop/documentation/termins_and_operations

    public class TinkoffService
    {
        private const string BaseUrl = "https://securepay.tinkoff.ru/v2/";

        private readonly string _terminalKey;
        private readonly string _secretKey;
        private readonly bool _sendReceiptData;

        public TinkoffService(string terminalKey, string secretKey, bool sendReceiptData)
        {
            _terminalKey = terminalKey;
            _secretKey = secretKey;
            _sendReceiptData = sendReceiptData;
        }

        /// <summary>
        /// Запрос создание заказа
        /// </summary>
        public TinkoffInitResponse Init(Order order, string description, string taxation, Func<OrderCurrency,float> getCurrencyRate)
        {
            Receipt receipt = _sendReceiptData
                ? new Receipt()
                {
                    Email = order.OrderCustomer.Email,
                    Phone = order.OrderCustomer.Phone,
                    Taxation = taxation,
                    Items = 
                            OrderService.RecalculateItemsPriceIncludingAllDiscounts(order.OrderItems, order.ShippingCost, order.Sum)
                                .Select(item => new Item()
                                {
                                    Name = item.Name.Length > 64 ? item.Name.Substring(0, 64) : item.Name,
                                    ShopCode = item.ArtNo,
                                    //Ean13 = item.
                                    Amount = (int)(Math.Round(item.Price * item.Amount, 2) * 100),
                                    Price = (int)(Math.Round(item.Price, 2) * 100),
                                    Quantity = item.Amount,
                                    Tax = GetTaxType(item.TaxType)
                                }).ToList()
                }
                : null;

            if (order.ShippingCost > 0 && receipt != null)
            {
                receipt.Items.Add(new Item()
                {
                    Name = "Доставка",
                    ShopCode = "Доставка",
                    Amount = (int)(Math.Round(order.ShippingCost, 2) * 100),
                    Price = (int)(Math.Round(order.ShippingCost, 2) * 100),
                    Quantity = 1,
                    Tax = GetTaxType(order.ShippingTaxType)
                });
            }

            var sum = (Math.Round(order.Sum * getCurrencyRate(order.OrderCurrency), 2) * 100);
            var data = new TinkoffInitData
            {
                TerminalKey=_terminalKey,
                Amount=Convert.ToInt32(sum),    // Сумма платежа в копейках
                OrderId=order.OrderID.ToString(),
                Description=description,
                Receipt = receipt
            };

            var response = MakeRequest<TinkoffInitResponse, TinkoffInitData>("Init", data);

            if (response == null)
                return null;

            if (!response.Success)
            {
                Debug.Log.Info(string.Format("TinkoffService Init. code: {0} error: {1}, details: {2}, response: {3}",
                                                response.ErrorCode, response.Message, response.Details, JsonConvert.SerializeObject(response)));
            }

            return response;
        }

        #region Private methods

        private string GetTaxType(TaxType? taxType)
        {
            if (taxType == null || taxType.Value == TaxType.Without)
                return "none";

            if (taxType.Value == TaxType.Zero)
                return "vat0";

            if (taxType.Value == TaxType.Ten)
                return "vat10";

            if (taxType.Value == TaxType.Eighteen)
                return "vat18";

            return "none";
        }


        private T MakeRequest<T, TData>(string url, TData data = null) 
            where T : class
            where TData : TinkoffBaseData
        {
            try
            {
                var request = WebRequest.Create(BaseUrl + url) as HttpWebRequest;
                request.Timeout = 5000;
                request.Method = "POST";
                request.ContentType = "application/json";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (data != null)
                {
                    if (string.IsNullOrEmpty(data.TerminalKey))
                        data.TerminalKey = _terminalKey;

                    data.Token = GenerateToken(AsDictionary(data));

                    string dataPost = JsonConvert.SerializeObject(data);

                    byte[] bytes = Encoding.UTF8.GetBytes(dataPost);
                    request.ContentLength = bytes.Length;

                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                    }
                }

                var responseContent = "";
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                            using (var reader = new StreamReader(stream))
                            {
                                responseContent = reader.ReadToEnd();
                            }
                    }
                }

                var dataAnswer = JsonConvert.DeserializeObject<T>(responseContent);

                return dataAnswer;
            }
            catch (WebException ex)
            {
                using (var eResponse = ex.Response)
                {
                    if (eResponse != null)
                    {
                        using (var eStream = eResponse.GetResponseStream())
                            if (eStream != null)
                                using (var reader = new StreamReader(eStream))
                                {
                                    var error = reader.ReadToEnd();
                                    Debug.Log.Error(error, ex);
                                }
                            else
                                Debug.Log.Error(ex);
                    }
                    else
                        Debug.Log.Error(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return null;
        }
        #endregion

        #region Help

        public string GenerateToken(Dictionary<string, string> data)
        {
            if (data.ContainsKey("Token"))
                data.Remove("Token");

            if (data.ContainsKey("Password"))
                data.Remove("Password");
            data.Add("Password", _secretKey);

            string token = "";
            foreach (var key in data.Keys.OrderBy(x => x))
            {
                var value = data[key];

                if (string.IsNullOrEmpty(value))
                    continue;

                token += value;
            }
            token = token.Sha256();

            data.Remove("Password");

            return token;
        }

        public Dictionary<string, string> AsDictionary(object source, BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (var property in source.GetType().GetProperties(bindingAttr))
            {
                var val = property.GetValue(source, BindingFlags.GetProperty, null, null, CultureInfo.InvariantCulture);
                if (!property.PropertyType.IsGenericType && property.PropertyType != typeof(string) && val != null)
                    dictionary.Add(property.Name, JsonConvert.SerializeObject(val));
                else
                    dictionary.Add(property.Name, val != null ? val.ToString() : null);
            }
            return dictionary;
        }

        public TinkoffNotifyData ReadNotifyData(string postPayload)
        {
            return JsonConvert.DeserializeObject<TinkoffNotifyData>(postPayload);
        }

        #endregion
    }
}
