using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class PlatronControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {

                return _valid ||
                       ValidateFormData(
                           new[] {txtMerchantId, txtCurrency, txtCurrencyValue, txtSecretKey},
                           new[] {txtCurrencyValue})
                           ? new Dictionary<string, string>
                               {
                                   {PlatronTemplate.MerchantId, txtMerchantId.Text},
                                   {PlatronTemplate.Currency, txtCurrency.Text},
                                   {PlatronTemplate.PaymentSystem, txtPaymentSystem.Text},
                                   {PlatronTemplate.CurrencyValue, txtCurrencyValue.Text},
                                   {PlatronTemplate.SecretKey, txtSecretKey.Text},
                                   {PlatronTemplate.SendReceiptData, cbSendReceiptData.Checked.ToString()},
                               }
                           : null;
            }
            set
            {
                txtMerchantId.Text = value.ElementOrDefault(PlatronTemplate.MerchantId);
                txtCurrency.Text = value.ElementOrDefault(PlatronTemplate.Currency);
                txtPaymentSystem.Text = value.ElementOrDefault(PlatronTemplate.PaymentSystem);
                txtCurrencyValue.Text = value.ElementOrDefault(PlatronTemplate.CurrencyValue);
                txtSecretKey.Text = value.ElementOrDefault(PlatronTemplate.SecretKey);
                cbSendReceiptData.Checked = value.ElementOrDefault(PlatronTemplate.SendReceiptData).TryParseBool();
            }
        }
 
    }
}