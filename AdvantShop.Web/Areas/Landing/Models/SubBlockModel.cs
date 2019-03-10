using AdvantShop.App.Landing.Domain;

namespace AdvantShop.App.Landing.Models
{
    public class SubBlockModel
    {
        public bool InPlace { get; set; }
        public string ViewPath { get; set; }

        public LpSubBlock SubBlock { get; set; }

    }
}
