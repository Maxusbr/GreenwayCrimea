using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Repository.Currencies;
using AdvantShop.Shipping;

namespace AdvantShop.Web.Admin.Handlers.Orders
{
    public class GetPayments
    {
        private readonly int _orderId;
        private readonly string _country;
        private readonly string _city;
        private readonly string _region;

        public GetPayments(int orderId, string country, string city, string region)
        {
            _orderId = orderId;
            _country = country;
            _city = city;
            _region = region;
        }

        public List<BasePaymentOption> Execute()
        {
            var order = OrderService.GetOrder(_orderId);
            if (order == null || order.OrderItems == null || order.OrderItems.Count == 0)
                return null;

            var shippingMethod = order.ShippingMethodId != 0
                                    ? ShippingMethodService.GetShippingMethod(order.ShippingMethodId)
                                    : null;
            var shipping =
                shippingMethod != null
                    ? new BaseShippingOption(shippingMethod) { Rate = order.ShippingCost }
                    : new BaseShippingOption() {Name = order.ArchivedShippingName, Rate = order.ShippingCost};
            
            var preOrder = new PreOrder
            {
                Items = order.OrderItems.Select(x => new PreOrderItem(x)).ToList(),
                CountryDest = _country ?? "",
                CityDest = _city ?? "",
                RegionDest = _region ?? "",
                Currency = order.OrderCurrency ?? CurrencyService.CurrentCurrency,
                ShippingOption = shipping,
                TotalDiscount = order.GetOrderDiscountPrice()
            };

            var manager = new PaymentManager(preOrder, null);
            return manager.GetOptions();
        }
    }
}
