
namespace AdvantShop.Web.Admin.Models.Settings
{
    public class NewsSettingsModel
    {
        public int NewsPerPage { get; set; }
        public int NewsMainPageCount { get; set; }

        public string MainPageText { get; set; }
        public bool RssViewNews { get; set; }
    }
}
