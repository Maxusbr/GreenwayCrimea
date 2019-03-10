using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class PSIGateConrol : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] { txtStoreKey, txtCurrencyValue },new []{txtCurrencyValue})
                           ? new Dictionary<string, string>
                               {
                                   {PSIGateTemplate.StoreKey, txtStoreKey.Text},
                                   {PSIGateTemplate.Sandbox, chkSandbox.Checked.ToString()},
                                   {PSIGateTemplate.CurrencyValue, txtCurrencyValue.Text}
                               }
                           : null;

            }
            set
            {
                txtStoreKey.Text = value.ElementOrDefault(PSIGateTemplate.StoreKey);
                chkSandbox.Checked = value.ElementOrDefault(PSIGateTemplate.Sandbox).TryParseBool();
                txtCurrencyValue.Text = value.ElementOrDefault(PSIGateTemplate.CurrencyValue);
            }
        }
    
    }
}