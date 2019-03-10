using AdvantShop.Core.Models;
using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.Models.Catalog
{
    public class ProductListPagingModel
    {
        public ProductListPagingModel(bool indepth)
        {
            Filter = new CategoryFiltering() {Indepth = indepth};
        }
        
        public ProductViewModel Products { get; set; }

        public Pager Pager { get; set; }

        public CategoryFiltering Filter { get; set; }        
    }
}