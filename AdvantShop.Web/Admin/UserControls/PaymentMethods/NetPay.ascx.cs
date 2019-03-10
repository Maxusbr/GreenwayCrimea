using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class NetPayControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] {txtApiKey, txtAuthSign})
                           ? new Dictionary<string, string>
                               {
                                   {NetPayTemplate.ApiKey, txtApiKey.Text},
                                   {NetPayTemplate.AuthSign, txtAuthSign.Text},
                                   {NetPayTemplate.TestMode, chkTestMode.Checked.ToString()}
                               }
                           : null;
            }
            set
            {
                txtApiKey.Text = value.ElementOrDefault(NetPayTemplate.ApiKey);
                txtAuthSign.Text = value.ElementOrDefault(NetPayTemplate.AuthSign);
                chkTestMode.Checked = value.ElementOrDefault(NetPayTemplate.TestMode).TryParseBool();
            }
        }
    }
}