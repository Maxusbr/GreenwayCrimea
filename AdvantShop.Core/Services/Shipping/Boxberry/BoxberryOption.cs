using AdvantShop.Orders;
using AdvantShop.Payment;
using Newtonsoft.Json;

namespace AdvantShop.Shipping.Boxberry
{
    public class BoxberryOption : BaseShippingOption
    {
        public BoxberryOption() { }
        public BoxberryOption(ShippingMethod method)
            : base(method)
        {
            HideAddressBlock = false;
        }

        public float BasePrice { get; set; }
        public float PriceCash { get; set; }
        public bool OnlyPrepaidOrders { get; set; }

        public override OrderPickPoint GetOrderPickPoint()
        {
            return new OrderPickPoint
            {
                PickPointId = Id,
                PickPointAddress = string.Empty,
                AdditionalData = JsonConvert.SerializeObject(this)
            };
        }

        public override bool ApplyPay(BasePaymentOption payOption)
        {
            if (payOption != null && payOption.GetType() == typeof(CashOnDeliverytOption))
                Rate = PriceCash;
            else
            {
                Rate = BasePrice;
            }
            return true;
        }
    }
}