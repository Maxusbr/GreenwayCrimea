using System.Collections.Generic;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Models;

namespace AdvantShop.ViewModel.Home
{
    public partial class MainPageProductsViewModel : BaseModel
    {
        public ProductViewModel BestSellers { get; set; }

        public ProductViewModel NewProducts { get; set; }

        public ProductViewModel Sales { get; set; }

        public List<ProductViewModel> ProductLists { get; set; }
        
        public MainPageProductsViewModel()
        {
            ProductLists = new List<ProductViewModel>();
        }
    }
}