using System.Web.Mvc;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Helpers;

namespace AdvantShop.Web.Infrastructure.Filters
{
    public class AdminAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }

            var customer = CustomerContext.CurrentCustomer;

            if (!customer.Enabled || !(customer.IsAdmin || customer.IsVirtual || customer.IsModerator || CustomerContext.IsDebug))
            {
                var controller = filterContext.RouteData.Values["controller"].ToString().ToLower();

                if (!controller.Equals("account"))
                {
                    var url = UrlService.GetAbsoluteLink("adminv2/login");

                    var request = filterContext.RequestContext.HttpContext.Request;
                    var referrer = request.Url != null ? request.Url.ToString() : "";

                    if (referrer.Contains(CommonHelper.GetParentDomain()) && !referrer.Contains("login") && !referrer.EndsWith("adminv2"))
                        url += "?from=" + filterContext.RequestContext.HttpContext.Request.RawUrl;

                    filterContext.Result = new RedirectResult(url);
                }
            }
        }
    }
}
