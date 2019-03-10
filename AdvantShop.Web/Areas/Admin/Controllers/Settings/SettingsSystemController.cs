using System;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Helpers;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Settings.System;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.ActionResults;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public partial class SettingsSystemController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.Settings.System.Title"));
            SetNgController(NgControllers.NgControllersTypes.SettingsSystemCtrl);

            var model = new GetSystemSettingsHandler().Execute();
            return View("index", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(SystemSettingsModel model)
        {
            if (ModelState.IsValid)
            {
                new SaveSystemSettingsHandler(model).Execute();
                ShowMessage(NotifyType.Success, T("Admin.Settings.SaveSuccess"));
            }
            else
            {
                ShowErrorMessages();
            }

            return Index();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult CheckLicense(string licKey)
        {
            return Json(new CommandResult { Result = SettingsLic.Activate(licKey) });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateSiteMaps()
        {
            return Json(new UpdateSiteMapsHandler().Execute());
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult FileStorageRecalc()
        {
            if (SettingsMain.CurrentFilesStorageLastUpdateTime < DateTime.Now.AddHours(-1) ||
                CustomerContext.CurrentCustomer.IsVirtual)
            {
                FilesStorageService.RecalcAttachmentsSizeInBackground();
            }

            return Json(new {result = true});
        }
    }
}
