using System;
using Newtonsoft.Json;

namespace AdvantShop.Shipping.NovaPoshta
{
    public class NovaResponseDelivery
    {
        [JsonProperty(PropertyName = "date")]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "timezone_type")]
        public int TimezoneType { get; set; }

        [JsonProperty(PropertyName = "timezone")]
        public string Timezone { get; set; }
    }
}