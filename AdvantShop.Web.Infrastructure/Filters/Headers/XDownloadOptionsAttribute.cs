using System;
using System.Web.Mvc;

namespace AdvantShop.Web.Infrastructure.Filters.Headers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class XDownloadOptionsAttribute : HttpHeaderAttributeBase
    {
        public override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            response.Headers[HeaderConstants.XDownloadOptionsHeader] = "noopen";
        }
    }
}
