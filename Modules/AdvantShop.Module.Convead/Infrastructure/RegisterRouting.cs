using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Web.Infrastructure.Routing;

namespace AdvantShop.Module.Convead.Infrastructure
{
    public class RegisterRouting : IRegisterRouting
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "ConveadHome",
                url: "convead",
                defaults: new { controller = "Convead", action = "Index" },
                namespaces: new[] { "AdvantShop.Module.Convead.Controllers" }
                );
        }
    }
}
