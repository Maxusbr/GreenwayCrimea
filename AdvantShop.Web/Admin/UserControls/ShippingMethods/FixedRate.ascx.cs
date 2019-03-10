using System.Collections.Generic;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Shipping.FixedRate;

namespace Admin.UserControls.ShippingMethods
{
    public partial class FixedRateControl : ParametersControl
    {

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] { txtShippingPrice },
                                                  new[] { txtShippingPrice },
                                                  new TextBox[0])
                           ? new Dictionary<string, string>
                               {
                                   {FixeRateShippingTemplate.ShippingPrice, txtShippingPrice.Text},
                                   //{FixeRateShippingTemplate.Extracharge, txtExtracharge.Text},
                                   {FixeRateShippingTemplate.DeliveryTime, txtDeliveryTime.Text},
                               }
                           : null;
            }
            set
            {
                //txtExtracharge.Text = value.ElementOrDefault(FixeRateShippingTemplate.Extracharge);
                txtShippingPrice.Text = value.ElementOrDefault(FixeRateShippingTemplate.ShippingPrice);
                txtDeliveryTime.Text = value.ElementOrDefault(FixeRateShippingTemplate.DeliveryTime);
            }
        }
    }
}