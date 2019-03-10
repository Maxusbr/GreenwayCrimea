using System;

namespace AdvantShop.Shipping.ShippingByProductAmount
{
    [Serializable]
    public class ShippingAmountRange
    {
        public float Amount { get; set; }
        public float ShippingPrice { get; set; }

        public override string ToString()
        {
            return Amount + "=" + ShippingPrice;
        }
    }
}