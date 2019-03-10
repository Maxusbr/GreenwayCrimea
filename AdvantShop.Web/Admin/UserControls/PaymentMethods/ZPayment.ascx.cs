using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class ZPaymentControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] {txtPurse, txtPassword, txtSecretKey})
                    ? new Dictionary<string, string>
                    {
                        {ZPaymentTemplate.Purse, txtPurse.Text},
                        {ZPaymentTemplate.Password, txtPassword.Text},
                        {ZPaymentTemplate.SecretKey, txtSecretKey.Text},
                    }
                    : null;
            }
            set
            {
                txtPurse.Text = value.ElementOrDefault(ZPaymentTemplate.Purse);
                txtPassword.Text = value.ElementOrDefault(ZPaymentTemplate.Password);
                txtSecretKey.Text = value.ElementOrDefault(ZPaymentTemplate.SecretKey);
            }
        }
    }
}