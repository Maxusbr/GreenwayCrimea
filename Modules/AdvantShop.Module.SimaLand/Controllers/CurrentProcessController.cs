using AdvantShop.Module.SimaLand.Service;
using AdvantShop.Web.Infrastructure.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdvantShop.Module.SimaLand.Controllers
{
    public class CurrentProcessController : ModuleController
    {
        public ActionResult Index()
        {
            var process = SimalandImportStatistic.Process;
            var currentProcess = SimalandImportStatistic.CurrentProcess;
            var totalCount = SimalandImportStatistic.Type == SimalandImportStatistic.ProcessType.ParseCategories ? SimalandImportStatistic.PrePareCategoriesInSimaLand : SimalandImportStatistic.PrePareTotalSlProductInShop;
            var totalHandled = SimalandImportStatistic.Type == SimalandImportStatistic.ProcessType.ParseCategories ? SimalandImportStatistic.TotalCountCategoriesProcess : SimalandImportStatistic.TotalAddProducts + SimalandImportStatistic.TotalUpdateProducts;
            var timeSpent = "";
            if (SimalandImportStatistic.Timer != null)
            {
               timeSpent = String.Format("{0:00}:{1:00}:{2:00}",
                SimalandImportStatistic.Timer.Elapsed.Hours, SimalandImportStatistic.Timer.Elapsed.Minutes, SimalandImportStatistic.Timer.Elapsed.Seconds);
            }
            return Json(new { process, currentProcess, totalCount, totalHandled, timeSpent });
        }
    }
}
