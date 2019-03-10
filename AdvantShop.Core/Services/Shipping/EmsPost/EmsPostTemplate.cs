using System.Collections.Generic;

namespace AdvantShop.Shipping.EmsPost
{
    public struct EmsPostTemplate
    {
        public const string ExtraPrice = "ExtraPrice";
        public const string DefaultWeight = "DefaultWeight";
        public const string CityFrom = "CityFrom";
        public const string MaxWeight = "MaxWeight";
        public const string DiscountType = "DiscountType";
    }

    public class EmsPostMaxWeightResponse
    {
        public EmsPostMaxWeight rsp;
    }

    public class EmsPostMaxWeight
    {
        public string stat { get; set; }
        public string err { get; set; }
        public float max_weight { get; set; }
    }

    public class EmsPostLocationsResponse
    {
        public EmsPostLocations rsp;
    }

    public class EmsPostLocations
    {
        public string stat { get; set; }
        public string err { get; set; }
        public List<EmsPostLocation> locations { get; set; }
    }

    public class EmsPostLocation
    {
        public string value { get; set; }
        public string name { get; set; }
        public string type { get; set; }
    }

    public class EmsPostPriceResponse
    {
        public EmsPostPrice rsp;
    }

    public class EmsPostPrice
    {
        public string stat { get; set; }
        public string err { get; set; }
        public float price { get; set; }
        public Dictionary<string, string> term { get; set; }
    }
}