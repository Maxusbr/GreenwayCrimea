using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Brands
{
    public class AdminBrandFilterModel : BaseFilterModel
    {
        public string BrandName { get; set; }

        public int ProductsCount { get; set; }

        public bool? Enabled { get; set; }

        public int SortOrder { get; set; }

        public int CountryId { get; set; }     

        public int SortingFrom { get; set; }

        public int SortingTo { get; set; }

        public bool? HasPhoto { get; set; }
    }
    
    public class AdminBrandRangeModel
    {
        public float Min { get; set; }
        public float Max { get; set; }
    }
}