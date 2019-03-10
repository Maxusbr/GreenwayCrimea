using Newtonsoft.Json;

namespace AdvantShop.Module.BingImagesSearch.Domain
{
    public class MediaSize
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }
    }
}
