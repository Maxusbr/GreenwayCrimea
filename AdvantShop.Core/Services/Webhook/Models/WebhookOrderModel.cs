using AdvantShop.Orders;

namespace AdvantShop.Core.Services.Webhook.Models
{
    public class WebhookOrderModel : WebhookModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public float Sum { get; set; }

        public static explicit operator WebhookOrderModel(Order order)
        {
            return new WebhookOrderModel
            {
                Id = order.OrderID,
                Number = order.Number,
                Sum = order.Sum
            };
        }
    }
}
