namespace AdvantShop.Shipping.CheckoutRu
{
    public enum DeliveryType
    {
        Postamat,
        Pvz,
        Express,
        Mail
    }

    public class CheckoutDeliveryModel
    {
        public int DeliveryId { get; set; }
        public float Cost { get; set; }
        public string Address { get; set; }
        public string Code { get; set; }
        public string Latiude { get; set; }
        public string Longitude { get; set; }
        public int MinDeliveryTerm { get; set; }
        public int MaxDeliveryTerm { get; set; }
        public string AdditionalInfo { get; set; }
        public bool Npp { get; set; }
        public DeliveryType CheckoutDeliveryType { get; set; }
    }
}