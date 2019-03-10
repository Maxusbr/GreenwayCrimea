
using System;
using Newtonsoft.Json;

namespace AdvantShop.ExportImport
{
    [Serializable()]
    public class ExportFeedGoogleMerchantCenterOptions : ExportFeedSettings
    {
        [JsonProperty(PropertyName = "Currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "RemoveHtml")]
        public bool RemoveHtml { get; set; }

        [JsonProperty(PropertyName = "DatafeedTitle")]
        public string DatafeedTitle { get; set; }

        [JsonProperty(PropertyName = "DatafeedDescription")]
        public string DatafeedDescription { get; set; }

        [JsonProperty(PropertyName = "GoogleProductCategory")]
        public string GoogleProductCategory { get; set; }

        [JsonProperty(PropertyName = "ProductDescriptionType")]
        public string ProductDescriptionType { get; set; }

        [JsonProperty(PropertyName = "OfferIdType")]
        public string OfferIdType { get; set; }

        [JsonProperty(PropertyName = "AllowPreOrderProducts")]
        public bool AllowPreOrderProducts { get; set; }

        [JsonProperty(PropertyName = "ExportNotAvailable")]
        public bool ExportNotAvailable { get; set; }

        [JsonProperty(PropertyName = "ColorSizeToName")]
        public bool ColorSizeToName { get; set; }
    }
}
