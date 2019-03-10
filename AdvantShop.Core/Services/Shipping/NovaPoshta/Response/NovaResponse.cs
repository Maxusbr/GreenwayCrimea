using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.Shipping.NovaPoshta
{
    public class NovaResponse<T> where T : class
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "data")]
        public List<T> Data { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public object Errors { get; set; }

        [JsonProperty(PropertyName = "warnings")]
        public object Warnings { get; set; }

        [JsonProperty(PropertyName = "info")]
        public object Info { get; set; }
    }
}