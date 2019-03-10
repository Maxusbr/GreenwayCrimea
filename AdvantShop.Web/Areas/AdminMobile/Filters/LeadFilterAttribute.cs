using System.Web.Mvc;
using AdvantShop.Customers;
using AdvantShop.Saas;

namespace AdvantShop.Areas.AdminMobile.Filters
{
    public class LeadFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SaasDataService.IsSaasEnabled && !SaasDataService.CurrentSaasData.HaveCrm)
            {
                filterContext.Result = new RedirectToRouteResult("AdminMobile_Login", null);
                return;
            }

            if (!CustomerContext.CurrentCustomer.IsAdmin && !CustomerContext.CurrentCustomer.IsManager)
            {
                filterContext.Result = new RedirectToRouteResult("AdminMobile_Login", null);
                return;
            }
        }
    }
}
