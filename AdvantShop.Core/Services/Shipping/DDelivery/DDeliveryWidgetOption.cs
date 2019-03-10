//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;

using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Core.Services.Shipping.DDelivery;

namespace AdvantShop.Shipping.DDelivery
{
    public class DDeliveryWidgetOption : BaseShippingOption
    {
        //public Dictionary<string, string> WidgetConfigData { get; set; }
        public DDeliveryObjectWidgetConfig WidgetConfigData { get; set; }

        public string PickpointId { get; set; }
        public string PickpointAddress { get; set; }
        public string PickpointAdditionalData { get; set; }
        //public BoxberryObjectPoint PickpointAdditionalDataObj { get; set; }

        public DDeliveryWidgetOption()
        {
        }

        public DDeliveryWidgetOption(ShippingMethod method)
            : base(method)
        {
            HideAddressBlock = true;
            Name = method.Name;
            ShippingType = method.ShippingType;
        }

        public override string Id
        {
            get { return MethodId + "_" + MethodId.GetHashCode(); }
        }
       

        public override string Template
        {
            get { return UrlService.GetUrl() + "scripts/_partials/shipping/extendTemplate/DdeliveryWidgetOption.html"; }
        }

        public override void Update(BaseShippingOption option)
        {
            var opt = option as DDeliveryWidgetOption;
            if (opt != null && opt.MethodId == this.MethodId)
            {
                this.PickpointId = opt.PickpointId;
                this.PickpointAddress = opt.PickpointAddress;
                this.PickpointAdditionalData = opt.PickpointAdditionalData;
                this.Rate = opt.Rate;
                this.DeliveryTime = opt.DeliveryTime;
                this.NameRate = opt.NameRate;

                if (!string.IsNullOrEmpty(opt.PickpointAdditionalData))
                {
                    //PickpointAdditionalDataObj =
                    //    JsonConvert.DeserializeObject<BoxberryObjectPoint>(opt.PickpointAdditionalData);                  
                }
            }
        }

        public override OrderPickPoint GetOrderPickPoint()
        {
            return !string.IsNullOrEmpty(PickpointId)
                ? new OrderPickPoint
                {
                    PickPointId = PickpointId,
                    PickPointAddress = PickpointAddress ?? string.Empty,
                    AdditionalData = PickpointAdditionalData
                }
                : null;
        }

        public override bool ApplyPay(BasePaymentOption payOption)
        {
            return true;
        }

        public override string GetDescriptionForPayment()
        {
            return string.Empty;
        }
    }
}
