using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Web.Infrastructure.Routing;

namespace AdvantShop.Module.ShippingPaymentPage
{
    public class RegisterRouting : IRegisterRouting
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "ShippingPaymentPage",
                url: "shipping-payment",
                defaults: new { controller = "Home", action = "ShippingPayment" },
                namespaces: new[] { "AdvantShop.Module.ShippingPaymentPage.Controllers" }
                );

            routes.MapRoute(
                name: "GetListProduct",
                url: "shipping-payment/getlistproduct",
                defaults: new { controller = "Home", action = "GetListProduct" },
                namespaces: new[] { "AdvantShop.Module.ShippingPaymentPage.Controllers" }
                );

        }
    }
}
