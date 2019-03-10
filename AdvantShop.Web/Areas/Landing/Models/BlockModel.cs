using AdvantShop.App.Landing.Domain;

namespace AdvantShop.App.Landing.Models
{
    public class BlockModel
    {
        public bool InPlace { get; set; }
        public LpBlock Block { get; set; }
        public LpBlockConfig Config { get; set; }
    }
}
