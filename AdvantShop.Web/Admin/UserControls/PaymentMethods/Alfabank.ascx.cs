using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class AlfabankControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] { txtUserName, txtPassword/*, txtMerchantLogin*/ })
                    ? new Dictionary<string, string>
                    {
                        {AlfabankTemplate.UserName, txtUserName.Text},
                        {AlfabankTemplate.Password, txtPassword.Text},
                        {AlfabankTemplate.MerchantLogin, txtMerchantLogin.Text},

                    }
                    : null;
            }
            set
            {
                txtUserName.Text = value.ElementOrDefault(AlfabankTemplate.UserName);
                txtMerchantLogin.Text = value.ElementOrDefault(AlfabankTemplate.MerchantLogin);
                txtPassword.Text = value.ElementOrDefault(AlfabankTemplate.Password);
            }
        }
    }
}