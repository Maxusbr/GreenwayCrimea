using System;

namespace AdvantShop.Shipping.ShippingByOrderPrice
{
    [Serializable]
    public class ShippingPriceRange
    {
        public float OrderPrice { get; set; }
        public float ShippingPrice { get; set; }

        public override string ToString()
        {
            return OrderPrice + "=" + ShippingPrice;
        }
    }
}