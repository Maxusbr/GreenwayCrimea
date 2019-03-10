using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Shipping;
using AdvantShop.Shipping.ShippingByWeight;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Admin.UserControls.ShippingMethods
{
    public partial class ShippingByWeightControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] {txtPricePerKg, txtExtracharge,},
                                                  new[] {txtPricePerKg, txtExtracharge,},
                                                  new TextBox[0])
                           ? new Dictionary<string, string>
                               {
                                   {ShippingByWeightTemplate.PricePerKg, txtPricePerKg.Text},
                                   {ShippingByWeightTemplate.Extracharge, txtExtracharge.Text},
                                   {ShippingByWeightTemplate .DeliveryTime, txtDeliveryTime.Text },

                                   {DefaultWeightParams.DefaultWeight, txtWeight.Text },
                               }
                           : null;
            }
            set
            {
                txtExtracharge.Text = value.ElementOrDefault(ShippingByWeightTemplate.Extracharge);
                txtPricePerKg.Text = value.ElementOrDefault(ShippingByWeightTemplate.PricePerKg);
                txtDeliveryTime.Text = value.ElementOrDefault(ShippingByWeightTemplate.DeliveryTime);

                txtWeight.Text = value.ElementOrDefault(DefaultWeightParams.DefaultWeight);
            }
        }
    }
}