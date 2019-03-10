using Newtonsoft.Json;

namespace AdvantShop.Module.BingImagesSearch.Domain
{
    public class Thumbnail
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
