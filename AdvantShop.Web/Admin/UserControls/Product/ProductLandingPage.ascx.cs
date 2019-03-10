using System;
using System.Collections.Generic;
using AdvantShop.Core.Services.Catalog;
using System.IO;
using System.Linq;

namespace Admin.UserControls.Products
{
    public partial class ProductLandingPage : System.Web.UI.UserControl
    {
        public int ProductID { set; get; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ProductID == 0)
                return;

            if (!IsPostBack)
            {
                var views = new List<string>();
                var path = Server.MapPath("~/landings/templates/");
                if (Directory.Exists(path))
                {
                    views.AddRange(Directory.GetDirectories(path).Select(p => new DirectoryInfo(p).Name));
                }

                lvCustomViews.DataSource = views;
                lvCustomViews.DataBind();


                ckTabBody.Text = ProductLandingPageService.GetDescriptionByProductId(ProductID, true);
            }
        }

        public void Save()
        {
            if (ProductID == 0)
                return;

            ProductLandingPageService.UpdateProductDescription(ProductID, ckTabBody.Text);
        }


    }
}