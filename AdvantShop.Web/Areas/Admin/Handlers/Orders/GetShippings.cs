using System.Linq;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Shipping;
using AdvantShop.Web.Admin.Models.Orders;

namespace AdvantShop.Web.Admin.Handlers.Orders
{
    public class GetShippings
    {
        private readonly int _orderId;
        private readonly string _country;
        private readonly string _city;
        private readonly string _region;
        private readonly string _zip;
        private readonly BaseShippingOption _shipping;
        private readonly bool _getAll;

        public GetShippings(int orderId, string country, string city, string region, string zip, BaseShippingOption shipping = null, bool getAll = true)
        {
            _orderId = orderId;
            _country = country;
            _city = city;
            _region = region;
            _zip = zip;
            _shipping = shipping;
            _getAll = getAll;
        }

        public OrderShippingsModel Execute()
        {
            var order = OrderService.GetOrder(_orderId);
            if (order == null)
                return null;

            var model = new OrderShippingsModel();

            if (order.OrderItems == null || order.OrderItems.Count == 0)
                return model;
            
            var preOrder = new PreOrder
            {
                Items = order.OrderItems.Select(x => new PreOrderItem(x)).ToList(),
                CountryDest = _country ?? "",
                CityDest = _city ?? "",
                RegionDest = _region ?? "",
                ZipDest = _zip ?? "",
                Currency = order.OrderCurrency ?? CurrencyService.CurrentCurrency,
                ShippingOption = _shipping,
                TotalDiscount = order.GetOrderDiscountPrice() + order.BonusCost
            };

            var manager = new ShippingManager(preOrder);
            model.Shippings = manager.GetOptions(_getAll);
            //model.SelectShipping = model.Shippings.FirstOrDefault();

            model.CustomShipping = new BaseShippingOption()
            {
                Name = "",
                Rate = 0,
                IsCustom = true
            };

            return model;
        }
    }
}
