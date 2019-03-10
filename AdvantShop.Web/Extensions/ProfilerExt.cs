using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.SQL;

namespace AdvantShop.Extensions
{
    public static class ProfilerExt
    {
        [Conditional("DEBUG")]
        public static void ToTempData(this Controller mController)
        {
            var profilerC = HttpContext.Current.Items["MiniProfiler_Actions"] as List<Profiling> ?? new List<Profiling>();
            var profilerT = mController.TempData["MiniProfiler_Actions"] as List<Profiling> ?? new List<Profiling>();
            profilerT.AddRange(profilerC);
            mController.TempData["MiniProfiler_Actions"] = profilerT;

            profilerC = HttpContext.Current.Items["MiniProfiler_Sql"] as List<Profiling> ?? new List<Profiling>();
            profilerT = mController.TempData["MiniProfiler_Sql"] as List<Profiling> ?? new List<Profiling>();
            profilerT.AddRange(profilerC);
            mController.TempData["MiniProfiler_Sql"] = profilerT;
            
        }

        [Conditional("DEBUG")]
        public static void ToContext(this Controller mController)
        {
            var profilerT = mController.TempData["MiniProfiler_Actions"] as List<Profiling> ?? new List<Profiling>();
            var profilerC = HttpContext.Current.Items["MiniProfiler_Actions"] as List<Profiling> ?? new List<Profiling>();
            profilerC.AddRange(profilerT);
            HttpContext.Current.Items["MiniProfiler_Actions"] = profilerC;

            profilerT = mController.TempData["MiniProfiler_Sql"] as List<Profiling> ?? new List<Profiling>();
            profilerC = HttpContext.Current.Items["MiniProfiler_Sql"] as List<Profiling> ?? new List<Profiling>();
            profilerC.AddRange(profilerT);
            HttpContext.Current.Items["MiniProfiler_Sql"] = profilerC;
        }
    }
}