using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class InterkassaControl : ParametersControl
    {

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtShopId }, null, null)
                           ? new Dictionary<string, string>
                               {
                                   {InterkassaTemplate.ShopId, txtShopId.Text}                                 
                               }
                           : null;
            }
            set
            {
                txtShopId.Text = value.ElementOrDefault(InterkassaTemplate.ShopId);
            }
        }

    }
}