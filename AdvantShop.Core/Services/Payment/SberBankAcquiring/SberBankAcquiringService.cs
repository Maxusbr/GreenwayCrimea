using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using AdvantShop.Core.Services.Payment.SberBankAcquiring;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using Newtonsoft.Json;
using HttpUtility = System.Web.HttpUtility;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Taxes;
using Tax = AdvantShop.Core.Services.Payment.SberBankAcquiring.Tax;

namespace AdvantShop.Payment
{
    // Документация: https://developer.sberbank.ru/acquiring-api-rest-requests1pay

    public class SberBankAcquiringService
    {
        private const string BaseUrl = "https://securepayments.sberbank.ru/payment/rest/";
        private const string BaseTestUrl = "https://3dsec.sberbank.ru/payment/rest/";

        private readonly string _userName;
        private readonly string _password;
        private readonly string _merchantLogin;
        private readonly bool _testMode;
        private readonly bool _sendReceiptData;

        public SberBankAcquiringService(string userName, string password, string merchantLogin, bool testMode, bool sendReceiptData)
        {
            _userName = userName;
            _password = password;
            _merchantLogin = merchantLogin;
            _testMode = testMode;
            _sendReceiptData = sendReceiptData;
        }

        /// <summary>
        /// Запрос  регистрации заказа
        /// </summary>
        public SberbankAcquiringRegisterResponse Register(Order order, string description, string returnUrl, string failUrl)
        {
            int index = 1;
            Receipt receipt = _sendReceiptData
                ? new Receipt()
                {
                    customerDetails = new CustomerDetails
                    {
                        email = order.OrderCustomer.Email,
                        //phone = order.OrderCustomer.Phone
                    },


                    cartItems = new CartItems
                    {
                        items =
                            OrderService.RecalculateItemsPriceIncludingAllDiscounts(order.OrderItems, order.ShippingCost, order.Sum)
                                .Select(item => new Item()
                                {
                                    positionId = (index++).ToString(),
                                    name = item.Name.Length > 100 ? item.Name.Substring(0, 100) : item.Name,
                                    itemCode = item.ArtNo,
                                    itemAmount = (int)(Math.Round(item.Price * 100, 2) * Math.Round(item.Amount, 2)),
                                    itemPrice = (int)(Math.Round(item.Price * 100, 2)),
                                    quantity = new Quantity()
                                    {
                                        value = item.Amount,
                                        measure = item.ProductID.HasValue
                                            ? ProductService.GetProduct(item.ProductID.Value).Unit.IsNotEmpty()
                                                ? ProductService.GetProduct(item.ProductID.Value).Unit
                                                : "штук"
                                            : "штук"
                                    },
                                    tax = new Tax() {taxType = GetTaxType(item.TaxType)}
                                }).ToList()
                    }
                }
                : null;

            if (order.ShippingCost > 0 && receipt != null)
            {
                receipt.cartItems.items.Add(new Item()
                {
                    name = "Доставка",
                    itemCode = "Доставка",
                    positionId = (index++).ToString(),
                    itemAmount = (int)(Math.Round(order.ShippingCost, 2) * 100),
                    itemPrice = (int)(Math.Round(order.ShippingCost, 2) * 100),
                    quantity = new Quantity { value = 1, measure = "штук" },
                    tax = new Tax() { taxType = GetTaxType(order.ShippingTaxType) }
                });
            }

            long sum = 0;

            if (receipt != null && receipt.cartItems != null && receipt.cartItems.items != null)
            {
                foreach (var item in receipt.cartItems.items)
                    sum += item.itemAmount;
            }
            else
            {
                sum = (long)Math.Round(order.Sum * 100);
            }

            int retriesNum = 0;
            string orderStrId;
            bool success = false;
            SberbankAcquiringRegisterResponse response;

            do
            {
                // если заказ уже есть в сбербанке, но был изменен на стороне магазина, подменяем id на id_номерпопытки
                orderStrId = retriesNum > 0
                    ? string.Format("{0}_{1}", order.OrderID, retriesNum)
                    : order.OrderID.ToString();

                var data = new Dictionary<string, object>()
                {
                    {"userName", _userName},
                    {"password", _password},
                    {"orderNumber", orderStrId},
                    {"amount", sum},    // Сумма платежа в копейках (или центах)
                    //{"currency", ""}, // ISO 4217
                    {"returnUrl", returnUrl},
                    {"failUrl", failUrl},
                    //{"pageView", "DESKTOP"}, // "MOBILE"
                    {"clientId", order.OrderCustomer.CustomerID},
                    {"merchantLogin", _merchantLogin},  // Чтобы зарегистрировать заказ от имени дочернего мерчанта, укажите его логин в этом параметре.
                    //{"bindingId", "" }                // Идентификатор связки, созданной ранее. Может использоваться, только если у магазина есть разрешение на работу со связками. Если этот параметр передаётся в данном запросе, то это означает: 1. Данный заказ может быть оплачен только с помощью связки; 2. Плательщик будет перенаправлен на платёжную страницу, где требуется только ввод CVC.
                };

                if (receipt != null)
                {
                    data.Add("orderBundle", JsonConvert.SerializeObject(receipt));
                }


                response = MakeRequest<SberbankAcquiringRegisterResponse>("register.do", data);

                if (response == null)
                    return null;

                string error;
                success = !HasRegisterError(response, out error);

                if (!success)
                {
                    Debug.Log.Info(string.Format("SberBankAcquiringService Register. code: {0} error: {1}, obj: {2}",
                                                    error, response.ErrorMessage, JsonConvert.SerializeObject(response)));

                }
                retriesNum++;
            } while (response.ErrorCode == "1" && retriesNum < 10);

            return success ? response : null;
        }

