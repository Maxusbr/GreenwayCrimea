using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class WebMoneyControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtPurse, txtSecretKey })
                           ? new Dictionary<string, string>
                               {
                                   {WebMoneyTemplate.Purse, txtPurse.Text},
                                   {WebMoneyTemplate.SecretKey, txtSecretKey.Text},
                               }
                           : null;
            }
            set
            {
                txtPurse.Text = value.ElementOrDefault(WebMoneyTemplate.Purse);
                txtSecretKey.Text = value.ElementOrDefault(WebMoneyTemplate.SecretKey);
            }
        }
    }
}