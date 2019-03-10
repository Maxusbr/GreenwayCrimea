using System;
using System.Web.Mvc;

namespace AdvantShop.Web.Infrastructure.Filters.Headers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoCacheHttpHeadersAttribute : HttpHeaderAttributeBase
    {
        public override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            response.Headers["Expires"] = "-1";
            response.Headers["Pragma"] = "no-cache";
        }
    }
}
