using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class DirectCreditControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtPartnerId, txtMinimumPrice, txtFirstPayment }, new[] { txtMinimumPrice, txtFirstPayment })
                           ? new Dictionary<string, string>
                               {
                                   {DirectCreditTemplate.PartnerId, txtPartnerId.Text},
                                   {DirectCreditTemplate.MinimumPrice, txtMinimumPrice.Text},
                                   {DirectCreditTemplate.FirstPayment, txtFirstPayment.Text}
                               }
                           : null;
            }
            set
            {
                txtPartnerId.Text = value.ElementOrDefault(DirectCreditTemplate.PartnerId);
                txtMinimumPrice.Text = value.ElementOrDefault(DirectCreditTemplate.MinimumPrice);
                txtFirstPayment.Text = value.ElementOrDefault(DirectCreditTemplate.FirstPayment);
              
            }
        }

    }
}