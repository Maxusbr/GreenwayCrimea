using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class WebPayControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(
                           new[] { txtStoreId, txtSecretKey }, null, new[] { txtStoreId })
                           ? new Dictionary<string, string>
                               {
                                   {WebPayTemplate.StoreId, txtStoreId.Text},
                                   {WebPayTemplate.SecretKey, txtSecretKey.Text},
                                   {WebPayTemplate.TestMode, cbTestMode.Checked.ToString()},
                               }
                           : null;
            }
            set
            {
                txtStoreId.Text = value.ElementOrDefault(WebPayTemplate.StoreId);
                txtSecretKey.Text = value.ElementOrDefault(WebPayTemplate.SecretKey);
                cbTestMode.Checked = value.ElementOrDefault(WebPayTemplate.TestMode).TryParseBool();
            }
        }
    }
}