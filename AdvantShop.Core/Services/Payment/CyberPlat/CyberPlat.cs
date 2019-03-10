//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Orders;

namespace AdvantShop.Payment
{
    [PaymentKey("CyberPlat")]
    public class CyberPlat : PaymentMethod
    {
        public float CurrencyValue { get; set; }
        
        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
        }
        public override NotificationType NotificationType
        {
            get { return NotificationType.ReturnUrl; }
        }

        public override void ProcessForm(Order order)
        {
            var sum = (order.Sum / CurrencyValue).ToString("F2").Replace(",", ".");
            string message = "Orderid=" + order.OrderID + "&Amount=" + sum + "&Currency=" + "&PaymentDetails=" + "&Email=" + "&FirstName=" +
                             "&LastName=" + "&MiddleName=none&Phone=" + "&Address=" + "&Language=" + "&return_url=";

            new PaymentFormHandler
             {
                 Url = "https://card.cyberplat.ru/cgi-bin/getform.cgi",
                 InputValues = new Dictionary<string, string>
                                      {
                                          {"version", "2.0"},
                                          {"message", message}
                                      }
             }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            var sum = (order.Sum / CurrencyValue).ToString("F2").Replace(",", ".");
            string message = "Orderid=" + order.OrderID + "&Amount=" + sum + "&Currency=" + "&PaymentDetails=" + "&Email=" + "&FirstName=" +
                             "&LastName=" + "&MiddleName=none&Phone=" + "&Address=" + "&Language=" + "&return_url=";

            return new PaymentFormHandler
             {
                 Url = "https://card.cyberplat.ru/cgi-bin/getform.cgi",
                 Page = page,
                 InputValues = new Dictionary<string, string>
                                      {
                                          {"version", "2.0"},
                                          {"message", message}
                                      }
             }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;
            if (!CheckData(req))
                return NotificationMessahges.InvalidRequestData;
            var paymentNumber = req["pg_order_id"];
            int orderID = 0;
            if (int.TryParse(paymentNumber, out orderID) && OrderService.GetOrder(orderID) != null)
            {
                OrderService.PayOrder(orderID, true);
                return NotificationMessahges.SuccessfullPayment(paymentNumber);
            }
            return NotificationMessahges.Fail;
        }


        private static bool CheckData(HttpRequest req)
        {
            return true;
        }
    }
}