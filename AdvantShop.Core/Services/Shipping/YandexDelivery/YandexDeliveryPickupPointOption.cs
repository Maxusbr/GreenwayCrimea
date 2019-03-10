using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Orders;

namespace AdvantShop.Shipping.ShippingYandexDelivery
{
    public class YandexDeliveryPickupPointOption : BaseShippingOption
    {
        public string WidgetCodeYa { get; set; }
        public string Weight { get; set; }
        public string Dimensions { get; set; }
        public string Cost { get; set; }
        public string Amount { get; set; }

        public string PickpointId { get; set; }
        public string PickpointAddress { get; set; }
        public string PickpointAdditionalData { get; set; }
        public string TariffId { get; set; }

        public bool ShowAssessedValue { get; set; }

        public List<YandexDeliveryListItem> PickPoints { get; set; }

        public YandexDeliveryPickupPointOption()
        {
        }

        public YandexDeliveryPickupPointOption(ShippingMethod method, List<YandexDeliveryListItem> items)
            : base(method)
        {
            HideAddressBlock = true;
            Name = "Постаматы и пункты самовывоза";
            PickPoints = items;

        }

        public override string Template
        {
            get { return UrlService.GetUrl() + "scripts/_partials/shipping/extendTemplate/YandexDeliveryPickupPointOption.html"; }
        }

        public override void Update(BaseShippingOption option)
        {
            var opt = option as YandexDeliveryPickupPointOption;
            if (opt != null && opt.Id == this.Id)
            {
                this.PickpointId = opt.PickpointId;
                this.PickpointAddress = opt.PickpointAddress;
                this.PickpointAdditionalData = opt.PickpointAdditionalData;
                this.TariffId = opt.TariffId;

                if (opt.TariffId != null)
                {
                    var pickPoint = this.PickPoints.FirstOrDefault(x => x.TariffId == opt.TariffId);
                    if (pickPoint != null)
                        Rate = pickPoint.CostWithRules;
                }
                else
                {
                    var pickpoints = this.PickPoints.Where(x => x.Type == "PICKUP" && x.PickupPoints != null && x.PickupPoints.Count > 0).ToList();
                    if (pickpoints.Count > 0)
                        Rate = pickpoints.Min(x => x.CostWithRules);
                }
            }
        }

        public override OrderPickPoint GetOrderPickPoint()
        {
            return new OrderPickPoint
            {
                PickPointId = PickpointId,
                PickPointAddress = PickpointAddress ?? string.Empty,
                AdditionalData = PickpointAdditionalData
            };
        }
    }
}