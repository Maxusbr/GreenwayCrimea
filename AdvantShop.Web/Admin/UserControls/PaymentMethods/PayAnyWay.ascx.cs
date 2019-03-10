using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class PayAnyWayControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] { txtMerchantId, txtCurrency, txtSecretKey, txtCurrencyValue }, new[] { txtCurrencyValue })
                           ? new Dictionary<string, string>
                               {

                                   {PayAnyWayTemplate.MerchantId, txtMerchantId.Text},
                                   {PayAnyWayTemplate.CurrencyLabel, txtCurrency.Text},
                                   {PayAnyWayTemplate.Signature, txtSecretKey.Text},
                                   {PayAnyWayTemplate.CurrencyValue, txtCurrencyValue.Text},
                                   {PayAnyWayTemplate.TestMode, chkSandbox.Checked.ToString()},
                                   {PayAnyWayTemplate.UnitId, txtUnitId.Text.Trim()},
                                   {PayAnyWayTemplate.LimitIds, txtLimitIds.Text.Trim()},
                                   {PayAnyWayTemplate.UseKassa, chkUseKassa.Checked.ToString()},
                               }
                           : null;
            }
            set
            {
                txtMerchantId.Text = value.ElementOrDefault(PayAnyWayTemplate.MerchantId);
                txtCurrency.Text = value.ElementOrDefault(PayAnyWayTemplate.CurrencyLabel);
                txtSecretKey.Text = value.ElementOrDefault(PayAnyWayTemplate.Signature);
                txtCurrencyValue.Text = value.ElementOrDefault(PayAnyWayTemplate.CurrencyValue);
                txtUnitId.Text = value.ElementOrDefault(PayAnyWayTemplate.UnitId);
                txtLimitIds.Text = value.ElementOrDefault(PayAnyWayTemplate.LimitIds);

                chkUseKassa.Checked = value.ElementOrDefault(PayAnyWayTemplate.UseKassa).TryParseBool();

                bool boolval;
                chkSandbox.Checked = !bool.TryParse(value.ElementOrDefault(PayAnyWayTemplate.TestMode), out boolval) || boolval;
            }
        }
    }
}