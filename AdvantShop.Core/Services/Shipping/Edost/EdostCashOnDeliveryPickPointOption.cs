using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Payment;
using AdvantShop.Orders;

namespace AdvantShop.Shipping.Edost
{
    public class EdostCashOnDeliveryPickPointOption : EdostCashOnDeliveryOption
    {
        public string Pickpointmap { get; set; }
        public string PickpointId { get; set; }
        public string PickpointAddress { get; set; }

        public EdostCashOnDeliveryPickPointOption()
        {
        }

        public EdostCashOnDeliveryPickPointOption(ShippingMethod method, EdostTarif tarif)
            : base(method, tarif)
        {
            Pickpointmap = tarif.PickpointMap;
            HideAddressBlock = true;
        }

        public override string Template
        {
            get { return UrlService.GetUrl() + "scripts/_partials/shipping/extendTemplate/EdostPickPointOption.html"; }
        }

        public override void Update(BaseShippingOption option)
        {
            var opt = option as EdostCashOnDeliveryPickPointOption;
            if (opt != null && opt.Id == this.Id)
            {
                this.PickpointId = opt.PickpointId;
                this.PickpointAddress = opt.PickpointAddress;
                this.Pickpointmap = opt.Pickpointmap;
            }
        }

        public override OptionValidationResult Validate()
        {
            var result = base.Validate();
            if (!string.IsNullOrEmpty(this.PickpointId)) return result;
            result.IsValid = false;
            result.ErrorMessage = LocalizationService.GetResource("Core.Shipping.EdostPickPointOption.PickpointError");
            return result;
        }

        public override OrderPickPoint GetOrderPickPoint()
        {
            return new OrderPickPoint
            {
                PickPointId = PickpointId,
                PickPointAddress = PickpointAddress,
            };
        }
    }
}
