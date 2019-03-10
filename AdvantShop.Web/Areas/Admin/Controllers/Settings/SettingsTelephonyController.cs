using System.Web.Mvc;
using AdvantShop.Core.Controls;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Settings.TelephonySettings;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Saas;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Core;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    [SaasFeature(ESaasProperty.HaveTelephony)]
    public partial class SettingsTelephonyController : BaseAdminController
    {
        public ActionResult Index()
        {
            var model = new GetIPTelephonySettings().Execute();

            SetMetaInformation(T("Admin.Settings.Telephony.Title"));
            SetNgController(NgControllers.NgControllersTypes.SettingsTelephonyCtrl);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IPTelephonySettingsModel model)
        {
            if (ModelState.IsValid)
            {
                var handler = new SaveIPTelephonySettings(model);
                try
                {
                    handler.Execute();
                    ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
                }
                catch (BlException e)
                {
                    ModelState.AddModelError(e.Property, e.Message);
                }
            }
            ShowErrorMessages();

            return Index();
        }

        #region Telphin

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult GetTelphinExtensions()
        {
            var extensions = new TelphinHandler().GetExtensions();
            return JsonOk(extensions);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddTelphinEvents(string extensionId)
        {
            try
            {
                var extensions = new TelphinHandler().AddEvents(extensionId);
                return JsonOk(extensions);
            }
            catch (BlException e)
            {
                return JsonError(e.Message);
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteTelphinEvents(string extensionId)
        {
            try
            {
                var extensions = new TelphinHandler().DeleteEvents(extensionId);
                return JsonOk(extensions);
            }
            catch (BlException e)
            {
                return JsonError(e.Message);
            }
        }

        #endregion
    }
}
