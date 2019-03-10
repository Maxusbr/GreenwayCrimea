
using System;
using AdvantShop.Core.Common.Attributes;
using Newtonsoft.Json;

namespace AdvantShop.ExportImport
{
    [Serializable()]
    public class ExportFeedYandexOptions : ExportFeedSettings
    {
        [JsonProperty(PropertyName = "Currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "RemoveHtml")]
        public bool RemoveHtml { get; set; }

        [JsonProperty(PropertyName = "Delivery")]
        public bool Delivery { get; set; }

        [JsonProperty(PropertyName = "Pickup")]
        public bool Pickup { get; set; }


        //[JsonProperty(PropertyName = "LocalDeliveryCost")]
        //public bool LocalDeliveryCost { get; set; }

        [JsonProperty(PropertyName = "DeliveryCost")]
        public ExportFeedYandexDeliveryCost DeliveryCost { get; set; }

        [JsonProperty(PropertyName = "GlobalDeliveryCost")]
        public string GlobalDeliveryCost { get; set; }

        [JsonProperty(PropertyName = "LocalDeliveryOption")]
        public string LocalDeliveryOption { get; set; }
        
        [JsonProperty(PropertyName = "ExportProductProperties")]
        public bool ExportProductProperties { get; set; }

        [JsonProperty(PropertyName = "ExportProductDiscount")]
        public bool ExportProductDiscount { get; set; }

        [JsonProperty(PropertyName = "SalesNotes")]
        public string SalesNotes { get; set; }

        [JsonProperty(PropertyName = "ShopName")]
        public string ShopName { get; set; }

        [JsonProperty(PropertyName = "CompanyName")]
        public string CompanyName { get; set; }

        [JsonProperty(PropertyName = "ColorSizeToName")]
        public bool ColorSizeToName { get; set; }

        [JsonProperty(PropertyName = "ProductDescriptionType")]
        public string ProductDescriptionType { get; set; }

        [JsonProperty(PropertyName = "OfferIdType")]
        public string OfferIdType { get; set; }
        
        //[JsonProperty(PropertyName = "Available")]
        //public bool Available { get; set; }

        [JsonProperty(PropertyName = "ExportPurchasePrice")]
        public bool ExportPurchasePrice { get; set; }

        [JsonProperty(PropertyName = "ExportBarCode")]
        public bool ExportBarCode { get; set; }

        [JsonProperty(PropertyName = "Store")]
        public bool Store { get; set; }

        //общая ставка
        [JsonProperty(PropertyName = "Bid")]
        public float? Bid { get; set; }

        [JsonProperty(PropertyName = "ExportRelatedProducts")]
        public bool ExportRelatedProducts { get; set; }

        [JsonProperty(PropertyName = "AllowPreOrderProducts")]
        public bool? AllowPreOrderProducts { get; set; }

        [JsonProperty(PropertyName = "ExportNotAvailable")]
        public bool ExportNotAvailable { get; set; }

        [JsonProperty(PropertyName = "ExportAllPhotos")]
        public bool ExportAllPhotos { get; set; }

        [JsonProperty(PropertyName = "TypeExportYandex")]
        public bool TypeExportYandex { get; set; }

        public ExportFeedYandexOptions() {
            //Available = true;
            Store = true;
        }

    }

    public enum ExportFeedYandexDeliveryCost
    {
        [Localize("Core.ExportImport.ExportFeedYandexDeliveryCost.None")]
        None = 0,

        [Localize("Core.ExportImport.ExportFeedYandexDeliveryCost.LocalDeliveryCost")]
        LocalDeliveryCost = 1,

        [Localize("Core.ExportImport.ExportFeedYandexDeliveryCost.GlobalDeliveryCost")]
        GlobalDeliveryCost = 2
    }

    public class ExportFeedYandexDeliveryCostOption
    {
        public string Cost { get; set; }

        public string Days { get; set; }

        public string OrderBefore { get; set; }
    }
}
