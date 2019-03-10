using Newtonsoft.Json;

namespace AdvantShop.Module.BingImagesSearch.Domain
{
    public class BingImage
    {
        [JsonProperty("accentColor")]
        public string AccentColor { get; set; }

        [JsonProperty("contentSize")]
        public string ContentSize { get; set; }

        [JsonProperty("contentUrl")]
        public string ContentUrl { get; set; }

        [JsonProperty("datePublished")]
        public string DatePublished { get; set; }

        [JsonProperty("encodingFormat")]
        public string EncodingFormat { get; set; }

        [JsonProperty("height")]
        public ushort Height { get; set; }

        [JsonProperty("hostPageDisplayUrl")]
        public string HostPageDisplayUrl { get; set; }

        [JsonProperty("hostPageUrl")]
        public string HostPageUrl { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("imageId")]
        public string ImageId { get; set; }

        [JsonProperty("imageInsightsToken")]
        public string ImageInsightsToken { get; set; }

        [JsonProperty("insightsSourcesSummary")]
        public InsightsSourcesSummary InsightsSourcesSummary { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("thumbnail")]
        public MediaSize Thumbnail { get; set; }

        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl { get; set; }

        [JsonProperty("webSearchUrl")]
        public string WebSearchUrl { get; set; }

        [JsonProperty("width")]
        public ushort Width { get; set; }
    }
}
