using AdvantShop.Core.Models;
using AdvantShop.Models.Catalog;

namespace AdvantShop.Areas.Mobile.Models.Catalog
{
    public class ProductListPagingMobileModel
    {
        public ProductListPagingMobileModel(bool indepth)
        {
            Filter = new CategoryFiltering() {Indepth = indepth};
        }
        
        public ProductViewMobileModel Products { get; set; }

        public Pager Pager { get; set; }

        public CategoryFiltering Filter { get; set; }
    }
}