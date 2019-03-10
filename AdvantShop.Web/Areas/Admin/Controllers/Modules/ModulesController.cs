using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Customers;
using AdvantShop.Trial;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Modules;
using AdvantShop.Web.Admin.Models.Modules;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Extensions;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Web.Infrastructure.Admin.ModelBinders;
using System.Collections.Generic;

namespace AdvantShop.Web.Admin.Controllers.Modules
{
    [Auth(RoleAction.Modules)]
    public class ModulesController : BaseAdminController
    {
        public ActionResult Index(ModulesFilterModel filter = null)
        {
            if (!string.IsNullOrEmpty(filter.Name = Request["name"]))
            {
                var model = new ModulesHandler().GetLocalModules(filter);
                if (model.Count == 0)
                    return Redirect(Url.RouteUrl(new { controller = "Modules", action = "Market" }) + "?name=" + filter.Name);
                else if (model.Count == 1)
                    return Redirect(Url.RouteUrl(new { controller = "Modules", action = "Details" }) + "?id=" + model.First().StringId);
            }

            SetMetaInformation(T("Admin.Modules.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.ModulesCtrl);

            return View();
        }

        public JsonResult GetLocalModules(ModulesFilterModel filter)
        {
            var model = new ModulesHandler().GetLocalModules(filter);
            return Json(model);
        }

        public JsonResult GetMarketModules(ModulesFilterModel filter)
        {
            var model = new ModulesHandler().GetMarketModules(filter);
            return Json(model);
        }

        public ActionResult Market(ModulesFilterModel filter = null)
        {
            SetMetaInformation(T("Admin.Modules.Market.Title"));
            SetNgController(NgControllers.NgControllersTypes.ModulesCtrl);

            return View();
        }

        public ActionResult Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction("Index");
            
            var module = new ModulesHandler().GetModule(id);
            if (module == null)
                return RedirectToAction("Index");
            
            var model = new DetailsModel { Module = module };

            var m = Activator.CreateInstance(AttachedModules.GetModuleById(module.StringId));
            if (m != null)
            {
                var settings = m as IAdminModuleSettings;
                if (settings != null && settings.AdminSettings != null && settings.AdminSettings.Count > 0)
                {
                    model.Settings = settings.AdminSettings;
                }
            }

            SetMetaInformation(T("Admin.Modules.Details.Title", module.Name));
            SetNgController(NgControllers.NgControllersTypes.ModuleCtrl);

            return View(model);
        }

        #region Install, update, uninstall, enable module

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult InstallModule(string stringId, string id, string version)
        {
            if (string.IsNullOrWhiteSpace(stringId)|| 
                string.IsNullOrWhiteSpace(id)|| 
                string.IsNullOrWhiteSpace(version))
            {
                return Json(new {result = false});
            }
            
            var moduleInst = AttachedModules.GetModuleById(stringId);
            if (moduleInst != null)
            {
                ModulesService.InstallModule(stringId, version);

                return Json(new {result = true, url = Url.AbsoluteActionUrl("Details", "Modules", new {id = stringId})});
            }

            ModulesRepository.SetModuleNeedUpdate(stringId, true);

            var message = ModulesService.GetModuleArchiveFromRemoteServer(id);
            if (message.IsNullOrEmpty())
            {
                HttpRuntime.UnloadAppDomain();

                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();

                return Json(new
                {
                    result = true,
                    url = Url.AbsoluteActionUrl("InstallModuleInDB", "Modules", new { stringId = stringId, id = id, version = version })
                });
            }

            return Json(new {result = false});
        }

        public ActionResult InstallModuleInDB(string stringId, string id, string version)
        {
            var moduleInst = AttachedModules.GetModuleById(stringId);
            if (moduleInst != null)
            {
                ModulesService.InstallModule(stringId, version);
            }
            return RedirectToAction("Details", "Modules", new { id = stringId });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateModule(string stringId, string id, string version)
        {
            ModulesRepository.SetModuleNeedUpdate(stringId, true);

            var message = ModulesService.GetModuleArchiveFromRemoteServer(id);
            if (message.IsNullOrEmpty())
            {
                ModulesService.InstallModule(stringId.ToLower(), version);
                ModulesRepository.SetModuleNeedUpdate(stringId, false);
            }

            HttpRuntime.UnloadAppDomain();

            return Json(new {result = true});
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateAllModules(List<Module> modules)
        {
            foreach (var module in modules)
            {
                if(module.Version == module.CurrentVersion)
                {
                    continue;
                }

                ModulesRepository.SetModuleNeedUpdate(module.StringId, true);

                var message = ModulesService.GetModuleArchiveFromRemoteServer(module.Id.ToString());
                if (message.IsNullOrEmpty())
                {
                    ModulesService.InstallModule(module.StringId.ToLower(), module.Version);
                    ModulesRepository.SetModuleNeedUpdate(module.StringId, false);
                }
            }

            HttpRuntime.UnloadAppDomain();

            return Json(new { result = true });
        }
        

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeEnabled(string stringId, bool enabled)
        {
            if (string.IsNullOrWhiteSpace(stringId))
                return Json(new { result = false });

            ModulesRepository.SetActiveModule(stringId, enabled);

            TrialService.TrackEvent(enabled ? TrialEvents.ActivateModule : TrialEvents.DeactivateModule, stringId);

            if (stringId.ToLower() == "yametrika" && enabled)
                TrialService.TrackEvent(TrialEvents.SetUpYandexMentrika, string.Empty);

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UninstallModule(string stringId)
        {
            ModulesService.UninstallModule(stringId);
            return Json(new {result = true});
        }

        #endregion
    }
}
