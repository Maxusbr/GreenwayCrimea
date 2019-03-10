using AdvantShop.Core.Models;
using AdvantShop.Models.Catalog;

namespace AdvantShop.Areas.Mobile.Models.Catalog
{
    public class CategoryPagingMobileModel
    {
        public CategoryPagingMobileModel(int categoryId, bool indepth)
        {
            Filter = new CategoryFiltering(categoryId, indepth);
        }

        public ProductViewMobileModel Products { get; set; }

        public Pager Pager { get; set; }

        public CategoryFiltering Filter { get; set; }
    }
}