using Newtonsoft.Json;

namespace AdvantShop.Module.BingImagesSearch.Domain
{
    public class BingError
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("parameter")]
        public string Parameter { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
