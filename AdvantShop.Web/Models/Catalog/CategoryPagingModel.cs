using AdvantShop.Core.Models;
using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.Models.Catalog
{
    public class CategoryPagingModel
    {
        public CategoryPagingModel(int categoryId, bool indepth)
        {
            Filter = new CategoryFiltering(categoryId, indepth);
        }

        public ProductViewModel Products { get; set; }

        public Pager Pager { get; set; }

        public CategoryFiltering Filter { get; set; }
    }
}