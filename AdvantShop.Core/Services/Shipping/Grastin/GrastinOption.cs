using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Shipping.Grastin;
using AdvantShop.Orders;
using AdvantShop.Payment;
using Newtonsoft.Json;

namespace AdvantShop.Shipping.Grastin
{
    public class GrastinOption : BaseShippingOption
    {
        public GrastinEventWidgetData PickpointAdditionalData { get; set; }

        public GrastinOption() { }
        public GrastinOption(ShippingMethod method)
            : base(method)
        {
        }
        public float BasePrice { get; set; }
        public float PriceCash { get; set; }

        public override OrderPickPoint GetOrderPickPoint()
        {
            return new OrderPickPoint
            {
                PickPointId = string.Empty,
                PickPointAddress = string.Empty,
                AdditionalData = JsonConvert.SerializeObject(PickpointAdditionalData)
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

        public override string GetDescriptionForPayment()
        {
            var diff = PriceCash - BasePrice;
            if (diff <= 0)
                return string.Empty;

            return string.Format("Стоимость доставки увеличится на {0}", diff.FormatPrice());
        }
    }
}
