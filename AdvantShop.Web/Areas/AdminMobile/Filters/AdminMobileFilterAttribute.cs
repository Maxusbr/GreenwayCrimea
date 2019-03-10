using System.Linq;
using System.Web.Mvc;
using AdvantShop.Customers;

namespace AdvantShop.Areas.AdminMobile.Filters
{
    public class AdminMobileFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            var currentCustomer = CustomerContext.CurrentCustomer;
            if (currentCustomer.CustomerRole != Role.Administrator && currentCustomer.CustomerRole != Role.Moderator)
            {
                var controller = filterContext.Controller.ToString().Split('.').Last();
                var action = filterContext.ActionDescriptor.ActionName;

                if (controller.ToLower() == "usercontroller" && action.ToLower() == "login")
                    return;
                
                filterContext.Result = new RedirectToRouteResult("AdminMobile_Login", null);
                return;
            }
        }
    }
}
