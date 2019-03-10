using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class SagePayControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] { txtVendor, txtCurrencyCode, txtPassword, txtCurrencyValue },new[]{txtCurrencyValue})
                           ? new Dictionary<string, string>
                               {
                                   {SagePayTemplate.Vendor, txtVendor.Text},
                                   {SagePayTemplate.Sandbox, chkSandbox.Checked.ToString()},
                                   {SagePayTemplate.CurrencyCode, txtCurrencyCode.Text},
                                   {SagePayTemplate.Password, txtPassword.Text},
                                   {SagePayTemplate.CurrencyValue, txtCurrencyValue.Text}
                               }
                           : null;
            }
            set
            {
                txtVendor.Text = value.ElementOrDefault(SagePayTemplate.Vendor);
                chkSandbox.Checked = value.ElementOrDefault(SagePayTemplate.Sandbox).TryParseBool();
                txtCurrencyCode.Text = value.ElementOrDefault(SagePayTemplate.CurrencyCode);
                txtPassword.Text = value.ElementOrDefault(SagePayTemplate.Password);
                txtCurrencyValue.Text = value.ElementOrDefault(SagePayTemplate.CurrencyValue);
            }
        }
  
    }
}