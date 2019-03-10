//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;

namespace AdvantShop.Payment
{
    [PaymentKey("eWAY")]
    public class eWAY : PaymentMethod
    {
        public string CustomerID { get; set; }
        public bool Sandbox { get; set; }
        //public string ReturnUrl { get; set; }
        public float CurrencyValue { get; set; }

        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }

        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.CancelUrl | UrlStatus.ReturnUrl | UrlStatus.NotificationUrl; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.ReturnUrl; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               //{eWAYTemplate.ReturnUrl, ReturnUrl},
                               {eWAYTemplate.Sandbox, Sandbox.ToString()},
                               {eWAYTemplate.CustomerID, CustomerID},
                               {eWAYTemplate.CurrencyValue, CurrencyValue.ToString()}
                           };
            }
            set
            {
                //ReturnUrl = value.ContainsKey(eWAYTemplate.ReturnUrl) ? value[eWAYTemplate.ReturnUrl] : "";
                CustomerID = value.ElementOrDefault(eWAYTemplate.CustomerID);
                Sandbox = value.ElementOrDefault(eWAYTemplate.Sandbox).TryParseBool();
                CurrencyValue = value.ElementOrDefault(eWAYTemplate.CurrencyValue).TryParseFloat(1);
            }
        }
        public override void ProcessForm(Order order)
        {
            new PaymentFormHandler
                {
                    Url = Sandbox ? "https://au.ewaygateway.com/Request/" : "https://www.eway.com.au/gateway/payment.asp",
                    InputValues = new Dictionary<string, string>
                                      {
                                          {"ewayCustomerID", CustomerID},
                                          {"eWAYURL", SuccessUrl},
                                          {"ewayTotalAmount", ((order.Sum / CurrencyValue) * 100) .ToString("F0")},
                                          {"ewayCustomerInvoiceDescription", GetOrderDescription(order.Number)},
                                          {"ewayTrxnNumber", order.Number}
                                      }
                }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            return new PaymentFormHandler
             {
                 Url = Sandbox ? "https://au.ewaygateway.com/Request/" : "https://www.eway.com.au/gateway/payment.asp",
                 Page = page,
                 InputValues = new Dictionary<string, string>
                                      {
                                          {"ewayCustomerID", CustomerID},
                                          {"eWAYURL", SuccessUrl},
                                          {"ewayTotalAmount", ((order.Sum / CurrencyValue) * 100) .ToString("F0")},
                                          {"ewayCustomerInvoiceDescription", GetOrderDescription(order.Number)},
                                          {"ewayTrxnNumber", order.Number}
                                      }
             }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            //TODO some other response processing
            if (Sandbox)
                return NotificationMessahges.TestMode;

            try
            {
                if (context.Request.UrlReferrer == null || context.Request.UrlReferrer.Host != "www.eway.com.au")
                    return NotificationMessahges.InvalidRequestData;
                var form = context.Request.Form;
                var orderNumber = form["ewayTrxnNumber"];
                var status = form["ewayTrxnStatus"].TryParseBool();
                var sum = form["eWAYReturnAmount"].Replace(",", "").Replace("$", "").Replace(".", "").TryParseFloat();
                var responseText = form["eWAYresponseText"];


                var order = OrderService.GetOrderByNumber(orderNumber);
                if (status && order.Sum != sum / 100)
                    return NotificationMessahges.InvalidRequestData;

                //TODO ORDER PAYMENT
                OrderService.PayOrder(order.OrderID, true);
                return NotificationMessahges.SuccessfullPayment(orderNumber);
            }
            catch (Exception ex)
            {
                return NotificationMessahges.LogError(ex);
            }
        }
    }
}