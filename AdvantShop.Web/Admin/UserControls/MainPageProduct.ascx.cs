//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Catalog;

namespace Admin.UserControls
{
    public partial class MainPageProduct : System.Web.UI.UserControl
    {
        public EProductOnMain Flag;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void LoadData()
        {
            var data = ProductOnMain.GetAdminProductsByType(Flag, 9);
            switch (Flag)
            {
                case EProductOnMain.Best:
                    locName.Text = Resources.Resource.Admin_UserControls_MainPageProduct_Bestseller;
                    break;
                case EProductOnMain.New:
                    locName.Text = Resources.Resource.Admin_UserControls_MainPageProduct_New;
                    break;
                case EProductOnMain.Sale:
                    locName.Text = Resources.Resource.Admin_UserControls_MainPageProduct_Discount;
                    break;
            }

            if (data.Rows.Count > 0)
            {
                rMainProduct.DataSource = data;
                rMainProduct.DataBind();
            }
            else
            {
                ProductsNoRecordsBlock.Visible = true;
            }
        }
    }
}