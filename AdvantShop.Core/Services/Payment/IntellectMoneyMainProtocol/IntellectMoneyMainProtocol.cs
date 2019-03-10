//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;

namespace AdvantShop.Payment
{
    [PaymentKey("IntellectMoneyMainProtocol")]
    public class IntellectMoneyMainProtocol : PaymentMethod
    {
        private const string Separator = "::";

        public string EshopId { get; set; }
        //public string RecipientCurrency { get; set; }
        public string Preference { get; set; }

        public string SecretKey { get; set; }


        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }
        public override NotificationType NotificationType
        {
            get { return NotificationType.Handler; }
        }
        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.NotificationUrl | UrlStatus.CancelUrl; }
        }
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {IntellectMoneyMainProtocolTemplate.EshopId , EshopId},
                               //{IntellectMoneyMainProtocolTemplate.RecipientCurrency  , RecipientCurrency},
                               {IntellectMoneyMainProtocolTemplate.Preference , Preference},
                               {IntellectMoneyMainProtocolTemplate.SecretKey , SecretKey},
                           };
            }
            set
            {
                EshopId = value.ElementOrDefault(IntellectMoneyMainProtocolTemplate.EshopId);
                //RecipientCurrency = value.ElementOrDefault(IntellectMoneyMainProtocolTemplate.RecipientCurrency);
                Preference = value.ElementOrDefault(IntellectMoneyMainProtocolTemplate.Preference);
                SecretKey = value.ElementOrDefault(IntellectMoneyMainProtocolTemplate.SecretKey);
            }
        }

        public static Dictionary<string, string> GetPaymentSystems()
        {
            return PaymentSystems;
        }

        public static readonly Dictionary<string, string> PaymentSystems = new Dictionary<string, string>
        {
            {"", "Не выбран"},
            {"inner", "Оплата с кошелька Rbk Money"},
            {"bankCard", "Банковская карта Visa/MasterCard"},
            {"exchangers", "Электронные платежные системы"},
            {"prepaidcard", "Предоплаченная карта RBK Money"},
            {"transfers", "Системы денежных переводов"},
            {"terminals", "Платёжные терминалы"},
            {"iFree", "SMS"},
            {"bank", "Банковский платёж"},
            {"postRus", "Почта России"},
            {"atm", "Банкоматы"},
            {"yandex", "Яндекс"},
            {"ibank", "Интернет банкинг"},
            {"euroset", "Евросеть"}
        };

        public override void ProcessForm(Order order)
        {
            var paymentNo = order.OrderID.ToString();
            var sum = (order.Sum * GetCurrencyRate(order.OrderCurrency)).ToString("F2").Replace(",", ".");
            new PaymentFormHandler
            {
                Url = "https://merchant.intellectmoney.ru/ru/",
                InputValues = new Dictionary<string, string>
                {
                    { "eshopId", EshopId },
                    { "orderId", paymentNo},
                    { "serviceName", "Order #" +order.OrderID},
                    { "recipientAmount", sum},
                    { "recipientCurrency", this.PaymentCurrency.Iso3},
                    //{ "userName", order.OrderCustomer.FirstName + " " + order.OrderCustomer.LastName},
                    { "user_email", order.OrderCustomer.Email},
                    { "preference", Preference }
            }
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            var paymentNo = order.OrderID.ToString();
            var sum = (order.Sum * GetCurrencyRate(order.OrderCurrency)).ToString("F2").Replace(",", ".");
            return new PaymentFormHandler
            {
                Url = "https://merchant.intellectmoney.ru/ru/",
                Page = page,
                InputValues = new Dictionary<string, string>
                {
                    { "eshopId", EshopId },
                    { "orderId", paymentNo},
                    { "serviceName", "Order #" +order.OrderID},
                    { "recipientAmount", sum},
                    { "recipientCurrency", this.PaymentCurrency.Iso3},
                    //{ "userName", order.OrderCustomer.FirstName + " " + order.OrderCustomer.LastName},
                    { "user_email", order.OrderCustomer.Email},
                    { "preference", Preference }
                }
            }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;
            if (!CheckData(req))
            {
                Debug.Log.Error(req.ServerVariables["ALL_RAW"]);
                return NotificationMessahges.InvalidRequestData;
            }

            var paymentNumber = req["orderId"];
            int orderID = 0, payStatus = 0;
            int.TryParse(req["paymentStatus"], out payStatus);
            if (payStatus != 5)
            {
                return string.Format("OK");
            }
            if (int.TryParse(paymentNumber, out orderID) && OrderService.GetOrder(orderID) != null)
            {
                OrderService.PayOrder(orderID, true);
                return string.Format("OK");
                //return NotificationMessahges.SuccessfullPayment(paymentNumber);
            }
            return NotificationMessahges.Fail;
        }

        private bool CheckData(HttpRequest req)
        {
            return
               (req["eshopId"] + Separator +
                req["orderId"] + Separator +
                HttpUtility.UrlDecode(req["serviceName"]) + Separator +
                req["eshopAccount"] + Separator +
                req["recipientAmount"] + Separator +
                req["recipientCurrency"] + Separator +
                req["paymentStatus"] + Separator +
                HttpUtility.UrlDecode(req["userName"]) + Separator +
                HttpUtility.UrlDecode(req["userEmail"]) + Separator +
                HttpUtility.UrlDecode(req["paymentData"]) + Separator +
                SecretKey).Md5(false) == req["hash"];
        }
    }
}