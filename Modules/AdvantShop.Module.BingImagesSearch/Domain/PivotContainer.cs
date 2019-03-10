using Newtonsoft.Json;
using System.Collections.Generic;

namespace AdvantShop.Module.BingImagesSearch.Domain
{
    public class PivotContainer
    {
        [JsonProperty("pivot")]
        public string Pivot { get; set; }

        [JsonProperty("suggestions")]
        public List<Query> Suggestions { get; set; }
    }
}
