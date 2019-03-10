namespace AdvantShop.Repository
{
    public class IpZone
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public int RegionId { get; set; }
        public string Region { get; set; }

        public int CityId { get; set; }
        public string City { get; set; }

        public int? DialCode { get; set; }

        public IpZone()
        {
            CountryName = string.Empty;
            Region = string.Empty;
            City = string.Empty;
        }
    }
}