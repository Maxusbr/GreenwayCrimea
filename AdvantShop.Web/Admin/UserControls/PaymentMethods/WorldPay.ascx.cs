using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class WorldPayControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] { txtInstID, txtCurrencyValue, txtCurrencyCode }, new[] {txtCurrencyValue})
                           ? new Dictionary<string, string>
                               {
                                   {WorldPayTemplate.Sandbox, chkSansbox.Checked.ToString()},
                                   {WorldPayTemplate.InstID, txtInstID.Text},
                                   {WorldPayTemplate.CurrencyValue, txtCurrencyValue.Text},
                                   {WorldPayTemplate.CurrencyCode, txtCurrencyCode.Text},
                               }
                           : null;
            }
            set
            {
                chkSansbox.Checked = value.ElementOrDefault(WorldPayTemplate.Sandbox).TryParseBool();
                txtInstID.Text = value.ElementOrDefault(WorldPayTemplate.InstID);
                txtCurrencyValue.Text = value.ElementOrDefault(WorldPayTemplate.CurrencyValue);
                txtCurrencyCode.Text = value.ElementOrDefault(WorldPayTemplate.CurrencyCode);
            }
        }
    
    }
}