        /// <summary>
        /// Запрос состояния заказа
        /// </summary>
        public SberbankAcquiringOrderStatusResponse GetOrderStatus(string orderId)
        {
            var data = new Dictionary<string, object>()
            {
                {"userName", _userName},
                {"password", _password},
                {"orderNumber", orderId},
            };

            var response = MakeRequest<SberbankAcquiringOrderStatusResponse>("getOrderStatusExtended.do", data);

            if (response == null)
                return null;

            string error;

            if (!CheckOrderStatusError(response, out error))
            {
                Debug.Log.Info(string.Format("SberBankAcquiringService GetOrderStatus. code: {0} error: {1}, errorcode: {2}, obj: {3}",
                                                error, response.ErrorMessage, response.ErrorCode, JsonConvert.SerializeObject(response)));
                return null;
            }

            return response;
        }



        #region Private methods

       /*
        0 – без НДС;
        1 – НДС по ставке 0%;
        2 – НДС чека по ставке 10%;
        3 – НДС чека по ставке 18%;
        4 – НДС чека по расчётной ставке 10/110;
        5 – НДС чека по расчётной ставке 18/118. 
        */
        private int GetTaxType(TaxType? taxType)
        {
            if (taxType == null || taxType.Value == TaxType.Without)
                return 0;

            if (taxType.Value == TaxType.Zero)
                return 1;

            if (taxType.Value == TaxType.Ten)
                return 2;

            if (taxType.Value == TaxType.Eighteen)
                return 3;
            
            return 0;
        }

        private bool HasRegisterError(SberbankAcquiringRegisterResponse response, out string error)
        {
            error = null;
            var code = response.ErrorCode ?? "";
            switch (code)
            {
                case "0":
                    error = "Обработка запроса прошла без системных ошибок";
                    break;

                case "1":
                    error = "Заказ с таким номером уже зарегистрирован в системе";
                    break;

                case "2":
                    error = "Обработка запроса прошла без системных ошибок";
                    break;

                case "3":
                    error = "Неизвестная (запрещенная) валюта";
                    break;

                case "4":
                    error = "Отсутствует обязательный параметр запроса";
                    break;

                case "5":
                    error = "Ошибка значение параметра запроса";
                    break;

                case "7":
                    error = "Системная ошибка";
                    break;
            }

            return code != "0" && code.IsNotEmpty();
        }

        private bool CheckOrderStatusError(SberbankAcquiringOrderStatusResponse response, out string error)
        {
            error = null;
            var code = response.OrderStatus ?? "";

            switch (code)
            {
                case "0":
                    error = "Заказ зарегистрирован, но не оплачен";
                    break;

                case "1":
                    error = "Предавторизованная сумма захолдирована (для двухстадийных платежей)";
                    break;

                case "2":
                    error = "Проведена полная авторизация суммы заказа";
                    break;

                case "3":
                    error = "Авторизация отменена";
                    break;

                case "4":
                    error = "По транзакции была проведена операция возврата";
                    break;

                case "5":
                    error = "Инициирована авторизация через ACS банка-эмитента";
                    break;

                case "6":
                    error = "Авторизация отклонена";
                    break;
            }

            return string.IsNullOrWhiteSpace(error) || code == "2";
        }

        private T MakeRequest<T>(string url, Dictionary<string, object> data = null) where T : class
        {
            try
            {
                var request = WebRequest.Create((_testMode ? BaseTestUrl : BaseUrl) + url) as HttpWebRequest;
                request.Timeout = 5000;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (data != null)
                {
                    string dataPost = "";
                    foreach (var key in data.Keys)
                    {
                        var value = data[key];

                        if (value == null || value.ToString() == "")
                            continue;

                        if (dataPost != "")
                            dataPost += "&";

                        dataPost += key + "=" + HttpUtility.UrlEncode(value.ToString());
                    }

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
                                    Debug.Log.Error(error);
                                }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return null;
        }

        #endregion
    }
}