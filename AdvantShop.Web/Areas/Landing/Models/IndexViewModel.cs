using System.Collections.Generic;
using AdvantShop.App.Landing.Domain;

namespace AdvantShop.App.Landing.Models
{
    public class IndexViewModel
    {
        public Lp LandingPage { get; set; }

        public List<LpBlock> Blocks { get; set; } 
    }
}
