using Newtonsoft.Json;

namespace AdvantShop.Core.Services.IPTelephony.Telphin
{
    public class TelphinRecordInfoResponse
    {
        [JsonProperty("record_url")]
        public string RecordUrl{ get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
