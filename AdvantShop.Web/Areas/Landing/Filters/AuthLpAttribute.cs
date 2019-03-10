using System.Web;
using System.Web.Mvc;
using AdvantShop.Customers;

namespace AdvantShop.App.Landing.Filters
{
    public class AuthLpAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            var customer = CustomerContext.CurrentCustomer;

            if (customer != null && !customer.IsAdmin && !customer.IsVirtual)
            {
                HttpContext.Current.Server.TransferRequest("error/notfound");
            }
        }
    }
}
