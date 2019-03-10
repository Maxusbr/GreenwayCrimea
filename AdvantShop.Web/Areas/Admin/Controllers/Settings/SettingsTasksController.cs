using System.Web.Mvc;
using AdvantShop.Core.Controls;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Settings.Tasks;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Controllers;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings, RoleAction.Tasks)]
    public class SettingsTasksController : BaseAdminController
    {
        public ActionResult Index()
        {
            var model = new LoadSaveTasksSettings().Load();

            SetMetaInformation(T("Admin.Settings.Tasks.Title"));
            SetNgController(NgControllers.NgControllersTypes.SettingsTasksCtrl);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(TasksSettingsModel model)
        {
            if (ModelState.IsValid)
            {
                new LoadSaveTasksSettings(model).Save();
                ShowMessage(NotifyType.Success, T("Admin.Settings.SaveSuccess"));
            }
            else
            {
                foreach (var modelState in ViewData.ModelState.Values)
                    foreach (var error in modelState.Errors)
                    {
                        ShowMessage(NotifyType.Error, error.ErrorMessage);
                    }
            }

            return Index();
        }
    }
}
