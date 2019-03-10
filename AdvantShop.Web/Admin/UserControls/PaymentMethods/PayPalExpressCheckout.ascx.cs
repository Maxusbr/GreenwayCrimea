using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class PayPalExpressCheckoutControl : ParametersControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (var currency in PayPalExpressCheckout.AvaliableCurrs)
            {
                ddlCurrencyCode.Items.Add(new ListItem(currency, currency));
            }
            ddlCurrencyCode.DataBind();
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid || ValidateFormData(new[] { txtCurrencyValue, txtPassword, txtUserId }, new[] { txtCurrencyValue })
                           ? new Dictionary<string, string>
                               {
                                   {PayPalExpressCheckoutTemplate.User, txtUserId.Text},
                                   {PayPalExpressCheckoutTemplate.Password, txtPassword.Text},
                                   {PayPalExpressCheckoutTemplate.Signature, txtSignature.Text},
                                   {PayPalExpressCheckoutTemplate.CurrencyCode, ddlCurrencyCode.SelectedValue},
                                   {PayPalExpressCheckoutTemplate.CurrencyValue, txtCurrencyValue.Text},
                                   {PayPalExpressCheckoutTemplate.Sandbox, chkSandbox.Checked.ToString()},
                                   
                               }
                           : null;
            }
            set
            {
                txtUserId.Text = value.ElementOrDefault(PayPalExpressCheckoutTemplate.User);
                txtPassword.Text = value.ElementOrDefault(PayPalExpressCheckoutTemplate.Password);
                txtSignature.Text = value.ElementOrDefault(PayPalExpressCheckoutTemplate.Signature);
                ddlCurrencyCode.SelectedValue = value.ElementOrDefault(PayPalExpressCheckoutTemplate.CurrencyCode);
                txtCurrencyValue.Text = value.ElementOrDefault(PayPalExpressCheckoutTemplate.CurrencyValue);
                chkSandbox.Checked = value.ElementOrDefault(PayPalExpressCheckoutTemplate.Sandbox).TryParseBool();
                
            }
        }
    }
}