using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Shipping
{
    public abstract class BaseShippingWithCargo : BaseShippingWithWeight
    {

        protected float _defaultLength;
        protected float _defaultWidth;
        protected float _defaultHeight;

        protected BaseShippingWithCargo(ShippingMethod method, PreOrder preOrder)
            : base(method, preOrder)
        {
            _method = method;
            _preOrder = preOrder;
            _defaultWeight = _method.Params.ElementOrDefault(DefaultWeightParams.DefaultWeight).TryParseFloat();
            _defaultLength = _method.Params.ElementOrDefault(DefaultCargoParams.DefaultLength).TryParseFloat();
            _defaultWidth = _method.Params.ElementOrDefault(DefaultCargoParams.DefaultWidth).TryParseFloat();
            _defaultHeight = _method.Params.ElementOrDefault(DefaultCargoParams.DefaultHeight).TryParseFloat();
        }

        protected override void DefaultIfNotSet()
        {
            var model = _preOrder.Items;
            foreach (var item in model)
            {
                item.Height = item.Height == 0 ? _defaultHeight : item.Height;
                item.Length = item.Length == 0 ? _defaultLength : item.Length;
                item.Width = item.Width == 0 ? _defaultWidth : item.Width;
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