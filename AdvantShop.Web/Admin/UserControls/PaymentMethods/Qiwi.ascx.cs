using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class QiwiControl : ParametersControl
    {
        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtQiwiId, txtPassword, txtPasswordNotify, txtProviderName, txtCurrencyCode, txtCurrencyValue },
                                        new[] { txtCurrencyValue })
                           ? new Dictionary<string, string>
                               {
                                   {QiwiTemplate.ProviderID, txtQiwiId.Text},
                                   {QiwiTemplate.RestID, txtRestID.Text},
                                   {QiwiTemplate.ProviderName, txtProviderName.Text},
                                   {QiwiTemplate.Password, txtPassword.Text},
                                   {QiwiTemplate.PasswordNotify, txtPasswordNotify.Text},
                                   {QiwiTemplate.CurrencyCode, txtCurrencyCode.Text},
                                   {QiwiTemplate.CurrencyValue, txtCurrencyValue.Text},
                               }
                           : null;
            }
            set
            {
                txtQiwiId.Text = value.ElementOrDefault(QiwiTemplate.ProviderID);
                txtRestID.Text = value.ElementOrDefault(QiwiTemplate.RestID);
                txtProviderName.Text = value.ElementOrDefault(QiwiTemplate.ProviderName);
                txtPassword.Text = value.ElementOrDefault(QiwiTemplate.Password);
                txtPasswordNotify.Text = value.ElementOrDefault(QiwiTemplate.PasswordNotify);
                txtCurrencyCode.Text = value.ElementOrDefault(QiwiTemplate.CurrencyCode);
                txtCurrencyValue.Text = value.ElementOrDefault(QiwiTemplate.CurrencyValue);
            }
        }

    }
}