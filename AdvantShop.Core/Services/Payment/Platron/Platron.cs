//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Serialization;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Taxes;

namespace AdvantShop.Payment
{
    [PaymentKey("Platron")]
    public class Platron : PaymentMethod
    {
        private const string BaseUrl = "https://www.platron.ru/";
        private const string Separator = ";";

        private const string ResultFormat =
            @"<?xml version='1.0' encoding='utf-8'?>
                <response>
	                <pg_salt>{0}</pg_salt>
	                <pg_status>{1}</pg_status>
	                <pg_description>{2}</pg_description>
	                <pg_sig>{3}</pg_sig>
                </response>";

        public string MerchantId { get; set; }
        public string Currency { get; set; }
        public string PaymentSystem { get; set; }
        public float CurrencyValue { get; set; }
        public string SecretKey { get; set; }

        public bool SendReceiptData { get; set; }

        public override ProcessType ProcessType
        {
            get { return ProcessType.ServerRequest; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.Handler | NotificationType.ReturnUrl; }
        }
        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.CancelUrl | UrlStatus.FailUrl | UrlStatus.NotificationUrl | UrlStatus.ReturnUrl; }
        }
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {PlatronTemplate.MerchantId , MerchantId},
                               {PlatronTemplate.Currency , Currency},
                               {PlatronTemplate.PaymentSystem , PaymentSystem},
                               {PlatronTemplate.CurrencyValue  , CurrencyValue.ToString()},
                               {PlatronTemplate.SecretKey, SecretKey},

                               {PlatronTemplate.SendReceiptData, SendReceiptData.ToString()},
                           };
            }
            set
            {
                MerchantId = value.ElementOrDefault(PlatronTemplate.MerchantId);
                Currency = value.ElementOrDefault(PlatronTemplate.Currency);
                PaymentSystem = value.ElementOrDefault(PlatronTemplate.PaymentSystem);
                CurrencyValue = value.ElementOrDefault(PlatronTemplate.CurrencyValue).TryParseFloat(1);
                SecretKey = value.ElementOrDefault(PlatronTemplate.SecretKey);

                SendReceiptData = value.ElementOrDefault(PlatronTemplate.SendReceiptData).TryParseBool();
            }
        }

        public static Dictionary<string, string> GetCurrencies()
        {
            return Currencies;
        }
        public static readonly Dictionary<string, string> Currencies = new Dictionary<string, string>
                                                                           {
                                                                               {"RUR", "Российские рубли"},
                                                                               {"USD", "Доллары США"},
                                                                               {"EUR", "Евро"},
                                                                           };
        public static Dictionary<string, string> GetPaymentSystems()
        {
            return PaymentSystems;
        }
        public static readonly Dictionary<string, string> PaymentSystems = new Dictionary<string, string>
                                                                           {
                                                                               {"WEBMONEYR", "ЭПС WebMoney, R-кошельки"},
                                                                               {"WEBMONEYZ", "ЭПС WebMoney, Z-кошельки"},
                                                                               {"WEBMONEYE", "ЭПС WebMoney, E-кошельки"},
                                                                               {"WEBMONEYRBANK","ЭПС WebMoney, R-кошельки с перечислением на расчетный счет в банке"},
                                                                               {"YANDEXMONEY","ЭПС Яндекс.Деньги"},
                                                                               {"MONEYMAILRU","ЭПС деньги@mail.ru"},
                                                                               {"RBKMONEY","ЭПС RbkMoney"},
                                                                               // Код ПС TRANSCRED отключен {"TRANSCRED","Кредитные карты через процессинг Транскредит банка"}, 
                                                                               {"BANKCARDPRU","Кредитные карты через процессинг Транскредит банка"},
                                                                               {"RAIFFEISEN","Кредитные карты через процессинг Райффайзен банка"},
                                                                               {"EUROSET","EUROSET"},
                                                                               {"ELECSNET","Терминалы Элекснет"},
                                                                               {"OSMP","Терминалы ОСМП / QIWI"},
                                                                               {"OSMP-II","Терминалы ОСМП / QIWI с активационным платежом"},
                                                                               {"BEELINEMK","Счет на телефоне Билайн"},
                                                                               {"UNIKASSA","Терминалы Уникасса"},
                                                                               {"COMEPAY","Терминалы ComePay"},
                                                                               {"PINPAY","Терминалы PinPay Express"},
                                                                               {"MOBW","Мобильный кошелек ОСМП / QIWI "},
                                                                               {"CONTACT","Система приёма платежей «Контакт»"},
                                                                               {"MASTERBANK","Банкоматы МастерБанка"},
                                                                               {"CASH","Наличные (включает EUROSET, ELECSNET, OSMP, OSMP-II, UNIKASSA, COMEPAY, ALLOCARD, CONTACT, MASTERBANK, PINPAY)"}
                                                                           };

        public override string ProcessServerRequest(Order order)
        {
            var result = SendPaymentRequest(order);
            if (result == null)
                return string.Empty;

            if (SendReceiptData)
            {
                var receiptResult = SendReceiptRequest(order, result.PaymentId);
                if (receiptResult != null && receiptResult.ErrorDescription.IsNotEmpty())
                {
                    Debug.Log.Error("error at creating receipt for order #" + order.OrderID + ": " + receiptResult.ErrorDescription);
                }
            }
            return result.RedirectUrl;
        }

        private PlatronPaymentResponse SendPaymentRequest(Order order)
        {
            var @params = new Dictionary<string, string>
            {
                { "pg_amount", GetFloatString(order.Sum / CurrencyValue)},
                { "pg_currency", Currency},
                { "pg_description", GetOrderDescription(order.Number)},
                { "pg_merchant_id", MerchantId},
                { "pg_order_id", order.OrderID.ToString()},
                { "pg_salt", Guid.NewGuid().ToString()},
                { "cms", "advantshop" }
            };
            if (order.OrderCustomer != null && order.OrderCustomer.StandardPhone.HasValue)
                @params.Add("pg_user_phone", order.OrderCustomer.StandardPhone.Value.ToString());

            return SendRequest<PlatronPaymentResponse>("init_payment.php", @params);
        }

        private PlatronResponse SendReceiptRequest(Order order, string paymentId)
        {
            var @params = new Dictionary<string, string>
                {
                    { "pg_merchant_id", MerchantId},
                    { "pg_operation_type", "payment"},
                    { "pg_salt", Guid.NewGuid().ToString()},
                };
            if (paymentId.IsNotEmpty())
                @params.Add("pg_payment_id", paymentId);
            else
                @params.Add("pg_order_id", order.OrderID.ToString());

            var orderItems = OrderService.RecalculateItemsPriceIncludingAllDiscounts(order.OrderItems.Where(x => x.Price > 0).ToList(), order.ShippingCost, order.Sum);
            for (int i = 0; i < orderItems.Count; i++)
            {
                // pg_amount: Сумма (float). Не обязательное поле. Если меньше pg_price * pg_quantity – воспринимается как скидка.

                @params.Add(string.Format("pg_items[{0}][pg_label]", i), orderItems[i].Name);
                @params.Add(string.Format("pg_items[{0}][pg_price]", i), GetFloatString(orderItems[i].Price / CurrencyValue));
                @params.Add(string.Format("pg_items[{0}][pg_quantity]", i), GetFloatString(orderItems[i].Amount));
                //0 – ставка НДС 0%; 10 – ставка НДС 10%; 18 – ставка НДС 18%; 110 – ставка НДС 10/110; 118 – ставка НДС 18/118; Если поле отсутствует – не облагается НДС
                var vatType = GetTaxType(orderItems[i].TaxType);
                if (vatType != "") // Без НДС
                    @params.Add(string.Format("pg_items[{0}][pg_vat]", i), vatType);
            }
            if (order.ShippingCost > 0)
            {
                @params.Add(string.Format("pg_items[{0}][pg_label]", orderItems.Count), "Доставка");
                @params.Add(string.Format("pg_items[{0}][pg_price]", orderItems.Count), GetFloatString(order.ShippingCost / CurrencyValue));
                @params.Add(string.Format("pg_items[{0}][pg_quantity]", orderItems.Count), "1");

                var vatType = GetTaxType(order.ShippingTaxType);
                if (vatType != "") // Без НДС
                    @params.Add(string.Format("pg_items[{0}][pg_vat]", orderItems.Count), vatType);
            }

            return SendRequest<PlatronResponse>("receipt.php", @params);
        }

        private T SendRequest<T>(string script, Dictionary<string, string> @params) where T : PlatronResponse
        {
            var signParams = new List<string> { script }; // вызываемый скрипт
            signParams.AddRange(@params.OrderBy(key => key.Key).Select(pair => pair.Value)); // параметры в алфавитном порядке
            @params.Add("pg_sig", GetSignature(signParams.AggregateString(Separator)));

            var queryParams = @params.Select(pair => string.Format("{0}={1}", pair.Key, pair.Value)).AggregateString("&");
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(BaseUrl + script);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                byte[] byteArray = Encoding.GetEncoding("utf-8").GetBytes(queryParams);
                request.ContentLength = byteArray.Length;

                using (var dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var dataStream = response.GetResponseStream())
                    {
                        if (dataStream != null)
                            using (var reader = new StreamReader(dataStream))
                            {
                                var serializer = new XmlSerializer(typeof(T));
                                return (T)serializer.Deserialize(reader);
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex.Message + " at Platron SendRequest to " + script + " with parameters: " + queryParams, ex);
            }
            return null;
        }

        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;
            //if (!CheckData(req))
            //    return InvalidRequestData;
            var paymentNumber = req["pg_order_id"];
            try
            {
                int orderID = 0;
                if (int.TryParse(paymentNumber, out orderID) && OrderService.GetOrder(orderID) != null)
                {
                    if (!string.IsNullOrWhiteSpace(req["pg_result"]) && req["pg_result"].Trim() == "1")
                    {
                        OrderService.PayOrder(orderID, true);
                    }
                    //else if (string.IsNullOrWhiteSpace(req["pg_refund_type"]))
                    //{
                    //    return string.Empty;
                    //}
                    return SuccessfullPayment(paymentNumber);
                }
                

            }
            catch { }
            return RejectedResponse;
        }

        protected string InvalidRequestData
        {
            get
            {
                const string desc = "Order not found";
                const string status = "error";
                return FormatNotificationResponse(desc, status);
            }
        }
        protected string RejectedResponse
        {
            get
            {
                const string desc = "Order not found";
                const string status = "rejected";
                return FormatNotificationResponse(desc, status);
            }
        }
        protected string SuccessfullPayment(string orderNumber)
        {
            const string desc = "Order payed";
            const string status = "ok";
            return FormatNotificationResponse(desc, status);

        }
        protected string FormatNotificationResponse(string desc, string status)
        {
            var salt = Guid.NewGuid().ToString();
            return string.Format(ResultFormat, salt, status, desc,
                                 GetSignature(HttpContext.Current.Request.Path.Split("/").LastOrDefault() + Separator + desc + Separator +
                                              salt + Separator + status));
        }

        private string GetSignature(string fields)
        {
            return (fields + Separator + SecretKey).Md5(false, Encoding.UTF8);
        }

        private bool CheckData(HttpRequest req)
        {
            if (string.IsNullOrWhiteSpace(req["pg_sig"]))
            {
                return false;
            }

            var parameters = new Dictionary<string, string>
                {
                    {"pg_salt",req["pg_salt"]},
                    {"pg_order_id",req["pg_order_id"]},
                    {"pg_payment_id",req["pg_payment_id"]},
                    {"pg_payment_system",req["pg_payment_system"]},
                    {"pg_amount",req["pg_amount"]},
                    {"pg_currency",req["pg_currency"]},
                    {"pg_net_amount",req["pg_net_amount"]},
                    {"pg_ps_amount",req["pg_ps_amount"]},
                    {"pg_ps_currency",req["pg_ps_currency"]},
                    {"pg_ps_full_amount",req["pg_ps_full_amount"]},
                    {"pg_payment_date",req["pg_payment_date"]},
                    {"pg_can_reject",req["pg_can_reject"]},
                    {"pg_result",req["pg_result"]}
                }.OrderBy(pair => pair.Key);
            var stringForSig = string.Empty;
            for (int i = 0; i < parameters.Count(); ++i)
            {
                if (!string.IsNullOrWhiteSpace(parameters.ElementAt(i).Value))
                {
                    stringForSig += parameters.ElementAt(i).Value + Separator;
                }
            }
            stringForSig += SecretKey;

            return string.Equals(stringForSig.Md5(), req["pg_sig"]);
        }

        private string GetFloatString(float val)
        {
            return val.ToString("F2").Replace(",", ".");
        }

        /*
            0 – ставка НДС 0%; 
            10 – ставка НДС 10%; 
            18 – ставка НДС 18%; 
            110 – ставка НДС 10/110; 
            118 – ставка НДС 18/118; 
            Если поле отсутствует – не облагается НДС 
        */
        private string GetTaxType(TaxType? taxType)
        {
            if (taxType == null || taxType.Value == TaxType.Without)
                return "";

            if (taxType == TaxType.Zero)
                return "0";

            if (taxType == TaxType.Ten)
                return "10";

            if (taxType == TaxType.Eighteen)
                return "18";

            return "";
        }
    }
}