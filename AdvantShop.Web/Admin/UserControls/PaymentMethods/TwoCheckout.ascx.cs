using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class TwoCheckoutControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] {txtSid, txtSecretWord})
                           ? new Dictionary<string, string>
                               {
                                   {TwoCheckoutTemplate.Sid, txtSid.Text},
                                   {TwoCheckoutTemplate.Sandbox, chkSandbox.Checked.ToString()},
                                   {TwoCheckoutTemplate.SecretWord, txtSecretWord.Text},
                               }
                           : null;
            }
            set
            {
                txtSid.Text = value.ElementOrDefault(TwoCheckoutTemplate.Sid);
                chkSandbox.Checked = value.ElementOrDefault(TwoCheckoutTemplate.Sandbox).TryParseBool();
                txtSecretWord.Text = value.ElementOrDefault(TwoCheckoutTemplate.SecretWord);
            }
        }
    
    }
}