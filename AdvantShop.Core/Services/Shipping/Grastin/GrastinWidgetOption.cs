//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using AdvantShop.Core.Services.Shipping.Grastin;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Core.UrlRewriter;
using Newtonsoft.Json;

namespace AdvantShop.Shipping.Grastin
{
    public class GrastinWidgetOption : BaseShippingOption
    {
        public Dictionary<string, string> WidgetConfigData { get; set; }

        public string PickpointId { get; set; }
        public string PickpointAddress { get; set; }
        public string PickpointAdditionalData { get; set; }
        public GrastinEventWidgetData PickpointAdditionalDataObj { get; set; }

        public GrastinWidgetOption()
        {
        }

        public GrastinWidgetOption(ShippingMethod method)
            : base(method)
        {
            HideAddressBlock = true;
            Name = method.Name;
        }

        public override string Id
        {
            get { return MethodId + "_" + MethodId.GetHashCode(); }
        }

        private string PostfixName
        {
            get { return !string.IsNullOrEmpty(NameRate) && !base.Name.EndsWith(string.Format(" ({0})", NameRate)) ? string.Format(" ({0})", NameRate) : string.Empty; }
        }

        public new string Name
        {
            get { return base.Name + PostfixName; }
            set { base.Name = !string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(PostfixName) && value.EndsWith(PostfixName) ? value.Replace(PostfixName, string.Empty) : value; }
        }

        public override string Template
        {
            get { return UrlService.GetUrl() + "scripts/_partials/shipping/extendTemplate/GrastinWidgetOption.html"; }
        }

        public override void Update(BaseShippingOption option)
        {
            var opt = option as GrastinWidgetOption;
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
                    try
                    {
                        PickpointAdditionalDataObj =
                            JsonConvert.DeserializeObject<GrastinEventWidgetData>(opt.PickpointAdditionalData);
                    }
                    catch (Exception)
                    {
                    }

                    HideAddressBlock = PickpointAdditionalDataObj == null ||
                                       (PickpointAdditionalDataObj.DeliveryType == EnDeliveryType.PickPoint &&
                                        PickpointAdditionalDataObj.Partner != EnPartner.RussianPost);
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
