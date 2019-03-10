//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Repository
{
    public class City
    {
        public int CityId { get; set; }
        public int RegionId { get; set; }
        public string Name { get; set; }
        public int CitySort { get; set; }
        public bool DisplayInPopup { get; set; }
        public string PhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
    }
}