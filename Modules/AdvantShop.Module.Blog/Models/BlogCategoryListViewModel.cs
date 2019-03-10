using System.Collections.Generic;
using AdvantShop.Module.Blog.Domain;

namespace AdvantShop.Module.Blog.Models
{
    public class BlogCategoryListViewModel
    {
        public List<BlogCategory> BlogCategories { get; set; }

        public int Selected { get; set; }

        public string Title { get; set; }
    }
}