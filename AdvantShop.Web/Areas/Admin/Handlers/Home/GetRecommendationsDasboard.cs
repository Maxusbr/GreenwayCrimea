using System.Collections.Generic;
using AdvantShop.Core.Services.DownloadableContent;
using AdvantShop.Web.Admin.ViewModels.Home;

namespace AdvantShop.Web.Admin.Handlers.Home
{
    public class GetRecommendationsDasboard
    {
        public RecommendationsDashboardViewModel Execute()
        {
            var news = ShopNewsService.GetLastNews();

            return new RecommendationsDashboardViewModel()
            {
                News = news.NewsRecommendations ?? new List<AdminShopNewsItem>()
            };
        }
    }
}
