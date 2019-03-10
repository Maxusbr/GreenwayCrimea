using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Web.Infrastructure.Routing;

namespace AdvantShop.Module.SimaLand.Infrastructure
{
    public class RegisterRouting : IRegisterRouting
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "pageload",
                url: "pageload",
                defaults: new { controller = "ComparisonCategory", action = "Load_Page" },
                namespaces: new[] { "AdvantShop.Module.SimaLand.Controllers" }
                ); 
            routes.MapRoute(
                name: "parsecategory",
                url: "parsecategory",
                defaults: new { controller = "ComparisonCategory", action = "ParseCategory" },
                namespaces: new[] { "AdvantShop.Module.SimaLand.Controllers" }
                );
            routes.MapRoute(
                name: "getlabelssimaland",
                url: "getlabels",
                defaults: new { controller = "Client", action = "GetLabel" },
                namespaces: new[] { "AdvantShop.Module.SimaLand.Controllers" }
                );
        }
    }
}
