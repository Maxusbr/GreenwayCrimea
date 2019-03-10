using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class IntellectMoneyMainProtocolControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] { txtEshopId })
                    ? new Dictionary<string, string>
                    {
                        {IntellectMoneyMainProtocolTemplate.EshopId, txtEshopId.Text},
                        {IntellectMoneyMainProtocolTemplate.Preference, ddlPaymentSystem.SelectedValue},
                        {IntellectMoneyMainProtocolTemplate.SecretKey, txtSecretKey.Text},
                    }
                    : null;
            }
            set
            {
                txtEshopId.Text = value.ElementOrDefault(IntellectMoneyMainProtocolTemplate.EshopId);
                txtSecretKey.Text = value.ElementOrDefault(IntellectMoneyMainProtocolTemplate.SecretKey);

                ddlPaymentSystem.DataSource = IntellectMoneyMainProtocol.PaymentSystems;
                ddlPaymentSystem.DataBind();
                if (ddlPaymentSystem.Items.FindByValue(value.ElementOrDefault(IntellectMoneyMainProtocolTemplate.Preference)) != null)
                {
                    ddlPaymentSystem.SelectedValue =
                        value.ElementOrDefault(IntellectMoneyMainProtocolTemplate.Preference);
                }
            }
        }
    }
}