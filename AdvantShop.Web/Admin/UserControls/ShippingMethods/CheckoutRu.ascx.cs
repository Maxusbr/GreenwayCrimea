using System;
using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Shipping.CheckoutRu;

namespace AdvantShop.Admin.UserControls.ShippingMethods
{
    public partial class CheckoutRu : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {

                return _valid || ValidateFormData(new[] { txtClientId })
                             ? new Dictionary<string, string>
                               {
                                   {CheckoutRuTemplate.ClientId, txtClientId.Text.Trim()},
                                   {CheckoutRuTemplate.Grouping, ckbGrouping.Checked.ToString()},
                                   //{CheckoutRuTemplate.ExtrachargeType, ddlExtrachargeType.SelectedValue},
                                   //{CheckoutRuTemplate.Extracharge, txtExtracharge.Text.TryParseFloat().ToString("F2")}
                               }
                             : null;

            }
            set
            {
                txtClientId.Text = value.ElementOrDefault(CheckoutRuTemplate.ClientId);
                ckbGrouping.Checked = Convert.ToBoolean(value.ElementOrDefault(CheckoutRuTemplate.Grouping));
                //ddlExtrachargeType.SelectedValue = value.ElementOrDefault(CheckoutRuTemplate.ExtrachargeType);
                //txtExtracharge.Text = value.ElementOrDefault(CheckoutRuTemplate.Extracharge);
            }
        }
    }
}