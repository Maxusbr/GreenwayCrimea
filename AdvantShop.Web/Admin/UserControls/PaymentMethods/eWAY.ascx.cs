using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class eWAYControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] {txtCustomerID, txtCurrencyValue}, new[] {txtCurrencyValue})
                           ? new Dictionary<string, string>
                               {
                                   {eWAYTemplate.CustomerID, txtCustomerID.Text},
                                   {eWAYTemplate.Sandbox, chkSandbox.Checked.ToString()},
                                   {eWAYTemplate.CurrencyValue, txtCurrencyValue.Text}
                               }
                           : null;
            }
            set
            {
                txtCustomerID.Text = value.ElementOrDefault(eWAYTemplate.CustomerID);
                chkSandbox.Checked = value.ElementOrDefault(eWAYTemplate.Sandbox).TryParseBool();
                txtCurrencyValue.Text = value.ElementOrDefault(eWAYTemplate.CurrencyValue);
            }
        }
   
    }
}