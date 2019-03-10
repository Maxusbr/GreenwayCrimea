using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Shipping.ShippingByOrderPrice;

namespace Admin.UserControls.ShippingMethods
{
    public partial class ShippingByOrderPriceControl : ParametersControl
    {
        private List<ShippingPriceRange> Ranges
        {
            get { return ((List<ShippingPriceRange>)ViewState["Ranges"]); }
            set { ViewState["Ranges"] = value; }
        }


        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                    {
                        {ShippingByOrderPriceTemplate.PriceRanges, Ranges.AggregateString(";")},
                        {ShippingByOrderPriceTemplate.DependsOnCartPrice, rbCart.Checked.ToString(CultureInfo.InvariantCulture)},
                        {ShippingByOrderPriceTemplate.DeliveryTime, txtDeliveryTime.Text }

                    };
            }
            set
            {
                Ranges = new List<ShippingPriceRange>();
                string param = value.ElementOrDefault(ShippingByOrderPriceTemplate.PriceRanges);
                if (param.IsNotEmpty())
                {
                    foreach (var item in param.Split(';'))
                    {
                        Ranges.Add(new ShippingPriceRange()
                            {
                                OrderPrice = item.Split('=')[0].TryParseFloat(),
                                ShippingPrice = item.Split('=')[1].TryParseFloat()
                            });
                    }
                }
                BindRepeater();
                rbCart.Checked = value.ElementOrDefault(ShippingByOrderPriceTemplate.DependsOnCartPrice).TryParseBool();
                rbTotalPrice.Checked = !rbCart.Checked;
                txtDeliveryTime.Text = value.ElementOrDefault(ShippingByOrderPriceTemplate.DeliveryTime);
            }
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Ranges.Add(new ShippingPriceRange()
                {
                    OrderPrice = txtOrderPrice.Text.TryParseFloat(),
                    ShippingPrice = txtShippingPrice.Text.TryParseFloat()
                });

            BindRepeater();
            txtOrderPrice.Text = "";
            txtShippingPrice.Text = "";
        }

        protected void rRanges_Delete(object sender, RepeaterCommandEventArgs e)
        {
            Ranges.RemoveAll(item => item.OrderPrice == e.CommandArgument.ToString().TryParseFloat());
            BindRepeater();
        }

        private void BindRepeater()
        {
            rRanges.DataSource = Ranges.OrderBy(item => item.OrderPrice);
            rRanges.DataBind();
        }

    }
}