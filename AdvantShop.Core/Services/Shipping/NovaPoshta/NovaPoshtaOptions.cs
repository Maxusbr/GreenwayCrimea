using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Shipping.NovaPoshta
{
    class NovaPoshtaOptions : BaseShippingOption
    {
        public NovaPoshtaOptions()
        {
        }

        public NovaPoshtaOptions(ShippingMethod method)
            : base(method)
        {
        }
    }
}
