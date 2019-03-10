using System.Web.Mvc;

namespace AdvantShop.Web.Infrastructure.Filters.Headers
{
    public abstract class HttpHeaderAttributeBase : ActionFilterAttribute
    {
        private static readonly object Marker = new object();
        private string _contextKey = "setHeader";
        private string Key
        {
            get { return _contextKey + "_" + GetType().Name; }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var context = filterContext.HttpContext;
            if (context.Items.Contains(Key))
            {
                base.OnActionExecuted(filterContext);
                return;
            }

            context.Items[Key] = Marker;
            SetHttpHeadersOnActionExecuted(filterContext);
            base.OnActionExecuted(filterContext);
        }

        public abstract void SetHttpHeadersOnActionExecuted(ActionExecutedContext filterContext);
    }
}
