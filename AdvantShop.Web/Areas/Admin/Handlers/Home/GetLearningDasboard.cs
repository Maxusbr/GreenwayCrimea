using System.Collections.Generic;
using AdvantShop.Core.Services.DownloadableContent;
using AdvantShop.Web.Admin.ViewModels.Home;

namespace AdvantShop.Web.Admin.Handlers.Home
{
    public class GetLearningDasboard
    {
        public LearnigDashboardViewModel Execute()
        {
            var news = ShopNewsService.GetLastNews();

            return new LearnigDashboardViewModel()
            {
                News = news.NewsLearning ?? new List<AdminShopNewsItem>()
            };
        }
    }
}
