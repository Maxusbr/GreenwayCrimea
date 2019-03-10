using Newtonsoft.Json;

namespace AdvantShop.App.Landing.Models.Inplace
{
    public class AddBlockModel
    {
        public int LpId { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }

        public int? ProductId { get; set; }
    }

    public class AddBlockResultModel
    {
        [JsonProperty("result")]
        public bool Result { get; set; }

        [JsonProperty("html")]
        public string Html { get; set; }
    }
}
