using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Settings.ClearDatas;
using AdvantShop.Web.Admin.Handlers.Settings.Common;
using AdvantShop.Web.Admin.Handlers.Settings.Mobiles;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Admin.ViewModels.Settings;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Saas;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public partial class SettingsController : BaseAdminController
    {
        #region Common settings

        public ActionResult Index()
        {
            var model = new GetCommonSettings().Execute();

            SetMetaInformation(T("Admin.Settings.Index.SettingsTitle"));
            SetNgController(NgControllers.NgControllersTypes.SettingsCtrl);

            return View("Common/Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CommonSettingsModel model)
        {
            if (ModelState.IsValid)
            {
                var handler = new SaveCommonSettings(model);
                handler.Execute();

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

            return RedirectToAction("Index");
        }

        [HttpPost]        
        public JsonResult GetRegions(int countryId)
        {
            if (ModelState.IsValid)
            {
                var handler = new GetRegions(countryId);
                return JsonOk( handler.Execute());                
            }
            else
            {
                foreach (var modelState in ViewData.ModelState.Values)
                    foreach (var error in modelState.Errors)
                    {
                        ShowMessage(NotifyType.Error, error.ErrorMessage);
                    }
                return JsonError();
            }            
        }


        #region Logo
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadLogo()
        {
            var uploadLogoPictureHandler = new UploadLogoPictureHandler();
            var result = uploadLogoPictureHandler.Execute();
            return JsonOk(new
            {
                picture = result.Picture,
                pictureId = result.PictureId
            });
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult UploadLogoByLink(string fileLink)
        {
            var uploadLogoPictureByLinkHandler = new UploadLogoPictureByLinkHandler(fileLink);
            var result = uploadLogoPictureByLinkHandler.Execute();
            return JsonOk(new {
                picture = result.Picture,
                pictureId = result.PictureId
            });
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult DeleteLogo()
        {
            FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, SettingsMain.LogoImageName));
            SettingsMain.LogoImageName = string.Empty;
            return JsonOk(new { picture = "../images/nophoto_small.jpg" });
        }
        #endregion

        #region Favicon
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadFavicon()
        {
            var uploadFaviconPictureHandler = new UploadFaviconPictureHandler();
            var result = uploadFaviconPictureHandler.Execute();
            return JsonOk(new
            {
                picture = result.Picture,
                pictureId = result.PictureId
            });
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult UploadFaviconByLink(string fileLink)
        {
            var uploadFaviconPictureByLinkHandler = new UploadFaviconPictureByLinkHandler(fileLink);
            var result = uploadFaviconPictureByLinkHandler.Execute();
            return JsonOk(new
            {
                picture = result.Picture,
                pictureId = result.PictureId
            });
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult DeleteFavicon()
        {
            FileHelpers.DeleteFile(FoldersHelper.GetPathAbsolut(FolderType.Pictures, SettingsMain.FaviconImageName));
            SettingsMain.FaviconImageName = string.Empty;
            return JsonOk(new { picture = "../images/nophoto_small.jpg" });
        }

        #endregion

        #endregion
        
        #region PaymentMethods

        public ActionResult PaymentMethods()
        {
            SetMetaInformation(T("Admin.Settings.PaymentMethods.Title"));
            SetNgController(NgControllers.NgControllersTypes.SettingsCtrl);

            return View("Payments/PaymentMethods", new PaymentMethodsSettingsModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentMethods(PaymentMethodsSettingsModel model)
        {
            return PaymentMethods();
        }

        #endregion

        #region ShippingMethods

        public ActionResult ShippingMethods()
        {
            SetMetaInformation(T("Admin.Settings.ShippingMethods.Title"));
            SetNgController(NgControllers.NgControllersTypes.SettingsCtrl);

            return View("Shippings/ShippingMethods", new ShippingMethodsSettingsModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShippingMethods(ShippingMethodsSettingsModel model)
        {
            return ShippingMethods();
        }

        #endregion

        #region MobileVersion

        [SaasFeature(Saas.ESaasProperty.MobileVersion)]
        public ActionResult MobileVersion()
        {
            var model = new LoadSaveMobileSettingsHandler(null).Load();

            SetMetaInformation(T("Admin.Settings.MobileVersion.Title"));
            SetNgController(NgControllers.NgControllersTypes.SettingsCtrl);

            return View("Mobile/MobileVersion", model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SaasFeature(Saas.ESaasProperty.MobileVersion)]
        public ActionResult MobileVersion(MobileVersionSettingsModel model)
        {
            var saveMobile = new LoadSaveMobileSettingsHandler(model);
            saveMobile.Save();

            ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
            return RedirectToAction("MobileVersion");
        }

        #endregion
        
        #region Users

        public ActionResult UsersSettings()
        {
            SetMetaInformation(T("Admin.Settings.Users.Title"));
            SetNgController(NgControllers.NgControllersTypes.SettingsUsersCtrl);

            var saasData = SaasDataService.CurrentSaasData;
            var employeesCount = EmployeeService.GetEmployeeCount();
            var employeesLimit = SaasDataService.IsSaasEnabled
                ? saasData.EmployeesCount
                : int.MaxValue;

            var model = new UsersSettingsViewModel
            {
                UsersViewModel = new UsersViewModel
                {
                    ManagersCount = ManagerService.GetManagersCount(),
                    ManagersLimitation = SaasDataService.IsSaasEnabled,
                    ManagersLimit = SaasDataService.IsSaasEnabled ? saasData.EmployeesCount : 0,

                    EmployeesCount = employeesCount,
                    EmployeesLimit = employeesLimit,
                    EnableEmployees = !SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && employeesCount < employeesLimit),

                    EnableManagersModule = SettingsCheckout.EnableManagersModule,
                    ShowManagersPage = SettingsCheckout.ShowManagersPage
                },

                ManagersOrderConstraint = SettingsManager.ManagersOrderConstraint,
                ManagersLeadConstraint = SettingsManager.ManagersLeadConstraint
            };

            return View("Users/UsersSettings", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsersSettings(UsersSettingsViewModel model)
        {
            SettingsManager.ManagersOrderConstraint = model.ManagersOrderConstraint;
            SettingsManager.ManagersLeadConstraint = model.ManagersLeadConstraint;

            ShowMessage(NotifyType.Success, T("Admin.Settings.SaveSuccess"));

            return UsersSettings();
        }

        #endregion

        #region Files

        public ActionResult Files()
        {
            SetMetaInformation(T("Admin.Settings.System.Title"));
            SetNgController(NgControllers.NgControllersTypes.SettingsCtrl);

            return View("Files/Files", new FilesSettingsModel());
        }

        #endregion
        
        #region ClearTrialData

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ClearTrialData()
        {
            var handler = new ClearTrialDataHandler();
            handler.Execute();

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ClearData(ClearDataViewModel model)
        {

            var cmd = new ClearDataSettingsHandler(model);
            cmd.Execute();

            return Json(new { result = true });
        }

        #endregion
    }
}
