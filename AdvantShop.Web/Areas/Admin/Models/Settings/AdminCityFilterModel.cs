using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Repository;

namespace AdvantShop.Web.Admin.Models.Settings
{
    public class AdminCityFilterModel : BaseFilterModel
    {
        public int CityId { get; set; }
        public int RegionId { get; set; }
        public string Name { get; set; }
        public string CitySort { get; set; }
        public int? SortingFrom { get; set; }
        public int? SortingTo { get; set; }
        public bool? DisplayInPopup { get; set; }
        public string PhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public int? id { get; set; }
        public int? cityCountrys { get; set; }
    }
}