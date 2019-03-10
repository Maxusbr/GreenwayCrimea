using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class TinkoffControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(
                           new[] { txtTerminalKey, txtSecretKey }, null, null)
                           ? new Dictionary<string, string>
                               {
                                   {TinkoffTemplate.TerminalKey, txtTerminalKey.Text},
                                   {TinkoffTemplate.SecretKey, txtSecretKey.Text},
                                   {TinkoffTemplate.Taxation, ddlTaxation.SelectedValue ?? ddlTaxation.Items[0].Value},
                                   {TinkoffTemplate.SendReceiptData, cbSendReceiptData.Checked.ToString()},
                               }
                           : null;
            }
            set
            {
                txtTerminalKey.Text = value.ElementOrDefault(TinkoffTemplate.TerminalKey);
                txtSecretKey.Text = value.ElementOrDefault(TinkoffTemplate.SecretKey);
                if (ddlTaxation.Items.FindByValue(value.ElementOrDefault(TinkoffTemplate.Taxation)) != null)
                {
                    ddlTaxation.SelectedValue = value.ElementOrDefault(TinkoffTemplate.Taxation);
                }

                cbSendReceiptData.Checked = value.ElementOrDefault(TinkoffTemplate.SendReceiptData).TryParseBool();
            }
        }
    }
}