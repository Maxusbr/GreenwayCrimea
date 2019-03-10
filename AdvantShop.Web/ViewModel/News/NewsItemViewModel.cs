using System.Collections.Generic;
using AdvantShop.CMS;
using AdvantShop.News;

namespace AdvantShop.ViewModel.News
{
    public class NewsItemViewModel
    {
        public NewsItem NewsItem { get; set; }

        public NewsCategoryListViewModel NewsCategoriesList { get; set; }

        public List<BreadCrumbs> BreadCrumbs { get; set; }

        public NewsProductsViewModel NewsProducts { get; set; }
    }
}