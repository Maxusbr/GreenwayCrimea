using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Shipping.ShippingByProductAmount;

namespace Admin.UserControls.ShippingMethods
{
    public partial class ShippingByProductAmountControl : ParametersControl
    {
        private List<ShippingAmountRange> Ranges
        {
            get { return ((List<ShippingAmountRange>)ViewState["Ranges"]); }
            set { ViewState["Ranges"] = value; }
        }


        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                    {
                        {ShippingByProductAmountTemplate.PriceRanges, Ranges.AggregateString(';')},
                        {ShippingByProductAmountTemplate.DeliveryTime, txtDeliveryTime.Text }
                    };
            }
            set
            {
                Ranges = new List<ShippingAmountRange>();
                string param = value.ElementOrDefault(ShippingByProductAmountTemplate.PriceRanges);
                if (param.IsNotEmpty())
                {
                    foreach (var item in param.Split(';'))
                    {
                        Ranges.Add(new ShippingAmountRange()
                        {
                            Amount = item.Split('=')[0].TryParseFloat(),
                            ShippingPrice = item.Split('=')[1].TryParseFloat()
                        });
                    }
                }
                BindRepeater();
                txtDeliveryTime.Text = value.ElementOrDefault(ShippingByProductAmountTemplate.DeliveryTime);
            }
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Ranges.Add(new ShippingAmountRange()
                {
                    Amount = txtAmount.Text.TryParseFloat(),
                    ShippingPrice = txtShippingPrice.Text.TryParseFloat()
                });

            BindRepeater();
            txtAmount.Text = "";
            txtShippingPrice.Text = "";
        }

        protected void rRanges_Delete(object sender, RepeaterCommandEventArgs e)
        {
            Ranges.RemoveAll(item => item.Amount == e.CommandArgument.ToString().TryParseFloat());
            BindRepeater();
        }

        private void BindRepeater()
        {
            rRanges.DataSource = Ranges.OrderBy(item => item.Amount);
            rRanges.DataBind();
        }
    }
}