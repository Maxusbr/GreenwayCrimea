using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.SQL;

namespace AdvantShop.Web.Infrastructure.Filters
{
    public class ProfilingActionFilter : ActionFilterAttribute
    {
        private Stopwatch _stopWatch = null;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _stopWatch.Stop();

            var controller = filterContext.Controller.ToString().Split('.').Last() + ".";
            var action = filterContext.ActionDescriptor.ActionName;

            var profiler = HttpContext.Current.Items["MiniProfiler_Actions"] as List<Profiling> ?? new List<Profiling>();
            profiler.Add(new Profiling(controller + action, _stopWatch.Elapsed.TotalMilliseconds));

            HttpContext.Current.Items["MiniProfiler_Actions"] = profiler;

            _stopWatch.Reset();
        }

        //public override void OnResultExecuted(ResultExecutedContext filterContext)
        //{
        //    _stopWatch.Stop();

        //    var controller = filterContext.Controller.ToString().Split('.').Last() + ".";
        //    var action = filterContext.RouteData.Values["action"];// ActionDescriptor.ActionName;

        //    var profiler = HttpContext.Current.Items["MiniProfiler_Actions"] as List<Profiling> ?? new List<Profiling>();
        //    profiler.Add(new Profiling(controller + action, _stopWatch.Elapsed.TotalMilliseconds));

        //    HttpContext.Current.Items["MiniProfiler_Actions"] = profiler;

        //    _stopWatch.Reset();
        //}
    }
}
