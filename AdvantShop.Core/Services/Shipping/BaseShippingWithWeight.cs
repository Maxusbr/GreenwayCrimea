using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Shipping
{
    public abstract class BaseShippingWithWeight : BaseShipping
    {
        protected float _defaultWeight;
        protected BaseShippingWithWeight(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {
            _method = method;
            _preOrder = preOrder;
            _defaultWeight = _method.Params.ElementOrDefault(DefaultWeightParams.DefaultWeight).TryParseFloat();
        }


        protected virtual void DefaultIfNotSet()
        {
            var model = _preOrder.Items;
            foreach (var item in model)
            {
                item.Weight = item.Weight == 0 ? _defaultWeight : item.Weight;
            }
            _preOrder.Items = model;
        }

        public override IEnumerable<BaseShippingOption> GetOptions()
        {
            DefaultIfNotSet();
            return CalcOptions();
        }
    }
}