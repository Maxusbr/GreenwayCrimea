using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class PayPalControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] { txtCurrencyValue, txtPdtCode, txtCurrencyCode, txtEmailID }, new[] { txtCurrencyValue })
                           ? new Dictionary<string, string>
                               {
                                   {PayPalTemplate.EMail, txtEmailID.Text},
                                   {PayPalTemplate.PDTCode, txtPdtCode.Text},
                                   {PayPalTemplate.CurrencyCode, txtCurrencyCode.Text},
                                   {PayPalTemplate.CurrencyValue, txtCurrencyValue.Text},
                                   {PayPalTemplate.Sandbox, chkSandbox.Checked.ToString()},
                                   {PayPalTemplate.ShowTaxAndShipping, chkShowTax.Checked.ToString()}
                               }
                           : null;
            }
            set
            {
                txtEmailID.Text = value.ElementOrDefault(PayPalTemplate.EMail);
                txtPdtCode.Text = value.ElementOrDefault(PayPalTemplate.PDTCode);
                txtCurrencyCode.Text = value.ElementOrDefault(PayPalTemplate.CurrencyCode);
                txtCurrencyValue.Text = value.ElementOrDefault(PayPalTemplate.CurrencyValue);
                chkSandbox.Checked = value.ElementOrDefault(PayPalTemplate.Sandbox).TryParseBool();
                chkShowTax.Checked = value.ElementOrDefault(PayPalTemplate.ShowTaxAndShipping).TryParseBool();
            }
        }
    }
}