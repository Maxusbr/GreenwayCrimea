using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class AssistControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtShopId, txtPassword, txtLogin, txtCurrencyValue, txtCurrencyCode, txtUrlWorkingMode },
                                        new[] {txtCurrencyValue}, 
                                        new[] {txtShopId})
                           ? new Dictionary<string, string>
                               {
                                   {AssistTemplate.Login, txtLogin.Text},
                                   {AssistTemplate.Password, txtPassword.Text},
                                   {AssistTemplate.MerchantID, txtShopId.Text},
                                   {AssistTemplate.UrlWorkingMode, txtUrlWorkingMode.Text},
                                   {AssistTemplate.Sandbox, chkSandbox.Checked.ToString()},
                                   {AssistTemplate.Delay, chkDelay.Checked.ToString()},
                                   {AssistTemplate.CurrencyCode, txtCurrencyCode.Text},
                                   {AssistTemplate.CurrencyValue, txtCurrencyValue.Text},
                                   //{AssistTemplate.AssistIdCcPayment, chkAssistIdCcPayment.Checked.ToString()},
                                   //{AssistTemplate.CardPayment, chkCardPayment.Checked.ToString()},
                                   //{AssistTemplate.PayCashPayment, chkPayCashPayment.Checked.ToString()},
                                   //{AssistTemplate.WebMoneyPayment, chkWebMoneyPayment.Checked.ToString()},
                                   //{AssistTemplate.QiwiBeelinePayment, chkQiwiBeelinePayment.Checked.ToString()}
                               }
                           : null;
            }
            set
            {

                txtLogin.Text = value.ElementOrDefault(AssistTemplate.Login);
                txtPassword.Text = value.ElementOrDefault(AssistTemplate.Password);
                txtShopId.Text = value.ElementOrDefault(AssistTemplate.MerchantID);
                txtUrlWorkingMode.Text = value.ElementOrDefault(AssistTemplate.UrlWorkingMode);

                txtCurrencyCode.Text = value.ElementOrDefault(AssistTemplate.CurrencyCode);
                txtCurrencyValue.Text = value.ElementOrDefault(AssistTemplate.CurrencyValue);
                bool boolval;
                chkSandbox.Checked = !bool.TryParse(value.ElementOrDefault(AssistTemplate.Sandbox), out boolval) || boolval;
                chkDelay.Checked = !bool.TryParse(value.ElementOrDefault(AssistTemplate.Delay), out boolval) || boolval;
                //chkAssistIdCcPayment.Checked = !bool.TryParse(value.ElementOrDefault(AssistTemplate.AssistIdCcPayment), out boolval) || boolval;
                //chkCardPayment.Checked = !bool.TryParse(value.ElementOrDefault(AssistTemplate.CardPayment), out boolval) || boolval;
                //chkPayCashPayment.Checked = !bool.TryParse(value.ElementOrDefault(AssistTemplate.PayCashPayment), out boolval) || boolval;
                //chkWebMoneyPayment.Checked = !bool.TryParse(value.ElementOrDefault(AssistTemplate.WebMoneyPayment), out boolval) || boolval;
                //chkQiwiBeelinePayment.Checked = !bool.TryParse(value.ElementOrDefault(AssistTemplate.QiwiBeelinePayment), out boolval) || boolval;
            }
        }
    }
}