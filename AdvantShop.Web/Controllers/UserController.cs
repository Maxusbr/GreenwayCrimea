using System;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;
using AdvantShop.Handlers.User;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Models.User;
using AdvantShop.Security;
using AdvantShop.Security.OAuth;
using AdvantShop.ViewModel.User;
using BotDetect.Web.Mvc;

namespace AdvantShop.Controllers
{
	public class UserController : BaseClientController
	{
		[HttpPost]
		public JsonResult LoginJson(string email, string password)
		{
			if (CustomerContext.CurrentCustomer.RegistredUser)
			{
				return Json(new { error = "User authorized", status = "error" });
			}

			if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
			{
				if (AuthorizeService.SignIn(email, password, false, true))
					return Json(new { error = "", status = "success" });
			}

			return Json(new { error = T("User.Login.WrongPassword"), status = "error" });
		}

		[HttpGet]
		public ActionResult Login(string redirectTo, string state, string code)
		{
			if (!string.IsNullOrEmpty(state) && state.Contains("googleanalytics"))
			{
				GoogleOAuth.LoginAnalytics(code, "login");

				return RedirectToRoute("Home", new { state = "googleanalytics" });
			}

			if (CustomerContext.CurrentCustomer.RegistredUser)
			{
				if (!string.IsNullOrEmpty(redirectTo) && redirectTo != "/")
					return Redirect(redirectTo);

				return RedirectToRoute(CustomerContext.CurrentCustomer.EMail.Contains("@temp") ? "MyAccount" : "Home");
			}

			SetMetaInformation(T("User.Login.Header"));

			return View();
		}

		public ActionResult LoginToken(string email, string hash, string redirectTo, bool? showhelp)
		{
			SettingsLic.ShowAdvantshopJivoSiteForm = showhelp ?? false;
			var customer = CustomerService.GetCustomerByEmail(email);
			if (customer != null)
			{
				var hashComputed = SecurityHelper.EncodeWithHmac(customer.EMail, customer.Password);
				if (hash == hashComputed && AuthorizeService.SignIn(customer.EMail, customer.Password, true, true))
				{
					if (!string.IsNullOrEmpty(redirectTo) && redirectTo != "/")
						return Redirect(redirectTo);

					if (string.IsNullOrEmpty(redirectTo))
						return Redirect("~/adminv2");
				}
			}

			return Redirect("~/");
		}

		public ActionResult Authorization(AuthorizationViewModel model)
		{
			if (model == null)
				model = new AuthorizationViewModel();

			var referrer = Request.UrlReferrer;
			if (model.RedirectTo == null && referrer != null)
			{
				if (!string.IsNullOrEmpty(referrer.ToString()) && referrer.Host.Contains(CommonHelper.GetParentDomain()))
					model.RedirectTo = referrer.ToString();
			}

			return PartialView(model);
		}

		public ActionResult Logout()
		{
			AuthorizeService.SignOut();

			if (Request.UrlReferrer != null)
			{
				var referrer = Request.UrlReferrer.ToString();

				if (!string.IsNullOrEmpty(referrer) && !(referrer.Contains("admin") || referrer.Contains("checkout")))
					return Redirect(referrer);
			}
			return RedirectToRoute("Home");
		}

