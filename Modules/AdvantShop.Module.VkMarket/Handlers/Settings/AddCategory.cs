using System;
using AdvantShop.Core;
using AdvantShop.Diagnostics;
using AdvantShop.Module.VkMarket.Domain;
using AdvantShop.Module.VkMarket.Models.Settings;
using AdvantShop.Module.VkMarket.Services;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Module.VkMarket.Handlers.Settings
{
    public class AddUpdateCategory : AbstractCommandHandler<bool>
    {
        private readonly CategoryModel _category;
        private readonly VkCategoryService _categoryService;
        private readonly VkMarketApiService _apiService;

        public AddUpdateCategory(CategoryModel category)
        {
            _category = category;
            _categoryService = new VkCategoryService();
            _apiService = new VkMarketApiService();
        }

        protected override bool Handle()
        {
            try
            {
                var cat = new VkCategory()
                {
                    Id = _category.Id,
                    VkId = _category.VkId,
                    Name = _category.Name,
                    VkCategoryId = _category.VkCategoryId,
                    SortOrder = _category.SortOrder
                };

                var updateSortOrder = true;

                if (_category.Id == 0)
                {
                    _categoryService.Add(cat);
                }
                else
                {
                    var c = _categoryService.Get(cat.Id);
                    updateSortOrder = c != null && c.SortOrder != cat.SortOrder;

                    _categoryService.Update(cat);
                    _categoryService.RemoveLinks(cat.Id);
                }

                if (_category.CategoryIds != null)
                {
                    foreach (var categoryId in _category.CategoryIds)
                    {
                        _categoryService.AddLink(categoryId, cat.Id);
                    }
                }

                // change sortorder
                if (updateSortOrder)
                {
                    var categories = _categoryService.GetList();
                    if (categories.Count > 1)
                    {
                        var index = categories.FindIndex(x => x.Id == _category.Id);
                        var after = index > 0 ? categories[index - 1].VkId : default(long?);
                        var before = index + 1 < categories.Count ? categories[index + 1].VkId : default(long?);

                        _apiService.ReorderAlbums(cat.VkId, before, after);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);

                if (ex.Message != null)
                {
                    if (ex.Message == "Access denied")
                        throw new BlException("У пользователя нет прав на создание категорий или в группе не активирована опция \"Товары в Вконтакте\"");

                    throw new BlException(ex.Message);
                }

                return false;
            }

            return true;
        }

    }
}
