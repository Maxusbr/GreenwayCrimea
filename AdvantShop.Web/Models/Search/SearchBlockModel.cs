using AdvantShop.Configuration;
namespace AdvantShop.Models.Search
{
    public partial class SearchBlockModel : BaseModel
    {
        public string Q { get; set; }

        public string ExampleLink { get; set; }

        public string ExampleText { get; set; }
    }
}