		#region OAuth
		public ActionResult LoginOpenId(string pageToRedirect, string code, string state)
		{
			//if (string.IsNullOrEmpty(pageToRedirect))
			//    pageToRedirect = Url.RouteUrl("Login");

			pageToRedirect = pageToRedirect.TrimStart('/').ToLower();


			var model = new LoginOpenIdViewModel()
			{
				DisplayFacebook = SettingsOAuth.FacebookActive,
				DisplayGoogle = SettingsOAuth.GoogleActive,
				DisplayMailRu = SettingsOAuth.MailActive,
				DisplayOdnoklassniki = SettingsOAuth.OdnoklassnikiActive,
				DisplayVk = SettingsOAuth.VkontakteActive,
				DisplayYandex = SettingsOAuth.YandexActive,
				PageToRedirect = pageToRedirect
			};

			if (!(model.DisplayFacebook || model.DisplayGoogle || model.DisplayMailRu ||
				model.DisplayOdnoklassniki || model.DisplayVk || model.DisplayYandex))
			{
				return new EmptyResult();
			}

			if (string.IsNullOrEmpty(code))
			{
				return PartialView(model);
			}

			var stateParam = state.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (stateParam.Length != 3 && string.Equals(stateParam[2], CustomerContext.CurrentCustomer.Id.ToString()))
			{
				return PartialView(model);
			}

			if (model.DisplayVk && string.Equals(stateParam[0], "vk"))
			{
				VkOAuth.Login(code, pageToRedirect);
			}

			if (model.DisplayOdnoklassniki && string.Equals(stateParam[0], "ok"))
			{
				OkOAuth.Login(code, pageToRedirect);
			}

			if (model.DisplayGoogle && string.Equals(stateParam[0], "google"))
			{
				GoogleOAuth.Login(code, pageToRedirect);
			}

			if (model.DisplayMailRu && string.Equals(stateParam[0], "mail"))
			{
				MailOAuth.Login(code, pageToRedirect);
			}

			if (model.DisplayYandex && string.Equals(stateParam[0], "yandex"))
			{
				YandexOAuth.Login(code, pageToRedirect);
			}

			if (model.DisplayFacebook && string.Equals(stateParam[0], "fb"))
			{
				FacebookOAuth.Login(code, pageToRedirect);
			}

			Response.Redirect(stateParam[1], false);

			return PartialView(model);
		}

		public ActionResult LoginVk(string pageToRedirect)
		{
			return Redirect(VkOAuth.OpenDialog(pageToRedirect));
		}

		public ActionResult LoginFacebook(string pageToRedirect)
		{
			return Redirect(FacebookOAuth.OpenDialog(pageToRedirect));
		}

		public ActionResult LoginGoogle(string pageToRedirect)
		{
			return Redirect(GoogleOAuth.OpenDialog(pageToRedirect));
		}

		public ActionResult LoginGoogleAnalytics(string pageToRedirect)
		{
			return Redirect(GoogleOAuth.OpenAnalyticsDialog(pageToRedirect));
		}

		public ActionResult LoginOk(string pageToRedirect)
		{
			return Redirect(OkOAuth.OpenDialog(pageToRedirect));
		}

		public ActionResult LoginMailRu(string pageToRedirect)
		{
			return Redirect(MailOAuth.OpenDialog(pageToRedirect));
		}

		public ActionResult LoginYandex(string pageToRedirect)
		{
			return Redirect(YandexOAuth.OpenDialog(pageToRedirect));
		}

		#endregion

		#region Registration

		public ActionResult Registration(CustomerGroupType сustomerGroup = CustomerGroupType.Buyer)
		{
			if (CustomerContext.CurrentCustomer.RegistredUser)
				return RedirectToRoute("Home");

			var model = new RegistrationViewModel { CustomerGroup = сustomerGroup };

			if (Demo.IsDemoEnabled)
			{
				model.IsDemo = true;
				model.Email = Demo.GetRandomEmail();
				model.FirstName = Demo.GetRandomName();
				model.LastName = Demo.GetRandomLastName();
				model.Phone = Demo.GetRandomPhone();
			}

			if (BonusSystem.IsActive)
			{
				model.IsBonusSystemActive = true;
				model.WantBonusCard = true;

				var bonuses = BonusSystem.BonusesForNewCard;
				if (bonuses != 0)
					model.BonusesForNewCard = bonuses.FormatPrice();
			}

			SetMetaInformation(T("User.Registration.Registration"));

			return View(model);
		}

		[HttpPost]
		[CaptchaValidation("CaptchaCode", "CaptchaSource")]
		public ActionResult Registration(RegistrationViewModel model)
		{
			if (CustomerContext.CurrentCustomer.RegistredUser)
				return RedirectToRoute("Home");

			if (SettingsMain.EnableCaptchaInRegistration && !ModelState.IsValid)
			{
				ShowMessage(NotifyType.Error, T("User.Registration.ErrorCaptcha"));
			}
			else
			{
				MvcCaptcha.ResetCaptcha("CaptchaSource");

				var handler = new RegistrationHandler();
				var errors = handler.IsValid(model);

				if (errors.Count == 0)
				{
					handler.Register(model);
					return Redirect(Url.RouteUrl("MyAccount") + "#?tab=orderhistory");
				}

				foreach (var error in errors)
					ShowMessage(NotifyType.Error, error);
			}

			if (BonusSystem.IsActive)
			{
				model.IsBonusSystemActive = true;

				var bonuses = BonusSystem.BonusesForNewCard;
				if (bonuses != 0)
					model.BonusesForNewCard = bonuses.FormatPrice();
			}

			SetMetaInformation(T("User.Registration.Registration"));

			return View(model);
		}

