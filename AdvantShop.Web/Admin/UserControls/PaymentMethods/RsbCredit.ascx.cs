using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class RsbCreditControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtPartnerId, txtMinimumPrice }, 
                                        new[] { txtMinimumPrice })
                           ? new Dictionary<string, string>
                               {
                                   {RsbCreditTemplate.PartnerId, txtPartnerId.Text},
                                   {RsbCreditTemplate.MinimumPrice, txtMinimumPrice.Text},
                                   {RsbCreditTemplate.FirstPayment, txtFirstPayment.Text}
                               }
                           : null;
            }
            set
            {
                txtPartnerId.Text = value.ElementOrDefault(RsbCreditTemplate.PartnerId);
                txtMinimumPrice.Text = value.ElementOrDefault(RsbCreditTemplate.MinimumPrice);
                txtFirstPayment.Text = value.ElementOrDefault(RsbCreditTemplate.FirstPayment);
            }
        }

    }
}