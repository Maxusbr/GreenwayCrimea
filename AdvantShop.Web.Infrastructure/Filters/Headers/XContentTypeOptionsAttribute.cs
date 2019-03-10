using System;
using System.Web.Mvc;

namespace AdvantShop.Web.Infrastructure.Filters.Headers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class XContentTypeOptionsAttribute : HttpHeaderAttributeBase
    {
        public override void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            response.Headers[HeaderConstants.XContentTypeOptionsHeader] = "nosniff";
        }
    }
}
