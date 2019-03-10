using System.Web.Mvc;
using AdvantShop.Customers;
using AdvantShop.Trial;
using AdvantShop.Web.Infrastructure.ActionResults;

namespace AdvantShop.Web.Infrastructure.Filters
{
    public class InPlaceAttribute : ActionFilterAttribute
    {
        public RoleAction RoleKey { get; set; }
        
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!Authorize())
            {
                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.HttpContext.Response.StatusDescription = "Access denied";

                filterContext.Result = new JsonNetResult() {Data = new {error = true}};
            }
        }

        private bool Authorize()
        {
            var customer = CustomerContext.CurrentCustomer;

            return customer.IsAdmin ||
                   customer.CustomerRole == Role.Moderator && customer.HasRoleAction(RoleKey) && RoleKey != RoleAction.None ||
                   customer.IsVirtual || 
                   TrialService.IsTrialEnabled;
        }
    }
}
