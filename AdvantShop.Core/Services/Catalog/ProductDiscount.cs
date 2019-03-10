using System;

namespace AdvantShop.Catalog
{
    public class ProductDiscount
    {
        public int ProductId { get; set; }
        public float Discount { get; set; }
        public DateTime? DateExpired { get; set; }
    }
}