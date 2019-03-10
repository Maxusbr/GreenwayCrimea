using System.Linq;
using AdvantShop.Core.Modules;
using AdvantShop.Mails;
using AdvantShop.Orders;

namespace AdvantShop.Web.Admin.Handlers.Orders
{
    public class NotifyStatusChanged
    {
        private readonly int _orderId;

        public NotifyStatusChanged(int orderId)
        {
            _orderId = orderId;
        }

        public bool Exectute()
        {
            var order = OrderService.GetOrder(_orderId);

            if (order == null || order.OrderStatus.Hidden)
                return false;

            var total = order.OrderItems != null
                ? order.OrderItems.Sum(item => item.Price*item.Amount)
                : order.OrderCertificates != null ? order.OrderCertificates.Sum(item => item.Sum) : 0;

            var orderItemsHtml =
                OrderService.GenerateOrderItemsHtml(order.OrderItems, order.OrderCurrency,
                    total, //order.Sum,
                    order.OrderDiscount, order.OrderDiscountValue,
                    order.Coupon, order.Certificate,
                    order.TotalDiscount,
                    order.ShippingCost, order.PaymentCost,
                    order.TaxCost,
                    order.BonusCost,
                    0);

            var mail = new OrderStatusMailTemplate(order.OrderStatus.StatusName, order.StatusComment.Replace("\r\n", "<br />"), order.Number, orderItemsHtml, string.IsNullOrEmpty(order.TrackNumber) ? string.Empty : order.TrackNumber);
            mail.BuildMail();

            SendMail.SendMailNow(order.OrderCustomer.CustomerID, order.OrderCustomer.Email, mail.Subject, mail.Body, true);

            ModulesExecuter.SendNotificationsOnOrderChangeStatus(order);

            return true;
        }
    }
}
