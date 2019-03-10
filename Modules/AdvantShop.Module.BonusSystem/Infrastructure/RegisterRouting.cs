using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Web.Infrastructure.Routing;

namespace AdvantShop.Module.BonusSystemModule.Infrastructure
{
    public class RegisterRouting : IRegisterRouting
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "GetBonusCard",
                url: "getbonuscard",
                defaults: new { controller = "BonusSystem", action = "GetBonusCard" },
                namespaces: new[] { "AdvantShop.Module.BonusSystemModule.Controllers" }
                );
        }
    }
}
