using System.Collections.Generic;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Shipping.SelfDelivery;

namespace Admin.UserControls.ShippingMethods
{
    public partial class SelfDeliveryControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new TextBox[0])
                           ? new Dictionary<string, string>
                               {
                                   {SelfDeliveryTemplate.ShippingPrice, txtShippingPrice.Text},
                                   {SelfDeliveryTemplate.DeliveryTime, txtDeliveryTime.Text},
                               }
                           : null;
            }
            set
            {
                txtShippingPrice.Text = value.ElementOrDefault(SelfDeliveryTemplate.ShippingPrice);
                txtDeliveryTime.Text = value.ElementOrDefault(SelfDeliveryTemplate.DeliveryTime);
            }
        }
    }
}