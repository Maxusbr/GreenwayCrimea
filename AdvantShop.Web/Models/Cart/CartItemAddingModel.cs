namespace AdvantShop.Models.Cart
{
    public class CartItemAddingModel
    {
        public int OfferId { get; set; }
        public int ProductId { get; set; }
        public float Amount { get; set; }
        public string AttributesXml { get; set; }
        public int Payment { get; set; }
    }
}