using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class GateLineControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtSite, txtPassword })
                           ? new Dictionary<string, string>
                               {
                                   {GateLineTemplate.Site, txtSite.Text},
                                   {GateLineTemplate.Password, txtPassword.Text},
                                   {GateLineTemplate.TestMode, chkTestMode.Checked.ToString()}
                               }
                           : null;
            }
            set
            {
                txtSite.Text = value.ElementOrDefault(GateLineTemplate.Site);
                txtPassword.Text = value.ElementOrDefault(GateLineTemplate.Password);
                chkTestMode.Checked = value.ElementOrDefault(GateLineTemplate.TestMode).TryParseBool();
            }
        }
    }
}