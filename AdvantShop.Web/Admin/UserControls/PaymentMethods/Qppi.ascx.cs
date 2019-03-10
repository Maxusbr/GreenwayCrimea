using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class QppiControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtMerchantXid, txtPrivateSecurityKey, txtExternalProjectName }, null, null)
                           ? new Dictionary<string, string>
                               {
                                   {QppiTemplate.MerchantXid, txtMerchantXid.Text},
                                   {QppiTemplate.PrivateSecurityKey, txtPrivateSecurityKey.Text},
                                   {QppiTemplate.Sandbox, chkSandbox.Checked.ToString()},
                                   {QppiTemplate.ExternalProjectName, txtExternalProjectName.Text.Reduce(30)} 
                               }
                           : null;
            }
            set
            {
                txtMerchantXid.Text = value.ElementOrDefault(QppiTemplate.MerchantXid);
                txtPrivateSecurityKey.Text = value.ElementOrDefault(QppiTemplate.PrivateSecurityKey);
                chkSandbox.Checked = value.ElementOrDefault(QppiTemplate.Sandbox).TryParseBool();
                txtExternalProjectName.Text = value.ElementOrDefault(QppiTemplate.ExternalProjectName);
            }
        }
    }
}