using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.Module.Blog.Models
{
    public class BlogProductsViewModel 
    {
        public ProductViewModel Products { get; set; }

        public bool HasItems
        {
            get { return Products != null && Products.Products.Count > 0; }
        }
    }
}
