//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using System.Linq;

namespace AdvantShop.Shipping.ShippingByShippingCost
{
    [ShippingKey("ShippingByShippingCost")]
    public class ShippingByShippingCost : BaseShipping
    {
        private readonly bool _byMaxShippingCost;
        private readonly bool _useAmount;

        public ShippingByShippingCost(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {
            _byMaxShippingCost = _method.Params.ElementOrDefault(ShippingByShippingCostTemplate.ByMaxShippingCost).TryParseBool();
            _useAmount = _method.Params.ElementOrDefault(ShippingByShippingCostTemplate.UseAmount).TryParseBool();
        }

        private float GetRate()
        {
            if (!_preOrder.Items.Any())
                return 0F;
            if (!_useAmount)
            {
                return _byMaxShippingCost
                    ? _preOrder.Items.Max(item => item.ShippingPrice)
                    : _preOrder.Items.Sum(item => item.ShippingPrice);
            }
            return _byMaxShippingCost
                ? _preOrder.Items
                    .Max(item => item.ShippingPrice * item.Amount)
                : _preOrder.Items
                    .Sum(item => item.ShippingPrice * item.Amount);
        }

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            var option = new BaseShippingOption(_method);
            option.Rate = GetRate();
            return new List<BaseShippingOption> { option };
        }
    }
}