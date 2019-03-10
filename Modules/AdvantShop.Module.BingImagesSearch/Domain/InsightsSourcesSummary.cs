using Newtonsoft.Json;

namespace AdvantShop.Module.BingImagesSearch.Domain
{
    public class InsightsSourcesSummary
    {
        [JsonProperty("recipeSourcesCount")]
        public uint RecipeSourcesCount { get; set; }

        [JsonProperty("shoppingSourcesCount")]
        public uint ShoppingSourcesCount { get; set; }
    }
}
