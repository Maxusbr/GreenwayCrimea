//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------


using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;

namespace AdvantShop.Payment
{
    /// <summary>
    /// Summary description for WebMoney
    /// </summary>
    [PaymentKey("WebMoney")]
    public class WebMoney : PaymentMethod
    {
        public string Purse { get; set; }
        public string WmID { get; set; }
        public string SecretKey { get; set; }

        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.NotificationUrl | UrlStatus.ReturnUrl | UrlStatus.FailUrl; }
        }

        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.ReturnUrl | NotificationType.Handler; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {WebMoneyTemplate.Purse, Purse},
                    {WebMoneyTemplate.SecretKey, SecretKey},

                };
            }
            set
            {
                Purse = value.ElementOrDefault(WebMoneyTemplate.Purse);
                SecretKey = value.ElementOrDefault(WebMoneyTemplate.SecretKey);
            }
        }

        public override void ProcessForm(Order order)
        {
            new PaymentFormHandler
            {
                Url = "https://merchant.webmoney.ru/lmi/payment.asp",
                InputValues = new Dictionary<string, string>
                {
                    {"LMI_PAYEE_PURSE", Purse},
                    {"LMI_PAYMENT_NO", order.OrderID.ToString()},
                    {"LMI_PAYMENT_DESC", GetOrderDescription(order.Number)},
                    {"LMI_PAYMENT_AMOUNT", (order.Sum*GetCurrencyRate(order.OrderCurrency)).ToString("F2").Replace(",", ".")},
                    {"LMI_RESULT_URL", NotificationUrl},
                    {"LMI_SUCCESS_URL", SuccessUrl},
                    {"LMI_SUCCESS_METHOD", "LINK"},
                    {"LMI_FAIL_URL", FailUrl},
                    {"LMI_FAIL_METHOD", "LINK"}
                }
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            return new PaymentFormHandler
            {
                Url = "https://merchant.webmoney.ru/lmi/payment.asp",
                Page = page,
                InputValues = new Dictionary<string, string>
                {
                    {"LMI_PAYEE_PURSE", Purse},
                    {"LMI_PAYMENT_NO", order.OrderID.ToString()},
                    {"LMI_PAYMENT_DESC", GetOrderDescription(order.Number)},
                    {"LMI_PAYMENT_AMOUNT", (order.Sum*GetCurrencyRate(order.OrderCurrency)).ToString("F2").Replace(",", ".")},
                    {"LMI_RESULT_URL", NotificationUrl},
                    {"LMI_SUCCESS_URL", SuccessUrl},
                    {"LMI_SUCCESS_METHOD", "LINK"},
                    {"LMI_FAIL_URL", FailUrl},
                    {"LMI_FAIL_METHOD", "LINK"}
                }
            }.ProcessRequest(true);
        }

        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;
            
            if (SuccessUrl.Contains(req.RawUrl))
                return LocalizationService.GetResource("Core.Payment.SuccessfullyPaid");
            
            // Параметр LMI_SECRET_KEY передается только по https. Без https проверка неполучится. Раскомментировать если требуется проверка.
            if (context.Request.IsSecureConnection && !CheckData(req))
                return NotificationMessahges.InvalidRequestData;


            var paymentNumber = req["lmi_payment_no"];
            int orderId;
            if (int.TryParse(paymentNumber, out orderId))
            {
                var order = OrderService.GetOrder(orderId);
                if (order != null &&
                    req["LMI_PAYMENT_AMOUNT"] == string.Format("{0:0.00}", order.Sum*GetCurrencyRate(order.OrderCurrency)).Replace(",", "."))
                {
                    if (req["LMI_PREREQUEST"] == "1")
                        return "YES";

                    OrderService.PayOrder(orderId, true);
                    return NotificationMessahges.SuccessfullPayment(order.Number);
                }
            }
            return NotificationMessahges.Fail;


        }

        public bool CheckData(HttpRequest req)
        {
            var fields = new string[]
            {
                "LMI_PAYEE_PURSE",
                "LMI_PAYMENT_AMOUNT",
                "LMI_PAYMENT_NO",
                "LMI_MODE",
                "LMI_SYS_INVS_NO",
                "LMI_SYS_TRANS_NO",
                "LMI_SYS_TRANS_DATE",
                "LMI_SECRET_KEY",
                "LMI_PAYER_PURSE",
                "WMIdLMI_PAYER_WM"
            };

            ;
            return (!fields.Any(val => string.IsNullOrEmpty(req[val]))
                    &&
                    fields.Aggregate<string, StringBuilder, string>(new StringBuilder(),
                        (str, field) =>
                            str.Append(field == "LMI_SECRET_KEY"
                                ? SecretKey
                                : field == "LMI_PAYEE_PURSE" ? Purse : req[field]), Strings.ToString).Md5(true) !=
                    req["LMI_HASH"]);
        }
    }
}