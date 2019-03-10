using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class GoogleCheckoutControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] {txtMerchantID, txtCurrencyValue, txtCurrencyCode}, new[] {txtCurrencyValue})
                           ? new Dictionary<string, string>
                               {
                                   {GoogleCheckoutTemplate.MerchantID, txtMerchantID.Text},
                                   {GoogleCheckoutTemplate.CurrencyValue, txtCurrencyValue.Text},
                                   {GoogleCheckoutTemplate.CurrencyCode, txtCurrencyCode.Text},
                                   {GoogleCheckoutTemplate.Sandbox, chkSandbox.Text}
                               }
                           : null;
            }
            set
            {
                txtMerchantID.Text = value.ElementOrDefault(GoogleCheckoutTemplate.MerchantID);
                txtCurrencyValue.Text = value.ElementOrDefault(GoogleCheckoutTemplate.CurrencyValue);
                txtCurrencyCode.Text = value.ElementOrDefault(GoogleCheckoutTemplate.CurrencyCode);
                chkSandbox.Checked = value.ElementOrDefault(GoogleCheckoutTemplate.Sandbox).TryParseBool();
            }
        }
    }
}