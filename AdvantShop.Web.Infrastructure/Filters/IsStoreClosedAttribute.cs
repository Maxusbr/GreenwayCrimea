using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Customers;

namespace AdvantShop.Web.Infrastructure.Filters
{
    /// <summary>
    /// Check is store closed
    /// </summary>
    public class IsStoreClosedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction || !SettingsMain.IsStoreClosed)
                return;

            var controller = (string)filterContext.RouteData.Values["controller"];
            var action = (string)filterContext.RouteData.Values["action"];

            if (controller == "Common" && (action == "ClosedStore" || action == "KeepAlive"))
                return;

            var customer = CustomerContext.CurrentCustomer;
            if (customer != null && !customer.IsAdmin && !customer.IsModerator)
            {
                filterContext.Result = new RedirectToRouteResult("Closed", null);
            }
        }
    }
}
