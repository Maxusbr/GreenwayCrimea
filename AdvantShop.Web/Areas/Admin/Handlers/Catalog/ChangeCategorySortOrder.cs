using System.Linq;
using AdvantShop.Catalog;

namespace AdvantShop.Web.Admin.Handlers.Catalog
{
    public class ChangeCategorySortOrder
    {
        private readonly int _categoryId;
        private readonly int? _prevCategoryId;
        private readonly int? _nextCategoryId;
        private readonly int? _parentCategoryId;

        public ChangeCategorySortOrder(int categoryId, int? prevCategoryId, int? nextCategoryId, int? parentCategoryId)
        {
            _categoryId = categoryId;
            _prevCategoryId = prevCategoryId;
            _nextCategoryId = nextCategoryId;
            _parentCategoryId = parentCategoryId;
        }

        public bool Execute()
        {
            var category = CategoryService.GetCategory(_categoryId);
            if (category == null)
                return false;

            Category prevCategory = null;
            Category nextCategory = null;

            if (_prevCategoryId != null)
                prevCategory = CategoryService.GetCategory(_prevCategoryId.Value);

            if (_nextCategoryId != null)
                nextCategory = CategoryService.GetCategory(_nextCategoryId.Value);
            
            if (prevCategory != null && nextCategory != null)
            {
                if (nextCategory.SortOrder - prevCategory.SortOrder > 1)
                {
                    category.SortOrder = prevCategory.SortOrder + 1;
                    CategoryService.UpdateCategorySortOrder(category.Name, category.SortOrder, category.CategoryId);
                }
                else
                {
                    UpdateSortOrderForAllCategories(category, prevCategory, nextCategory);
                }
            }
            else if (prevCategory != null || nextCategory != null)
            {
                UpdateSortOrderForAllCategories(category, prevCategory, nextCategory);
            }

            if (_parentCategoryId != null && _parentCategoryId != category.ParentCategoryId)
            {
                var parentCategory = CategoryService.GetCategory(_parentCategoryId.Value);
                if (parentCategory != null)
                {
                    category.ParentCategoryId = _parentCategoryId.Value;
                    CategoryService.UpdateCategory(category, true);

                    CategoryService.RecalculateProductsCountManual();
                    CategoryService.ClearCategoryCache();
                }
            }

            return true;
        }

        private void UpdateSortOrderForAllCategories(Category category, Category prevCategory, Category nextCategory)
        {
            var categories =
                CategoryService.GetChildCategoriesByCategoryId(category.ParentCategoryId, false)
                    .Where(x => x.CategoryId != category.CategoryId)
                    .OrderBy(x => x.SortOrder)
                    .ToList();

            if (prevCategory != null)
            {
                var index = categories.FindIndex(x => x.CategoryId == prevCategory.CategoryId);
                categories.Insert(index + 1, category);
            }
            else if (nextCategory != null)
            {
                var index = categories.FindIndex(x => x.CategoryId == nextCategory.CategoryId);
                categories.Insert(index > 0 ? index - 1 : 0, category);
            }

            for (int i = 0; i < categories.Count; i++)
            {
                categories[i].SortOrder = i * 10 + 10;
                CategoryService.UpdateCategorySortOrder(categories[i].Name, categories[i].SortOrder, categories[i].CategoryId);
            }
        }
    }
}
