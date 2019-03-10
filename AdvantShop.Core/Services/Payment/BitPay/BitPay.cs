using System;
using System.Collections.Generic;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;
using System.Text;
using AdvantShop.Diagnostics;

namespace AdvantShop.Payment
{
    [PaymentKey("BitPay")]
    public class BitPay: PaymentMethod
    {
        /*
         * Documantation - https://bitpay.com/downloads/bitpayApi.pdf
         */
        public string ApiKey { get; set; }
        public string Currency { get; set; }


        public override ProcessType ProcessType
        {
            get { return ProcessType.Javascript; } //.FormPost; }
        }

        public override NotificationType NotificationType
        {
            get { return NotificationType.ReturnUrl | NotificationType.Handler; }
        }
        public override UrlStatus ShowUrls
        {
            get { return UrlStatus.CancelUrl | UrlStatus.ReturnUrl | UrlStatus.NotificationUrl; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                           {
                               {BitPayTemplate.ApiKey, ApiKey},
                               {BitPayTemplate.Currency, Currency}
                           };
            }
            set
            {
                ApiKey = value.ElementOrDefault(BitPayTemplate.ApiKey);
                Currency = value.ElementOrDefault(BitPayTemplate.Currency);
            }
        }

        private string GetPosData(IList<OrderItem> items)
        {
            string res = "";

            for (int i = 0; i < items.Count; i++)
            {
                res += string.Format("\"artNo_{0}\": \"{1}\"", (i + 1), items[i].ArtNo);
            }

            return "'{ " + res + " }'";
        }

        public override void ProcessForm(Order order)
        {
            new PaymentFormHandler
            {
                Url = "https://bitpay.com/api/invoice/",
                InputValues = new Dictionary<string, string>
                                      {
                                          //{"posData", GetPosData(order.OrderItems)},
                                          {"price", order.Sum.ToString("F2").Replace(",", ".")},
                                          {"currency", Currency},
                                          {"orderID", order.OrderID.ToString()},
                                          {"itemDesc", GetOrderDescription(order.Number)},
                                          {"physical", "true"}
                                      }
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            return new PaymentFormHandler
            {
                Url = "https://bitpay.com/api/invoice/",
                InputValues = new Dictionary<string, string>
                                      {
                                          //{"posData", GetPosData(order.OrderItems)},
                                          {"price", order.Sum.ToString("F2").Replace(",", ".")},
                                          {"currency", Currency},
                                          {"orderID", order.OrderID.ToString()},
                                          {"itemDesc", GetOrderDescription(order.Number)},
                                          {"physical", "true"}
                                      }
            }.ProcessRequest();
        }



        public override string ProcessJavascript(Order order)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<script type=\"text/javascript\"> ");
            sb.AppendLine("function bitpay() { ");
            sb.AppendLine("$.ajax({ dataType: \"json\", type: \"POST\",  url: \"httphandlers/orderconfirmation/bitpayservice.ashx\",\n");
            sb.AppendFormat("  data: {{ orderId: \"{0}\" }}, ",  order.OrderID);
            sb.AppendLine("  success: function (data) { if (data != null && data.error == \"\") { window.location = data.url; } },");
            sb.AppendLine("});");

            sb.AppendLine("} ");
            sb.AppendLine("</script>");

            return sb.ToString();
        }

        public override string ProcessJavascriptButton(Order order)
        {
            return "bitpay();";
        }



        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;

            try
            {
                Debug.Log.Error(string.Format("{0}{1}{2}{3}", req["id"], req["url"], req["posData"], req["status"]));
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return NotificationMessahges.InvalidRequestData;
        }        
    }
}