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
using AdvantShop.Orders;

namespace AdvantShop.Payment
{
    public enum PayOnlineType
    {
        Select,
        WebMoney,
        QIWI,
        YandexMoney,
        CreditCard_EN,
        CreditCard_RU

    }

    [PaymentKey("PayOnline")]
    public class PayOnline : PaymentMethod
    {
        private string Url
        {
            get
            {
                switch (PayType)
                {
                    default:
                        //‘орма выбора платежного инструмента:
                        return "https://secure.payonlinesystem.com/ru/payment/select/";

                    case PayOnlineType.QIWI:
                        //‘орма оплаты через QIWI:
                        return "https://secure.payonlinesystem.com/ru/payment/select/qiwi/";

                    case PayOnlineType.WebMoney:
                        //‘орма оплаты через WebMoney:
                        return "https://secure.payonlinesystem.com/ru/payment/select/paymaster/";

                    case PayOnlineType.YandexMoney:
                        return "https://secure.payonlinesystem.com/ru/payment/select/yandexmoney/";

                    case PayOnlineType.CreditCard_EN:
                        //‘орма оплаты с банковской карты Ц английский интерфейс
                        return "https://secure.payonlinesystem.com/en/payment/";

                    case PayOnlineType.CreditCard_RU:
                        //‘орма оплаты с банковской карты  Ц русский интерфейс
                        return "https://secure.payonlinesystem.com/ru/payment/";

                    
                }
            }
        }

        public string MerchantId { get; set; }
        public string Currency { get; set; }
        public string SecretKey { get; set; }
        public float CurrencyValue { get; set; }
        public PayOnlineType PayType { get; set; }

       
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
            get { return UrlStatus.NotificationUrl; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {PayOnlineTemplate.MerchantId, MerchantId},
                               {PayOnlineTemplate.Currency, Currency},
                               {PayOnlineTemplate.SecretKey, SecretKey},
                               {PayOnlineTemplate.CurrencyValue, CurrencyValue.ToString()},
                               {PayOnlineTemplate.PayType, ((int)PayType).ToString()}
                           };
            }
            set
            {
                MerchantId = value.ElementOrDefault(PayOnlineTemplate.MerchantId);
                Currency = value.ElementOrDefault(PayOnlineTemplate.Currency);
                SecretKey = value.ElementOrDefault(PayOnlineTemplate.SecretKey);
                float decVal = 0;
                CurrencyValue = float.TryParse(value.ElementOrDefault(PayOnlineTemplate.CurrencyValue), out decVal) ? decVal : 1;
                int intval;
                PayType = int.TryParse(value.ElementOrDefault(PayOnlineTemplate.PayType), out intval) ? (PayOnlineType)intval : PayOnlineType.Select;
            }
        }

        public override void ProcessForm(Order order)
        {
            var paymentNo = order.OrderID.ToString();
            var sum = (order.Sum / CurrencyValue).ToString("F2").Replace(",", ".");

            new PaymentFormHandler
            {
                Url = Url,
                InputValues = new Dictionary<string, string>
                                      {
                                          {"MerchantId", MerchantId},
                                          {"OrderId", paymentNo},
                                          {"Amount",sum},
                                          {"Currency",Currency},
                                          {"SecurityKey",GetMd5("MerchantId="+MerchantId+"&OrderId="+paymentNo+"&Amount="+sum+"&Currency="+Currency+"&PrivateSecurityKey="+SecretKey) },
                                          {"ReturnUrl", HttpUtility.UrlDecode(SuccessUrl)},
                                          {"FailUrl", HttpUtility.UrlDecode(FailUrl)}
                                         }
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            var paymentNo = order.OrderID.ToString();
            var sum = (order.Sum / CurrencyValue).ToString("F2").Replace(",", ".");

            return new PaymentFormHandler
                {
                    Url = Url,
                    Page = page,
                    InputValues = new Dictionary<string, string>
                                      {
                                          {"MerchantId", MerchantId},
                                          {"OrderId", paymentNo},
                                          {"Amount",sum},
                                          {"Currency",Currency},
                                          {"SecurityKey",GetMd5("MerchantId="+MerchantId+"&OrderId="+paymentNo+"&Amount="+sum+"&Currency="+Currency+"&PrivateSecurityKey="+SecretKey) },
                                          {"ReturnUrl", HttpUtility.UrlDecode(SuccessUrl)},
                                          {"FailUrl", HttpUtility.UrlDecode(FailUrl)}
                                         }
                }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;
            if (!CheckData(req))
                return NotificationMessahges.InvalidRequestData;
            var paymentNumber = req["OrderId"];
            int orderID = 0;
            if (int.TryParse(paymentNumber, out orderID) && OrderService.GetOrder(orderID) != null)
            {
                OrderService.PayOrder(orderID, true);
                return NotificationMessahges.SuccessfullPayment(paymentNumber);
            }
            return NotificationMessahges.Fail;
        }

        private static string GetMd5(string str)
        {
            return str.Md5(false, Encoding.UTF8);
        }

        private static bool CheckData(HttpRequest req)
        {
            return !new[]
                        {
                            "DateTime",
                            "TransactionID",
                            "OrderId",
                            "Amount",
                            "Currency",
                            "PrivateSecurityKey",
                        }.Any(field => string.IsNullOrEmpty(req[field]))
                   &&
                   ("DateTime=" + req["DateTime"] + "&TransactionID=" + req["TransactionID"] +
                    "&OrderId=" + req["OrderId"] + "&Amount=" + req["Amount"] +
                    "&Currency=" + req["Currency"] + "&PrivateSecurityKey=" + req["PrivateSecurityKey"]
                   )
                       .Md5(true) == req["SecurityKey"];
        }
    }
}