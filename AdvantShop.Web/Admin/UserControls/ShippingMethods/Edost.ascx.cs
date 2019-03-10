using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Helpers;
using AdvantShop.Shipping.Edost;
using AdvantShop.Trial;

namespace Admin.UserControls.ShippingMethods
{
    public partial class EdostControl : ParametersControl
    {
        private bool _hidePassword;
        protected void Page_Init(object sender, EventArgs e)
        {
            _hidePassword = (Demo.IsDemoEnabled || TrialService.IsTrialEnabled ) && false;
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                rvRate.Validate();

                var required = new List<TextBox> { txtShopId, txtRate };
                if (!_hidePassword)
                    required.Add(txtPassword);

                return rvRate.IsValid && (_valid || ValidateFormData(required, new[] { txtRate }))
                           ? new Dictionary<string, string>
                               {
                                   {EdostTemplate.ShopId, txtShopId.Text},
                                   {EdostTemplate.Password, txtPassword.Text},
                                   {EdostTemplate.EnabledCOD, chbcreateCOD.Checked.ToString()  },
                                   {EdostTemplate.EnabledPickPoint , chbcreatePickPoint.Checked.ToString()  },
                                   {EdostTemplate.ShipIdCOD, hfCod.Value  },
                                   {EdostTemplate.ShipIdPickPoint, hfPickPoint.Value },
                                   {EdostTemplate.Rate, txtRate.Text },
                                   {EdostTemplate.DefaultWeight, txtWeight.Text },
                                   {EdostTemplate.DefaultLength, txtLength.Text },
                                   {EdostTemplate.DefaultWidth, txtWidth.Text },
                                   {EdostTemplate.DefaultHeight, txtHeight.Text },
                               }
                           : null;
            }
            set
            {
                txtShopId.Text = value.ElementOrDefault(EdostTemplate.ShopId);
                txtRate.Text = value.ElementOrDefault(EdostTemplate.Rate) ?? "1" ;
                txtPassword.Text = value.ElementOrDefault(EdostTemplate.Password);
                txtPassword.Visible = !_hidePassword;
                lPassword.Visible = _hidePassword;

                chbcreateCOD.Checked = SQLDataHelper.GetBoolean(value.ElementOrDefault(EdostTemplate.EnabledCOD));
                chbcreatePickPoint.Checked = SQLDataHelper.GetBoolean(value.ElementOrDefault(EdostTemplate.EnabledPickPoint));
                hfCod.Value = value.ElementOrDefault(EdostTemplate.ShipIdCOD);
                hfPickPoint.Value = value.ElementOrDefault(EdostTemplate.ShipIdPickPoint);

                txtWeight.Text = value.ElementOrDefault(EdostTemplate.DefaultWeight);
                txtLength.Text = value.ElementOrDefault(EdostTemplate.DefaultLength);
                txtWidth.Text = value.ElementOrDefault(EdostTemplate.DefaultWidth);
                txtHeight.Text = value.ElementOrDefault(EdostTemplate.DefaultHeight);
            }
        }
    }
}