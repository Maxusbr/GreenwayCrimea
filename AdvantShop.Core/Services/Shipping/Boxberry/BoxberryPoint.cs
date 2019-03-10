namespace AdvantShop.Shipping.Boxberry
{
    public class BoxberryPoint : BaseShippingPoint
    {
        public bool OnlyPrepaidOrders { get; set; }
        public float BasePrice { get; set; }
        public float PriceCash { get; set; }
        public string DeliveryPeriod { get; set; }
    }
}