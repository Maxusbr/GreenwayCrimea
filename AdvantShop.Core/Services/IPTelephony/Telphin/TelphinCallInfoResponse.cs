using Newtonsoft.Json;

namespace AdvantShop.Core.Services.IPTelephony.Telphin
{
    public class TelphinCallInfoResponse
    {
        [JsonProperty("cdr")]
        public TelphinCdrResponse[] Cdr { get; set; }
    }

    public class TelphinCdrResponse
    {
        [JsonProperty("record_uuid")]
        public string RecordUuid { get; set; }
    }
}
