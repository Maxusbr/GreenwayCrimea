using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Services.CMS;
using AdvantShop.Customers;
using AdvantShop.Web.Infrastructure.Extensions;
using System.Diagnostics;

namespace AdvantShop.Handlers.Menu
{
    public class MenuHandler
    {
        private const int CacheTime = 60;
        private readonly UrlHelper _urlHelper;

        public MenuHandler()
        {
            _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        /// <summary>
        /// Get catalog menu items
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public MenuItemModel GetCatalogMenuItems(int categoryId)
        {
            return CacheManager.Get(CacheNames.MenuCatalog + categoryId + "_" + HttpContext.Current.Request.Url.Host, CacheTime, () =>
            {
                if (categoryId != 0)
                {
                    var subCategories = CategoryService.GetChildCategoriesByCategoryIdForMenu(categoryId).Where(cat=> !cat.Hidden).ToList();
                    categoryId = subCategories.Count > 0 ? subCategories[0].ParentCategoryId : 0;
                }
                var menuItems = new MenuItemModel()
                {
                    SubItems = GetRootSubcategories(categoryId)
                };
                foreach (var subCategory in menuItems.SubItems.Where(x => x.HasChild ))
                {                   
                    subCategory.SubItems = GetSubcategories(subCategory.ItemId);
                    if (subCategory.DisplaySubItems)
                    {
                        foreach (var subChildrenCategory in subCategory.SubItems.Where(x => x.HasChild))
                        {
                            subChildrenCategory.SubItems = GetSubcategories(subChildrenCategory.ItemId);
                        }
                    }

                    if (subCategory.DisplayBrandsInMenu)
                    {                        
                        subCategory.Brands = BrandService.GetBrandsByCategoryId(subCategory.ItemId, true);
                    }
                }
                return menuItems;
            });
        }

        /// <summary>
        /// Get main menu items
        /// </summary>
        /// <returns></returns>
        public List<MenuItemModel> GetMenuItems()
        {
            var isRegistered = CustomerContext.CurrentCustomer.RegistredUser;
            var cacheName = !isRegistered
                                ? CacheNames.GetMainMenuCacheObjectName() + "MainMenu"
                                : CacheNames.GetMainMenuAuthCacheObjectName() + "MainMenu";
            var menuType = isRegistered
                                ? EMenuItemShowMode.Authorized
                                : EMenuItemShowMode.NotAuthorized;

            return CacheManager.Get(cacheName, CacheTime, () =>
            {
                var menuItems = MenuService.GetMenuItems(0, EMenuType.Top, menuType);

                foreach (var item in menuItems.Where(item => item.HasChild))
                    item.SubItems = MenuService.GetMenuItems(item.ItemId, EMenuType.Top, menuType);

                return menuItems;
            });
        }


        #region Help methods

        private List<MenuItemModel> GetSubcategories(int categoryId)
        {
            
            var subcategories =
                CategoryService.GetChildCategoriesByCategoryId(categoryId, false)
                    .Where(cat => cat.Enabled && !cat.Hidden)
                    .Select(c => new MenuItemModel()
                    {
                        ItemId = c.CategoryId,
                        ItemParentId = c.ParentCategoryId,
                        Name = c.Name,
                        UrlPath = _urlHelper.AbsoluteRouteUrl("Category", new { url = c.UrlPath }),
                        HasChild = c.HasChild,
                        DisplayBrandsInMenu = c.DisplayBrandsInMenu,
                        DisplaySubItems = c.DisplaySubCategoriesInMenu,
                        ProductsCount = c.ProductsCount
                    }).ToList();

            return subcategories;
        }

        private List<MenuItemModel> GetRootSubcategories(int categoryId)
        {
            return
                CategoryService.GetChildCategoriesByCategoryIdForMenu(categoryId).Where(cat=> !cat.Hidden)
                    .Select(c => new MenuItemModel()
                    {
                        ItemId = c.CategoryId,
                        ItemParentId = c.ParentCategoryId,
                        Name = c.Name,
                        UrlPath = _urlHelper.AbsoluteRouteUrl("Category", new { url = c.UrlPath }),
                        HasChild = c.HasChild,
                        DisplayBrandsInMenu = c.DisplayBrandsInMenu,
                        DisplaySubItems = c.DisplaySubCategoriesInMenu,
                        IconPath = c.Icon != null ? c.Icon.IconSrc() : "",
                        ProductsCount = c.ProductsCount
                    }).ToList();
        }

        #endregion
    }
}
