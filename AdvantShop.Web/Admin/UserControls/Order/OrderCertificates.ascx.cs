//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;

namespace Admin.UserControls.Order
{
    public partial class OrderCertificates : System.Web.UI.UserControl
    {
        public Currency OrderCurrency { get; set; }

        public IList<GiftCertificate> Certificates { get; set; }
    
        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;

            if (!IsPostBack)
            {
                LoadCertificates();
            }
        }

        private void LoadCertificates()
        {
            lvOrderCertificates.DataSource = Certificates;
            lvOrderCertificates.DataBind();
        }
    }
}