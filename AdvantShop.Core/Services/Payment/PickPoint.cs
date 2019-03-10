//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Shipping;
using AdvantShop.Shipping.Edost;
using System.Collections.Generic;
using AdvantShop.Shipping.Grastin;

namespace AdvantShop.Payment
{
    public class PickPointOption : BasePaymentOption
    {
        public PickPointOption()
        {
        }

        public PickPointOption(PaymentMethod menthod, float preCoast)
            : base(menthod, preCoast)
        {
        }
    }

    [PaymentKey("PickPoint")]
    public class PickPoint : PaymentMethod
    {
        public int ShippingMethodId { get; set; }

        public override ProcessType ProcessType
        {
            get { return ProcessType.None; }
        }

        public const string ShippingMethodTemplate = "ShippingMethod";

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {ShippingMethodTemplate, ShippingMethodId.ToString()}
                };
            }
            set { ShippingMethodId = value.ElementOrDefault(ShippingMethodTemplate).TryParseInt(); }
        }

        public override IEnumerable<string> ShippingKeys
        {
            get { return new List<string> {"eDost", "Grastin" }; }
        }

        public override BasePaymentOption GetOption(BaseShippingOption shippingOption, float preCoast)
        {
            if (this.Parameters[ShippingMethodTemplate].TryParseInt() != shippingOption.MethodId)
                return null;

            var shippingOptionType = shippingOption.GetType();
            if (typeof (EdostPickPointOption).IsAssignableFrom(shippingOptionType) ||
                shippingOptionType == typeof(GrastinPointOption) ||
                shippingOptionType == typeof(GrastinWidgetOption))
            {
                var option = new PickPointOption(this, preCoast);

                if (typeof(EdostCashOnDeliveryOption).IsAssignableFrom(shippingOptionType) ||
                    (shippingOptionType == typeof(BaseShippingOption) && shippingOption.ShippingType == "Edost"))
                {
                    option.Desc = shippingOption.GetDescriptionForPayment();
                }

                if ((shippingOptionType == typeof(GrastinPointOption) || shippingOptionType == typeof(GrastinWidgetOption)) && shippingOption.ShippingType == "Grastin")
                {
                    if (shippingOptionType == typeof(GrastinWidgetOption))
                    {
                        var pointData = ((GrastinWidgetOption)shippingOption).PickpointAdditionalDataObj;
                        if (pointData == null || pointData.DeliveryType != Core.Services.Shipping.Grastin.EnDeliveryType.PickPoint)
                        {
                            return null;
                        }
                    }

                    option.Desc = shippingOption.GetDescriptionForPayment();
                }

                return option;
            }

            return null;
        }
    }
}