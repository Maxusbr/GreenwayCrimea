using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Shipping.Grastin;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Core.UrlRewriter;
using Newtonsoft.Json;

namespace AdvantShop.Shipping.Grastin
{
    public class GrastinPointOption : BaseShippingOption
    {
        public GrastinEventWidgetData PickpointAdditionalData { get; set; }

        public GrastinPointOption() { }
        public GrastinPointOption(ShippingMethod method)
            : base(method)
        {
            HideAddressBlock = true;
        }
        public float BasePrice { get; set; }
        public float PriceCash { get; set; }

        public GrastinPoint SelectedPoint { get; set; }

        public List<GrastinPoint> ShippingPoints { get; set; }

        public override string Template
        {
            get { return UrlService.GetUrl() + "scripts/_partials/shipping/extendTemplate/GrastinOption.html"; }
        }

        public override void Update(BaseShippingOption option)
        {
            var opt = option as GrastinPointOption;
            if (opt != null && opt.Id == this.Id)
            {
                this.SelectedPoint = opt.SelectedPoint != null ? this.ShippingPoints.FirstOrDefault(x => x.Id == opt.SelectedPoint.Id) : null;
                if (this.SelectedPoint != null)
                    PickpointAdditionalData.PickPointId = this.SelectedPoint.Code;
            }
        }

        public override OrderPickPoint GetOrderPickPoint()
        {
            return new OrderPickPoint
            {
                PickPointId = SelectedPoint != null ? SelectedPoint.Code : string.Empty,
                PickPointAddress = SelectedPoint != null ? SelectedPoint.Address : string.Empty,
                AdditionalData = JsonConvert.SerializeObject(PickpointAdditionalData)
            };
        }

        public override bool ApplyPay(BasePaymentOption payOption)
        {
            if (payOption != null && payOption.GetType() == typeof(PickPointOption))
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
