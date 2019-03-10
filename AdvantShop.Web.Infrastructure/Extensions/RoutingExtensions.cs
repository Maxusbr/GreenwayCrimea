using System.Web.Mvc;
using AdvantShop.Web.Infrastructure.Routing;

namespace AdvantShop.Web.Infrastructure.Extensions
{
    public static class RoutingExtensions
    {
        public static void MapSubRoute(this AreaRegistrationContext context, string name, string subdomain, string url, object defaults, string[] namespaces)
        {
            var route = new SubDomainRoute(subdomain, context.AreaName, url, defaults, namespaces);
            context.Routes.Add(name, route);
        }

        public static void MapMobileRoute(this AreaRegistrationContext context, string name, string subdomain, string url, object defaults, string[] namespaces)
        {
            var route = new MobileSubDomainRoute(subdomain, context.AreaName, url, defaults, namespaces);
            context.Routes.Add(name, route);
        }
    }
}
