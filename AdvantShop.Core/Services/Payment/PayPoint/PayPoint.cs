//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Payment
{
    [PaymentKey("PayPoint")]
    public class PayPoint : PaymentMethod
    {
        private static string Url
        {
            get
            {
                return "https://www.secpay.com/java-bin/ValCard";
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
                                                                     "AUD",
                                                                     "CAD",
                                                                     "EUR",
                                                                     "GBP",
                                                                     "HKD",
                                                                     "JPY",
                                                                     "USD"
                                                                 };
        public string Merchant { get; set; }
        public string CurrencyCode { get; set; }
        public float CurrencyValue { get; set; }
        public string Password { get; set; }
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {PayPointTemplate.Merchant, Merchant},
                               {PayPointTemplate.Password, Password},
                               {PayPointTemplate.CurrencyCode, CurrencyCode},
                               {PayPointTemplate.CurrencyValue, CurrencyValue.ToString()}
                           };
            }
            set
            {
                Merchant = value.ElementOrDefault(PayPointTemplate.Merchant);
                Password = value.ElementOrDefault(PayPointTemplate.Password);
                CurrencyCode = value.ElementOrDefault(PayPointTemplate.CurrencyCode, "USD");
                float decVal = 0;
                if (float.TryParse(value.ElementOrDefault(PayPointTemplate.CurrencyValue), out decVal))
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
            var sum = string.Format("{0:0.00}", order.Sum / CurrencyValue);
            new PaymentFormHandler
            {
                Url = Url,
                InputValues = new Dictionary<string, string>
                                {
                                    {"merchant", Merchant},
                                    {"trans_id", paymentNo},
                                    {"amount", sum},
                                    {"callback", SuccessUrl},
                                    {"currency", CurrencyCode},
                                    {"digest", (paymentNo + sum + Password).Md5()}
                                }
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            var paymentNo = order.OrderID.ToString();
            var sum = string.Format("{0:0.00}", order.Sum / CurrencyValue);
            return new PaymentFormHandler
             {
                 Url = Url,
                 Page = page,
                 InputValues = new Dictionary<string, string>
                                {
                                    {"merchant", Merchant},
                                    {"trans_id", paymentNo},
                                    {"amount", sum},
                                    {"callback", SuccessUrl},
                                    {"currency", CurrencyCode},
                                    {"digest", (paymentNo + sum + Password).Md5()}
                                }
             }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;
            if (!CheckData(req))
                return NotificationMessahges.InvalidRequestData;
            var paymentNumber = req["trans_id"];
            int orderID = 0;
            if (int.TryParse(paymentNumber, out orderID) &&
                OrderService.GetOrder(orderID) != null)
            {
                var order = OrderService.GetOrder(orderID);
                if (order != null && req["amount"] == string.Format("{0:0.00}", order.Sum / CurrencyValue))
                {
                    OrderService.PayOrder(orderID, true);
                    return NotificationMessahges.SuccessfullPayment(order.Number);
                }

            }
            return NotificationMessahges.Fail;
        }

        private static bool CheckData(HttpRequest req)
        {
            Func<string, string> getParams = query => QueryHelper.ChangeQueryParam(req.Url.PathAndQuery, "hash", null);
            Func<string> postParams = () =>
                                      req.Form.Cast<KeyValuePair<string, string>>().Where(
                                          item => req["md_flds"].Contains(item.Key))
                                          .Aggregate(new StringBuilder(),
                                                     (curr, item) =>
                                                     curr.AppendFormat("{0}={1}&", item.Key, item.Value),
                                                     curr => curr.ToString().TrimEnd('&'));
            return !new[]
                        {
                            "valid",
                            "trans_id",
                            "hash",
                            "amount"
                        }.Any(param => string.IsNullOrEmpty(req[param]))
                   && req["valid"].ToLower() == "true"
                   && req["code"] == "A"
                   && (req.HttpMethod == "GET"
                           ? getParams(req.Url.PathAndQuery).Md5() == req["hash"]
                           : !string.IsNullOrEmpty(req["md_flds"]) && postParams().Md5() == req["hash"]);
        }
    }
}