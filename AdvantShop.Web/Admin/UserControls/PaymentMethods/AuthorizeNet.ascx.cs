using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class AuthorizeNetControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] {txtLogin, txtTransactionKey, txtCurrencyValue},
                                                  new[] {txtCurrencyValue},
                                                  null)
                           ? new Dictionary<string, string>
                               {
                                   {AuthorizeNetTemplate.Login, txtLogin.Text},
                                   {AuthorizeNetTemplate.TransactionKey, txtTransactionKey.Text},
                                   {AuthorizeNetTemplate.CurrencyValue, txtCurrencyValue.Text},
                                   {AuthorizeNetTemplate.Sandbox, chkSandbox.Checked.ToString()}
                               }
                           : null;
            }
            set { txtLogin.Text = value.ElementOrDefault(AuthorizeNetTemplate.Login);
                txtTransactionKey.Text = value.ElementOrDefault(AuthorizeNetTemplate.TransactionKey);
                txtCurrencyValue.Text = value.ElementOrDefault(AuthorizeNetTemplate.CurrencyValue);
                chkSandbox.Checked = value.ElementOrDefault(AuthorizeNetTemplate.Sandbox).TryParseBool();
            }
        }
    }
}