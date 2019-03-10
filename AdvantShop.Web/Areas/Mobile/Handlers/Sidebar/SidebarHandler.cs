using System.Linq;
using AdvantShop.Areas.Mobile.Models.Sidebar;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Customers;
using AdvantShop.Repository;

namespace AdvantShop.Areas.Mobile.Handlers.Sidebar
{
    public class SidebarHandler
    {
        public SidebarMobileViewModel Get()
        {
            var model = new SidebarMobileViewModel()
            {
                Customer = CustomerContext.CurrentCustomer,
                StoreName = SettingsMain.ShopName,
                DisplayCity = SettingsMobile.DisplayCity,
                CurrentCity = SettingsMobile.DisplayCity ? IpZoneContext.CurrentZone.City : string.Empty
            };

            var isRegistered = CustomerContext.CurrentCustomer.RegistredUser;
            var cacheName = !isRegistered
                                ? CacheNames.GetMainMenuCacheObjectName() + "TopMenu_Mobile" 
                                : CacheNames.GetMainMenuAuthCacheObjectName() + "TopMenu_Mobile";

            var menuType = isRegistered
                                ? EMenuItemShowMode.Authorized
                                : EMenuItemShowMode.NotAuthorized;

            model.Menu = CacheManager.Get(cacheName, () => MenuService.GetMenuItems(0, EMenuType.Mobile, menuType).ToList());


            return model;
        }
    }
}