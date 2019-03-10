using System.Collections.Generic;
using AdvantShop.Catalog;

namespace AdvantShop.Models.Cart
{
    public class CartItemModel
    {
        public string Price { get; set; }
        public string PriceWithDiscount { get; set; }
        public Discount Discount { get; set; }
        public string DiscountText { get; set; }
        public float Amount { get; set; }
        public string Sku { get; set; }
        public string PhotoPath { get; set; }
        public string PhotoAlt { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Cost { get; set; }
        public int ShoppingCartItemId { get; set; }
        public List<EvaluatedCustomOptions> SelectedOptions { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string Avalible { get; set; }
        public float AvailableAmount { get; set; }
        public float MinAmount { get; set; }
        public float MaxAmount { get; set; }
        public float Multiplicity { get; set; }
        public bool FrozenAmount { get; set; }
        public bool IsGift { get; set; }
        public string Unit { get; set; }
    }
}