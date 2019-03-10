using System.Collections.Generic;
using AdvantShop.CMS;
using AdvantShop.Module.Blog.Domain;

namespace AdvantShop.Module.Blog.Models
{
    public class BlogItemViewModel
    {
        public BlogItem BlogItem { get; set; }

        public BlogCategoryListViewModel BlogCategoriesList { get; set; }

        public List<BreadCrumbs> BreadCrumbs { get; set; }

        public string SbAfterText { get; set; }

        public string SbRightBlock { get; set; }

        public bool ShowAddDate { get; set; }

        public BlogProductsViewModel BlogProducts { get; set; }
    }
}
