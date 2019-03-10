using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Web.Infrastructure.ActionResults;

namespace AdvantShop.Web.Infrastructure.Filters
{
    public class ValidateAjaxAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var modelState = filterContext.Controller.ViewData.ModelState;
            if (modelState.IsValid) return;
         
            var errors = new List<string>();
            foreach (var state in modelState.Values)
            {
                foreach (var error in state.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            filterContext.Result = new JsonNetResult()
            {
                Data = new CommandResult
                {
                    Errors = errors
                }
            };
            //filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }

        // evo: right way error
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    if (!filterContext.HttpContext.Request.IsAjaxRequest())
        //        return;

        //    var modelState = filterContext.Controller.ViewData.ModelState;
        //    if (modelState.IsValid) return;
        //    var errorModel =
        //        from x in modelState.Keys
        //        where modelState[x].Errors.Count > 0
        //        select new
        //        {
        //            key = x,
        //            errors = modelState[x].Errors.Select(y => y.ErrorMessage).ToArray()
        //        };
        //    filterContext.Result = new JsonResult()
        //    {
        //        Data = errorModel
        //    };
        //    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //}
    }
}
