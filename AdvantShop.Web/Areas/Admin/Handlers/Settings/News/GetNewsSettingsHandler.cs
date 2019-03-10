using AdvantShop.Configuration;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.News
{
    public class GetNewsSettingsHandler
    {
        public NewsSettingsModel Execute()
        {
            var model = new NewsSettingsModel
            {
                NewsPerPage = SettingsNews.NewsPerPage,
                NewsMainPageCount = SettingsNews.NewsMainPageCount,
                MainPageText = SettingsNews.MainPageText,
                RssViewNews = SettingsNews.RssViewNews
            };

            return model;
        }
    }
}
