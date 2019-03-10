using Newtonsoft.Json;

namespace AdvantShop.App.Landing.Models
{
    public class ResultModel
    {
        [JsonProperty("result")]
        public bool Result { get; set; }
    }
}
