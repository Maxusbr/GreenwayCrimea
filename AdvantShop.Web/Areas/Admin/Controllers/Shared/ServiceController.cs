using System;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Webhook;
using AdvantShop.Customers;
using AdvantShop.Saas;
using AdvantShop.Trial;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Shared
{
    public class ServiceController : BaseAdminController
    {
        string executionTime;

        public ServiceController()
        {
            executionTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
            ViewBag.ExecutionTime = executionTime;
            ViewBag.Hash = (SettingsLic.LicKey + executionTime + SettingsLic.ClientCode).Md5();
        }

        public ActionResult Tariffs()
        {
            SetMetaInformation(T("Admin.Service.Tariffs"));
            SetNgController(NgControllers.NgControllersTypes.TariffsCtrl);
            return View();
        }

        public ActionResult ChangeTariff()
        {
            SetMetaInformation(T("Admin.Service.ChangeTariff"));
            SetNgController(NgControllers.NgControllersTypes.TariffsCtrl);
            return View();
        }

        public ActionResult Domains()
        {
            SetMetaInformation(T("Admin.Service.Domain"));
            SetNgController(NgControllers.NgControllersTypes.TariffsCtrl);
            return View();
        }

        public ActionResult SupportCenter()
        {
            SetMetaInformation(T("Admin.Service.SupportCenter"));
            SetNgController(NgControllers.NgControllersTypes.TariffsCtrl);
            return View();
        }

        public ActionResult Actions(string id)
        {
            ViewBag.ActionId = id;
            SetMetaInformation(T("Admin.Service.Tariffs"));
            SetNgController(NgControllers.NgControllersTypes.TariffsCtrl);
            return View();
        }

        public ActionResult GetFeature(string id)
        {
            return View("~/areas/admin/views/service/GetFeature.cshtml", (object)id);
        }

        public ActionResult BuyTemplate(string id)
        {
            SetMetaInformation(T("Admin.Service.BuyTemplate"));
            SetNgController(NgControllers.NgControllersTypes.TariffsCtrl);
            return View((object)id);
        }

        public ActionResult RoleAccessIsDenied(RoleAction roleActionKey)
        {
            return View("~/areas/admin/views/service/RoleAccessIsDenied.cshtml", (object)roleActionKey.Localize());
        }

        public JsonResult RoleAccessIsDeniedJson(RoleAction roleActionKey)
        {
            return JsonError("access denied");
        }

        public ActionResult UnderConstruction()
        {
            return View("~/areas/admin/views/service/UnderConstruction.cshtml");
        }

        public ActionResult Academy(int? id)
        {
            SetMetaInformation(T("Admin.Service.Academy"));
            ViewBag.LessonId = id;
            SetNgController(NgControllers.NgControllersTypes.TariffsCtrl);
            TrialService.TrackEvent(ETrackEvent.Trial_VisitAcademy);
            return View();
        }

        public ActionResult ResetSaasFromAdmin()
        {
            SaasDataService.GetSaasData(true);
            return Redirect(Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : "~/adminv2/");
        }


        [HttpPost, WebhookAuth(EWebhookType.None), ExcludeFilter(typeof(AdminAuthAttribute))]
        public JsonResult Reset(string apikey)
        {
            SaasDataService.GetSaasData(true);
            return Json(true);
        }
    }
}
