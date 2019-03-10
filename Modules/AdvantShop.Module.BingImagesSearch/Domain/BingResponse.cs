using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AdvantShop.Module.BingImagesSearch.Domain
{
    public class BingResponse
    {
        [JsonProperty("_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EBingResponseType Type { get; set; }

        [JsonProperty("displayRecipeSourcesBadges")]
        public bool DisplayRecipeSourcesBadges { get; set; }

        [JsonProperty("displayShoppingSourcesBadges")]
        public bool DisplayShoppingSourcesBadges { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("isFamilyFriendly")]
        public bool IsFamilyFriendly { get; set; }

        [JsonProperty("nextOffsetAddCount")]
        public int NextOffsetAddCount { get; set; }

        [JsonProperty("pivotSuggestions")]
        public List<PivotContainer> PivotSuggestions { get; set; }

        [JsonProperty("queryExpansions")]
        public List<Query> QueryExpansions { get; set; }

        [JsonProperty("readLink")]
        public string ReadLink { get; set; }

        [JsonProperty("totalEstimatedMatches")]
        public long TotalEstimatedMatches { get; set; }

        [JsonProperty("value")]
        public List<BingImage> Value { get; set; }

        [JsonProperty("webSearchUrl")]
        public string WebSearchUrl { get; set; }

        [JsonProperty("errors")]
        public List<BingError> Errors { get; set; }
    }
}
