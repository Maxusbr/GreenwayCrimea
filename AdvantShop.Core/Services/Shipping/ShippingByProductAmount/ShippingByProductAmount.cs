using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvantShop.Shipping.ShippingByProductAmount
{
    [ShippingKey("ShippingByProductAmount")]
    public class ShippingByProductAmount : BaseShipping
    {
        private readonly List<ShippingAmountRange> _priceRanges;

        public ShippingByProductAmount(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {
            _priceRanges = GetRange();
            _priceRanges = _priceRanges.OrderBy(item => item.Amount).ToList();
        }

        private List<ShippingAmountRange> GetRange()
        {
            var priceRanges = new List<ShippingAmountRange>();

            var ranges = _method.Params.ElementOrDefault(ShippingByProductAmountTemplate.PriceRanges);
            if (ranges.IsNullOrEmpty())
                throw new Exception("no exist " + ShippingByProductAmountTemplate.PriceRanges);

            foreach (var item in ranges.Split(';'))
            {
                var arr = item.Split('=');

                if (arr.Length != 2) continue;
                priceRanges.Add(new ShippingAmountRange()
                {
                    Amount = arr[0].TryParseFloat(),
                    ShippingPrice = arr[1].TryParseFloat()
                });
            }
            return priceRanges;
        }

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            var rate = GetRate(_preOrder.Items.Sum(item => item.Amount));
            if (rate >= 0)
            {
                var option = new BaseShippingOption(_method)
                {
                    Rate = rate,
                    DeliveryTime = _method.Params.ElementOrDefault(ShippingByProductAmountTemplate.DeliveryTime)
                };
                return new List<BaseShippingOption> { option };
            }
            else
                return new List<BaseShippingOption>();
        }

        private float GetRate(float amount)
        {
            float shippingPrice = -1;
            foreach (var range in _priceRanges)
            {
                if (amount >= range.Amount)
                {
                    shippingPrice = range.ShippingPrice;
                }
            }
            return shippingPrice;
        }
    }
}