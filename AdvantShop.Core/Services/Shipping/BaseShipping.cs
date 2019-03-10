using System;
using System.Collections.Generic;

namespace AdvantShop.Shipping
{
    public abstract class BaseShipping : IShipping
    {
        protected ShippingMethod _method;
        protected PreOrder _preOrder;

        protected BaseShipping()
        {
        }

        protected BaseShipping(ShippingMethod method, PreOrder preOrder)
        {
            _method = method;
            _preOrder = preOrder;
        }

        protected virtual IEnumerable<BaseShippingOption> CalcOptions()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<BaseShippingOption> GetOptions()
        {
            return CalcOptions();
        }
    }
}