using System;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Security;
using AdvantShop.Web.Admin.Handlers.Account;
using AdvantShop.Web.Admin.Handlers.Users;
using AdvantShop.Web.Admin.Models.Users;
using AdvantShop.Web.Admin.ViewModels.Account;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using BotDetect.Web.Mvc;

namespace AdvantShop.Web.Admin.Controllers.Shared
{
    public class AccountController : BaseController
    {
        public ActionResult Login()
        {
            var customer = CustomerContext.CurrentCustomer;
            if (customer.Enabled && (customer.IsAdmin || customer.IsVirtual || customer.IsModerator))
                return RedirectToAction("Index", "Home");

            var model = new AccountLoginViewModel();

            var from = Request["from"];
            if (from != null && from.StartsWith("/"))
                model.From = from;

            var count = Convert.ToInt32(Session["admin_login_count"]);
            if (count > 0)
                model.ShowCaptcha = true;

            return View(model);
        }

        [HttpPost]
        [CaptchaValidation("CaptchaCode", "CaptchaSource")]
        public ActionResult Login(string txtLogin, string txtPassword, string from)
        {
            var email = txtLogin;
            var password = txtPassword;
            var count = Convert.ToInt32(Session["admin_login_count"]);

            if (!string.IsNullOrEmpty(email) && 
                !string.IsNullOrEmpty(password) && 
                (count == 0 || (count > 0 && ModelState.IsValidField("CaptchaCode"))))
            {
                if (AuthorizeService.SignIn(email, password, false, true))
                {
                    Session.Remove("admin_login_count");
                    MvcCaptcha.ResetCaptcha("CaptchaSource");

                    if (!string.IsNullOrWhiteSpace(from) && from.StartsWith("/"))
                        return Redirect(UrlService.GenerateBaseUrl() + "/" + from.TrimStart('/'));

                    return RedirectToAction("Index", "Home");
                }
            }

            Session["admin_login_count"] = Convert.ToInt32(Session["admin_login_count"]) + 1;

            return RedirectToAction("Login");
        }

        public ActionResult SetPassword(string email, string hash)
        {
            var model = new GetForgotPasswordViewModel(email, hash).Execute();
            model.FirstVisit = true;

            return View("ForgotPassword", model);
        }

        public ActionResult ForgotPassword(string email, string hash)
        {
            var model = new GetForgotPasswordViewModel(email, hash).Execute();

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string email)
        {
            var model = new ForgotPasswordViewModel() { View = EForgotPasswordView.EmailSent };
            if (email.IsNullOrEmpty())
            {
                model.View = EForgotPasswordView.ForgotPassword;
                ModelState.AddModelError(string.Empty, "Укажите email.");
                return View(model);
            }

            var customer = CustomerService.GetCustomerByEmail(email);
            if (customer == null || !(customer.IsAdmin || customer.IsModerator))
            {
                model.View = EForgotPasswordView.ForgotPassword;
                ModelState.AddModelError(string.Empty, "Сотрудник с указанным email не найден.");
            }
            else
            {
                var mailTpl = new UserPasswordRepairMailTemplate(customer.EMail, 
                    ValidationHelper.DeleteSigns(SecurityHelper.GetPasswordHash(customer.Password)).ToLower());
                mailTpl.BuildMail();

                SendMail.SendMailNow(customer.Id, customer.EMail, mailTpl.Subject, mailTpl.Body, true);
            }

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string newPassword, string newPasswordConfirm, string email, string hash, bool firstVisit = false)
        {
            var model = new ForgotPasswordViewModel()
            {
                View = EForgotPasswordView.PasswordRecovery,
                Email = email,
                Hash = hash,
                FirstVisit = firstVisit
            };

            if (newPassword.IsNullOrEmpty() || newPasswordConfirm.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, "Введите пароль");
                return View("ForgotPassword", model);
            }
            if (newPassword.Length < 6)
            {
                ModelState.AddModelError(string.Empty, "Пароль должен содержать не менее 6 символов");
                return View("ForgotPassword", model);
            }
            if (newPassword != newPasswordConfirm)
            {
                ModelState.AddModelError(string.Empty, "Введенные пароли не совпадают");
                return View("ForgotPassword", model);
            }

            var customer = CustomerService.GetCustomerByEmail(model.Email);
            if (customer != null && (customer.IsAdmin || customer.IsModerator))
            {
                if (ValidationHelper.DeleteSigns(SecurityHelper.GetPasswordHash(customer.Password)).ToLower() == model.Hash.ToLower())
                {
                    CustomerService.ChangePassword(customer.Id, newPassword, false);
                    AuthorizeService.SignIn(model.Email, newPasswordConfirm, false, true);
                    if (firstVisit)
                    {
                        return Redirect(Url.RouteUrl(new { controller = "Home", action = "Index" }) + "#?user=me");
                    }

                    model.View = EForgotPasswordView.PasswordChanged;
                }
                else
                {
                    model.View = EForgotPasswordView.RecoveryError;
                }
            }
            
            return View("ForgotPassword", model);
        }

        public JsonResult GetUserInfo(Guid customerId)
        {
            var dbModel = CustomerService.GetCustomer(customerId);
            if (dbModel == null)
                return JsonError(T("Admin.Users.Validate.NotFound"));
            return JsonOk(new GetUserModel(dbModel).Execute());
        }

        public JsonResult GetUserFormData()
        {
            return ProcessJsonResult(new GetUserFormDataHandler(CustomerContext.CustomerId));
        }

        public JsonResult GetCurrentUser()
        {
            return JsonOk(new GetUserModel(CustomerContext.CurrentCustomer).Execute());
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateCurrentUser(AdminUserModel model)
        {
            if (model.CustomerId != CustomerContext.CustomerId)
                return JsonError();
            return ProcessJsonResult(new AddEditUserHandler(model, true));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SendChangePasswordMail()
        {
            return ProcessJsonResult(new SendChangePasswordEmailHandler(CustomerContext.CustomerId));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangePasswordJson(string password, string passwordConfirm)
        {
            return ProcessJsonResult(new ChangePasswordHandler(CustomerContext.CustomerId, password, passwordConfirm));
        }
    }
}