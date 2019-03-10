using System.Collections.Generic;
using AdvantShop.CMS;
using AdvantShop.Core.Models;
using AdvantShop.Module.Blog.Domain;

namespace AdvantShop.Module.Blog.Models
{
    public class BlogCategoryViewModel
    {
        public BlogCategoryListViewModel SubCategories { get; set; }

        public BlogCategory BlogCategory { get; set; }
        
        public List<BlogItem> BlogItems { get; set; }

        public Pager Pager { get; set; }

        public List<BreadCrumbs> BreadCrumbs { get; set; }

        public string BlogTitle { get; set; }

        public string SbRightBlock { get; set; }

        public bool ShowAddDate { get; set; }
    }
}
