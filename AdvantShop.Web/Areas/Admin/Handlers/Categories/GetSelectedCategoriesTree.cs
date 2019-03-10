using System.Collections.Generic;
using AdvantShop.Catalog;

namespace AdvantShop.Web.Admin.Handlers.Categories
{
    public class GetSelectedCategoriesTree
    {
        private readonly List<int> _ids;

        public GetSelectedCategoriesTree(List<int> ids)
        {
            _ids = ids;
        }

        public List<int> Execute()
        {
            var selectedIds = new List<int>();

            foreach (var categoryId in _ids)
            {
                var category = CategoryService.GetCategory(categoryId);
                if (category != null)
                {
                    if (!selectedIds.Contains(category.CategoryId))
                        selectedIds.Add(category.CategoryId);

                    var subCategories = CategoryService.GetChildCategoriesByCategoryId(category.CategoryId, false);

                    if (subCategories != null && subCategories.Count > 0)
                        GetCategoryWithSubCategoriesIds(subCategories, selectedIds);
                }
            }

            return selectedIds;
        }

        private void GetCategoryWithSubCategoriesIds(List<Category> categories, List<int> result)
        {
            if (categories == null)
                return;

            foreach (var category in categories)
            {
                if (!result.Contains(category.CategoryId))
                    result.Add(category.CategoryId);

                if (category.HasChild)
                    GetCategoryWithSubCategoriesIds(CategoryService.GetChildCategoriesByCategoryId(category.CategoryId, false), result);
            }
        }
    }
}
