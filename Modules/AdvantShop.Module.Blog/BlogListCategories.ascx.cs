using System;
using System.Web.UI.WebControls;
using AdvantShop.Module.Blog.Domain;

namespace AdvantShop.Module.Blog
{
    public partial class BlogListCategories : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            lvBlogCategories.DataSource = BlogService.GetListBlogCategory();
            lvBlogCategories.DataBind();
        }

        protected void lvBlogCategoriesItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "deleteCategory":
                    int itemCategoryId;
                    if (Int32.TryParse(e.CommandArgument.ToString(), out itemCategoryId))
                    {
                        BlogService.DeleteBlogcategory(itemCategoryId);
                    }
                    LoadData();
                    break;
            }
        }
    }
}