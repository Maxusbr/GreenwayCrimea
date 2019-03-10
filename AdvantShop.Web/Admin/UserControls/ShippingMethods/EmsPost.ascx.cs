using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Shipping.EmsPost;

namespace Admin.UserControls.ShippingMethods
{
    public partial class EmsPostControl : ParametersControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            ddlShippingCityFrom.Items.AddRange(EmsPostService.GetCities().Select(city => new ListItem(city.name, city.value)).ToArray());
            ddlShippingCityFrom.Items.AddRange(EmsPostService.GetRegions().Select(city => new ListItem(city.name, city.value)).ToArray());
        }
        public override Dictionary<string, string> Parameters
        {
            get
            {
                var maxWeight = EmsPostService.GetMaxWeight();
                
                return _valid ||
                       ValidateFormData(new[] {txtShippingWeight, txtExtraPrice},
                                        new[] {txtShippingWeight, txtExtraPrice},
                                        new TextBox[0])
                    ? new Dictionary<string, string>
                    {
                        {EmsPostTemplate.CityFrom, ddlShippingCityFrom.SelectedValue},
                        {EmsPostTemplate.DefaultWeight, txtShippingWeight.Text},
                        {EmsPostTemplate.ExtraPrice, txtExtraPrice.Text},
                        {EmsPostTemplate.MaxWeight, maxWeight != 0 ? maxWeight.ToString() : 31.5.ToString()}
                    }
                    : null;
            }
            set
            {
                ddlShippingCityFrom.SelectedValue = value.ElementOrDefault(EmsPostTemplate.CityFrom);
                txtShippingWeight.Text = value.ElementOrDefault(EmsPostTemplate.DefaultWeight) ?? "1";
                txtExtraPrice.Text = value.ElementOrDefault(EmsPostTemplate.ExtraPrice) ?? "0";
                lblMaxWeight.Text = value.ElementOrDefault(EmsPostTemplate.MaxWeight);
            }
        }
    }
}