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
    [PaymentKey("Moneybookers")]
    public class Moneybookers : PaymentMethod
    {
        private string Url
        {
            get
            {
                return Sandbox
                           ? "http://www.moneybookers.com/app/test_payment.pl"
                           : "https://www.moneybookers.com/app/payment.pl";
            }
        }
        public string PayToEmai { get; set; }
        public string CurrencyCode { get; set; }
        public float CurrencyValue { get; set; }
        public string SecretWord { get; set; }
        public bool Sandbox { get; set; }
       

        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }
        public override NotificationType NotificationType
        {
            get { return NotificationType.Handler; }
        }

        public static readonly List<string> AvailableCurrs = new List<string>
                                                                 {
                                                                     "EUR","USD","GBP","HKD","SGD","JPY","CAD",
                                                                     "AUD","CHF","DKK","SEK","NOK","ILS","MYR",
                                                                     "NZD","TRY","AED","MAD","QAR","SAR","TWD",
                                                                     "THB","CZK","HUF","SKK","EEK","BGN","PLN",
                                                                     "ISK","INR","LVL","KRW","ZAR","RON","HRK",
                                                                     "LTL","JOD","OMR","RSD","TND"
                                                                 };
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {MoneybookersTemplate.PayToEmai , PayToEmai},
                               {MoneybookersTemplate.CurrencyIso , CurrencyCode},
                               {MoneybookersTemplate.CurrencyValue  , CurrencyValue.ToString()},
                               {MoneybookersTemplate.Sandbox, Sandbox.ToString()},
                               {MoneybookersTemplate.SecretWord, SecretWord}
                           };
            }
            set
            {
                PayToEmai = value.ElementOrDefault(MoneybookersTemplate.PayToEmai);
                CurrencyCode = value.ElementOrDefault(MoneybookersTemplate.CurrencyIso);
                SecretWord = value.ElementOrDefault(MoneybookersTemplate.SecretWord);
                CurrencyValue =value.ElementOrDefault(MoneybookersTemplate.CurrencyValue).TryParseFloat(1);
                Sandbox = value.ElementOrDefault(MoneybookersTemplate.Sandbox).TryParseBool();
            }
        }

        public override void ProcessForm(Order order)
        {
            var sum = (order.Sum / CurrencyValue).ToString("F2").Replace(",", ".");
            var shopName = Configuration.SettingsMain.ShopName;
            if (shopName.Length > 30)
                shopName = shopName.Substring(0, 26) + "...";
            new PaymentFormHandler
            {
                Url = Url,
                InputValues = new Dictionary<string, string>
                                      {
                                          {"pay_to_email", PayToEmai },
                                          {"recipient_description", shopName },
                                          {"transaction_id",order.OrderID.ToString()},
                                          {"return_url", SuccessUrl},
                                          {"cancel_url", CancelUrl},
                                          {"status_url", NotificationUrl},
                                          {"language","en_US" },
                                          {"amount",sum },
                                          {"currency", CurrencyCode },
                                          {"detail1_description", "Order ID:"},
                                          {"detail1_text", order.OrderID.ToString()},
                                          
                                      }
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            var sum = (order.Sum / CurrencyValue).ToString("F2").Replace(",", ".");
            var shopName = Configuration.SettingsMain.ShopName;
            if (shopName.Length > 30)
                shopName = shopName.Substring(0, 26) + "...";
            return new PaymentFormHandler
              {
                  Url = Url,
                  Page = page,
                  InputValues = new Dictionary<string, string>
                                      {
                                          {"pay_to_email", PayToEmai },
                                          {"recipient_description", shopName },
                                          {"transaction_id",order.OrderID.ToString()},
                                          {"return_url", SuccessUrl},
                                          {"cancel_url", CancelUrl},
                                          {"status_url", NotificationUrl},
                                          {"language","en_US" },
                                          {"amount",sum },
                                          {"currency", CurrencyCode },
                                          {"detail1_description", "Order ID:"},
                                          {"detail1_text", order.OrderID.ToString()},
                                          
                                      }
              }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;
            if (!CheckData(req))
                return NotificationMessahges.InvalidRequestData;
            var paymentNumber = req["transaction_id"];
            int orderID = 0;
            if (int.TryParse(paymentNumber, out orderID) && OrderService.GetOrder(orderID) != null && (req["status"] == "2"))
            {
                OrderService.PayOrder(orderID, true);
                return NotificationMessahges.SuccessfullPayment(paymentNumber);
            }
            return NotificationMessahges.Fail;
        }

        private bool CheckData(HttpRequest req)
        {
            return !new[]
                        {
                            "pay_to_email",
                            "pay_from_email",
                            "merchant_id",
                            "transaction_id",
                            "mb_transaction_id",
                            "mb_amount",
                            "mb_currency",
                            "status",
                            "md5sig",
                            "amount",
                            "currency"
                        }.Any(field => string.IsNullOrEmpty(req[field]))
                               &&
                               (req["merchant_id"] +
                               req["transaction_id"] +
                                SecretWord.Md5(true, Encoding.ASCII) +
                                req["mb_amount"] +
                                req["mb_currency"] +
                                req["status"]).Md5() == req["md5sig"];
        }

    }
}