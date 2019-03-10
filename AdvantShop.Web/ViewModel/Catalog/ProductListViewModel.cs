using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Core.Models;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Models;
using AdvantShop.Models.Catalog;

namespace AdvantShop.ViewModel.Catalog
{
    public partial class ProductListViewModel : BaseModel
    {
        public string Title { get; set; }

        public EProductOnMain Type { get; set; }

        public int ListId { get; set; }

        public ProductViewModel Products { get; set; }

        public Pager Pager { get; set; }

        public CategoryFiltering Filter { get; set; }

        public List<BreadCrumbs> BreadCrumbs { get; set; }

        public bool ShowNew { get; set; }
        public bool ShowBest { get; set; }
        public bool ShowSale { get; set; }
        public List<ProductList> ProductLists { get; set; } 

        public bool HasProducts
        {
            get { return Filter != null && Products != null && Products.Products.Count > 0; }
        }

        public TagViewModel TagView { get; set; }

        //filtering
        public int? Page { get; set; }

        public float? PriceFrom { get; set; }

        public float? PriceTo { get; set; }

        public string Brand { get; set; }

        public ESortOrder Sort { get; set; }
        public Tag Tag { get; internal set; }

        public string Description { get; set; }

        //filtering
    }
}