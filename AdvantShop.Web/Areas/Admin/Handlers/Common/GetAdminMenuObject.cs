using System.Collections.Generic;
using System.Web.Hosting;
using AdvantShop.Core.Caching;
using AdvantShop.Web.Admin.Models.Menus;
using Newtonsoft.Json;
using System.Web;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Web.Admin.Handlers.Common
{
    public class GetAdminMenuObject
    {
        public List<AdminMenuModel> Execute()
        {
            var items = CacheManager.Get(CacheNames.AdminMenu, () =>
            {
                var filePath = HostingEnvironment.MapPath("~/Areas/Admin/menu.json");
                if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                {
                    var json = System.IO.File.ReadAllText(filePath);

                    var menuObject = JsonConvert.DeserializeObject<List<AdminMenuModel>>(json);
                    if (menuObject != null)
                    {
                        return menuObject;
                    }
                }
                return new List<AdminMenuModel>();
            });

            var menu = items.DeepClone();

            SetVisible(menu);
            SetSelected(menu);
            SetOpaсity(menu);

            return menu;

        }

        private void SetVisible(List<AdminMenuModel> items)
        {
            foreach (var menuItem in items)
            {
                menuItem.SetVisible(menuItem);
            }
        }

        private void SetSelected(List<AdminMenuModel> items)
        {
            var controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"] as string;
            var action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"] as string;

            if (controller != null)
                controller = controller.ToLower();
            if (action != null)
                action = action.ToLower();

            foreach (var menuItem in items)
            {
                if (menuItem.SetSelected(action, controller))
                    break;
            }
        }

        private void SetOpaсity(List<AdminMenuModel> items)
        {            
            foreach (var item in items)
            {
                item.SetActiveInSaas(item);
            }
        }
    }
}
