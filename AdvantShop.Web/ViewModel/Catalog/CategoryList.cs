using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Models;

namespace AdvantShop.ViewModel.Catalog
{
    public class CategoryListViewModel : BaseModel
    {
        public CategoryListViewModel()
        {
            Categories = new List<Category>();
            CategoriesWithProducts = new List<CategoryProductsViewModel>();
        }

        public bool DisplayProductCount { get; set; }

        public int CountProductsInLine { get; set; }

        public int CountCategoriesInLine { get; set; }

        public List<Category> Categories { get; set; }

        public List<CategoryProductsViewModel> CategoriesWithProducts { get; set; }
    }
}