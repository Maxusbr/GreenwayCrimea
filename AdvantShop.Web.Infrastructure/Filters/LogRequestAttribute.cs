using System.Web.Mvc;
using AdvantShop.Diagnostics;
using AdvantShop.Web.Infrastructure.Extensions;

namespace AdvantShop.Web.Infrastructure.Filters
{
    public class LogRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction)
                return;

            if (filterContext.HttpContext != null &&
                filterContext.HttpContext.Request != null)
            {
                Debug.Log.Info(filterContext.HttpContext.Request.GetRequestRawData());
            }
        }
    }
}
