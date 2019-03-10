using System;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.FilePath;

namespace Admin.UserControls.Catalog
{
    public partial class CategoryView : System.Web.UI.UserControl
    {
        public int CategoryID { get; set; }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            var categories = CategoryService.GetChildCategoriesByCategoryId(CategoryID, false);
            lvCategories.DataSource = categories;
            lvCategories.DataBind();

            lblCategories.Text = categories.Count.ToString();

            commandPanel.Visible = categories.Count != 0;
        }

        protected string RenderCategoryImage(string photoName, string categoryName)
        {
            return string.Format("<img src='{0}' alt='{1}' />", photoName.IsNotEmpty() ? FoldersHelper.GetImageCategoryPath(CategoryImageType.Small, photoName, true) :
                                                                                 "../images/nophoto_small.jpg", categoryName);
        }

        protected void lvCategories_OnItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (string.Equals("DeleteCategory", e.CommandName))
            {
                CategoryService.DeleteCategoryAndPhotos(Convert.ToInt32(e.CommandArgument));
                CategoryService.RecalculateProductsCountManual();
            }
            Response.Redirect(Request.RawUrl);
        }

        protected void lbDeleteSelectedCategories_Click(object sender, EventArgs e)
        {
            foreach (var item in lvCategories.Items)
            {
                var ckb = (CheckBox) item.FindControl("ckbSelectCategory");
                if (ckb != null && ckb.Checked)
                {
                    int categoryId = 0;
                    if (Int32.TryParse(ckb.Attributes["data-categorylist-categoryid"], out categoryId))
                    {
                        CategoryService.DeleteCategoryAndPhotos(categoryId);
                    }
                }
            }
            CategoryService.RecalculateProductsCountManual();
            Response.Redirect(Request.RawUrl);
        }
    }
}