using System.Web.Mvc;
using AdvantShop.Web.Infrastructure.Extensions;
using AdvantShop.Core;
using AdvantShop.Catalog;

namespace AdvantShop.Areas.Mobile
{
    public class MobileAreaRegistration : AreaRegistration
    {
        private const string Subdomain = "m";

        public override string AreaName
        {
            get
            {
                return "Mobile";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapMobileRoute(
                "Mobile_Home",  //name
                Subdomain,      //subdomain
                "",             //url
                new { controller = "Home", action = "Index" }, //defaults
                new[] { "AdvantShop.Areas.Mobile.Controllers" }//namespaces
                );

            context.MapMobileRoute(
                "Mobile_Product",
                Subdomain,
                url: "products/{url}",
                defaults: new { controller = "Product", action = "Index" },
                namespaces: new[] { "AdvantShop.Areas.Mobile.Controllers" }
            );

            context.MapMobileRoute(
                "Mobile_Category",
                Subdomain,
                url: "categories/{url}",
                defaults: new { controller = "Catalog", action = "Index" },
                namespaces: new[] { "AdvantShop.Areas.Mobile.Controllers" }
            );

            context.MapMobileRoute(
                "Mobile_CategoryTag",
                Subdomain,
                url: "categories/{url}/tag/{tagUrl}",
                defaults: new { controller = "Catalog", action = "Index" },
                namespaces: new[] { "AdvantShop.Areas.Mobile.Controllers" }
            );

            context.MapMobileRoute(
                    "Mobile_CatalogRoot",
                    Subdomain,
                    url: "catalog",
                    defaults: new { controller = "Catalog", action = "Index", CategoryId = 0 },
                    namespaces: new[] { "AdvantShop.Areas.Mobile.Controllers" }
                );

            context.MapMobileRoute(
                "Mobile_ProductList",
                Subdomain,
                url: "productlist/{type}/{list}",
                defaults: new { controller = "Catalog", action = "ProductList", type = UrlParameter.Optional, list = UrlParameter.Optional },
                namespaces: new[] { "AdvantShop.Areas.Mobile.Controllers" }
            );

            context.MapMobileRoute(
                "Mobile_ProductListTag",
                Subdomain,
                url: "productlist/{type}/tag/{tagUrl}/{list}",
                defaults: new { controller = "Catalog", action = "ProductList", type = UrlParameter.Optional, list = UrlParameter.Optional },
                namespaces: new[] { "AdvantShop.Areas.Mobile.Controllers" }
            );

            context.MapMobileRoute(
                "Mobile_Search",
                Subdomain,
                url: "search",
                defaults: new { controller = "Catalog", action = "Search" },
                namespaces: new[] { "AdvantShop.Areas.Mobile.Controllers" }
            );

            context.MapMobileRoute(
                "Mobile_StaticPage",
                Subdomain,
                url: "pages/{url}",
                defaults: new { controller = "StaticPage", action = "Index" },
                namespaces: new[] { "AdvantShop.Areas.Mobile.Controllers" }
            );

            context.MapMobileRoute(
                "Mobile_Cart",
                Subdomain,
                url: "cart",
                defaults: new { controller = "Cart", action = "Index" },
                namespaces: new[] { "AdvantShop.Areas.Mobile.Controllers" }
            );

            //context.MapMobileRoute(
            //    "Mobile_Checkout",
            //    Subdomain,
            //    url: "checkout",
            //    defaults: new { controller = "Checkout", action = "Index" },
            //    namespaces: new[] { "AdvantShop.Areas.Mobile.Controllers" }
            //);

            context.MapMobileRoute(
                "Mobile_CheckoutSuccess",
                Subdomain,
                url: "checkout/success/{code}",
                defaults: new { controller = "Checkout", action = "Success", code = UrlParameter.Optional },
                namespaces: new[] { "AdvantShop.Areas.Mobile.Controllers" }
            );

            //context.MapMobileRoute(
            //    "Mobile_CheckoutConfirm",
            //    Subdomain,
            //    url: "checkoutconfirm",
            //    defaults: new { controller = "Checkout", action = "CheckoutConfirm" },
            //    namespaces: new[] { "AdvantShop.Areas.Mobile.Controllers" }
            //);

            context.MapMobileRoute(
                "Mobile_ChangeCity",
                Subdomain,
                url: "changecity",
                defaults: new { controller = "Home", action = "ChangeCity" },
                namespaces: new[] { "AdvantShop.Areas.Mobile.Controllers" }
            );

            //context.MapMobileRoute(
            //    "Mobile_ToFullVersion",
            //    Subdomain,
            //    url: "fullversion",
            //    defaults: new { controller = "Home", action = "ToFullVersion" },
            //    namespaces: new[] { "AdvantShop.Areas.Mobile.Controllers" }
            //);

            context.MapMobileRoute(
                "Mobile_Login",
                Subdomain,
                url: "login",
                defaults: new { controller = "User", action = "Login" },
                namespaces: new[] { "AdvantShop.Areas.Mobile.Controllers" }
            );


            context.MapRoute(
                "Mobile_Root",
                url: "mobile",
                defaults: new { controller = "Error", action = "NotFound", area = "" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            context.MapRoute(
                "Mobile_Default",
                url: "mobile/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "AdvantShop.Areas.Mobile.Controllers" }
            );

            //context.Routes.Add("Mobile_Subdomain",
            //    new SubDomainRoute("{controller}/{action}/{id}",
            //        new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //        new[] { "AdvantShop.Areas.Mobile.Controllers" }));

        }
    }
}