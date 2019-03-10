using System;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.Loging.TrafficSource
{
    public class TrafficSource
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ShopId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Guid? CustomerId { get; set; }


        [JsonProperty(PropertyName = "d", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreateOn { get; set; }

        [JsonProperty(PropertyName = "r", NullValueHandling = NullValueHandling.Ignore)]
        public string Referrer { get; set; }

        [JsonProperty(PropertyName = "u", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "s", NullValueHandling = NullValueHandling.Ignore)]
        public string utm_source { get; set; }

        [JsonProperty(PropertyName = "m", NullValueHandling = NullValueHandling.Ignore)]
        public string utm_medium { get; set; }

        [JsonProperty(PropertyName = "ca", NullValueHandling = NullValueHandling.Ignore)]
        public string utm_campaign { get; set; }

        [JsonProperty(PropertyName = "co", NullValueHandling = NullValueHandling.Ignore)]
        public string utm_content { get; set; }

        [JsonProperty(PropertyName = "t", NullValueHandling = NullValueHandling.Ignore)]
        public string utm_term { get; set; }

        [JsonProperty(PropertyName = "h")]
        public string Hash { get; set; }
    }
}