//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Payment
{
    /// <summary>
    /// Summary description for PSIGate
    /// </summary>
    [PaymentKey("PSIGate")]
    public class PSIGate : PaymentMethod
    {
        private string Url
        {
            get
            {
                return Sandbox
                           ? "https://devcheckout.psigate.com/HTMLPost/HTMLMessenger"
                           : "https://checkout.psigate.com/HTMLPost/HTMLMessenger";
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

        public string StoreKey { get; set; }
        public bool Sandbox { get; set; }
        public float CurrencyValue { get; set; }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {PSIGateTemplate.StoreKey, StoreKey},
                               {PSIGateTemplate.Sandbox, Sandbox.ToString()},
                               {PSIGateTemplate.CurrencyValue, CurrencyValue.ToString()}
                           };
            }
            set
            {
                StoreKey = value.ElementOrDefault(PSIGateTemplate.StoreKey);

                float decVal = 0;
                if (float.TryParse(value.ElementOrDefault(PSIGateTemplate.CurrencyValue), out decVal))
                {
                    CurrencyValue = decVal;
                }
                else
                {
                    var usd = CurrencyService.Currency("USD");
                    CurrencyValue = usd != null ? usd.Rate : 1;
                }

                bool boolval;
                if (bool.TryParse(value.ElementOrDefault(PSIGateTemplate.Sandbox), out boolval))
                    Sandbox = boolval;
            }

        }
        public override void ProcessForm(Order order)
        {
            var paymentNo = order.OrderID.ToString();
            var sum = (order.Sum / CurrencyValue).ToString("F2").Replace(",", ".");
            var inputVals = new Dictionary<string, string>
                                {
                                    {"StoreKey", StoreKey},
                                    {"ThanksURL", SuccessUrl},
                                    {"NoThanksURL", CancelUrl},
                                    {"ResponseFormat", "HTML1"},
                                    {"OrderID", paymentNo},
                                    {"FullTotal", sum}
                                };
            if (Sandbox)
                inputVals.Add("TestResult", "R"); // Random test results
            new PaymentFormHandler
            {
                Url = Url,
                InputValues = inputVals
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            var paymentNo = order.OrderID.ToString();
            var sum = (order.Sum / CurrencyValue).ToString("F2").Replace(",", ".");
            var inputVals = new Dictionary<string, string>
                                {
                                    {"StoreKey", StoreKey},
                                    {"ThanksURL", SuccessUrl},
                                    {"NoThanksURL", CancelUrl},
                                    {"ResponseFormat", "HTML1"},
                                    {"OrderID", paymentNo},
                                    {"FullTotal", sum}
                                };
            if (Sandbox)
                inputVals.Add("TestResult", "R"); // Random test results
            return new PaymentFormHandler
             {
                 Url = Url,
                 InputValues = inputVals,
                 Page = page
             }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;
            if (!CheckData(req))
                return NotificationMessahges.InvalidRequestData;
            var paymentNumber = req["OrderID"];
            int orderID = 0;
            if (int.TryParse(paymentNumber, out orderID) &&
                OrderService.GetOrder(orderID) != null &&
                (req["Approved"].ToUpper() == "APPROVED" && req["ReturnCode"][0] == 'Y'))
            {
                var order = OrderService.GetOrder(orderID);
                if (order != null && req["FullTotal"] == (order.Sum / CurrencyValue).ToString("F2").Replace(",", "."))
                {
                    OrderService.PayOrder(orderID, true);
                    return NotificationMessahges.SuccessfullPayment(order.Number);
                }

            }
            return NotificationMessahges.Fail;
        }

        private bool CheckData(HttpRequest req)
        {
            return
            !new[]
                 {
                     "OrderID",
                     "FullTotal",
                     "Approved",
                     "ReturnCode"
                 }.Any(param => string.IsNullOrEmpty(req[param]));
        }
    }

}