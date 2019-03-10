using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Web.Infrastructure.Routing;

namespace AdvantShop.Module.StoreReviews.Infrastructure
{
    public class RegisterRouting : IRegisterRouting
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "StoreReviewsHome",
                url: "storereviews",
                defaults: new { controller = "StoreReviews", action = "Index" },
                namespaces: new[] { "AdvantShop.Module.StoreReviews.Controllers" }
                );
        }
    }
}
