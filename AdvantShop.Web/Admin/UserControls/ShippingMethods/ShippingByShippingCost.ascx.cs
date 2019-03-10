using System.Collections.Generic;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Shipping.ShippingByShippingCost;

namespace Admin.UserControls.ShippingMethods
{
    public partial class ShippingByShippingCostControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(null, null, new TextBox[0])
                           ? new Dictionary<string, string>
                               {
                                   {ShippingByShippingCostTemplate.ByMaxShippingCost, RadioButtonByMax.Checked ? "true" : "false"},
                                   {ShippingByShippingCostTemplate.UseAmount, RadioButtonUseAmount.Checked ? "true" : "false"},
                               }
                           : null;
            }
            set
            {
                var valueByMax = value.ElementOrDefault(ShippingByShippingCostTemplate.ByMaxShippingCost);
                if (valueByMax == null || valueByMax.TryParseBool())
                {
                    RadioButtonByMax.Checked = true;
                }
                else
                {
                    RadioButtonBySum.Checked = true;
                }

                var valueUseAmount = value.ElementOrDefault(ShippingByShippingCostTemplate.UseAmount);
                if (valueUseAmount.TryParseBool())
                {
                    RadioButtonUseAmount.Checked = true;
                }
                else
                {
                    RadioButtonDontUseAmount.Checked = true;
                }
            }
        }

   
    }
}