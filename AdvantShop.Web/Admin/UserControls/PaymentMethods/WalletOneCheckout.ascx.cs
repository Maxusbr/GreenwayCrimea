using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class WalletOneCheckoutControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtMerchantId, txtSecretKey },
                                        null,null)
                           ? new Dictionary<string, string>
                               {
                                   {WalletOneCheckoutTemplate.MerchantId, txtMerchantId.Text},
                                   {WalletOneCheckoutTemplate.SecretKey, txtSecretKey.Text},
                                   {WalletOneCheckoutTemplate.PayWaysEnabled, txtPayWaysEnabled.Text},
                                   {WalletOneCheckoutTemplate.PayWaysDisabled, txtPayWaysDisabled.Text}
                               }
                           : null;
            }
            set
            {
                txtMerchantId.Text = value.ElementOrDefault(WalletOneCheckoutTemplate.MerchantId);
                txtSecretKey.Text = value.ElementOrDefault(WalletOneCheckoutTemplate.SecretKey);
                txtPayWaysEnabled.Text = value.ElementOrDefault(WalletOneCheckoutTemplate.PayWaysEnabled);
                txtPayWaysDisabled.Text = value.ElementOrDefault(WalletOneCheckoutTemplate.PayWaysDisabled);
            }
        }
    }
}