//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using AdvantShop.Catalog;
using AdvantShop.Core.SQL;

namespace Tools.core
{
    public partial class ProcessBrokenCategories : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnShowBrokenCategoriesClick(object sender, EventArgs e)
        {
            lvBrokenCcategories.DataSource =
                SQLDataAccess.ExecuteTable(
                    "Select Name From Catalog.Category as chiledCat Where (Select COUNT(CategoryID) from Catalog.Category Where CategoryID = chiledCat.ParentCategory) = 0",
                    CommandType.Text);
            lvBrokenCcategories.DataBind();
        }

        protected void btnDeleteBrokenCategoriesClick(object sender, EventArgs e)
        {
            var brokenCategories = GetBrokenCategoriesIDs();
            while (brokenCategories != null && brokenCategories.Count > 0)
            {
                foreach (var brokenCategory in brokenCategories)
                {
                    CategoryService.DeleteCategoryAndPhotos(brokenCategory);    
                }
                brokenCategories = GetBrokenCategoriesIDs();
            }
        }
    
        protected List<int> GetBrokenCategoriesIDs()
        {
            return
                SQLDataAccess.ExecuteReadColumn<int>(
                    "(Select CategoryID From Catalog.Category as chiledCat Where (Select COUNT(CategoryID) from Catalog.Category Where CategoryID = chiledCat.ParentCategory) = 0)",
                    CommandType.Text,
                    "CategoryID");
        }

    }
}