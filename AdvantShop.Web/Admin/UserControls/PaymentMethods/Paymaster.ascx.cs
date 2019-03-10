using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class PaymasterControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtMerchantId,  txtSecretWord},
                                        null,null)
                           ? new Dictionary<string, string>
                               {
                                   {PaymasterTemplate.MerchantId, txtMerchantId.Text},
                                   {PaymasterTemplate.SecretWord, txtSecretWord.Text},
                               }
                           : null;
            }
            set
            {
                txtMerchantId.Text = value.ElementOrDefault(PaymasterTemplate.MerchantId);
                txtSecretWord.Text = value.ElementOrDefault(PaymasterTemplate.SecretWord);
            }
        }
    }
}