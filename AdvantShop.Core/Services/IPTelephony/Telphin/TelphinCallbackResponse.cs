using Newtonsoft.Json;

namespace AdvantShop.Core.Services.IPTelephony.Telphin
{
    public class TelphinCallbackResponse
    {
        [JsonProperty("call_api_id")]
        public string CallApiId { get; set; }
        [JsonProperty("call_id")]
        public string CallId { get; set; }
    }
}
