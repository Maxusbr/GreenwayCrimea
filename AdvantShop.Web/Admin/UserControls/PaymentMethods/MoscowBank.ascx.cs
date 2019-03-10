using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class MoscowBankControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtMerchant, txtTerminal, txtMerchName, txtEmail, txtKey, txtCurrencyLabel, txtCurrencyValue },
                                        new[] { txtCurrencyValue })
                           ? new Dictionary<string, string>
                               {
                                   {MoscowBankTemplate.Merchant, txtMerchant.Text},
                                   {MoscowBankTemplate.Terminal, txtTerminal.Text},
                                   {MoscowBankTemplate.MerchName, txtMerchName.Text},
                                   {MoscowBankTemplate.Email, txtEmail.Text},
                                   {MoscowBankTemplate.Key, txtKey.Text},
                                   {MoscowBankTemplate.CurrencyLabel, txtCurrencyLabel.Text},
                                   {MoscowBankTemplate.CurrencyValue, txtCurrencyValue.Text}
                               }
                           : null;
            }
            set
            {
                txtMerchant.Text = value.ElementOrDefault(MoscowBankTemplate.Merchant);
                txtTerminal.Text = value.ElementOrDefault(MoscowBankTemplate.Terminal);
                txtMerchName.Text = value.ElementOrDefault(MoscowBankTemplate.MerchName);
                txtEmail.Text = value.ElementOrDefault(MoscowBankTemplate.Email);
                txtKey.Text = value.ElementOrDefault(MoscowBankTemplate.Key);
                txtCurrencyLabel.Text = value.ElementOrDefault(MoscowBankTemplate.CurrencyLabel);
                txtCurrencyValue.Text = value.ElementOrDefault(MoscowBankTemplate.CurrencyValue);
            }
        }
    }
}