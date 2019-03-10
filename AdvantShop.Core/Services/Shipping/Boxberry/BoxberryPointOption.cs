using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Orders;
using Newtonsoft.Json;
using AdvantShop.Payment;

namespace AdvantShop.Shipping.Boxberry
{
    public class BoxberryPointOption : BaseShippingOption
    {
        public BoxberryPointOption() { }
        public BoxberryPointOption(ShippingMethod method)
            : base(method)
        {
            HideAddressBlock = true;
        }

        public BoxberryPoint SelectedPoint { get; set; }

        public List<BoxberryPoint> ShippingPoints { get; set; }

        public float BasePrice { get; set; }
        public float PriceCash { get; set; }
        public bool OnlyPrepaidOrders { get; set; }


        public override string Template
        {
            get { return UrlService.GetUrl() + "scripts/_partials/shipping/extendTemplate/BoxberryOption.html"; }
        }

        public override void Update(BaseShippingOption option)
        {
            var opt = option as BoxberryPointOption;
            if (opt != null && opt.Id == this.Id)
            {
                this.SelectedPoint = opt.SelectedPoint != null ? this.ShippingPoints.FirstOrDefault(x => x.Id == opt.SelectedPoint.Id) : null;
                var selectedPoint = this.SelectedPoint;
                if (selectedPoint != null)
                {
                    this.Rate = selectedPoint.BasePrice;
                    this.BasePrice = selectedPoint.BasePrice;
                    this.PriceCash = selectedPoint.PriceCash;
                    this.OnlyPrepaidOrders = selectedPoint.OnlyPrepaidOrders;
                }
            }
        }

        public override OrderPickPoint GetOrderPickPoint()
        {
            return new OrderPickPoint
            {
                PickPointId = Id,
                PickPointAddress = SelectedPoint != null ? SelectedPoint.Address : string.Empty,
                AdditionalData = SelectedPoint != null ? JsonConvert.SerializeObject(SelectedPoint) : null
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