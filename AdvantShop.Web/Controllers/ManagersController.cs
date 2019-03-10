using System.Collections.Generic;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Mails;
using AdvantShop.Saas;
using AdvantShop.SEO;
using AdvantShop.Web.Infrastructure.Controllers;
using System.Web.Mvc;
using AdvantShop.CMS;
using AdvantShop.Customers;
using AdvantShop.ViewModel.Managers;
using AdvantShop.Web.Infrastructure.Extensions;

namespace AdvantShop.Controllers
{
    public partial class ManagersController : BaseClientController
    {
        public ActionResult Index(int? departmentId)
        {
            if (!SettingsCheckout.ShowManagersPage 
                && (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.EmployeesCount == 0)))
            {
                return Error404();
            }

            Department department = departmentId != null ? DepartmentService.GetDepartment((int)departmentId) : null;

            var model = new ManagersViewModel
            {
                Managers = ManagerService.GetManagersList(),
                Departments = new DepartmentsListViewModel
                {
                    Departments = DepartmentService.GetDepartmentsList(true),
                    Selected = departmentId ?? -1
                }
            };

            if (department != null)
            {
                model.BreadCrumbs = new List<BreadCrumbs>()
                {
                    new BreadCrumbs(T("MainPage"), Url.AbsoluteRouteUrl("Home")),
                    new BreadCrumbs(T("Managers.Index.MangersTitle"), Url.AbsoluteRouteUrl("Managers")),
                    new BreadCrumbs(department.Name, Url.AbsoluteRouteUrl("Managers", new { departmentId}))
                };
            }
            else
            {
                model.BreadCrumbs = new List<BreadCrumbs>()
                {
                    new BreadCrumbs(T("MainPage"), Url.AbsoluteRouteUrl("Home")),
                    new BreadCrumbs(T("Managers.Index.MangersTitle"), Url.AbsoluteRouteUrl("Managers"))
                };
            }

            SetNgController(NgControllers.NgControllersTypes.ManagersCtrl);

            var metaString = T("Managers.Index.MangersTitle") + (department != null ? " - " + department.Name : string.Empty);

            SetMetaInformation(new MetaInfo
            {
                Title = metaString,
                MetaKeywords = metaString,
                MetaDescription = metaString,
                H1 = metaString
            });

            return View(model);
        }

        [HttpPost]
        public JsonResult GetModalParams()
        {
            return Json(new
            {
                SettingsCheckout.IsShowUserAgreementText,
                SettingsCheckout.UserAgreementText
            });
        }

        [HttpPost]
        public JsonResult RequestCall(string clientName, string clientPhone, string comment, int managerId)
        {
            var manager = ManagerService.GetManager(managerId);
            if (manager == null || string.IsNullOrWhiteSpace(clientName) || string.IsNullOrWhiteSpace(clientPhone))
                return Json(false);

            var mailTemplate = new FeedbackMailTemplate(SettingsMain.SiteUrl, SettingsMain.ShopName,
                                                            HttpUtility.HtmlEncode(clientName),
                                                            "",
                                                            HttpUtility.HtmlEncode(clientPhone),
                                                            T("Managers.RequestCall.RequestCallSubject"),
                                                            T("Managers.RequestCall.Manager") + ": " + HttpUtility.HtmlEncode(manager.FirstName + " " + manager.LastName), 
                                                            string.Empty);

            mailTemplate.BuildMail();

            if (!string.IsNullOrWhiteSpace(manager.Email))
                SendMail.SendMailNow(manager.CustomerId, manager.Email, mailTemplate.Subject, mailTemplate.Body, true);

            SendMail.SendMailNow(manager.CustomerId, SettingsMail.EmailForFeedback, mailTemplate.Subject, mailTemplate.Body, true);

            return Json(true);
        }

        [HttpPost]
        public JsonResult SendEmail(string clientName, string email, string emailText, int managerId)
        {
            var manager = ManagerService.GetManager(managerId);
            if (manager == null || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(emailText))
                return Json(false);

            var message = string.Format("{0} ({1}) написал {2}", HttpUtility.HtmlEncode(clientName),
                HttpUtility.HtmlEncode(email), HttpUtility.HtmlEncode(emailText));

            SendMail.SendMailNow(manager.CustomerId, manager.Email, "Письмо для менеджера " + SettingsMain.SiteUrl, message, true);

            return Json(true);
        }
    }
}