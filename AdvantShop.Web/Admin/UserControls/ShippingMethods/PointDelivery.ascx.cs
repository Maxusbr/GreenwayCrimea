using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Shipping.PointDelivery;
using Newtonsoft.Json;

namespace Admin.UserControls.ShippingMethods
{
    public partial class PointDeliveryControl : ParametersControl
    {
        //private List<PointDeliveryPoint> Points
        //{
        //    get { return ((List<PointDeliveryPoint>)ViewState["Points"]); }
        //    set { ViewState["Points"] = value; }
        //}

        private List<string> Points
        {
            get { return ((List<string>)ViewState["Points"]); }
            set { ViewState["Points"] = value; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                {                    //{PointDeliveryTemplate.Points, JsonConvert.SerializeObject(Points)}
                    { PointDeliveryTemplate.Points, Points != null && Points.Count > 0 ? Points.Aggregate((workingSentence, next) =>
                    next + ";" + workingSentence) : string.Empty},
                    { PointDeliveryTemplate.DeliveryTime, txtDeliveryTime.Text },
                    { PointDeliveryTemplate.ShippingPrice, txtShippingPrice.Text }
                };
            }
            set
            {
                txtDeliveryTime.Text = value.ElementOrDefault(PointDeliveryTemplate.DeliveryTime);
                txtShippingPrice.Text = value.ElementOrDefault(PointDeliveryTemplate.ShippingPrice);
                //Points = new List<PointDeliveryPoint>();
                Points = new List<string>();
                string param = value.ElementOrDefault(PointDeliveryTemplate.Points);
                if (param.IsNotEmpty())
                {
                    //Points = JsonConvert.DeserializeObject<List<PointDeliveryPoint>>(param);
                    foreach (var item in param.Split(';'))
                    {
                        Points.Add(item);
                    }
                }
                BindRepeater();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            //Points.Add(new PointDeliveryPoint
            //{
            //    Address = txtNewPoint.Text,
            //    TimeSpan = Convert.ToInt32(txtNewPointTimeSpan.Text),
            //    Rate = Convert.ToSingle(txtNewPointPrice.Text)
            //});

            Points.Add(txtNewPoint.Text);

            BindRepeater();
            txtNewPoint.Text = string.Empty;
            //txtNewPointTimeSpan.Text = string.Empty;
            //txtNewPointPrice.Text = string.Empty;
        }

        protected void rPoints_Delete(object sender, RepeaterCommandEventArgs e)
        {
            //var pointToRemove = Points.FirstOrDefault(item => item.Address == e.CommandArgument.ToString());
            //if (pointToRemove != null)
            if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
            {
                //Points.Remove(pointToRemove);
                Points.Remove(e.CommandArgument.ToString());
                BindRepeater();
            }
        }

        private void BindRepeater()
        {
            rPoints.DataSource = Points;
            rPoints.DataBind();
        }
    }
}