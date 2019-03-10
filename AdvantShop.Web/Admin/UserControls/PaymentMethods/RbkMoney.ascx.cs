using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class RbkMoneyControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] { txtEshopId })
                           ? new Dictionary<string, string>
                               {
                                   {RbkmoneyTemplate.EshopId, txtEshopId.Text},
                                   {RbkmoneyTemplate.Preference, ddlPaymentSystem.SelectedValue},
                               }
                           : null;
            }
            set
            {
                txtEshopId.Text = value.ElementOrDefault(RbkmoneyTemplate.EshopId);
                
                ddlPaymentSystem.DataSource = Rbkmoney.PaymentSystems;
                ddlPaymentSystem.DataBind();
                if (ddlPaymentSystem.Items.FindByValue(value.ElementOrDefault(RbkmoneyTemplate.Preference)) != null)
                {
                    ddlPaymentSystem.SelectedValue = value.ElementOrDefault(RbkmoneyTemplate.Preference);
                }
            }
        }
    }
}