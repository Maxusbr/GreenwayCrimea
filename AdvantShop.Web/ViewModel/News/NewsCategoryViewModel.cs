using System.Collections.Generic;
using AdvantShop.CMS;
using AdvantShop.Core.Models;
using AdvantShop.News;

namespace AdvantShop.ViewModel.News
{
    public class NewsCategoryViewModel
    {
        public NewsCategory NewsCategory { get; set; }

        public bool Selected { get; set; }

        public NewsCategoryListViewModel SubCategories { get; set; }

        public List<NewsItem> News { get; set; }

        public Pager Pager { get; set; }

        public List<BreadCrumbs> BreadCrumbs { get; set; }

        public string H1 { get; set;}
    }
}