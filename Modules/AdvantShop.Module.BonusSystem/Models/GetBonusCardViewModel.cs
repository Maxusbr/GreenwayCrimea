using System.Collections.Generic;
using AdvantShop.CMS;
using AdvantShop.Core.Services.Bonuses;

namespace AdvantShop.Module.BonusSystemModule.Models
{
    public class GetBonusCardViewModel
    {
        public List<BreadCrumbs> BreadCrumbs { get; set; }

        public string BonusTextBlock { get; set; }

        public string BonusRightTextBlock { get; set; }

        public List<BonusGrade> Grades { get; set; } 
    }
}