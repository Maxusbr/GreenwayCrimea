using System.Collections.Generic;
using AdvantShop.Core.Modules.Interfaces;

namespace AdvantShop.ViewModel.ProductDetails
{
    public class ProductTabsViewModel : BaseProductViewModel
    {
        public ProductTabsViewModel()
        {
            Tabs = new List<ITab>();
        }

        public ProductDetailsViewModel ProductModel { get; set; }
        
        public List<ITab> Tabs { get; set; }

        public string AdditionalDescription { get; set; }

        public int ReviewsCount { get; set; }

        public bool UseStandartReviews { get; set; }
    }
}