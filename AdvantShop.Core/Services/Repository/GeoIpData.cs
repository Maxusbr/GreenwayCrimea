namespace AdvantShop.Repository
{
    public class GeoIpData
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }

        public GeoIpData()
        {
            Country = string.Empty;
            State = string.Empty;
            City = string.Empty;
        }
    }
}