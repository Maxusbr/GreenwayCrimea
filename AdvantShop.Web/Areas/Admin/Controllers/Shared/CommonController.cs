using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using AdvantShop.SEO;
using AdvantShop.Web.Admin.Handlers.Common;
using AdvantShop.Web.Admin.Models.Menus;
using AdvantShop.Web.Admin.ViewModels.Common;
using AdvantShop.Web.Infrastructure.Filters;
using Newtonsoft.Json;
using AdvantShop.Trial;

namespace AdvantShop.Web.Admin.Controllers.Shared
{
    public partial class CommonController : BaseAdminController
    {
        [ChildActionOnly]
        public ActionResult NavMenu()
        {
            var area = Request.RequestContext.RouteData.DataTokens["area"] as string;
            if (area == null || !area.ToLower().Equals("adminv2"))
            {                           
                return new EmptyResult();
            }

            var menuItems = new GetAdminMenuObject().Execute();
            var selectedRootNode = menuItems.FirstOrDefault(x => x.Selected);
            if (selectedRootNode != null && selectedRootNode.MenuItems != null)
            {
                return PartialView(selectedRootNode.MenuItems);
            }

            return PartialView(new List<AdminMenuModel>());
        }

        [ChildActionOnly]
        public ActionResult LeftMenu()
        {
            var currentCustomer = CustomerContext.CurrentCustomer;

            var model = new LeftMenuViewModel()
            {
                MenuItems = new GetAdminMenuObject().Execute(),
                AvatarSrc = !string.IsNullOrEmpty(CustomerContext.CurrentCustomer.Avatar)
                    ? FoldersHelper.GetPath(FolderType.Avatar, CustomerContext.CurrentCustomer.Avatar, true)
                    : UrlService.GetAdminStaticUrl() + "images/no-photo.png",
                NoAvatarSrc = UrlService.GetAdminStaticUrl() + "images/no-photo.png",
                CustomerId = currentCustomer.Id
            };

            if (!currentCustomer.IsAdmin)
            {
                var avalableRoles = RoleActionService.GetCustomerRoleActionsByCustomerId(CustomerContext.CurrentCustomer.Id);

                model.DisplayCatalog = avalableRoles.Any(item => item.Role == RoleAction.Catalog);
                model.DisplayCustomers = avalableRoles.Any(item => item.Role == RoleAction.Customers);                
                model.DisplayOrders = avalableRoles.Any(item => item.Role == RoleAction.Orders);
                model.DisplayCrm = avalableRoles.Any(item => item.Role == RoleAction.Crm);
                model.DisplayCms = avalableRoles.Any(item => item.Role == RoleAction.Cms);
            }

            return PartialView(model);
        }


        [ChildActionOnly]
        public ActionResult SaasInformation()
        {
            if (SaasDataService.IsSaasEnabled && (CustomerContext.CurrentCustomer.IsAdmin || CustomerContext.CurrentCustomer.IsVirtual))
            {
                return PartialView(SaasDataService.CurrentSaasData);
            }

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult UpdateSaasInformation()
        {
            SaasDataService.GetSaasData(true);
            return Redirect(Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : UrlService.GetAdminUrl());
        }

        #region Avatar

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadAvatar(HttpPostedFileBase file)
        {
            var customer = CustomerContext.CurrentCustomer;
            if (customer == null)
                return Json(false);

            var result = new UploadAvatar(customer, file).Execute();

            return result != null
                ? Json(new { result = true, file = result })
                : Json(new { result = false, error = T("Admin.Error.InvalidImageFormat") });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadAvatarCropped(string name, string base64String)
        {
            var customer = CustomerContext.CurrentCustomer;
            if (customer == null)
                return Json(false);

            var result = new UploadAvatarCropped(customer, name, base64String).Execute();

            return result != null
                ? Json(new { result = true, file = result })
                : Json(new { result = false, error = T("Admin.Error.InvalidImageFormat") });
        }


        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadAvatarByUrl(string url)
        {
            var customer = CustomerContext.CurrentCustomer;
            if (customer == null)
                return Json(false);

            // Сохраняем картинку во временную папку из-за того что js не может cors
            var handler = new UploadAvatarByUrl(customer, url);
            var result = handler.Execute();

            return result != null
                ? Json(new { result = true, file = result })
                : Json(new { result = false, error = T("Admin.Error.InvalidImageFormat") });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteAvatar(Guid? customerId)
        {
            var customer = customerId.HasValue ? CustomerService.GetCustomer(customerId.Value) : CustomerContext.CurrentCustomer;
            if (customer == null)
                return Json(false);

            var handler = new DeleteAvatar(customer);
            var result = handler.Execute();

            return result != null
                ? Json(new { result = true, file = result })
                : Json(new { result = false, error = T("Admin.Error.InvalidImageFormat") });
        }

        [HttpPost]
        public JsonResult GetAvatar(Guid? customerId)
        {
            var customer = customerId.HasValue ? CustomerService.GetCustomer(customerId.Value) : CustomerContext.CurrentCustomer;
            if (customer == null || customer.Avatar.IsNullOrEmpty())
                return Json(null);

            return Json(FoldersHelper.GetPath(FolderType.Avatar, customer.Avatar, false));
        }

        #endregion

        #region Images

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadImageByUrl(string url)
        {
            var result = new UploadImageByUrl(url).Execute();
            return result != null
                ? Json(new { result = true, file = result })
                : Json(new { result = false, error = T("Admin.Error.InvalidImageFormat") });
        }

        #endregion

        [HttpGet]
        public JsonResult GetLastStatistics()
        {
            return Json(new GetLastStastics().Execute());
        }

        public JsonResult GenerateUrl(string name)
        {
            return Json(StringHelper.TransformUrl(StringHelper.Translit(name)));
        }

        [ChildActionOnly]
        public ActionResult MetaVariablesDescription(MetaType type, bool showInstruction = true)
        {
            var model = new MetaVariablesDescriptionModel
            {
                MetaVariables = MetaInfoService.GetMetaVariables(type),
                ShowInstruction = showInstruction
            };
            return PartialView(model);
        }

        [ChildActionOnly]
        public MvcHtmlString MetaVariablesСomplete(MetaType type)
        {
            return new MvcHtmlString(JsonConvert.SerializeObject(MetaInfoService.GetMetaVariables(type).Select(x => x.Value)));
        }


        public ActionResult Oldversion()
        {
            CommonHelper.SetCookie("oldadmin", "true", new TimeSpan(365, 0, 0, 0, 0), false);
            return Redirect("~/admin");
        }

        public ActionResult PictureUploader(PictureUploader model)
        {
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Counter()
        {
            if (!TrialService.IsTrialEnabled)
                return new EmptyResult();

            return PartialView((object)TrialService.TrialCounter);
        }

        [ChildActionOnly]
        public MvcHtmlString FilesHelpText(EAdvantShopFileTypes type, string maxFileSize)
        {
            var model = FileHelpers.GetFilesHelpText(type, maxFileSize);
            return new MvcHtmlString(model);
        }
    }
}
