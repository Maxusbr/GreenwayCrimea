using System.Web.Mvc;
using AdvantShop.Core.Controls;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Settings.Socials;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Controllers;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public partial class SettingsSocialController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.Settings.Social.Title"));
            SetNgController(NgControllers.NgControllersTypes.SettingsSocialCtrl);

            var model = new GetSocialSettingsHandler().Execute();
            return View("index", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(SocialSettingsModel model)
        {
            if (ModelState.IsValid)
            {
                new SaveSocialSettingsHandler(model).Execute();
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
