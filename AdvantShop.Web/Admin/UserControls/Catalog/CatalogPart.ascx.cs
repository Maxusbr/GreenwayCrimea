using System;
using System.Web;
using System.Web.UI;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;

namespace Admin.UserControls.Catalog
{
    public partial class CatalogPart : UserControl
    {
        protected bool SelectedTotalProducts = false;
        protected bool SelectedNew = false;
        protected bool SelectedBest = false;
        //private string _url = HttpContext.Current.Request.RawUrl.ToLower();
        
        public enum SelectedItem
        {
            None,
            AllProducts,
            WithoutCategory,
            New,
            Best,
            Sale
        }

        protected SelectedItem selectedItem;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (System.IO.Path.GetFileName(Request.Path).ToLower() == "catalog.aspx" && Request["categoryid"].IsNotEmpty())
            {
                switch (Request["categoryid"].ToLower())
                {
                    case "allproducts" :
                        selectedItem = SelectedItem.AllProducts;
                        break;
                    case "withoutcategory":
                        selectedItem = SelectedItem.WithoutCategory;
                        break;
                }
            }
            else if (System.IO.Path.GetFileName(Request.Path).ToLower() == "productsonmain.aspx" && Request["type"].IsNotEmpty())
            {
                switch (Request["type"].ToLower())
                {
                    case "new":
                        selectedItem = SelectedItem.New;
                        break;
                    case "best":
                        selectedItem = SelectedItem.Best;
                        break;
                    case "sale":
                        selectedItem = SelectedItem.Sale;
                        break;
                }
            }
        }

        protected void lbRecalculate_Click(object sender, EventArgs e)
        {
            CategoryService.RecalculateProductsCountManual();
            CategoryService.SetCategoryHierarchicallyEnabled(0);
            ProductService.PreCalcProductParamsMassInBackground();
            Response.Redirect(Request.Url.ToString());
        }
    }
}