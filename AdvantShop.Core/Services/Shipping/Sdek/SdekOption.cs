using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Orders;
using Newtonsoft.Json;

namespace AdvantShop.Shipping.Sdek
{
    public class SdekOption : BaseShippingOption
    {
        public string TariffId { get; set; }
        public BaseShippingPoint SelectedPoint { get; set; }

        public List<BaseShippingPoint> ShippingPoints { get; set; }

        public SdekOption()
        {
        }

        public SdekOption(ShippingMethod method)
            : base(method)
        {
            HideAddressBlock = true;
        }

        public override string Template
        {
            get { return UrlService.GetUrl() + "scripts/_partials/shipping/extendTemplate/SdekOption.html"; }
        }

        public override void Update(BaseShippingOption option)
        {
            var opt = option as SdekOption;

            if (opt != null && opt.Id == this.Id && opt.ShippingPoints != null)
            {
                this.SelectedPoint = opt.SelectedPoint != null && this.ShippingPoints != null ? this.ShippingPoints.FirstOrDefault(x => x.Id == opt.SelectedPoint.Id) : null;
                this.SelectedPoint = this.SelectedPoint ?? opt.ShippingPoints.FirstOrDefault();
            }
        }
        public override OrderPickPoint GetOrderPickPoint()
        {
            if (SelectedPoint == null) return null;
           var temp= new OrderPickPoint
            {
                PickPointId = SelectedPoint.Code,
                PickPointAddress = SelectedPoint.Address,
                AdditionalData = JsonConvert.SerializeObject(SelectedPoint)
            };
            return temp;
        }
    }
}