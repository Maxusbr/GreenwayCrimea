using System.Collections.Generic;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;
using AdvantShop.Shipping;

namespace Admin.UserControls.PaymentMethods
{
    public partial class PickPointControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                    {
                        {PickPoint.ShippingMethodTemplate,ddlShipings.SelectedValue }
                    };
            }
            set
            {
                ddlShipings.Items.Clear();
                ddlShipings.Items.Add(new ListItem { Text = Resources.Resource.Admin_PaymentMethods_NotSelected, Value = @"0" });
                var keys = new PickPoint().ShippingKeys;
                var temp = new List<ShippingMethod>();
                foreach (var key in keys)
                {
                    temp.AddRange(ShippingMethodService.GetShippingMethodByType(key));
                }
                for (int i = 0; i < temp.Count; i++)
                {
                    ddlShipings.Items.Add(new ListItem { Text = temp[i].Name, Value = temp[i].ShippingMethodId.ToString() });
                }
                ddlShipings.SelectedValue = value.ElementOrDefault(PickPoint.ShippingMethodTemplate);
            }
        }
    }
}