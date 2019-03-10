using System;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Settings.ApiSettings;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Controllers;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public partial class SettingsApiController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.Settings.System.Api"));
            SetNgController(NgControllers.NgControllersTypes.SettingsCtrl);

            var model = new LoadSaveApiSettingsHandler(null).Load();

            return View("Index", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(APISettingsModel model)
        {
            new LoadSaveApiSettingsHandler(model).Save();

            ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult Generate()
        {
            var key = Guid.NewGuid().ToString().Sha256();
            return Json(key);
        }
    }
}
