using System.Collections.Generic;
using AdvantShop.Catalog;

namespace AdvantShop.Areas.Mobile.Models.Catalog
{
    public class CategoryListMobileViewModel
    {
        public List<Category> Categories { get; set; }
        public bool DisplayProductCount { get; set; }
        public int PhotoHeight { get; set; }
        public int CountCategoriesInLine { get; set; }
        public ECategoryDisplayStyle DisplayStyle { get; set; }
    }
}