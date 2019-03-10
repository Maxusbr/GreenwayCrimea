using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Areas.AdminMobile.Filters;
using AdvantShop.Controllers;
using AdvantShop.Customers;
using AdvantShop.Saas;
using AdvantShop.Web.Infrastructure.Controllers;

namespace AdvantShop.Areas.AdminMobile.Controllers
{
    [AdminMobileFilter]
    public partial class BaseAdminMobileController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveMobileAdmin)
            {
                Error404();
            }
        }

        protected ActionResult Error404()
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "NotFound");

            IController errorController = new ErrorController();
            errorController.Execute(new RequestContext(HttpContext, routeData));
            return new EmptyResult();
        }

        protected bool HasRole(RoleAction roleAction)
        {
            var customer = CustomerContext.CurrentCustomer;

            if (customer.IsAdmin || customer.IsVirtual)
                return true;

            if (customer.IsModerator)
            {
                var customerRolesActions = RoleActionService.GetCustomerRoleActionsByCustomerId(customer.Id);

                if (customerRolesActions.Any(x => x.Role == roleAction))
                    return true;
            }

            return false;
        }
    }
}