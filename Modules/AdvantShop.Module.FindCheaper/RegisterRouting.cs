using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Web.Infrastructure.Routing;

namespace AdvantShop.Module.FindCheaper
{
    public class RegisterRouting : IRegisterRouting
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "AddRequest",
                url: "findcheaper/addrequest",
                defaults: new { controller = "Home", action = "AddRequest" },
                namespaces: new[] { "AdvantShop.Module.FindCheaper.Controllers" }
                );          
        }
    }
}
