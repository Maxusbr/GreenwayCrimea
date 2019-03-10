using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Models.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvantShop.Handlers.Catalog
{
    public class FilterSelectCategoryHandler
    {
        private int _categoryId;

        public FilterSelectCategoryHandler(int categoryId) {
            _categoryId = categoryId;
        }

        public FilterItemModel Get()
        {
            var model = new FilterItemModel()
            {
                Expanded = true,
                Type = "categoryId",
                Title = LocalizationService.GetResource("Catalog.FilterSelectCategory.FilterTitle"),
                Control = "selectSearch"
            };

            var categories = CategoryService.GetChildCategoriesByCategoryId(0, false).Where(cat => cat.Enabled && !cat.Hidden);

            foreach (var category in categories)
            {
                model.Values.Add(new FilterListItemModel()
                {
                    Id = category.CategoryId.ToString(),
                    Text = category.Name,
                    Selected = category.CategoryId == _categoryId
                });
            }

            return model;
        }

        public Task<List<FilterItemModel>> GetAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                Localization.Culture.InitializeCulture();
                return new List<FilterItemModel>
                {
                    Get()
                };
            });

        }
    }    
}