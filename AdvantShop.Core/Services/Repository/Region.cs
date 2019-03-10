//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Repository
{
    public class Region
    {
        public int RegionId { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string RegionCode { get; set; }
        public int SortOrder { get; set; }
    }
}