using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class KupivkreditControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtShopId, txtShowCaseId, txtMinimumPrice, txtFirstPayment, txtPromoCode },
                                        new[] { txtMinimumPrice, txtFirstPayment })
                           ? new Dictionary<string, string>
                               {
                                   {KupivkreditTemplate.ShopId, txtShopId.Text},
                                   {KupivkreditTemplate.ShowCaseId, txtShowCaseId.Text},
                                   {KupivkreditTemplate.UseTest, chkSandbox.Checked.ToString()},
                                   {KupivkreditTemplate.MinimumPrice, txtMinimumPrice.Text},
                                   {KupivkreditTemplate.FirstPayment, txtFirstPayment.Text},
                                   {KupivkreditTemplate.PromoCode, txtPromoCode.Text}
                               }
                           : null;
            }
            set
            {
                txtShopId.Text = value.ElementOrDefault(KupivkreditTemplate.ShopId);
                txtShowCaseId.Text = value.ElementOrDefault(KupivkreditTemplate.ShowCaseId);
                chkSandbox.Checked = value.ElementOrDefault(KupivkreditTemplate.UseTest).TryParseBool();
                txtMinimumPrice.Text = value.ElementOrDefault(KupivkreditTemplate.MinimumPrice);
                txtFirstPayment.Text = value.ElementOrDefault(KupivkreditTemplate.FirstPayment);
                txtPromoCode.Text = value.ElementOrDefault(KupivkreditTemplate.PromoCode);
            }
        }

    }
}