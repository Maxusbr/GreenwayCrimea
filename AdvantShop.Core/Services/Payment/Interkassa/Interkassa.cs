using System.Collections.Generic;
using System.Web;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;

namespace AdvantShop.Payment
{
    [PaymentKey("Interkassa")]
    public class Interkassa : PaymentMethod
    {
        public string ShopId { get; set; }
        
        public override ProcessType ProcessType
        {
            get { return ProcessType.FormPost; }
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
                               {InterkassaTemplate.ShopId, ShopId}
                           };
            }
            set
            {
                ShopId = value.ElementOrDefault(InterkassaTemplate.ShopId);
            }
        }


        public override void ProcessForm(Order order)
        {
            new PaymentFormHandler
            {
                Url = "http://www.interkassa.com/lib/payment.php",
                InputValues = new Dictionary<string, string>
                {
                    {"ik_shop_id", ShopId},
                    {
                        "ik_payment_amount",
                        (order.Sum*GetCurrencyRate(order.OrderCurrency)).ToString("F2").Replace(",", ".")
                    },
                    {"ik_payment_id", order.OrderID.ToString()},
                    {"ik_payment_desc", GetOrderDescription(order.Number)},
                    {"ik_paysystem_alias", ""}
                }
            }.Post();
        }

        public override string ProcessFormString(Order order, PageWithPaymentButton page)
        {
            return new PaymentFormHandler
            {
                Url = "http://www.interkassa.com/lib/payment.php",
                InputValues = new Dictionary<string, string>
                {
                    {"ik_shop_id", ShopId},
                    {
                        "ik_payment_amount",
                        (order.Sum*GetCurrencyRate(order.OrderCurrency)).ToString("F2").Replace(",", ".")
                    },
                    {"ik_payment_id", order.OrderID.ToString()},
                    {"ik_payment_desc", GetOrderDescription(order.Number)},
                    {"ik_paysystem_alias", ""}
                }
            }.ProcessRequest();
        }

        public override string ProcessResponse(HttpContext context)
        {
            var req = context.Request;
            var orderID = 0;
            if (CheckFields(context) && req["ik_payment_state"] == "success" && int.TryParse(req["ik_payment_id"], out orderID))
            {
                Order order = OrderService.GetOrder(orderID);
                if (order != null)
                {
                    OrderService.PayOrder(orderID, true);
                    return NotificationMessahges.SuccessfullPayment(orderID.ToString());
                }
            }
            return NotificationMessahges.InvalidRequestData;
        }

        private bool CheckFields(HttpContext context)
        {
            // check summ
            return true;
        }
    }
}