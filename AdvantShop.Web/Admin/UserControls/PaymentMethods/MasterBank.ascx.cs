using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class MasterBankControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtTerminal, txtSecretWord }, null, null)
                           ? new Dictionary<string, string>
                               {
                                   {MasterBankTemplate.Terminal, txtTerminal.Text},
                                   {MasterBankTemplate.SecretWord, txtSecretWord.Text}
                               }
                           : null;
            }
            set
            {
                txtTerminal.Text = value.ElementOrDefault(MasterBankTemplate.Terminal);
                txtSecretWord.Text = value.ElementOrDefault(MasterBankTemplate.SecretWord);
            }
        }

    }
}