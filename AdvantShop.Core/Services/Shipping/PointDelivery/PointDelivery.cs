using System.Collections.Generic;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using System.Linq;

namespace AdvantShop.Shipping.PointDelivery
{
    [ShippingKey("PointDelivery")]
    public class PointDelivery : BaseShipping
    {        
        private readonly List<string> _points;

        public PointDelivery(ShippingMethod method, PreOrder preOrder) : base(method, preOrder)
        {
            var points = _method.Params.ElementOrDefault(PointDeliveryTemplate.Points);

            if (string.IsNullOrEmpty(points))
            {
                return;
            }
            _points = points.Split(';').ToList();
        }

        protected override IEnumerable<BaseShippingOption> CalcOptions()
        {
            var rate = 0f;
            float.TryParse(_method.Params.ElementOrDefault(PointDeliveryTemplate.ShippingPrice), out rate);

            var opt = new PointDeliveryOption(_method)
            {
                Rate = rate,
                DeliveryTime = _method.Params.ElementOrDefault(PointDeliveryTemplate.DeliveryTime)
            };
            return new List<BaseShippingOption>() { opt };
        }
    }
}