using System.Collections.Generic;
using AdvantShop.News;

namespace AdvantShop.ViewModel.News
{
    public class NewsCategoryListViewModel
    {
        public List<NewsCategory> NewsCategories { get; set; }

        public int Selected { get; set; }
    }
}