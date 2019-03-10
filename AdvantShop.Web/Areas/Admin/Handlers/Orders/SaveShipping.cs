using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Shipping;

namespace AdvantShop.Web.Admin.Handlers.Orders
{
    public class SaveShipping
    {
        private readonly Order _order;
        private readonly string _country;
        private readonly string _city;
        private readonly string _region;
        private readonly BaseShippingOption _shipping;

        public SaveShipping(Order order, string country, string city, string region, BaseShippingOption shipping)
        {
            _order = order;
            _country = country;
            _city = city;
            _region = region;
            _shipping = shipping;
        }

        public void Execute()
        {
            var trackChanges = !_order.IsDraft;
            var changedBy = new OrderChangedBy(CustomerContext.CurrentCustomer);

            var customer = _order.OrderCustomer;

            if (customer != null && (customer.Country != _country || customer.Region != _region || customer.City != _city))
            {
                customer.Country = _country ?? "";
                customer.Region = _region ?? "";
                customer.City = _city ?? "";

                OrderService.UpdateOrderCustomer(customer, changedBy, trackChanges);
            }
            
            _order.ShippingMethodId = _shipping.MethodId;
            _order.ArchivedShippingName = _shipping.Name ?? _shipping.NameRate;
            _order.ShippingCost = _shipping.Rate;
            _order.ShippingTaxType = _shipping.TaxType;
            var orderPickPoint = _shipping.GetOrderPickPoint();

            if (orderPickPoint != null)
                OrderService.AddUpdateOrderPickPoint(_order.OrderID, orderPickPoint);
            else
                OrderService.DeleteOrderPickPoint(_order.OrderID);

            new UpdateOrderTotal(_order).Execute();
        }
    }
}
