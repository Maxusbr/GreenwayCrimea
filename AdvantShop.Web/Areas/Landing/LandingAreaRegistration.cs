using System.Web.Mvc;

namespace AdvantShop.App.Landing
{
    public class LandingAreaRegistration : AreaRegistration 
    {
        public override string AreaName
        {
            get { return "Landing"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "Landing",
                url: "lp/{url}",
                defaults: new { controller = "Landing", action = "Index", area = "Landing" },
                namespaces: new[] { "AdvantShop.App.Landing.Controllers" }
                );

            context.MapRoute(
                "Landing_Admin",
                "AdminV2/Landing/{controller}/{action}/{id}",
                new { controller = "LandingAdmin", action = "Index", area = "Landing", id = UrlParameter.Optional },
                namespaces: new[] { "AdvantShop.App.Landing.Controllers" }
            );

            context.MapRoute(
                name: "Landing_default",
                url: "Landing/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", area = "Landing", id = UrlParameter.Optional },
                namespaces: new[] { "AdvantShop.App.Landing.Controllers" }
            );
        }
    }
}
