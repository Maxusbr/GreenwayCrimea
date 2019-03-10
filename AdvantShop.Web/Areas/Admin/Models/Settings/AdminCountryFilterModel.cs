using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Settings
{
    public class AdminCountryFilterModel : BaseFilterModel
    {
        public string Name { get; set; }

        public string ISO2 { get; set; }

        public string ISO3 { get; set; }

        public bool? DisplayInPopup { get; set; }

        public int SortOrder { get; set; }

        public int? DialCode { get; set; }

        public int? SortingFrom { get; set; }

        public int? SortingTo { get; set; }

        public int CountryId { get; set; }
    }
}
