namespace AdvantShop.Shipping
{    
    public class BaseShippingPoint
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }
}