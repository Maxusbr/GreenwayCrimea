using System.Collections.Generic;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Shipping.FreeShipping;

namespace Admin.UserControls.ShippingMethods
{
    public partial class FreeShippingControl : ParametersControl
    {

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new TextBox[0])
                           ? new Dictionary<string, string>
                               {
                                   {FreeShippingTemplate.DeliveryTime, txtDeliveryTime.Text},
                               }
                           : null;
            }
            set
            {
                txtDeliveryTime.Text = value.ElementOrDefault(FreeShippingTemplate.DeliveryTime);
            }
        }
    }
}