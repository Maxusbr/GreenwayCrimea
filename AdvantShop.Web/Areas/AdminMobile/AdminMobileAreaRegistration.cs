using System.Web.Mvc;

namespace AdvantShop.Areas.AdminMobile
{
    public class AdminMobileAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AdminMobile";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "AdminMobile_GetOrders",
                url: "adminmobile/getlastorders",
                defaults: new { controller = "Orders", action = "GetLastOrders" },
                namespaces: new[] { "AdvantShop.Areas.AdminMobile.Controllers" }
            );

            context.MapRoute(
                name: "AdminMobile_LastOrders",
                url: "adminmobile/getorders",
                defaults: new { controller = "Orders", action = "GetOrders" },
                namespaces: new[] { "AdvantShop.Areas.AdminMobile.Controllers" }
            );

            context.MapRoute(
                name: "AdminMobile_ChangeOrderStatus",
                url: "adminmobile/changeorderstatus",
                defaults: new { controller = "Orders", action = "ChangeOrderStatus" },
                namespaces: new[] { "AdvantShop.Areas.AdminMobile.Controllers" }
            );

            context.MapRoute(
                name: "AdminMobile_SetOrderPaid",
                url: "adminmobile/setorderpaid",
                defaults: new { controller = "Orders", action = "SetOrderPaid" },
                namespaces: new[] { "AdvantShop.Areas.AdminMobile.Controllers" }
            );

            context.MapRoute(
                name: "AdminMobile_Orders",
                url: "adminmobile/orders/{statusId}",
                defaults: new { controller = "Orders", action = "Index", statusId = UrlParameter.Optional },
                namespaces: new[] { "AdvantShop.Areas.AdminMobile.Controllers" }
            );

            context.MapRoute(
                name: "AdminMobile_Order",
                url: "adminmobile/order/{orderId}",
                defaults: new { controller = "Orders", action = "OrderItem" },
                namespaces: new[] { "AdvantShop.Areas.AdminMobile.Controllers" }
            );

            context.MapRoute(
                name: "AdminMobile_Tasks",
                url: "adminmobile/tasks",
                defaults: new { controller = "Tasks", action = "Index" },
                namespaces: new[] { "AdvantShop.Areas.AdminMobile.Controllers" }
            );

            context.MapRoute(
                name: "AdminMobile_Task",
                url: "adminmobile/task/{taskId}",
                defaults: new { controller = "Tasks", action = "Task" },
                namespaces: new[] { "AdvantShop.Areas.AdminMobile.Controllers" }
            );


            context.MapRoute(
                name: "AdminMobile_Leads",
                url: "adminmobile/leads",
                defaults: new { controller = "Leads", action = "Index" },
                namespaces: new[] { "AdvantShop.Areas.AdminMobile.Controllers" }
            );

            context.MapRoute(
                name: "AdminMobile_Lead",
                url: "adminmobile/lead/{id}",
                defaults: new { controller = "Leads", action = "Lead" },
                namespaces: new[] { "AdvantShop.Areas.AdminMobile.Controllers" }
            );


            context.MapRoute(
                name: "AdminMobile_Login",
                url: "adminmobile/login",
                defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "AdvantShop.Areas.AdminMobile.Controllers" }
            );

            context.MapRoute(
                name: "AdminMobile_Logout",
                url: "adminmobile/logout",
                defaults: new { controller = "User", action = "Logout", id = UrlParameter.Optional },
                namespaces: new[] { "AdvantShop.Areas.AdminMobile.Controllers" }
            );

            context.MapRoute(
                name: "AdminMobile_Graphics",
                url: "adminmobile/graphics",
                defaults: new { controller = "Orders", action = "Graphics" },
                namespaces: new[] { "AdvantShop.Areas.AdminMobile.Controllers" }
            );

            context.MapRoute(
                name: "AdminMobile_GetGraphics",
                url: "adminmobile/getgraphics",
                defaults: new { controller = "Orders", action = "GetGraphics" },
                namespaces: new[] { "AdvantShop.Areas.AdminMobile.Controllers" }
            );

            context.MapRoute(
                name: "AdminMobile_Attendance",
                url: "adminmobile/attendance",
                defaults: new { controller = "Attendance", action = "Index" },
                namespaces: new[] { "AdvantShop.Areas.AdminMobile.Controllers" }
            );

            context.MapRoute(
                name: "AdminMobile_Home",
                url: "adminmobile",
                defaults: new { controller = "Home", action = "Index" },
                namespaces: new[] { "AdvantShop.Areas.AdminMobile.Controllers" }
            );

            context.MapRoute(
                name: "AdminMobile_Default",
                url: "adminmobile/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "AdvantShop.Areas.AdminMobile.Controllers" }
            );
        }
    }
}