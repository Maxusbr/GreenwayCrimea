using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Catalog
{
    public enum ECatalogShowMethod
    {
        Normal,
        AllProducts,
        OnlyInCategories,
        OnlyWithoutCategories
    }

    public class CatalogFilterModel : BaseFilterModel
    {
        public ECatalogShowMethod ShowMethod { get; set; }


        public int? CategoryId { get; set; }
        public string ColorId { get; set; }
        public string SizeId { get; set; }


        public float PriceFrom { get; set; }
        public float PriceTo { get; set; }

        public int? AmountFrom { get; set; }
        public int? AmountTo { get; set; }

        public int? SortingFrom { get; set; }
        public int? SortingTo { get; set; }

        public string ArtNo { get; set; }

        public string Name { get; set; }

        public bool? HasPhoto { get; set; }

        public bool? Enabled { get; set; }

        public int? BrandId { get; set; }

        public int? ListId { get; set; }

        public string CategorySearch { get; set; }

        public string ExcludeIds { get; set; }
    }

    public class CatalogRangeModel
    {
        public float Min { get; set; }
        public float Max { get; set; }
    }
}