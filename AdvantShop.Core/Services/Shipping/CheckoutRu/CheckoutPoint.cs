namespace AdvantShop.Shipping.CheckoutRu
{
    public class CheckoutPoint : BaseShippingPoint
    {
        public string Delivery { get; set; }
        public string DeliveryType { get; set; }
        public string MinDeliveryTerm { get; set; }
        public string MaxDeliveryTerm { get; set; }
        public float Rate { get; set; }
    }
}