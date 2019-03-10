using AdvantShop.Catalog;
using AdvantShop.Module.CategoriesOnMainPage.Service;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AdvantShop.Module.CategoriesOnMainPage.Models
{
    public class CategoryOnMainPage
    {
        public CategoryOnMainPage()
        {
            CategoryId = CategoryService.DefaultNonCategoryId;
        }

        public int CategoryId { get; set; }

        public string NameUpCategory { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string URL { get; set; }

        public int SortOrder { get; set; }
    }

    public class CategoriesOnMainPageView
    {
        public CategoriesOnMainPageView()
        {
            Categories = new List<CategoryOnMainPage>();
        }

        public int CountCategoriesInLine { get; set; }

        public List<CategoryOnMainPage> Categories { get; set; }
    }
}
