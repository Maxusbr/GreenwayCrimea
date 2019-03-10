using AdvantShop.Catalog;

namespace AdvantShop.Models.Catalog
{
    public partial class CategoryModel : BaseModel
    {
        public int? CategoryId { get; set; }

        public string Url { get; set; }

        public string TagUrl { get; set; }
        public int TagId { get; set; }

        public bool Indepth { get; set; }
        
        public int? Page { get; set; }

        public string Brand { get; set; }

        public string Size { get; set; }

        public string Color { get; set; }

        public float? PriceFrom { get; set; }

        public float? PriceTo { get; set; }

        public string Prop { get; set; }

        public bool Available { get; set; }

        //public string Preorder { get; set; }

        public ESortOrder? Sort { get; set; }

        public string ViewMode { get; set; }

        public string SearchQuery { get; set; }
    }
}