		#endregion

		#region Forgot password

		public ActionResult ForgotPassword(string email, string recoverycode)
		{
			var model = new ForgotPasswordModel() { View = "forgotpass", Email = email, RecoveryCode = recoverycode };

			if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(recoverycode))
			{
				var customer = CustomerService.GetCustomerByEmail(email);
				if (customer != null)
				{
					model.View =
						ValidationHelper.DeleteSigns(SecurityHelper.GetPasswordHash(!string.IsNullOrWhiteSpace(customer.Password) ? customer.Password : customer.EMail)).ToLower()
							!= recoverycode.ToLower()
							? "recoveryError"
							: "recovery";
				}
			}

			SetMetaInformation(T("User.ForgotPassword.PasswordRecovery"));

			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public ActionResult ForgotPassword(string email)
		{
			var model = new ForgotPasswordModel() { View = "emailSend" };

			var customer = CustomerService.GetCustomerByEmail(email);
			if (customer != null)
			{
				var strLink = SettingsMain.SiteUrl + "/forgotpassword?Email=" + customer.EMail + "&recoverycode=" +
							  ValidationHelper.DeleteSigns(SecurityHelper.GetPasswordHash(!string.IsNullOrWhiteSpace(customer.Password) ? customer.Password : customer.EMail));

				var pwdRepairMail =
					new PwdRepairMailTemplate(
						ValidationHelper.DeleteSigns(SecurityHelper.GetPasswordHash(!string.IsNullOrWhiteSpace(customer.Password) ? customer.Password : customer.EMail)).ToLower(),
						customer.EMail, strLink);
				pwdRepairMail.BuildMail();

				SendMail.SendMailNow(customer.Id, customer.EMail, pwdRepairMail.Subject, pwdRepairMail.Body, true);
			}
			else
			{
				model.View = "emailSendError";
			}

			SetMetaInformation(T("User.ForgotPassword.PasswordRecovery"));

			return View(model);
		}

		[HttpPost]
		public ActionResult ChangePassword(string newPassword, string newPasswordConfirm, string email, string recoveryCode)
		{
			var model = new ForgotPasswordModel() { View = "recovery", Email = email, RecoveryCode = recoveryCode };

			SetMetaInformation(T("User.ForgotPassword.PasswordRecovery"));

			if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(newPasswordConfirm) ||
				newPassword != newPasswordConfirm)
			{
				ShowMessage(NotifyType.Error, T("User.ForgotPassword.PasswordDiffrent"));
				return View("ForgotPassword", model);
			}

			var customer = CustomerService.GetCustomerByEmail(model.Email);
			if (customer != null)
			{
				if (ValidationHelper.DeleteSigns(SecurityHelper.GetPasswordHash(!string.IsNullOrWhiteSpace(customer.Password) ? customer.Password : customer.EMail)).ToLower() ==
					model.RecoveryCode.ToLower())
				{
					CustomerService.ChangePassword(customer.Id, newPassword, false);
					AuthorizeService.SignIn(model.Email, newPasswordConfirm, false, true);

					model.View = "passwordChanged";
				}
				else
				{
					model.View = "recoveryError";
				}
			}

			return View("ForgotPassword", model);
		}

		#endregion

		#region ClientCode

		[ChildActionOnly]
		public ActionResult ClientCode()
		{
			if (!SettingsDesign.ShowClientId)
				return new EmptyResult();

			var code = ClientCodeService.GetClientCode(CustomerContext.CustomerId);

			return PartialView(new ClientCodeViewModel()
			{
				Code = code.ToString("##,##0").Replace(",", "-").Replace("\u00A0", "-")
			});
		}

		#endregion

	}
}