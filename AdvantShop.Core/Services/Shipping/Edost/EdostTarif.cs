namespace AdvantShop.Shipping.Edost
{
    public class EdostTarif
    {
        public int Id { get; set; }
        public float Price { get; set; }
        public float PriceCash { get; set; }
        public float PriceTransfer { get; set; }
        public string Name { get; set; }
        public string PickpointMap { get; set; }
        public string Company { get; set; }
        public string Day { get; set; }
    }
}