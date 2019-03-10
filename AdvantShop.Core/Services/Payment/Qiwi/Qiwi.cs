using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using System.Collections.Generic;
using AdvantShop.Repository.Currencies;
using AdvantShop.Shipping;
using Newtonsoft.Json;
using AdvantShop.Helpers;

namespace AdvantShop.Payment
{
    public class QiwiPaymentOption : BasePaymentOption
    {
        public QiwiPaymentOption()
        { }

        public QiwiPaymentOption(PaymentMethod menthod, float preCoast)
            : base(menthod, preCoast)
        { }
        public string Phone { get; set; }

        public override PaymentDetails GetDetails()
        {
            var phone = StringHelper.ConvertToStandardPhone(Phone);
            return new PaymentDetails { Phone = phone != null ? phone.ToString() : Phone };
        }

        public override string Template
        {
            get { return UrlService.GetUrl() + "scripts/_partials/payment/extendTemplate/QiwiPaymentOption.html"; }
        }

        public override bool Update(BasePaymentOption temp)
        {
            var curent = temp as QiwiPaymentOption;
            if (curent == null) return false;
            Phone = curent.Phone;
            return true;
        }
    }

    [PaymentKey("QIWI")]
    public class Qiwi : PaymentMethod
    {
        public string ProviderId { get; set; }
        public string RestId { get; set; }
        public string Password { get; set; }
        public string PasswordNotify { get; set; }
        public string ProviderName { get; set; }
        public string CurrencyCode { get; set; }
        public float CurrencyValue { get; set; }


