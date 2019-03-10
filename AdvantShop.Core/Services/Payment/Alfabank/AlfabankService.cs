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
using System.Web;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using Newtonsoft.Json;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Core.Services.Payment.Alfabank
{
    public class AlfabankService
    {
        public const string ReturnUrlParamNameMerchantOrder = "merchantorderid";
        public const string ReturnUrlParamNameAlfaOrder = "orderId";

        private const string TestUrl = "https://web.rbsuat.com/ab/rest/";

        private const string GeneralUrl = "https://pay.alfabank.ru/payment/rest/";

        private readonly string _userName;
        private readonly string _password;
        private readonly string _merchantLogin;
        private readonly bool _useTestMode;

        public AlfabankService(string userName, string password, string merchantLogin, string useTestMode)
        {
            _userName = userName;
            _password = password;
            _merchantLogin = merchantLogin;
            _useTestMode = useTestMode.TryParseBool(true) ?? true;
        }

        /// <summary>
        /// Регистрация заказа
        /// </summary>
        public AlfabankRegisterResponse Register(Order order, string description, Func<OrderCurrency, float> getCurrencyRate, string returnUrl, string failUrl)
        {
            var sum = (int)(Math.Round(order.Sum * getCurrencyRate(order.OrderCurrency), 2) * 100);

            int retriesNum = 0;
            string orderStrId;
            bool success = false;
            AlfabankRegisterResponse response;

            do
            {
                // если заказ уже есть в сбербанке, но был изменен на стороне магазина, подменяем id на id_номерпопытки
                orderStrId = retriesNum > 0
                    ? string.Format("{0}_{1}", order.OrderID, retriesNum)
                    : order.OrderID.ToString();

                var data = new Dictionary<string, string>()
                {
                    {"userName", _userName},
                    {"password", _password},
                    {"orderNumber", orderStrId},
                    {"amount", sum.ToString(CultureInfo.InvariantCulture)},    // Сумма платежа в копейках (или центах)
                    //{"currency", ""}, // ISO 4217
                    {"returnUrl", string.Format("{0}{1}", returnUrl, string.Format("{0}{1}={2}", (returnUrl.Contains("?") ? "&" : "?"), ReturnUrlParamNameMerchantOrder, HttpUtility.UrlEncode(order.Number)))},
                    {"failUrl", failUrl},
                    //{"pageView", "DESKTOP"}, // "MOBILE"
                    {"clientId", order.OrderCustomer.CustomerID.ToString()},
                    //{"bindingId", "" }                // Идентификатор связки, созданной ранее. Может использоваться, только если у магазина есть разрешение на работу со связками. Если этот параметр передаётся в данном запросе, то это означает: 1. Данный заказ может быть оплачен только с помощью связки; 2. Плательщик будет перенаправлен на платёжную страницу, где требуется только ввод CVC.
                };

                if (!string.IsNullOrEmpty(_merchantLogin))
                    data.Add("merchantLogin", _merchantLogin);  // Чтобы зарегистрировать заказ от имени дочернего мерчанта, укажите его логин в этом параметре.


                response = MakeRequest<AlfabankRegisterResponse>("register.do", data);

                if (response == null)
                    return null;

                success = response.ErrorCode == 0;

                if (!success)
                {
                    Debug.Log.Info(string.Format("AlfabankService Register. code: {0} error: {1}, obj: {2}",
                                                    response.ErrorCode, response.ErrorMessage, JsonConvert.SerializeObject(response)));
                }
                retriesNum++;
            } while (response.ErrorCode == 1 && retriesNum < 20);

            return success ? response : null;
        }

        /// <summary>
        /// Запрос состояния заказа
        /// </summary>
        public AlfabankOrderStatusResponse GetOrderStatus(string alfaOrderId, string merchantOrderid)
        {
            var data = new Dictionary<string, string>()
            {
                {"userName", _userName},
                {"password", _password},
            };

            if (!string.IsNullOrEmpty(alfaOrderId))
                data.Add("orderId", alfaOrderId);

            if (!string.IsNullOrEmpty(merchantOrderid))
                data.Add("merchantOrderNumber", merchantOrderid);

            var response = MakeRequest<AlfabankOrderStatusResponse>("getOrderStatusExtended.do", data);

            if (response == null)
                return null;

            var success = response.ErrorCode == 0;

            if (!success)
            {
                Debug.Log.Info(string.Format("AlfabankService GetOrderStatus. code: {0} error: {1}, obj: {2}",
                                                response.ErrorCode, response.ErrorMessage, JsonConvert.SerializeObject(response)));
            }

            return response;
        }

        #region Private methods

        private T MakeRequest<T>(string url, Dictionary<string, string> data = null) where T : class
        {
            try
            {
                var request = WebRequest.Create((_useTestMode ? TestUrl : GeneralUrl) + url) as HttpWebRequest;
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

                        if (string.IsNullOrEmpty(value))
                            continue;

                        if (dataPost != "")
                            dataPost += "&";

                        dataPost += key + "=" + HttpUtility.UrlEncode(value);
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

    }
}
