//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;

namespace AdvantShop.Trial
{
    public class AchievementLevel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SortOrder { get; set; }

        public bool Unlocked { get; set; }

        public bool Complete { get; set; }
        public List<Achievement> Achievements { get; set; }
    }
}