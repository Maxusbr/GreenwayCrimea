//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Shipping;
using AdvantShop.Shipping.Edost;
using AdvantShop.Shipping.Sdek;
using AdvantShop.Shipping.CheckoutRu;
using AdvantShop.Shipping.NovaPoshta;
using AdvantShop.Shipping.Boxberry;
using AdvantShop.Shipping.Grastin;

namespace AdvantShop.Payment
{
    public class CashOnDeliverytOption : BasePaymentOption
    {
        public CashOnDeliverytOption()
        {
        }

        public CashOnDeliverytOption(PaymentMethod menthod, float preCoast)
            : base(menthod, preCoast)
        {
        }      
    }

    /// <summary>
    /// cash on delivery
    /// </summary>
    [PaymentKey("CashOnDelivery")]
    public class CashOnDelivery : PaymentMethod, IPaymentCurrencyHide
    {
        public int ShippingMethodId { get; set; }

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

        public override ProcessType ProcessType
        {
            get { return ProcessType.None; }
        }

        public const string ShippingMethodTemplate = "ShippingMethod";

        public override IEnumerable<string> ShippingKeys
        {
            get { return new List<string> { "Edost", "NovaPoshta", "Sdek", "CheckoutRu", "Boxberry", "Grastin" }; }
        }

        public override BasePaymentOption GetOption(BaseShippingOption shippingOption, float preCoast)
        {
            if (this.Parameters[CashOnDelivery.ShippingMethodTemplate].TryParseInt() != shippingOption.MethodId)
                return null;

            var shippingOptionType = shippingOption.GetType();

            if (typeof(EdostCashOnDeliveryOption).IsAssignableFrom(shippingOptionType) ||
                shippingOptionType == typeof(SdekOption) || 
                shippingOptionType == typeof(CheckoutOption) ||
                shippingOptionType == typeof(BoxberryOption) ||
                shippingOptionType == typeof(GrastinOption) ||
                shippingOptionType == typeof(GrastinWidgetOption) ||
                shippingOptionType == typeof(NovaPoshtaOptions) || 
                shippingOptionType == typeof(CheckoutPointOption) ||
                (shippingOptionType == typeof(BaseShippingOption) && shippingOption.ShippingType == "Edost"))
            {
                var option = new CashOnDeliverytOption(this, preCoast);

                if (typeof(EdostCashOnDeliveryOption).IsAssignableFrom(shippingOptionType) ||
                    (shippingOptionType == typeof(BaseShippingOption) && shippingOption.ShippingType == "Edost"))
                {
                    option.Desc = shippingOption.GetDescriptionForPayment();
                }

                if ((shippingOptionType == typeof(GrastinOption) || shippingOptionType == typeof(GrastinWidgetOption)) && shippingOption.ShippingType == "Grastin")
                {
                    if (shippingOptionType == typeof(GrastinWidgetOption))
                    {
                        var pointData = ((GrastinWidgetOption)shippingOption).PickpointAdditionalDataObj;
                        if (pointData == null || pointData.DeliveryType != Core.Services.Shipping.Grastin.EnDeliveryType.Courier)
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