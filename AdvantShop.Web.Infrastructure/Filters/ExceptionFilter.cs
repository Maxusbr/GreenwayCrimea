using System;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Diagnostics;

namespace AdvantShop.Web.Infrastructure.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }
            

            Debug.Log.Error("ExceptionFilter", filterContext.Exception);
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = new HttpException(null, filterContext.Exception).GetHttpCode();
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

            //exceptionContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            //exceptionContext.HttpContext.Response.Clear();
            //exceptionContext.HttpContext.Server.ClearError();
        }
    }

    public class UnhandleExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            //this.OnException(context);
            //base(context);
            context.ExceptionHandled = true;
            Debug.Log.Error("UnhandleExceptionAttribute", context.Exception);
        }
    }
}
