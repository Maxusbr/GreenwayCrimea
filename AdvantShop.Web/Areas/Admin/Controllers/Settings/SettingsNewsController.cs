using System.Web.Mvc;
using AdvantShop.Core.Controls;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Settings.News;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Controllers;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public partial class SettingsNewsController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.Settings.News.Title"));
            SetNgController(NgControllers.NgControllersTypes.SettingsNewsCtrl);

            var model = new GetNewsSettingsHandler().Execute();

            return View("index", model);
        }

        [HttpPost]
        public ActionResult Index(NewsSettingsModel model)
        {
            if (ModelState.IsValid)
            {
                new SaveNewsSettingsHandler(model).Execute();
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
