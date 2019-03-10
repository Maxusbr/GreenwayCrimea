using System;
using System.Linq;
using AdvantShop.Module.VkMarket.Models.Settings;
using AdvantShop.Module.VkMarket.Services;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Module.VkMarket.Handlers.Settings
{
    public class GetCategories
    {
        private readonly VkCategoryService _vkCategoryService = new VkCategoryService();
        private readonly VkMarketApiService _vkMarketApiService = new VkMarketApiService();


        public FilterResult<CategoryModel> Execute()
        {
            var marketCategories = _vkMarketApiService.GetMarketCategories();

            var categories =
                _vkCategoryService.GetList().Select(x => new CategoryModel(x, marketCategories)).ToList();
            

            var model = new FilterResult<CategoryModel>
            {
                TotalItemsCount = categories.Count,
                TotalPageCount = 1,
                DataItems = categories
            };

            return model;
        }

    }
}
