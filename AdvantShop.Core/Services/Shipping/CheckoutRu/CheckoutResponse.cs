namespace AdvantShop.Shipping.CheckoutRu
{
    public class CheckoutResponse
    {
        public CheckoutOrder order { get; set; }
        public bool error { get; set; }
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
    }

    public class CheckoutOrder
    {
        public int id { get; set; }
    }
}