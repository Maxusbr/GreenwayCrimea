using System.Collections.Generic;
using AdvantShop.CMS;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Services.CMS;
using AdvantShop.Customers;

namespace AdvantShop.Handlers.Common
{
    public class MenuTopHanlder
    {
        public List<MenuItemModel> GetTopMenuItems()
        {
            var isRegistered = CustomerContext.CurrentCustomer.RegistredUser;
            var cacheName = !isRegistered
                                ? CacheNames.GetMainMenuCacheObjectName() + "TopMenu"
                                : CacheNames.GetMainMenuAuthCacheObjectName() + "TopMenu";

            var menuType = isRegistered
                                ? EMenuItemShowMode.Authorized
                                : EMenuItemShowMode.NotAuthorized;

            return CacheManager.Get(cacheName, () => MenuService.GetMenuItems(0, EMenuType.Top, menuType));
        }
    }
}