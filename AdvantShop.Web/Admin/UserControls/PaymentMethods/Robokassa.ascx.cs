using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class RobokassaControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] { txtMerchantLogin, txtPassword, txtPasswordNotify })
                    ? new Dictionary<string, string>
                    {
                        {RobokassaTemplate.MerchantLogin, txtMerchantLogin.Text},
                        {RobokassaTemplate.Password, txtPassword.Text},
                        {RobokassaTemplate.PasswordNotify, txtPasswordNotify.Text},
                        {RobokassaTemplate.SendReceiptData, cbSendReceiptData.Checked.ToString()},
                    }
                    : null;
            }
            set
            {
                txtMerchantLogin.Text = value.ElementOrDefault(RobokassaTemplate.MerchantLogin);
                txtPassword.Text = value.ElementOrDefault(RobokassaTemplate.Password);
                txtPasswordNotify.Text = value.ElementOrDefault(RobokassaTemplate.PasswordNotify);
                cbSendReceiptData.Checked = value.ElementOrDefault(RobokassaTemplate.SendReceiptData).TryParseBool();
            }
        }
    }
}