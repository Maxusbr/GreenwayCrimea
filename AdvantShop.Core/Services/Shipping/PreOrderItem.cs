using AdvantShop.Catalog;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Orders;

namespace AdvantShop.Shipping
{
    public class PreOrderItem
    {
        public PreOrderItem()
        {
        }

        public PreOrderItem(ShoppingCartItem shoppingCartItem)
        {
            var product = shoppingCartItem.Offer.Product;


            Id = shoppingCartItem.OfferId;
            ArtNo = shoppingCartItem.OfferId.ToString();

            Name = product.Name;
            Price = shoppingCartItem.PriceWithDiscount;
            Amount = shoppingCartItem.Amount;

            ShippingPrice = product.ShippingPrice ?? 0;
            Weight = product.Weight;
            Width = product.Width;
            Height = product.Height;
            Length = product.Length;
        }

        public PreOrderItem(OrderItem orderItem)
        {
            Id = orderItem.OrderItemID;
            ArtNo = orderItem.ArtNo;

            Name = orderItem.Name;
            Price = orderItem.Price;
            Amount = orderItem.Amount;

            var product = orderItem.ProductID != null ? ProductService.GetProduct((int)orderItem.ProductID) : null;
            if (product != null)
            {
                ShippingPrice = product.ShippingPrice ?? 0;
                Weight = product.Weight;
                Width = product.Width;
                Height = product.Height;
                Length = product.Length;
            }
        }

        public PreOrderItem(LeadItem item)
        {
            Name = item.Name;
            Price = item.Price;
            Amount = item.Amount;

            var product = item.ProductId != null ? ProductService.GetProduct((int)item.ProductId) : null;
            if (product != null)
            {
                ShippingPrice = product.ShippingPrice ?? 0;
                Weight = product.Weight;
                Width = product.Width;
                Height = product.Height;
                Length = product.Length;
            }
        }

        public int Id { get; set; }
        public string ArtNo { get; set; }

        public string Name { get; set; }
        public float Price { get; set; }
        public float ShippingPrice { get; set; }
        public float Amount { get; set; }
        public float Weight { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Length { get; set; }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^
                   Price.GetHashCode() ^
                   ShippingPrice.GetHashCode() ^
                   Amount.GetHashCode() ^
                   Weight.GetHashCode() ^
                   Width.GetHashCode() ^
                   Height.GetHashCode() ^
                   Length.GetHashCode();
        }
    }
}