using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Orders;
using Newtonsoft.Json;

namespace AdvantShop.Shipping.CheckoutRu
{
    public class CheckoutPointOption : BaseShippingOption
    {
        public CheckoutPointOption() { }
        public CheckoutPointOption(ShippingMethod method)
            : base(method)
        {
            HideAddressBlock = true;
        }

        public CheckoutPoint SelectedPoint { get; set; }

        public List<CheckoutPoint> ShippingPoints { get; set; }
        public string DeliveryType { get; set; }

        public override string Template
        {
            get { return UrlService.GetUrl() + "scripts/_partials/shipping/extendTemplate/CheckoutPointOption.html"; }
        }

        public override void Update(BaseShippingOption option)
        {
            var opt = option as CheckoutPointOption;
            if (opt != null && opt.Id == this.Id)
            {
                this.SelectedPoint = opt.SelectedPoint != null ? this.ShippingPoints.FirstOrDefault(x=>x.Id == opt.SelectedPoint.Id) : null;
                var selectedPoint = this.SelectedPoint;
                if (selectedPoint != null) this.Rate = selectedPoint.Rate;
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
    }
}