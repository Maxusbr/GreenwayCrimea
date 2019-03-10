using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class YesCreditControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] {txtMerchantId, txtMinimumPrice},
                           new[] {txtMinimumPrice})
                    ? new Dictionary<string, string>
                    {
                        {YesCreditTemplate.MerchantId, txtMerchantId.Text},
                        {YesCreditTemplate.MinimumPrice, txtMinimumPrice.Text},
                        {YesCreditTemplate.FirstPayment, txtFirstPayment.Text}
                    }
                    : null;
            }
            set
            {
                txtMerchantId.Text = value.ElementOrDefault(YesCreditTemplate.MerchantId);
                txtMinimumPrice.Text = value.ElementOrDefault(YesCreditTemplate.MinimumPrice);
                txtFirstPayment.Text = value.ElementOrDefault(YesCreditTemplate.FirstPayment);
            }
        }
    }
}