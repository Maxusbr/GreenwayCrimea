using AdvantShop.Core.Services.Api;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.OneC
{
    public class OneCResponse : ApiResponse
    {
        public OneCResponse() {}
        public OneCResponse(string status, string errors) : base(status, errors) { }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string packageid { get; set; }
    }

    public class OneCDeletedItemsResponse : BaseApiResponse
    {
        public string ids { get; set; }
        public string status { get; set; }
    }
}