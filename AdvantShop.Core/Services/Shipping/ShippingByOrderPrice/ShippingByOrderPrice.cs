using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace AdvantShop.Shipping.ShippingByOrderPrice
{
    [ShippingKey("ShippingByOrderPrice")]
    public class ShippingByOrderPrice : BaseShipping
    {
        private readonly List<ShippingPriceRange> _priceRanges;
        private readonly bool _dependsOnCartPrice;

        public ShippingByOrderPrice(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {
            _priceRanges = new List<ShippingPriceRange>();
            var ranges = _method.Params.ElementOrDefault(ShippingByOrderPriceTemplate.PriceRanges);
            if (ranges.IsNullOrEmpty())
                return;
            _priceRanges = GetRange(ranges);
            _priceRanges = _priceRanges.OrderBy(item => item.OrderPrice).ToList();
            _dependsOnCartPrice = _method.Params.ElementOrDefault(ShippingByOrderPriceTemplate.DependsOnCartPrice).TryParseBool();
        }

        private List<ShippingPriceRange> GetRange(string strRange)
        {
            var priceRanges = new List<ShippingPriceRange>();

            foreach (var item in strRange.Split(';'))
            {
                if (item.Split('=').Length == 2)
                {
                    priceRanges.Add(new ShippingPriceRange()
                    {
                        OrderPrice = item.Split('=')[0].TryParseFloat(), // / _preOrder.Currency.Rate,
                        ShippingPrice = item.Split('=')[1].TryParseFloat() // / _preOrder.Currency.Rate
                    });
                }
            }
            return priceRanges;
        }

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            var totalPrice = _preOrder.Items.Sum(item=> item.Price * item.Amount);
            var rate = GetRate(totalPrice, totalPrice - _preOrder.TotalDiscount);

            if (rate != -1)
            {
                var option = new BaseShippingOption(_method)
                {
                    Rate = rate,
                    DeliveryTime = _method.Params.ElementOrDefault(ShippingByOrderPriceTemplate.DeliveryTime)
                };

                return new List<BaseShippingOption> { option };
            }
            else
                return null;
        }

        private float GetRate(float cartPrice, float orderPrice)
        {
            float comparePrice = _dependsOnCartPrice ? cartPrice : orderPrice;
            float shippingPrice = -1;
            foreach (var range in _priceRanges)
            {
                if (comparePrice >= range.OrderPrice)
                {
                    shippingPrice = range.ShippingPrice;
                }
            }
            return shippingPrice;
        }
    }
}