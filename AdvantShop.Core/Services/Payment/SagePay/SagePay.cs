//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Payment
{
    [PaymentKey("SagePay")]
    public class SagePay : PaymentMethod
    {
        private string Url
        {
            get
            {
                return Sandbox
                    ? "https://test.sagepay.com/gateway/service/vspform-register.vsp"
                    : "https://live.sagepay.com/gateway/service/vspform-register.vsp";
            }
        }

        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.ReturnUrl; }
        }

        public static readonly List<string> AvaliableCurrs = new List<string>
        {
            "EUR",
            "GBP",
            "USD"
        };

        public string Vendor { get; set; }
        public bool Sandbox { get; set; }
        public string CurrencyCode { get; set; }
        public string Password { get; set; }
        public float CurrencyValue { get; set; }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {SagePayTemplate.Vendor, Vendor},
                    {SagePayTemplate.Sandbox, Sandbox.ToString()},
                    {SagePayTemplate.CurrencyCode, CurrencyCode},
                    {SagePayTemplate.Password, Password},
                    {SagePayTemplate.CurrencyValue, CurrencyValue.ToString()}
                };
            }
            set
            {
                Vendor = value.ElementOrDefault(SagePayTemplate.Vendor);
                Sandbox = value.ElementOrDefault(SagePayTemplate.Sandbox).TryParseBool();
                CurrencyCode = value.ElementOrDefault(SagePayTemplate.CurrencyCode, "USD");
                float decVal;
                if (float.TryParse(value.ElementOrDefault(SagePayTemplate.CurrencyValue), out decVal))
                {
                    CurrencyValue = decVal;
                }
                else
                {
                    var usd = CurrencyService.Currency(CurrencyCode);
                    CurrencyValue = usd != null ? usd.Rate : 1;
                }
            }
        }

        public override void ProcessForm(Order order)
        {
            var paymentNo = order.OrderID.ToString();
            var sum = string.Format("{0:0.00}", order.Sum/CurrencyValue);
            var description = string.Format("Order #{0} payment", order.Number);

            new PaymentFormHandler
            {
                Url = Url,
                InputValues = new Dictionary<string, string>
                {

                    {"VPSProtocol", "2.23"},
                    {"TxType", "PAYMENT"},
                    {"Vendor", Vendor},
                    {"VendorTxCode", paymentNo},
                    {"Amount", sum},
                    {"Currency", CurrencyCode},
                    {"Description", description},
                    {"SuccessURL", SuccessUrl},
                    {"FailureURL", FailUrl},
                    {"Crypt", GetCrypt(paymentNo, sum, CurrencyCode, description, SuccessUrl, FailUrl)}
                }
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            var paymentNo = order.OrderID.ToString();
            var sum = string.Format("{0:0.00}", order.Sum/CurrencyValue);
            var description = string.Format("Order #{0} payment", order.Number);

            return new PaymentFormHandler
            {
                Url = Url,
                Page = page,
                InputValues = new Dictionary<string, string>
                {

                    {"VPSProtocol", "2.23"},
                    {"TxType", "PAYMENT"},
                    {"Vendor", Vendor},
                    {"VendorTxCode", paymentNo},
                    {"Amount", sum},
                    {"Currency", CurrencyCode},
                    {"Description", description},
                    {"SuccessURL", SuccessUrl},
                    {"FailureURL", FailUrl},
                    {"Crypt", GetCrypt(paymentNo, sum, CurrencyCode, description, SuccessUrl, FailUrl)}
                }
            }.ProcessRequest();
        }

        private string GetCrypt(string vendorTxCode, string amount, string currency, string description,
            string successUrl, string failUrl)
        {
            return
                SagePayUtils.EncryptAndEncode(
                    string.Format("VendorTxCode={0}Amount={1}Currency={2}Description={3}SuccessURL={4}FailureURL={5}",
                        vendorTxCode, amount, currency, description, successUrl, failUrl), Password);

        }

        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;
            if (GetData(req) == null)
                return NotificationMessahges.InvalidRequestData;
            var paymentNumber = req["VendorTxCode"];
            int orderID;
            if (int.TryParse(paymentNumber, out orderID) &&
                OrderService.GetOrder(orderID) != null)
            {
                var order = OrderService.GetOrder(orderID);
                if (order != null && req["Amount"] == string.Format("{0:0.00}", order.Sum/CurrencyValue))
                {
                    OrderService.PayOrder(orderID, true);
                    return NotificationMessahges.SuccessfullPayment(order.Number);
                }

            }
            return NotificationMessahges.Fail;
        }

        private NameValueCollection GetData(HttpRequest req)
        {
            if (string.IsNullOrEmpty(req["Crypt"]))
                return null;
            var queryParams = HttpUtility.ParseQueryString(SagePayUtils.DecodeAndDecrypt(req["Crypt"], Password));
            return new[]
            {
                "Status",
                "StatusDetail",
                "VendorTxCode",
                "VPSTxId",
                "Amount",
            }.Any(item => string.IsNullOrEmpty(queryParams[item]))
                ? null
                : queryParams;
        }
    }
}