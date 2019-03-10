using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class OnPayControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] { txtformpay, txtSecretKey,txtCurrencyLabel, txtCurrencyValue }, new[] { txtCurrencyValue })
                           ? new Dictionary<string, string>
                               { 
                                   {OnPayTemplate.FormPay, txtformpay.Text},
                                   {OnPayTemplate.SendMethod, rbdSendPost.Checked ? "POST" : "GET"},
                                   {OnPayTemplate.CheckMd5, chbMd5.Checked.ToString()},
                                   {OnPayTemplate.SecretKey, txtSecretKey.Text},
                                   {OnPayTemplate.CurrencyLabel, txtCurrencyLabel.Text},
                                   {OnPayTemplate.CurrencyValue, txtCurrencyValue.Text}
                               }
                           : null;
            }
            set
            {
            
                txtformpay.Text = value.ElementOrDefault(OnPayTemplate.FormPay);
                txtCurrencyLabel.Text = value.ElementOrDefault(OnPayTemplate.CurrencyLabel);
                lbformpay.Text = "http://secure.onpay.ru/pay/" + value.ElementOrDefault(OnPayTemplate.FormPay);
                bool  checkMd5;
                chbMd5.Checked = !bool.TryParse(value.ElementOrDefault(OnPayTemplate.CheckMd5), out checkMd5 ) || checkMd5;
                if (chbMd5.Checked)
                {
                    chbMd5.Text = Resources.Resource.Admin_PaymentMethod_yesmd5;
                }
                else
                {
                    chbMd5.Text = Resources.Resource.Admin_PaymentMethod_nomd5;
                }
                rbdSendPost.Checked = value.ElementOrDefault(OnPayTemplate.SendMethod)=="POST";
                rbdSendGet.Checked = !rbdSendPost.Checked;
                txtSecretKey.Text = value.ElementOrDefault(OnPayTemplate.SecretKey);
                txtCurrencyValue.Text = value.ElementOrDefault(OnPayTemplate.CurrencyValue);
            }
        }
    
   
    }
}