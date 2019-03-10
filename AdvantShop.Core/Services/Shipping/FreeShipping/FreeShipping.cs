//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Shipping.FreeShipping
{
    public struct FreeShippingTemplate
    {
        public const string DeliveryTime = "DeliveryTime";
    }

    [ShippingKey("FreeShipping")]
    public class FreeShipping : BaseShipping
    {
        public FreeShipping(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {
        }

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            var option = new BaseShippingOption(_method);
            option.Rate = GetRate();
            option.DeliveryTime = _method.Params.ElementOrDefault(FreeShippingTemplate.DeliveryTime);
            return new List<BaseShippingOption> { option };
        }

        private float GetRate()
        {
            return 0F;
        }
    }
}