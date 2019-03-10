using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Web.Infrastructure.Routing;

namespace AdvantShop.Module.JivoSite.Infrastructure
{
    public class RegisterRouting : IRegisterRouting
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "jivowebhook",
                url: "jivosite/webhook",
                defaults: new { controller = "Home", action = "Webhook" },
                namespaces: new[] { "AdvantShop.Module.Jivosite.Controllers" }
                );

            routes.MapRoute(
                name: "jivosite_login",
                url: "jivosite/login",
                defaults: new { controller = "Home", action = "Login" },
                namespaces: new[] { "AdvantShop.Module.Jivosite.Controllers" }
                );
        }
    }
}
