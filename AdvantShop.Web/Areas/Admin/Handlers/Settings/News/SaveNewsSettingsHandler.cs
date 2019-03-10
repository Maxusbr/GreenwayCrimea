using AdvantShop.Configuration;
using AdvantShop.Web.Admin.Models.Settings;

namespace AdvantShop.Web.Admin.Handlers.Settings.News
{
    public class SaveNewsSettingsHandler
    {
        private NewsSettingsModel _model;

        public SaveNewsSettingsHandler(NewsSettingsModel model)
        {
            _model = model;
        }

        public void Execute()
        {
            SettingsNews.NewsPerPage = _model.NewsPerPage;
            SettingsNews.NewsMainPageCount = _model.NewsMainPageCount;
            SettingsNews.MainPageText = _model.MainPageText;
            SettingsNews.RssViewNews = _model.RssViewNews;
        }
    }
}
