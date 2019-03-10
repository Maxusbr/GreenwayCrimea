using AdvantShop.Core.UrlRewriter;
using AdvantShop.Orders;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AdvantShop.Shipping.PointDelivery
{
    public class PointDeliveryOption : BaseShippingOption
    {
        public BaseShippingPoint SelectedPoint { get; set; }
        public List<BaseShippingPoint> ShippingPoints { get; set; }

        public PointDeliveryOption()
        {
        }

        public PointDeliveryOption(ShippingMethod method) : base(method)
        {
            var temp = method.Params["Points"];
            ShippingPoints = new List<BaseShippingPoint>();

            ShippingPoints = temp.Split(';').Select((x, index) => new BaseShippingPoint
            {
                Id = index,
                Address = x
            }).ToList();
            HideAddressBlock = true;
        }

        public override string Template
        {
            get { return UrlService.GetUrl() + "scripts/_partials/shipping/extendTemplate/PointDeliverySelectOption.html"; }
        }

        public override void Update(BaseShippingOption option)
        {
            var opt = option as PointDeliveryOption;
            if (opt != null && this.Id == opt.Id)
            {
                this.SelectedPoint = opt.SelectedPoint != null ? this.ShippingPoints.FirstOrDefault(x => x.Id == opt.SelectedPoint.Id) : null;
                this.SelectedPoint = this.SelectedPoint ?? opt.ShippingPoints.FirstOrDefault();
            }
        }

        public override OrderPickPoint GetOrderPickPoint()
        {
            return new OrderPickPoint
            {
                PickPointId = Id,
                PickPointAddress = SelectedPoint.Address,
                AdditionalData = JsonConvert.SerializeObject(SelectedPoint)
            };
        }

    }
}
