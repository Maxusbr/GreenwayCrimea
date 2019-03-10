using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class MonexyControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtMerchantId, txtShopName })
                           ? new Dictionary<string, string>
                               {
                                   {MoneXyTemplate.MerchantId, txtMerchantId.Text},
                                   {MoneXyTemplate.MerchantCurrency, ddlCurrency.SelectedValue},
                                   {MoneXyTemplate.MerchantShopName, txtShopName.Text},
                                   {MoneXyTemplate.IsCheckHash, chkIsCheckHash.Checked.ToString()},
                                   {MoneXyTemplate.SecretKey, txtSecretKey.Text},
                                   //{MoneXyTemplate.MerchantCurrencyValue, txtCurrencyValue.Text},
                               }
                           : null;
            }
            set
            {
                txtMerchantId.Text = value.ElementOrDefault(MoneXyTemplate.MerchantId);
                ddlCurrency.SelectedValue = value.ElementOrDefault(MoneXyTemplate.MerchantCurrency);
                txtShopName.Text = value.ElementOrDefault(MoneXyTemplate.MerchantShopName);
                chkIsCheckHash.Checked = value.ElementOrDefault(MoneXyTemplate.IsCheckHash).TryParseBool();
                txtSecretKey.Text = value.ElementOrDefault(MoneXyTemplate.SecretKey);
                //txtCurrencyValue.Text = value.ElementOrDefault(MoneXyTemplate.MerchantCurrencyValue);
            }
        }
    }
}