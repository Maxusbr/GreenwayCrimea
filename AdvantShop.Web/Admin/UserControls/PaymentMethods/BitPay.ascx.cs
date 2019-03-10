using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class BitPayControls : ParametersControl
    {

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtApiKey, txtCurrency }, null, null)
                           ? new Dictionary<string, string>
                               {
                                   {BitPayTemplate.ApiKey, txtApiKey.Text},
                                   {BitPayTemplate.Currency, txtCurrency.Text}
                               }
                           : null;
            }
            set
            {
                txtApiKey.Text = value.ElementOrDefault(BitPayTemplate.ApiKey);
                txtCurrency.Text = value.ElementOrDefault(BitPayTemplate.Currency);
            }
        }

    }
}