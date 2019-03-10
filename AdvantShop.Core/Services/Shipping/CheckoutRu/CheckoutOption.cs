using AdvantShop.Orders;
using Newtonsoft.Json;

namespace AdvantShop.Shipping.CheckoutRu
{
    public class CheckoutOption : BaseShippingOption
    {
        public CheckoutOption() { }
        public CheckoutOption(ShippingMethod method)
            : base(method)
        {
            HideAddressBlock = false;
        }

        [JsonProperty(PropertyName = "Delivery")]
        public new string DeliveryId { get; set; }

        [JsonProperty(PropertyName = "DeliveryType")]
        public string DeliveryType { get; set; }

        [JsonProperty(PropertyName = "MinDeliveryTerm")]
        public string MinDeliveryTerm { get; set; }

        [JsonProperty(PropertyName = "MaxDeliveryTerm")]
        public string MaxDeliveryTerm { get; set; }

        public override OrderPickPoint GetOrderPickPoint()
        {
            return new OrderPickPoint
            {
                PickPointId = Id,
                PickPointAddress = string.Empty,
                AdditionalData = JsonConvert.SerializeObject(this)
            };
        }
    }
}