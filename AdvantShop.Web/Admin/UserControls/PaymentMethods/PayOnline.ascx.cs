using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class PayOnlineControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] { txtMerchantId, txtCurrency, txtSecretKey, txtCurrencyValue }, new[] {txtCurrencyValue})
                           ? new Dictionary<string, string>
                               {

                                   {PayOnlineTemplate.MerchantId, txtMerchantId.Text},
                                   {PayOnlineTemplate.Currency, txtCurrency.Text},
                                   {PayOnlineTemplate.SecretKey, txtSecretKey.Text},
                                   {PayOnlineTemplate.CurrencyValue, txtCurrencyValue.Text},
                                   {PayOnlineTemplate.PayType, ddlPayType.SelectedValue}
                               }
                           : null;
            }
            set
            {
                txtMerchantId.Text = value.ElementOrDefault(PayOnlineTemplate.MerchantId);
                txtCurrency.Text = value.ElementOrDefault(PayOnlineTemplate.Currency);
                txtSecretKey.Text = value.ElementOrDefault(PayOnlineTemplate.SecretKey);
                txtCurrencyValue.Text = value.ElementOrDefault(PayOnlineTemplate.CurrencyValue);
                ddlPayType.SelectedValue = value.ElementOrDefault(PayOnlineTemplate.PayType);
            }
        }
    
    }
}