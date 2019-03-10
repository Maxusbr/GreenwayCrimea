using System.Linq;
using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.ViewModel.News
{
    public class NewsProductsViewModel
    {
        public ProductViewModel Products { get; set; }

        public bool HasItems
        {
            get { return Products != null && Products.Products.Any(); }
        }
    }
}