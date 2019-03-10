//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Shipping.Sdek
{
    public class SdekTemplate : DefaultCargoParams
    {
        public const string AuthLogin = "authLogin";
        public const string AuthPassword = "authPassword";
        //public const string ActiveTariffs = "activeTariffs";
        public const string Tariff = "Tariffs";
        public const string AdditionalPrice = "additionalPrice";
        public const string CityFrom = "cityFrom";
		public const string DefaultCourierNameContact = "DefaultCourierNameContact";
		public const string DefaultCourierPhone = "DefaultCourierPhone";
        public const string DefaultCourierCity = "DefaultCourieCity";
        public const string DefaultCourierStreet = "DefaultCourierStreet";
		public const string DefaultCourierHouse = "DefaultCourierHouse";
		public const string DefaultCourierFlat = "DefaultCourierFlat";
        public const string TypeAdditionPrice = "typeAdditionPrice";
        public const string DescriptionForSendOrder = "DescriptionForSendOrder";
        public const string DeliveryNote = "DeliveryNote";
    }

    public class SdekParamsSendOrder
    {
        public SdekParamsSendOrder()
        { }
        public SdekParamsSendOrder(ShippingMethod method)
        {
            Description = method.Params.ElementOrDefault(SdekTemplate.DescriptionForSendOrder);
            DefaultLength = method.Params.ElementOrDefault(DefaultCargoParams.DefaultLength);
            DefaultWidth = method.Params.ElementOrDefault(DefaultCargoParams.DefaultWidth);
            DefaultHeight = method.Params.ElementOrDefault(DefaultCargoParams.DefaultHeight);
        }
        public string Description { get; set; }
        public string DefaultLength { get; set; }
        public string DefaultWidth { get; set; }
        public string DefaultHeight { get; set; }
    }
}