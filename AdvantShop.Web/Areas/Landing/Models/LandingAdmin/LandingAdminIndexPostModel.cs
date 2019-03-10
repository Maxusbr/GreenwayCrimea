using AdvantShop.App.Landing.Domain;

namespace AdvantShop.App.Landing.Models.LandingAdmin
{
    public class LandingAdminIndexPostModel
    {
        public string Template { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Enabled { get; set; }

        public LpType Type { get; set; }
        public LpGoal Goal { get; set; }
        public string ProductIds { get; set; }
    }
}
