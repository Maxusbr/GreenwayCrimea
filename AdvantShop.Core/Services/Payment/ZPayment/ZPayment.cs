//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;

namespace AdvantShop.Payment
{
    [PaymentKey("ZPayment")]
    public class ZPayment : PaymentMethod
    {
        private const string Url = "https://z-payment.ru/merchant.php";
        public string Purse { get; set; }
        //public string WmID { get; set; }
        public string Password { get; set; }
        public string SecretKey { get; set; }

        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.ReturnUrl; }
        }

        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.ReturnUrl; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {ZPaymentTemplate.Purse, Purse},
                    //{ZPaymentTemplate.WmID, WmID},
                    {ZPaymentTemplate.Password, Password},
                    {ZPaymentTemplate.SecretKey, SecretKey}

                };
            }
            set
            {
                Purse = value.ElementOrDefault(ZPaymentTemplate.Purse);
                //WmID = value.ElementOrDefault(ZPaymentTemplate.WmID);
                Password = value.ElementOrDefault(ZPaymentTemplate.Password);
                SecretKey = value.ElementOrDefault(ZPaymentTemplate.SecretKey);
            }
        }

        public override void ProcessForm(Order order)
        {
            var paymentNo = order.OrderID.ToString();
            var sum = (order.Sum*GetCurrencyRate(order.OrderCurrency)).ToString("F2").Replace(",", ".");

            new PaymentFormHandler
            {
                Url = Url,
                InputValues = new Dictionary<string, string>
                {
                    {"LMI_PAYEE_PURSE", Purse},
                    {"LMI_PAYMENT_NO", paymentNo},
                    {"LMI_PAYMENT_DESC", "Order #" + order.OrderID},
                    {"LMI_PAYMENT_AMOUNT", sum},
                    {"ZP_SIGN", GetSign(paymentNo, sum)}
                }
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            var paymentNo = order.OrderID.ToString();
            var sum = (order.Sum*GetCurrencyRate(order.OrderCurrency)).ToString("F2").Replace(",", ".");
            return new PaymentFormHandler
            {
                Url = Url,
                Page = page,
                InputValues = new Dictionary<string, string>
                {
                    {"LMI_PAYEE_PURSE", Purse},
                    {"LMI_PAYMENT_NO", paymentNo},
                    {"LMI_PAYMENT_DESC", GetOrderDescription(order.Number)},
                    {"LMI_PAYMENT_AMOUNT", sum},
                    {"ZP_SIGN", GetSign(paymentNo, sum)}
                }
            }.ProcessRequest(true);
        }

        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;

            if (!req["LMI_PREREQUEST"].IsNullOrEmpty() && req["LMI_PREREQUEST"] == "1")
                return "YES";

            if (!CheckData(req))
                return NotificationMessahges.InvalidRequestData;
            var paymentNumber = req["LMI_PAYMENT_NO"];
            int orderID = 0;
            if (int.TryParse(paymentNumber, out orderID) && OrderService.GetOrder(orderID) != null)
            {
                OrderService.PayOrder(orderID, true);
                return NotificationMessahges.SuccessfullPayment(paymentNumber);
            }
            return NotificationMessahges.Fail;
        }

        private string GetSign(string paymentNo, string sum)
        {
            return (Purse + paymentNo + sum + Password).Md5(false);
        }

        private bool CheckData(HttpRequest req)
        {
            return !new[]
            {
                "LMI_PAYEE_PURSE",
                "LMI_PAYMENT_AMOUNT",
                "LMI_PAYMENT_NO",
                "LMI_MODE",
                "LMI_SYS_INVS_NO",
                "LMI_SYS_TRANS_NO",
                "LMI_SYS_TRANS_DATE",
                "LMI_PAYER_PURSE",
                "LMI_PAYER_WM"
            }.Any(field => string.IsNullOrEmpty(req[field]))
                   &&
                   (req["LMI_PAYEE_PURSE"] +
                    req["LMI_PAYMENT_AMOUNT"] +
                    req["LMI_PAYMENT_NO"] +
                    req["LMI_MODE"] +
                    req["LMI_SYS_INVS_NO"] +
                    req["LMI_SYS_TRANS_NO"] +
                    req["LMI_SYS_TRANS_DATE"] +
                    SecretKey +
                    req["LMI_PAYER_PURSE"] +
                    req["LMI_PAYER_WM"])
                       .Md5(true) == req["LMI_HASH"];
        }
    }
}