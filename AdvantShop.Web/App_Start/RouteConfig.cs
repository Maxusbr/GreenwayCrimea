using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Core;
using AdvantShop.Catalog;
using AdvantShop.Diagnostics;
using AdvantShop.Web.Infrastructure.Routing;

namespace AdvantShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            RegisterAllRoutes(routes);

            routes.MapRoute(
                name: "Product",
                url: "products/{url}",
                defaults: new { controller = "Product", action = "Index" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "Category",
                url: "categories/{url}",
                defaults: new { controller = "Catalog", action = "Index" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "CategoryTag",
                url: "categories/{url}/tag/{tagUrl}",
                defaults: new { controller = "Catalog", action = "Index" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                    name: "CatalogRoot",
                    url: "catalog",
                    defaults: new { controller = "Catalog", action = "Index", CategoryId = 0 },
                    namespaces: new[] { "AdvantShop.Controllers" }
                );

            routes.MapRoute(
                name: "StaticPage",
                url: "pages/{url}",
                defaults: new { controller = "StaticPage", action = "Index" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "NewsHome",
                url: "news",
                defaults: new { controller = "News", action = "NewsCategory" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "NewsRss",
                url: "news/rss",
                defaults: new { controller = "News", action = "Rss" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "News",
                url: "news/{url}",
                defaults: new { controller = "News", action = "NewsItem" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "NewsSubscribe",
                url: "newssubscribe",
                defaults: new { controller = "News", action = "Subscribe" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "Managers",
                url: "managers",
                defaults: new { controller = "Managers", action = "Index" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "Search",
                url: "search",
                defaults: new { controller = "Search", action = "Index" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "NewsCategory",
                url: "newscategory/{url}",
                defaults: new { controller = "News", action = "NewsCategory" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );


            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "User", action = "Login" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new { controller = "User", action = "Logout" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "Registration",
                url: "registration",
                defaults: new { controller = "User", action = "Registration" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "Cart",
                url: "cart",
                defaults: new { controller = "Cart", action = "Index" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "Checkout",
                url: "checkout",
                defaults: new { controller = "Checkout", action = "Index" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "CheckoutSuccess",
                url: "checkout/success/{code}",
                defaults: new { controller = "Checkout", action = "Success" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "BrandRoot",
                url: "manufacturers",
                defaults: new { controller = "Brand", action = "Index" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "Brand",
                url: "manufacturers/{url}",
                defaults: new { controller = "Brand", action = "BrandItem" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "Compare",
                url: "compare",
                defaults: new { controller = "Compare", action = "Index" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );
            
            routes.MapRoute(
                name: "ProductList",
                url: "productlist/{type}/{list}",
                defaults: new { controller = "Catalog", action = "ProductList", type = UrlParameter.Optional, list = UrlParameter.Optional },
                constraints: new { type = "[a-z]*", list = "[0-9]*" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
               name: "ProductListTag",
               url: "productlist/{type}/tag/{tagUrl}/{list}",
               defaults: new { controller = "Catalog", action = "ProductList", type = UrlParameter.Optional, list = UrlParameter.Optional },
               constraints: new { type = "[a-z]*", list = "[0-9]*" },
               namespaces: new[] { "AdvantShop.Controllers" }
           );

            routes.MapRoute(
                name: "PreOrderByCode",
                url: "preorder/linkbycode",
                defaults: new { controller = "PreOrder", action = "LinkByCode" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "PreOrder",
                url: "preorder/{offerid}",
                defaults: new { controller = "PreOrder", action = "Index", offerid = UrlParameter.Optional },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "GiftCertificate",
                url: "giftcertificate",
                defaults: new { controller = "GiftCertificate", action = "Index" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "Wishlist",
                url: "wishlist",
                defaults: new { controller = "Wishlist", action = "Index" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "Feedback",
                url: "feedback",
                defaults: new { controller = "Feedback", action = "Index" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "PrintOrder",
                url: "printorder/{code}",
                defaults: new { controller = "Checkout", action = "PrintOrder" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "ForgotPassword",
                url: "forgotpassword",
                defaults: new { controller = "User", action = "ForgotPassword" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "PaymentReturnUrl",
                url: "paymentreturnurl/{advPaymentId}",
                defaults: new { controller = "PaymentStatus", action = "Success", advPaymentId = UrlParameter.Optional },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "PaymentNotification",
                url: "paymentnotification/{advPaymentId}",
                defaults: new { controller = "PaymentStatus", action = "Notification", advPaymentId = UrlParameter.Optional },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
               name: "Cancel",
               url: "cancel",
               defaults: new { controller = "PaymentStatus", action = "Cancel", id = UrlParameter.Optional },
               namespaces: new[] { "AdvantShop.Controllers" }
           );

            routes.MapRoute(
               name: "Fail",
               url: "fail",
               defaults: new { controller = "PaymentStatus", action = "Fail", id = UrlParameter.Optional },
               namespaces: new[] { "AdvantShop.Controllers" }
           );

            routes.MapRoute(
                name: "MyAccount",
                url: "myaccount",
                defaults: new { controller = "MyAccount", action = "Index" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
                name: "Closed",
                url: "closed",
                defaults: new { controller = "Common", action = "ClosedStore" },
                namespaces: new[] { "AdvantShop.Controllers" }
            );

            routes.MapRoute(
               name: "GetBonusCardRoute",
               url: "getbonuscard",
               defaults: new { controller = "Bonuses", action = "GetBonusCard" },
               namespaces: new[] { "AdvantShop.Controllers" }
               );

            routes.MapRoute(
               name: "Business",
               url: "business",
               defaults: new { controller = "Business", action = "Index" },
               namespaces: new[] { "AdvantShop.Controllers" }
               );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "AdvantShop.Controllers" }
            );


        }

        private static void RegisterAllRoutes(RouteCollection routes)
        {
            var iRegisterRouting = typeof(IRegisterRouting);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains("AdvantShop")))
            {
                Type[] types = null;
                try
                {
                    types = assembly.GetTypes().Where(x => x.GetInterfaces().Contains(iRegisterRouting)).ToArray();
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }
                if (types == null)
                {
                    return;
                }
                foreach (var type in types)
                {
                    try
                    {
                        var instance = (IRegisterRouting)Activator.CreateInstance(type);
                        instance.RegisterRoutes(routes);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }
                }

            }
        }
    }
}