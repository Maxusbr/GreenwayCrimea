using System;
using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Payment;

namespace Admin.UserControls.PaymentMethods
{
    public partial class DibsControl : ParametersControl
    {
        public void Page_Load(object sender, EventArgs e)
        {
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return _valid ||
                       ValidateFormData(new[] { txtMerchant, txtCurrencyRate })
                           ? new Dictionary<string, string>
                               {
                                   {DibsTemplate.Merchant, txtMerchant.Text},
                                   {DibsTemplate.CurrencyRate, txtCurrencyRate.Text},
                                   {DibsTemplate.Currency, ddlCurrencies.SelectedValue},
                               }
                           : null;
            }
            set
            {
                txtMerchant.Text = value.ElementOrDefault(DibsTemplate.Merchant);
                txtCurrencyRate.Text = value.ElementOrDefault(DibsTemplate.CurrencyRate);

                ddlCurrencies.Items.Clear();
                ddlCurrencies.SelectedValue = null;

                var items = new List<object>();
                foreach (var currency in Dibs.GetCurrencies())
                {
                    items.Add(new
                        {
                            value = currency.Key,
                            text = currency.Value
                        });
                }

                ddlCurrencies.DataSource = items;
                ddlCurrencies.DataBind();

                ddlCurrencies.SelectedValue = value.ElementOrDefault(DibsTemplate.Currency);
            }
        }
    }
}