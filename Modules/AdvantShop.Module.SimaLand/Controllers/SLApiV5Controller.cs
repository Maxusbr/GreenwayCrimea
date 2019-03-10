using AdvantShop.Module.SimaLand.Models;
using AdvantShop.Web.Infrastructure.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.IO;
using AdvantShop.Module.SimaLand.Service;
using AdvantShop.Module.SimaLand.ViewModel;
using AdvantShop.Core.Scheduler;
using AdvantShop.Security;
using System.Web;

namespace AdvantShop.Module.SimaLand.Controllers
{
    public class SLApiV5Controller : ModuleController
    {

        bool allow = Secure.VerifyAccess();

        [HttpPost]
        public ActionResult SlLogin(SlLoginModel slUser)
        {
            VerifyAccess();
            return Json(ApiService.SignIn(slUser));
        }

        public ActionResult GetMarkups()
        {
            VerifyAccess();

            var model = new RangeViewModel {
                Added = false,
                ranges = PSLModuleSettings.PriceRange
            };

            return PartialView("~/Modules/SimaLand/Views/slapiv5/ranges.cshtml", model);
        }

        [HttpGet]
        public ActionResult AddMarkup()
        {
            VerifyAccess();
            var model = new RangeViewModel
            {
                Added = true,
                ranges = PSLModuleSettings.PriceRange
            };

            return PartialView("~/Modules/SimaLand/Views/slapiv5/ranges.cshtml", model);
        }

        [HttpPost]
        public ActionResult AddMarkup(MarkupPriceRange range)
        {
            VerifyAccess();
            MarkupService.AddMarkup(range);
            var model = new RangeViewModel
            {
                Added = false,
                ranges = PSLModuleSettings.PriceRange
            };

            return PartialView("~/Modules/SimaLand/Views/slapiv5/ranges.cshtml", model);
        }

        public ActionResult EditMarkup(int id)
        {
            VerifyAccess();
            var model = new RangeViewModel
            {
                Added = false,
                Edited = id,
                ranges = PSLModuleSettings.PriceRange
            };

            return PartialView("~/Modules/SimaLand/Views/slapiv5/ranges.cshtml", model);
        }

        [HttpPost]
        public ActionResult UpdateMarkup(MarkupPriceRange range)
        {
            VerifyAccess();
            var ranges = PSLModuleSettings.PriceRange;

            ranges.Remove(ranges.First(x => x.Id == range.Id));
            ranges.Add(range);

            PSLModuleSettings.PriceRange = ranges;

            var model = new RangeViewModel
            {
                Added = false,
                ranges = PSLModuleSettings.PriceRange
            };

            return PartialView("~/Modules/SimaLand/Views/slapiv5/ranges.cshtml", model);
        }

        public ActionResult DeleteMarkup(int Id)
        {
            VerifyAccess();
            var ranges = PSLModuleSettings.PriceRange;

            ranges.Remove(ranges.First(x => x.Id == Id));

            PSLModuleSettings.PriceRange = ranges;

            var model = new RangeViewModel
            {
                Added = false,
                ranges = PSLModuleSettings.PriceRange
            };

            return PartialView("~/Modules/SimaLand/Views/slapiv5/ranges.cshtml", model);
        }

        public ActionResult UpdateBalance()
        {
            VerifyAccess();
            if (SimalandImportStatistic.Process == false)
                Task.Factory.StartNew(() => UpdateBalanceService.UpdateBalancePriceJob());
            return Content("started");
        }

        public ActionResult GetValidPriceAndBalance()
        {
            VerifyAccess();
            return Json(new {toUpdate = "Доступно для обновления: " + SimalandProductService.GetTotalCountSlProducts(), process = SimalandImportStatistic.Process });
        }

        public ActionResult SavePriceBalanceSettings(bool au, int h)
        {

            VerifyAccess();
            PSLModuleSettings.AutoUpdateBalance = au;
            PSLModuleSettings.TimePeriodBalance = h;

            if (!au)
            {
                if (SimaLand.GetTasks().Count > 0)
                    TaskManager.TaskManagerInstance().RemoveModuleTask(SimaLand.GetTasks().First(x => x.Enabled == false));
            }
            TaskManager.TaskManagerInstance().ManagedTask(TaskSettings.Settings);
            return Json(new { error = false, message = "Сохранено"});
        }

        public ActionResult LoadPriceBalanceSettings()
        {
            VerifyAccess();
            return Json(new { au = PSLModuleSettings.AutoUpdateBalance, h = PSLModuleSettings.TimePeriodBalance }, JsonRequestBehavior.AllowGet);
        }

        private void VerifyAccess()
        {
            if (!allow)
            {
                throw new HttpException(403, "Доступ запрещён!");
            }
        }

    }
}
