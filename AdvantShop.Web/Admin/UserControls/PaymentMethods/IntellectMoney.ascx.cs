using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class IntellectMoneyControl : ParametersControl
    {

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtMerchantId, txtSecretKey }, null, null)
                           ? new Dictionary<string, string>
                               {
                                   {IntellectMoneyTemplate.MerchantId, txtMerchantId.Text},
                                   {IntellectMoneyTemplate.SecretKey, txtSecretKey.Text},
                               }
                           : null;
            }
            set
            {
                txtMerchantId.Text = value.ElementOrDefault(IntellectMoneyTemplate.MerchantId);
                txtSecretKey.Text = value.ElementOrDefault(IntellectMoneyTemplate.SecretKey);
            }
        }

    }
}