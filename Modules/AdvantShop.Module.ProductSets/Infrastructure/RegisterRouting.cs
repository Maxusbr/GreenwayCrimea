using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Web.Infrastructure.Routing;

namespace AdvantShop.Module.ProductSets.Infrastructure
{
    public class RegisterRouting : IRegisterRouting
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "FormatPrice",
                url: "productsets/formatprice",
                defaults: new { controller = "ProductSet", action = "FormatPrice" },
                namespaces: new[] { "AdvantShop.Module.ProductSets.Controllers" }
                );
        }
    }
}
