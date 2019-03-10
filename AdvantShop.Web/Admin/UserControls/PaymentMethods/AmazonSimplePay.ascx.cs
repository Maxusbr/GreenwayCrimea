using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class AmazonSimplePayControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] { txtAccessKey, txtSecretKey }, null,null)
                           ? new Dictionary<string, string>
                               {
                                   {AmazonSimplePayTemplate.AccessKey, txtAccessKey.Text},
                                   {AmazonSimplePayTemplate.SecretKey, txtSecretKey.Text},
                                   {AmazonSimplePayTemplate.Sandbox, chkSandbox.Checked.ToString()}
                               }
                           : null;
            }
            set { txtAccessKey.Text = value.ElementOrDefault(AmazonSimplePayTemplate.AccessKey);
                txtSecretKey.Text = value.ElementOrDefault(AmazonSimplePayTemplate.SecretKey);
                bool boolval;
                chkSandbox.Checked = !bool.TryParse(value.ElementOrDefault(AmazonSimplePayTemplate.Sandbox), out boolval) ||
                                     boolval;
            }
        }

    }
}