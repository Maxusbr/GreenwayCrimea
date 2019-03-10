using System.Web.Mvc;

namespace AdvantShop.Areas.Api
{
    public class ApiAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Api"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "Api_1C",
                url: "api/1c/{action}/",
                defaults: new {controller = "OneC", action = "Index"},
                namespaces: new[] {"AdvantShop.Areas.Api.Controllers"}
                );

            context.MapRoute(
                name: "Api_Default",
                url: "api/{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
                namespaces: new[] {"AdvantShop.Areas.Api.Controllers"}
                );
        }
    }
}