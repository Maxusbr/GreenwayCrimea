using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Settings
{
    public class AdminRegionFilterModel : BaseFilterModel
    {
        public int RegionId { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string RegionCode { get; set; }
        public int SortOrder { get; set; }
        public int? SortingFrom { get; set; }
        public int? SortingTo { get; set; }
        public int? id { get; set; }
    }
}
