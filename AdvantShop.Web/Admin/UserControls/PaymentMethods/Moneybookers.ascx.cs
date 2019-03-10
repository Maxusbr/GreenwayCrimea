using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class MoneybookersControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] {txtPayToEmai, txtCurrencyCode, txtCurrencyValue, txtSecretWord},
                                        new[] {txtCurrencyValue})
                           ? new Dictionary<string, string>
                               {
                                   {MoneybookersTemplate.PayToEmai, txtPayToEmai.Text},
                                   {MoneybookersTemplate.CurrencyIso, txtCurrencyCode.Text},
                                   {MoneybookersTemplate.CurrencyValue, txtCurrencyValue.Text},
                                   {MoneybookersTemplate.SecretWord, txtSecretWord.Text},
                                   {MoneybookersTemplate.Sandbox, chkSandbox.Checked.ToString()},
                               }
                           : null;
            }
            set
            {
                txtPayToEmai.Text = value.ElementOrDefault(MoneybookersTemplate.PayToEmai);
                txtCurrencyCode.Text = value.ElementOrDefault(MoneybookersTemplate.CurrencyIso);
                txtCurrencyValue.Text = value.ElementOrDefault(MoneybookersTemplate.CurrencyValue);
                txtSecretWord.Text = value.ElementOrDefault(MoneybookersTemplate.SecretWord);
                chkSandbox.Checked = value.ElementOrDefault(MoneybookersTemplate.Sandbox).TryParseBool();
            }
        }
   
    }
}