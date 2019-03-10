//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;

namespace AdvantShop.Payment
{
    [PaymentKey("TwoCheckout")]
    public class TwoCheckout : PaymentMethod
    {
        public string Sid { get; set; }
        public bool Sandbox { get; set; }
        public string SecretWord { get; set; }
        //public float CurrencyValue { get; set; }
       
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
                               {TwoCheckoutTemplate.Sid, Sid},
                               {TwoCheckoutTemplate.Sandbox, Sandbox.ToString()},
                               {TwoCheckoutTemplate.SecretWord, SecretWord},
                               //{TwoCheckoutTemplate.ReturnUrl, ReturnUrl},
                               //{TwoCheckoutTemplate.CurrencyValue, CurrencyValue.ToString()}
                           };
            }
            set
            {
                Sid = value.ElementOrDefault(TwoCheckoutTemplate.Sid);
                SecretWord = value.ElementOrDefault(TwoCheckoutTemplate.SecretWord);
                //ReturnUrl = value.ContainsKey(TwoCheckoutTemplate.ReturnUrl) ? value[TwoCheckoutTemplate.ReturnUrl] : "";
                Sandbox = value.ElementOrDefault(TwoCheckoutTemplate.Sandbox).TryParseBool();
                //CurrencyValue = value.ElementOrDefault(TwoCheckoutTemplate.CurrencyValue).TryParseFloat(true) ?? 1;
            }
        }

        public override void ProcessForm(Order order)
        {
            var dict = new Dictionary<string, string>
                {
                    {"sid", Sid},
                    {"mode", "2CO"},
                    //{"cart_order_id", order.Number},
                    //{"merchant_order_id", order.Number},
                    //{"total", (order.Sum/CurrencyValue).ToString("F2").Replace(",", ".")},
                    {"demo", Sandbox ? "Y" : "N"},
                    //{"return_url", SuccessUrl},
                    //{"x_receipt_link_url", SuccessUrl},
                    //{"id_type", "1"},
                    //{"lang", "en"}
                };

            for( int i=0; i< order.OrderItems.Count; i++)
            {
                dict.Add("li_" + i + "_type", "product");
                dict.Add("li_" + i + "_name", order.OrderItems[i].Name.Replace(">", "").Replace("<", ""));
                dict.Add("li_" + i + "_quantity", order.OrderItems[i].Amount.ToString());
                dict.Add("li_" + i + "_price", order.OrderItems[i].Price.ToString("F2"));
                dict.Add("li_" + i + "_tangible", "Y");
            }

            new PaymentFormHandler
                {
                    Url = "https://www.2checkout.com/checkout/purchase",
                    InputValues = dict
                }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            var dict = new Dictionary<string, string>
                {
                    {"sid", Sid},
                    {"mode", "2CO"},
                    //{"cart_order_id", order.Number},
                    //{"merchant_order_id", order.Number},
                    //{"total", (order.Sum/CurrencyValue).ToString("F2").Replace(",", ".")},
                    {"demo", Sandbox ? "Y" : "N"},
                    //{"return_url", SuccessUrl},
                    //{"x_receipt_link_url", SuccessUrl},
                    //{"id_type", "1"},
                    //{"lang", "en"}
                };

            for (int i = 0; i < order.OrderItems.Count; i++)
            {
                dict.Add("li_" + i + "_type", "product");
                dict.Add("li_" + i + "_name", order.OrderItems[i].Name.Replace(">", "").Replace("<", ""));
                dict.Add("li_" + i + "_quantity", order.OrderItems[i].Amount.ToString());
                dict.Add("li_" + i + "_price", order.OrderItems[i].Price.ToString("F2"));
                dict.Add("li_" + i + "_tangible", "Y");
            }

            return new PaymentFormHandler
             {
                 Url = "https://www.2checkout.com/checkout/purchase",
                 Page = page,
                 InputValues = dict
             }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            //TODO handler notification process
            if (Sandbox)
                return NotificationMessahges.TestMode;
            if (string.IsNullOrEmpty(SecretWord))
                return "Secret word must be specified";
            try
            {
                var form = context.Request.Form;
                if (!ValidateRequest(form))
                    return NotificationMessahges.InvalidRequestData;

                var order = OrderService.GetOrderByNumber(form["vendor_order_id"]);
                if (order == null) //|| (order.Sum / CurrencyValue).ToString("F2").Replace(",", ".") != form["invoice_usd_amount"])
                    return NotificationMessahges.InvalidRequestData;

                OrderService.PayOrder(order.OrderID, true);
                return "";
            }
            catch (Exception ex)
            {
                return NotificationMessahges.LogError(ex);
            }

        }

        private bool ValidateRequest(NameValueCollection form)
        {
            if (string.IsNullOrEmpty(form["message_type"]))
                return false;
            if (form["message_type "].Trim() != "ORDER_CREATED")
                return false;
            if (string.IsNullOrEmpty(form["sale_id"]))
                return false;
            if (string.IsNullOrEmpty(form["vendor_id"]))
                return false;
            if (string.IsNullOrEmpty(form["invoice_id"]))
                return false;
            if (string.IsNullOrEmpty(form["vendor_order_id"]))
                return false;
            if (string.IsNullOrEmpty(form["invoice_usd_amount"]))
                return false;
            if (string.IsNullOrEmpty(form["md5_hash"]))
                return false;
            if (form["md5_hash"].ToLower() != (form["sale_id"] + form["vendor_id"] + form["invoice_id"] + SecretWord).Md5())
                return false;
            return true;
        }
    }
}