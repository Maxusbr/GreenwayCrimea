//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using AdvantShop.Core.Services.Shipping.Boxberry;
using AdvantShop.Orders;
using AdvantShop.Payment;
using Newtonsoft.Json;
using AdvantShop.Core.UrlRewriter;

namespace AdvantShop.Shipping.Boxberry
{
    public class BoxberryWidgetOption : BaseShippingOption
    {
        public Dictionary<string, object> WidgetConfigData { get; set; }

        public string PickpointId { get; set; }
        public string PickpointAddress { get; set; }
        public string PickpointAdditionalData { get; set; }
        public BoxberryObjectPoint PickpointAdditionalDataObj { get; set; }

        public BoxberryWidgetOption()
        {
        }

        public BoxberryWidgetOption(ShippingMethod method)
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
            get { return UrlService.GetUrl() + "scripts/_partials/shipping/extendTemplate/BoxberryWidgetOption.html"; }
        }

        public override void Update(BaseShippingOption option)
        {
            var opt = option as BoxberryWidgetOption;
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
                    PickpointAdditionalDataObj =
                        JsonConvert.DeserializeObject<BoxberryObjectPoint>(opt.PickpointAdditionalData);                  
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
