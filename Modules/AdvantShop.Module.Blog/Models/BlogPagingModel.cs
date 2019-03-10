using System.Collections.Generic;
using AdvantShop.Core.Models;
using AdvantShop.Module.Blog.Domain;

namespace AdvantShop.Module.Blog.Models
{
    public class BlogPagingModel
    {
        public List<BlogItem> BlogItems { get; set; }

        public Pager Pager { get; set; }
    }
}
