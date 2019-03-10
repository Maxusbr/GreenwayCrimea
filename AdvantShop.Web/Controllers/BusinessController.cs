using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.SEO;
using AdvantShop.ViewModel.StaticPage;
using AdvantShop.Core.Services.InplaceEditor;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Saas;
using AdvantShop.ViewModel.Feedback;
using AdvantShop.Web.Infrastructure.Controllers;
using BotDetect.Web.Mvc;

namespace AdvantShop.Controllers
{
	public partial class BusinessController : BaseClientController
	{
		public ActionResult Index(string url)
		{
			var staticPage = StaticPageService.GetStaticPage("business");
			if (staticPage == null || !staticPage.Enabled)
				return Error404();

			SetMetaInformation(staticPage.Meta, staticPage.PageName);
			SetNgController(NgControllers.NgControllersTypes.FeedbackCtrl);

			var model = new FeedbackViewModel { MessageType = FeedbackType.Offer };
			var customer = CustomerContext.CurrentCustomer;
			if (customer != null)
			{
				model.Email = customer.EMail;
				model.Name = customer.FirstName;
				model.Phone = customer.Phone;
			}
			
			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[CaptchaValidation("CaptchaCode", "CaptchaSource")]
		public ActionResult Index(FeedbackViewModel feedbackModel)
		{
			var isValid = true;
			if (string.IsNullOrWhiteSpace(feedbackModel.Name) || string.IsNullOrWhiteSpace(feedbackModel.Email) ||
				string.IsNullOrWhiteSpace(feedbackModel.Phone) || !ValidationHelper.IsValidEmail(feedbackModel.Email))
			{
				isValid = false;
				ShowMessage(NotifyType.Error, T("Feedback.Index.WrongData"));
			}

			feedbackModel.PageName = "Посадочная №1";

			if (isValid) return RedirectToAction("Success", "Feedback", feedbackModel);

			var staticPage = StaticPageService.GetStaticPage("business");
			if (staticPage == null || !staticPage.Enabled)
				return Error404();

			//var metaInformation = 
			SetMetaInformation(staticPage.Meta, staticPage.PageName);
			SetNgController(NgControllers.NgControllersTypes.FeedbackCtrl);

			feedbackModel.Name = HttpUtility.HtmlEncode(feedbackModel.Name);
			feedbackModel.LastName = HttpUtility.HtmlEncode(feedbackModel.LastName);
			feedbackModel.Email = HttpUtility.HtmlEncode(feedbackModel.Email);
			feedbackModel.Phone = HttpUtility.HtmlEncode(feedbackModel.Phone);
			feedbackModel.Message = HttpUtility.HtmlEncode(feedbackModel.Message);
			feedbackModel.OrderNumber = HttpUtility.HtmlEncode(feedbackModel.OrderNumber);

			return View(feedbackModel);
		}

		[ChildActionOnly]
		public ActionResult Logo()
		{
			if (string.IsNullOrEmpty(SettingsMain.LogoImageName))
				return new EmptyResult();

			var model = new Areas.Mobile.Models.Home.LogoMobileModel
			{
				ImgSource = FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.LogoImageName, false),
				LogoAlt = SettingsMain.LogoImageAlt
			};

			return PartialView(model);
		}
	}
}