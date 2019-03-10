using System;

namespace AdvantShop.Shipping.NovaPoshta
{
    public class NovaRequestPrice
    {
        public Guid CitySender { get; set; }
        public Guid CityRecipient { get; set; }
        public string ServiceType { get; set; }
        public float Weight { get; set; }
        public float Cost { get; set; }
    }
}