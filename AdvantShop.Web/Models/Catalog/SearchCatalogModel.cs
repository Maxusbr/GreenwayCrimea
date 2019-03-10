using AdvantShop.Catalog;
using AdvantShop.Core.Models;
using AdvantShop.ViewModel.Catalog;
using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.Models.Catalog
{
    public class SearchCatalogViewModel : BaseModel
    {
        //filtering
        public int? Page { get; set; }

        public string Q { get; set; }

        public float? PriceFrom { get; set; }

        public float? PriceTo { get; set; }

        public string Brand { get; set; }

        public int CategoryId { get; set; }

        public ESortOrder Sort { get; set; }
        //filtering

        public string ViewMode { get; set; }

        public bool AllowChangeViewMode { get; set; }

        public ProductViewModel Products { get; set; }

        public CategoryListViewModel Categories { get; set; }

        public Pager Pager { get; set; }

        public EProductOnMain TypeFlag { get; set; }

        public bool HasProducts
        {
            get { return Products != null && Products.Products.Count > 0; }
        }
    }
}