using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class PayPointConrol : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] {txtMerchant, txtCurrencyCode, txtCurrencyValue, txtPassword},
                                        new[] {txtCurrencyValue})
                           ? new Dictionary<string, string>
                               {
                                   {PayPointTemplate.Merchant, txtMerchant.Text},
                                   {PayPointTemplate.CurrencyCode, txtCurrencyCode.Text},
                                   {PayPointTemplate.CurrencyValue, txtCurrencyValue.Text},
                                   {PayPointTemplate.Password, txtPassword.Text}
                               }
                           : null;
            }
            set
            {
                txtMerchant.Text = value.ElementOrDefault(PayPointTemplate.Merchant);
                txtCurrencyCode.Text = value.ElementOrDefault(PayPointTemplate.CurrencyCode);
                txtCurrencyValue.Text = value.ElementOrDefault(PayPointTemplate.CurrencyValue);
                txtPassword.Text = value.ElementOrDefault(PayPointTemplate.Password);
            }
        }
  
    }
}