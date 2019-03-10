namespace AdvantShop.Shipping.DDelivery
{
    public class DDeliveryPoint : BaseShippingPoint
    {        
        public float Rate { get; set; }        
        public string DeliveryDate { get; set; }

        public int DeliveryCompanyId { get; set; }
        public int DeliveryTypeId { get; set; }
    }
}