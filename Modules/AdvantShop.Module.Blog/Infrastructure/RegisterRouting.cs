using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Web.Infrastructure.Routing;

namespace AdvantShop.Module.Blog.Infrastructure
{
    public class RegisterRouting : IRegisterRouting
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "BlogHome",
                url: "blog",
                defaults: new { controller = "Home", action = "BlogCategory" },
                namespaces: new[] { "AdvantShop.Module.Blog.Controllers" }
                );

            routes.MapRoute(
               name: "BlogRss",
               url: "blog/rss",
               defaults: new { controller = "Home", action = "Rss", category = UrlParameter.Optional },
               namespaces: new[] { "AdvantShop.Module.Blog.Controllers" }
               );

            routes.MapRoute(
                name: "BlogCategory",
                url: "blog/category/{category}",
                defaults: new { controller = "Home", action = "BlogCategory", category = UrlParameter.Optional },
                namespaces: new[] { "AdvantShop.Module.Blog.Controllers" }
                );

            routes.MapRoute(
                name: "BlogItem",
                url: "blog/{url}",
                defaults: new { controller = "Home", action = "BlogItem" },
                namespaces: new[] { "AdvantShop.Module.Blog.Controllers" }
                );
        }
    }
}
