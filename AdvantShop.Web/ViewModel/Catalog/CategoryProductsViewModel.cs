using System.Collections.Generic;
using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.ViewModel.Catalog
{
    public class CategoryProductsViewModel : ProductViewModel
    {
        public CategoryProductsViewModel(List<ProductModel> products) : base(products)
        {
            DisplayBuyButton = false;
            DisplayPreOrderButton = false;
            DisplayComparison = false;
            DisplayPhotoPreviews = false;
        }

        public string Url { get; set; }
        public int ProductsCount { get; set; }
    }
}