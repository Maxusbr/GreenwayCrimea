using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class MailRuConrol : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] {txtKey, txtShopID, txtCurrencyCode})//, txtCurrencyValue},
                                        //new[] {txtCurrencyValue})
                           ? new Dictionary<string, string>
                               {
                                   {MailRuTemplate.Key, txtKey.Text},
                                   {MailRuTemplate.ShopID, txtShopID.Text},
                                   {MailRuTemplate.CurrencyCode, txtCurrencyCode.Text},
                                   {MailRuTemplate.KeepUnique, chkKeepUnique.Checked.ToString()},
                                   //{MailRuTemplate.CurrencyValue, txtCurrencyValue.Text},
                                   {MailRuTemplate.CryptoHex, txtCryptoHex.Text}
                               }
                           : null;
            }
            set
            {
                txtKey.Text = value.ElementOrDefault(MailRuTemplate.Key);
                txtShopID.Text = value.ElementOrDefault(MailRuTemplate.ShopID);
                txtCurrencyCode.Text = value.ElementOrDefault(MailRuTemplate.CurrencyCode);
                chkKeepUnique.Checked = value.ElementOrDefault(MailRuTemplate.KeepUnique).TryParseBool();
                //txtCurrencyValue.Text = value.ElementOrDefault(MailRuTemplate.CurrencyValue);
                txtCryptoHex.Text = value.ElementOrDefault(MailRuTemplate.CryptoHex);
            }
        }
    }
}