        public override ProcessType ProcessType
        {
            get { return ProcessType.ServerRequest; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.Handler; }
        }


        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.CancelUrl | UrlStatus.ReturnUrl | UrlStatus.NotificationUrl; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {QiwiTemplate.ProviderID, ProviderId},
                               {QiwiTemplate.RestID, RestId},
                               {QiwiTemplate.Password, Password},
                               {QiwiTemplate.PasswordNotify, PasswordNotify},
                               {QiwiTemplate.ProviderName, ProviderName},
                               {QiwiTemplate.CurrencyCode, CurrencyCode},
                               {QiwiTemplate.CurrencyValue, CurrencyValue.ToString()}
                           };
            }
            set
            {
                ProviderId = value.ElementOrDefault(QiwiTemplate.ProviderID);
                RestId = value.ElementOrDefault(QiwiTemplate.RestID);
                Password = value.ElementOrDefault(QiwiTemplate.Password);
                PasswordNotify = value.ElementOrDefault(QiwiTemplate.PasswordNotify);
                ProviderName = value.ElementOrDefault(QiwiTemplate.ProviderName);
                CurrencyCode = value.ElementOrDefault(QiwiTemplate.CurrencyCode);
                CurrencyValue = value.ElementOrDefault(QiwiTemplate.CurrencyValue).TryParseFloat(1);
            }
        }

        public override string ProcessServerRequest(Order order)
        {
            int retriesNum = 0;
            string result = "";
            var qiwiAnswer = new QiwiTemplate.QIWIAnswer();

            string orderStrId;
            do
            {
                // если заказ уже есть в системе qiwi, но был изменен на стороне магазина, подменяем id на id_номерпопытки
                orderStrId = retriesNum > 0
                    ? string.Format("{0}_{1}", order.OrderID, retriesNum)
                    : order.OrderID.ToString();

                var webRequest = (HttpWebRequest)WebRequest.Create(string.Format("https://api.qiwi.com/api/v2/prv/{0}/bills/{1}", 
                    Parameters[QiwiTemplate.ProviderID], orderStrId));
                webRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(
                    Encoding.UTF8.GetBytes((Parameters.ContainsKey(QiwiTemplate.RestID) ? Parameters[QiwiTemplate.RestID] : Parameters[QiwiTemplate.ProviderID]) + ":" + Parameters[QiwiTemplate.Password]));

                webRequest.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                webRequest.PreAuthenticate = true;
                webRequest.Method = "PUT";
                webRequest.Accept = "text/json";

                string data = string.Format("user={0}&amount={1}&ccy={2}&comment={3}&lifetime={4}",
                    HttpUtility.UrlEncode("tel:+" + (order.PaymentDetails != null ? order.PaymentDetails.Phone : "")),
                                            (order.Sum / CurrencyValue).ToString("F2").Replace(",", "."),
                                            Parameters[QiwiTemplate.CurrencyCode],
                                            order.OrderID.ToString(),
                                            HttpUtility.UrlEncode(DateTime.Now.AddDays(45).ToString("yyyy-MM-ddTHH:mm:ss"))
                    );

                var bytes = Encoding.UTF8.GetBytes(data);
                webRequest.ContentLength = bytes.Length;
                using (var requestStream = webRequest.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }

                try
                {
                    using (var response = webRequest.GetResponse())
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            if (stream != null)
                                using (var reader = new StreamReader(stream))
                                {
                                    result = reader.ReadToEnd();
                                }
                        }
                    }
                }
                catch (WebException e)
                {
                    using (var eResponse = e.Response)
                    {
                        using (var eStream = eResponse.GetResponseStream())
                        {
                            if (eStream != null)
                                using (var reader = new StreamReader(eStream))
                                {
                                    result = reader.ReadToEnd();
                                }
                        }
                    }
                }

                retriesNum++;
                if (result.IsNotEmpty())
                    qiwiAnswer = JsonConvert.DeserializeObject<QiwiTemplate.QIWIAnswer>(result);

            } while (result.IsNotEmpty() && qiwiAnswer.response.result_code == (int)QiwiTemplate.CreateCode.BillIsExist && retriesNum < 10);

            if (result.IsNotEmpty())
            {
                if (qiwiAnswer.response.bill != null && qiwiAnswer.response.result_code == (int)QiwiTemplate.CreateCode.Sucsses && qiwiAnswer.response.bill.status == "waiting")
                {
                    return string.Format(
                        "https://api.qiwi.com/order/external/main.action?shop={0}&transaction={1}&successUrl={2}&failUrl={3}",
                        Parameters[QiwiTemplate.ProviderID], 
                        orderStrId,
                        HttpUtility.UrlEncode(SuccessUrl),
                        HttpUtility.UrlEncode(FailUrl)
                        );

                }
                else
                {
                    Debug.Log.Error(result);
                }
            }

            return string.Empty;
        }

        public override string ProcessResponse(HttpContext context)
        {
            // Коды завершения уведомлений
            // 0    Успех
            // 5    Ошибка формата параметров запроса
            // 13   Ошибка соединения с базой данных
            // 150  Ошибка проверки пароля
            // 151  Ошибка проверки подписи
            // 300  Ошибка связи с сервером

            // Статусы счетов
            // Код статуса  Описание                                  Финальный статус
            // waiting      Счет выставлен, ожидает оплаты                  Нет
            // paid         Счет оплачен                                    Да
            // rejected     Счет отклонен                                   Да
            // unpaid       Ошибка при проведении оплаты. Счет не оплачен   Да
            // expired      Время жизни счета истекло. Счет не оплачен      Да

            HttpRequest req = context.Request;
            int resultCode;

            string orderStrId = req["bill_id"] ?? string.Empty;
            if (orderStrId.Contains("_TEST_"))
                orderStrId = orderStrId.Replace("_TEST_", string.Empty);
            else
                orderStrId = orderStrId.Contains("_") ? orderStrId.Split(new[] { '_' })[0] : orderStrId;

            if (req.Headers["Authorization"] == "Basic " + Convert.ToBase64String(
                Encoding.UTF8.GetBytes(Parameters[QiwiTemplate.ProviderID] + ":" +
                (Parameters[QiwiTemplate.PasswordNotify].IsNotEmpty() ? Parameters[QiwiTemplate.PasswordNotify] : Parameters[QiwiTemplate.Password]))))
            {
                int orderId = 0;
                if (int.TryParse(orderStrId, out orderId) && OrderService.GetOrder(orderId) != null)
                {
                    if (req["status"] == "paid")
                        OrderService.PayOrder(orderId, true);

                    // QIWI: Любой ответ, содержащий код результата обработки уведомления, отличный от 0, интерпретируется сервером Visa QIWI Wallet как временная ошибка провайдера.
                    resultCode = 0;
                }
                else
                {
                    resultCode = 5;
                }
            }
            else
            {
                resultCode = 150;
            }

            var result = string.Format("<?xml version=\"1.0\"?><result><result_code>{0}</result_code></result>", resultCode);

            context.Response.Clear();
            context.Response.ContentType = "text/xml";
            context.Response.Write(result);
            context.Response.End();
            return result;
        }

        public override BasePaymentOption GetOption(BaseShippingOption shippingOption, float preCoast)
        {
            var option = new QiwiPaymentOption(this, preCoast);
            return option;
        }
    }
}