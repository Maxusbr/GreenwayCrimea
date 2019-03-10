namespace AdvantShop.Module.YaBuying.Domain
{
    public class YaMarketShipping
    {
        public int Id { get; set; }
        public int ShippingMethodId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int MinDate { get; set; }
        public int MaxDate { get; set; }
    }
}