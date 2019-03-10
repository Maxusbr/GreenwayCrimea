using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Web.Infrastructure.Routing;

namespace AdvantShop.Module.RussianPostPrintBlank.Infrastructure
{
    public class RegisterRouting : IRegisterRouting
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "PrintFormPrint",
                url: "RussianPostPrintBlank",
                defaults: new { controller = "PrintForm", action = "Print" },
                namespaces: new[] { "AdvantShop.Module.RussianPostPrintBlank.Controllers" }
            );

            routes.MapRoute(
                name: "PrintFormPrintByTemplate",
                url: "RussianPostPrintFormByTemplate",
                defaults: new { controller = "PrintForm", action = "PrintByTemplate" },
                namespaces: new[] { "AdvantShop.Module.RussianPostPrintBlank.Controllers" }
            );
        }
    }
}
