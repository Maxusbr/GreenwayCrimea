namespace AdvantShop.Web.Admin.Models.Settings
{
    public class MobileVersionSettingsModel
    {
        public MobileVersionSettingsModel()
        {
        }

        public bool Enabled { get; set; }
        public string MobilePhone { get; set; }
        public string MainPageProductCountMobile { get; set; }
        public bool ShowCity { get; set; }
        public bool ShowSlider { get; set; }
        public bool DisplayHeaderTitle { get; set; }
        public string HeaderCustomTitle { get; set; }
        public bool IsFullCheckout { get; set; }
        public bool RedirectToSubdomain { get; set; }
        public string Robots { get; set; }
    }
}
