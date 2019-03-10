using Newtonsoft.Json;

namespace AdvantShop.Module.BingImagesSearch.Domain
{
    public class Query
    {
        [JsonProperty("displayText")]
        public string DisplayText { get; set; }

        [JsonProperty("searchLink")]
        public string SearchLink { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("thumbnail")]
        public Thumbnail Thumbnail { get; set; }

        [JsonProperty("webSearchUrl")]
        public string WebSearchUrl { get; set; }
    }
}
