using AdvantShop.Customers;
using AdvantShop.Orders;

namespace AdvantShop.Web.Admin.Handlers.Orders
{
    public class UpdateOrderItems
    {
        private readonly Order _order;

        public UpdateOrderItems(Order order)
        {
            _order = order;
        }

        public bool Execute()
        {
            var changedBy = new OrderChangedBy(CustomerContext.CurrentCustomer);
            var trackChanges = !_order.IsDraft;
            var oldOrderItems = OrderService.GetOrderItems(_order.OrderID);

            OrderService.AddUpdateOrderItems(_order.OrderItems, oldOrderItems, _order.OrderID, changedBy, trackChanges);

            var status = _order.OrderStatus;

            if (status != null && status.Command == OrderStatusCommand.Increment)
            {
                OrderService.IncrementProductsCountAccordingOrder(_order.OrderID);
            }
            else if (status != null && status.Command == OrderStatusCommand.Decrement)
            {
                OrderService.DecrementProductsCountAccordingOrder(_order.OrderID);
            }

            new UpdateOrderTotal(_order).Execute();

            return true;
        }
    }
}
