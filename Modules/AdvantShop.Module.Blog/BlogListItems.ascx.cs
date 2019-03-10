using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop.Core.Modules;
using AdvantShop.Module.Blog.Domain;

namespace AdvantShop.Module.Blog
{
    public partial class BlogListItems : UserControl
    {
        protected static List<BlogCategory> categories;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            categories = BlogService.GetListBlogCategory();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            lvBlogItems.DataSource = BlogService.GetListBlogItem(false);
            lvBlogItems.DataBind();
        }

        protected void lvBlogItemsItemCommand(object sender, ListViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "deleteItem":
                    int itemId;
                    if (Int32.TryParse(e.CommandArgument.ToString(), out itemId))
                    {
                        BlogService.DeleteBlogItem(itemId);
                    }
                    break;
            }
        }

        protected string RenderCategoryName(int itemCategoryId)
        {
            BlogCategory category = BlogService.GetBlogCategory(itemCategoryId);
            if (category != null)
            {
                return category.Name;
            }
            return string.Empty;
        }

        protected string RenderItemLink(string itemUrlPath, int itemCategoryId)
        {
            return "../" + ModuleSettingsProvider.GetSettingValue<string>("PageUrlPath", Blog.ModuleID) + "/" + itemUrlPath;
        }
    }
}