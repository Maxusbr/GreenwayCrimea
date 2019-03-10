//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using System;

namespace AdvantShop.Shipping.SelfDelivery
{
    [ShippingKey("SelfDelivery")]
    public class SelfDelivery : BaseShipping
    {
        private readonly float _shippingPrice;
        public SelfDelivery(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {
            _shippingPrice = _method.Params.ElementOrDefault(SelfDeliveryTemplate.ShippingPrice,"").TryParseFloat();

        }

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            var option = new BaseShippingOption(_method);
            option.DeliveryTime = _method.Params.ElementOrDefault(SelfDeliveryTemplate.DeliveryTime);
            option.Rate = _shippingPrice;
            option.HideAddressBlock = true;
            return new List<BaseShippingOption> { option };
        }     
    }
}