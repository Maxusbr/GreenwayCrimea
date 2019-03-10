//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Shipping.FixedRate
{
     [ShippingKey("FixedRate")]
    public class FixeRateShipping : BaseShipping
    {
        private readonly float _shippingPrice;
        //private readonly float _extracharge;
         
        public FixeRateShipping(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {
            _shippingPrice = method.Params.ElementOrDefault(FixeRateShippingTemplate.ShippingPrice).TryParseFloat();
            //_extracharge = method.Params.ElementOrDefault(FixeRateShippingTemplate.Extracharge).TryParseFloat();
        }

        private float GetRate()
        {
            return _shippingPrice;//+ _extracharge;
        }

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            var option = new BaseShippingOption(_method);
            option.Rate = GetRate();
            option.DeliveryTime = _method.Params.ElementOrDefault(FixeRateShippingTemplate.DeliveryTime);
            return new List<BaseShippingOption> { option };
        }
    }
}