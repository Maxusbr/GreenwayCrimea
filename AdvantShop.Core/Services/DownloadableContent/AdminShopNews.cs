using System.Collections.Generic;

namespace AdvantShop.Core.Services.DownloadableContent
{
    public class AdminShopNews
    {
        public List<AdminShopNewsItem> NewsLearning { get; set; }
        public List<AdminShopNewsItem> NewsRecommendations { get; set; }
        public List<AdminShopNewsItem> NewsPartners { get; set; }
    }

    public class AdminShopNewsItem
    {
        public string Name { get; set; }
        public string LinkSrc { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
    }
}
