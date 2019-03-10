using Newtonsoft.Json;

namespace AdvantShop.Module.Roistat.Models.Roistat
{
    public class RoistatPagination
    {
        [JsonProperty(PropertyName = "total_count")]
        public int TotalCount { get; set; }

        [JsonProperty(PropertyName = "limit")]
        public int Limit { get; set; }
    }
}
