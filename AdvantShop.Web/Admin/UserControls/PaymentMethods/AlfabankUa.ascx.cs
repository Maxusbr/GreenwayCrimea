using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class AlfabankUaControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtPartnerId, txtMinimumPrice, txtFirstPayment },
                                        new[] { txtMinimumPrice, txtFirstPayment })
                    ? new Dictionary<string, string>
                    {
                        {AlfabankUaTemplate.PartnerId, txtPartnerId.Text},
                        {AlfabankUaTemplate.MinimumPrice, txtMinimumPrice.Text},
                        {AlfabankUaTemplate.FirstPayment, txtFirstPayment.Text},
                    }
                    : null;
            }
            set
            {
                txtPartnerId.Text = value.ElementOrDefault(AlfabankUaTemplate.PartnerId);
                txtMinimumPrice.Text = value.ElementOrDefault(AlfabankUaTemplate.MinimumPrice);
                txtFirstPayment.Text = value.ElementOrDefault(AlfabankUaTemplate.FirstPayment);
            }
        }
    }
}