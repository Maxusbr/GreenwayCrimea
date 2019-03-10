//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Orders
{
    [Serializable]
    public class OrderPriceDiscount
    {
        public int OrderPriceDiscountId { get; set; }

        public float PriceRange { get; set; }

        public double PercentDiscount { get; set; }
    }
}