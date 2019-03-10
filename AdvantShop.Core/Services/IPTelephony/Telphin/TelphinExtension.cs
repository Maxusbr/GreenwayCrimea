using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.IPTelephony.Telphin
{
    public class TelphinExtension
    {
        public TelphinExtension()
        {
            Events = new List<TelphinEvent>();
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("extension")]
        public string Extension
        {
            get { return Name.DefaultOrEmpty().Split("*").LastOrDefault(); }
        }

        [JsonProperty("events")]
        public List<TelphinEvent> Events { get; set; }
    }
}
