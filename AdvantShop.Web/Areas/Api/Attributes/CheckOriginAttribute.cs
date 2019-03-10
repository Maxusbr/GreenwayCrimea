using System.Web.Mvc;

namespace AdvantShop.Areas.Api.Attributes
{
    public class CheckOriginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var origin = filterContext.HttpContext.Request.Headers["Origin"];

            if (origin != null && origin.Contains("chrome-extension://"))
            {
                filterContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", origin);
            }
        }
    }
}