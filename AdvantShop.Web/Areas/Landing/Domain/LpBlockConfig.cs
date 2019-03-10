using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdvantShop.App.Landing.Domain
{
    public class LpBlockConfig
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Picture { get; set; }


        [JsonProperty("subblocks")]
        public List<LpSubBlockConfig> SubBlocks { get; set; }
        
        public dynamic Settings { get; set; }

        public string BlockPath { get; set; }
    }

    public class LpSubBlockConfig
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Placeholder { get; set; }
        public dynamic Settings { get; set; }
    }
}
