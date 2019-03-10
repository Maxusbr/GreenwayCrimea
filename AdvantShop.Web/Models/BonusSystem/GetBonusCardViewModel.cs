using System.Collections.Generic;
using AdvantShop.CMS;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Bonuses.Model;

namespace AdvantShop.Models.BonusSystemModule
{
    public class GetBonusCardViewModel
    {
        public List<BreadCrumbs> BreadCrumbs { get; set; }

        public string BonusTextBlock { get; set; }

        public string BonusRightTextBlock { get; set; }

        public List<Grade> Grades { get; set; } 
    }
}