using System.Collections.Generic;
using AdvantShop.Core.Models;
using AdvantShop.News;

namespace AdvantShop.Models.News
{
    public class NewsPagingModel : BaseModel
    {
        public List<NewsItem> News { get; set; }

        public Pager Pager { get; set; }
    }
}