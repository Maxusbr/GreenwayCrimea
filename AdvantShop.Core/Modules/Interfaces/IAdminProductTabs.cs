//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Web.Routing;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface IAdminProductTabs : IModule
    {
        IList<AdminProductTabItem> GetAdminProductTabs(int productId);
    }

    
    public class AdminProductTabItem
    {
        public string Title { get; private set; }
        public ModuleRoute Route { get; private set; }

        public AdminProductTabItem(string tabTitle, string actionName, string controllerName)
            : this(tabTitle, actionName, controllerName, null) { }

        public AdminProductTabItem(string tabTitle, string actionName, string controllerName, RouteValueDictionary routeValueDictionary)
        {
            if (routeValueDictionary == null)
                routeValueDictionary = new RouteValueDictionary();

            if (routeValueDictionary.ContainsKey("area"))
                routeValueDictionary["area"] = "";
            else
                routeValueDictionary.Add("area", "");

            Title = tabTitle;
            Route = new ModuleRoute()
            {
                ActionName = actionName,
                ControllerName = controllerName,
                RouteValues = routeValueDictionary
            };
        }
    }
}
