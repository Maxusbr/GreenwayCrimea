using AdvantShop.Orders;
using AdvantShop.Payment;
using Newtonsoft.Json;

namespace AdvantShop.Shipping.DDelivery
{
    public class DDeliveryOption : BaseShippingOption
    {
        public DDeliveryOption() { }
        public DDeliveryOption(ShippingMethod method)
            : base(method)
        {
            HideAddressBlock = false;
        }

        public int DeliveryCompanyId { get; set; }
        public int DeliveryTypeId { get; set; }

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