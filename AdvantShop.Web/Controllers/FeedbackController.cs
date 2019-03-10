using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.ViewModel.Feedback;
using AdvantShop.Web.Infrastructure.Controllers;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Crm.DealStatuses;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Orders;
using AdvantShop.Saas;
using BotDetect.Web.Mvc;

namespace AdvantShop.Controllers
{
	public class FeedbackController : BaseClientController
	{
		public ActionResult Index(FeedbackType messageType = FeedbackType.Offer)
		{
			var model = new FeedbackViewModel() { MessageType = messageType };
			var customer = CustomerContext.CurrentCustomer;
			if (customer != null)
			{
				model.Email = customer.EMail;
				model.Name = customer.FirstName;
				model.LastName = customer.LastName;
				model.Phone = customer.Phone;
			}

			SetNgController(NgControllers.NgControllersTypes.FeedbackCtrl);
			SetMetaInformation(T("Feedback.Index.FeedbackHeader"));

			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[CaptchaValidation("CaptchaCode", "CaptchaSource")]
		public ActionResult Index(FeedbackViewModel feedbackModel)
		{
			SetNgController(NgControllers.NgControllersTypes.FeedbackCtrl);
			SetMetaInformation(T("Feedback.Index.FeedbackHeader"));

			var isValid = true;
			if (string.IsNullOrWhiteSpace(feedbackModel.Name) || string.IsNullOrWhiteSpace(feedbackModel.Email) || 
			    string.IsNullOrWhiteSpace(feedbackModel.Phone) || !ValidationHelper.IsValidEmail(feedbackModel.Email) ||
				feedbackModel.MessageType == FeedbackType.Question && 
			    string.IsNullOrWhiteSpace(feedbackModel.Message))
			{
				isValid = false;
				ShowMessage(NotifyType.Error, T("Feedback.Index.WrongData"));
			}

			if (feedbackModel.MessageType == FeedbackType.Question)
			{
				if (SettingsMain.EnableCaptchaInFeedback)
				{
					if (!ModelState.IsValidField("CaptchaCode"))
					{
						isValid = false;
						ShowMessage(NotifyType.Error, T("User.Registration.ErrorCaptcha"));
					}
					MvcCaptcha.ResetCaptcha("CaptchaSource");
				}

				if (SettingsCheckout.IsShowUserAgreementText && !feedbackModel.Agree)
				{
					isValid = false;
					ShowMessage(NotifyType.Error, T("User.Registration.ErrorAgreement"));
				}
			}
			

			feedbackModel.Name = HttpUtility.HtmlEncode(feedbackModel.Name);
			feedbackModel.LastName = HttpUtility.HtmlEncode(feedbackModel.LastName);
			feedbackModel.Email = HttpUtility.HtmlEncode(feedbackModel.Email);
			feedbackModel.Phone = HttpUtility.HtmlEncode(feedbackModel.Phone);
			feedbackModel.Message = HttpUtility.HtmlEncode(feedbackModel.Message);
			feedbackModel.OrderNumber = HttpUtility.HtmlEncode(feedbackModel.OrderNumber);

			var model = new FeedbackViewModel();

			if (isValid)
			{
				var crmEnabled = !SaasDataService.IsSaasEnabled ||
								 (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveCrm);

				if (crmEnabled)
				{
					feedbackModel.PageName = T("Feedback.Index.FeedbackHeader");
					CreateLead(feedbackModel);
				}
				else
				{
					var mail = new FeedbackMailTemplate(SettingsMain.SiteUrl, SettingsMain.ShopName,
														feedbackModel.Name, feedbackModel.Email, feedbackModel.Phone,
														T("Feedback.Index.FeedbackForm") + ": " + feedbackModel.MessageType.Localize(),
														feedbackModel.Message, feedbackModel.OrderNumber);

					mail.BuildMail();
					SendMail.SendMailNow(CustomerContext.CustomerId, SettingsMail.EmailForFeedback, mail.Subject, mail.Body, true, feedbackModel.Email);
				}

				return View("Success", model);
			}

			model.Message = feedbackModel.Message;
			model.Name = feedbackModel.Name;
			model.LastName = feedbackModel.LastName;
			model.Email = feedbackModel.Email;
			model.OrderNumber = feedbackModel.OrderNumber;
			model.Phone = feedbackModel.Phone;
			model.MessageType = feedbackModel.MessageType;
			return View(model);
		}

		public ActionResult Success(FeedbackViewModel feedbackModel)
		{
			SetNgController(NgControllers.NgControllersTypes.FeedbackCtrl);
			SetMetaInformation(T("Feedback.Index.FeedbackHeader"));

			feedbackModel.Name = HttpUtility.HtmlEncode(feedbackModel.Name);
			feedbackModel.LastName = HttpUtility.HtmlEncode(feedbackModel.LastName);
			feedbackModel.Email = HttpUtility.HtmlEncode(feedbackModel.Email);
			feedbackModel.Phone = HttpUtility.HtmlEncode(feedbackModel.Phone);
			feedbackModel.Message = HttpUtility.HtmlEncode(feedbackModel.Message);
			feedbackModel.OrderNumber = HttpUtility.HtmlEncode(feedbackModel.OrderNumber);

			var crmEnabled = !SaasDataService.IsSaasEnabled ||
			                 (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveCrm);

			if (crmEnabled)
			{
				CreateLead(feedbackModel);
			}
			else
			{
				var mail = new FeedbackMailTemplate(SettingsMain.SiteUrl, SettingsMain.ShopName,
					feedbackModel.Name, feedbackModel.Email, feedbackModel.Phone,
					T("Feedback.Index.FeedbackForm") + ": " + feedbackModel.MessageType.Localize(),
					feedbackModel.Message, feedbackModel.OrderNumber);

				mail.BuildMail();
				SendMail.SendMailNow(CustomerContext.CustomerId, SettingsMail.EmailForFeedback, mail.Subject, mail.Body, true, feedbackModel.Email);
			}

			return View(feedbackModel);
		}

		private void CreateLead(FeedbackViewModel model)
		{
			var message = $"{model.MessageType.Localize()} со страницы " +
			              $"\"{model.PageName}\"" +
			              $"{(!string.IsNullOrWhiteSpace(model.OrderNumber) ? " к заказу " + model.OrderNumber : "")}: " +
			              $"\n{(!string.IsNullOrWhiteSpace(model.Message) ? model.Message.Replace("\n", "<br>") : "Запрос подключения")}";

			var source = OrderSourceService.GetOrderSource(OrderType.Feedback);

			var lead = new Lead()
			{
				Email = model.Email,
				FirstName = model.Name,
				LastName = model.LastName,
				Phone = model.Phone,

				Customer = new Customer(CustomerGroupService.DefaultCustomerGroup)
				{
					FirstName = model.Name,
					LastName = model.LastName,
					EMail = model.Email,
					Phone = model.Phone,
					StandardPhone = model.Phone != null ? StringHelper.ConvertToStandardPhone(model.Phone) : default(long?),
					CustomerRole = Role.User
				},

				Comment = message,

				OrderSourceId = source != null ? source.Id : 0
			};
			if (model.MessageType == FeedbackType.Offer)
			{
				var dealStatus = DealStatusService.GetList().FirstOrDefault(o => o.Name.Equals("Запрос на регистрацию"));
				if (dealStatus != null)
					lead.DealStatusId = dealStatus.Id;
			}
			LeadService.AddLead(lead);
		}
	}
}