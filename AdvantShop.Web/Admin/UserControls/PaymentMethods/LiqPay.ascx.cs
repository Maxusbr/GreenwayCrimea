using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class LiqPayControl : ParametersControl
    {

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtMerchantId, txtMerchantSig }, null, null)
                           ? new Dictionary<string, string>
                               {
                                   {LiqPayTemplate.MerchantId, txtMerchantId.Text},
                                   {LiqPayTemplate.MerchantSig, txtMerchantSig.Text},
                                   {LiqPayTemplate.MerchantISO, ddlMerchantISO.SelectedValue}
                               }
                           : null;
            }
            set
            {
                txtMerchantId.Text = value.ElementOrDefault(LiqPayTemplate.MerchantId);
                txtMerchantSig.Text = value.ElementOrDefault(LiqPayTemplate.MerchantSig);
                ddlMerchantISO.Text = value.ElementOrDefault(LiqPayTemplate.MerchantISO);
            }
        }

    }
}