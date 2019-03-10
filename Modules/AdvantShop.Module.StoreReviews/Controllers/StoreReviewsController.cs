using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.CMS;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Modules;
using AdvantShop.Customers;
using AdvantShop.Module.StoreReviews.Domain;
using AdvantShop.Module.StoreReviews.Handlers;
using AdvantShop.Module.StoreReviews.Models;
using AdvantShop.SEO;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Extensions;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Configuration;

namespace AdvantShop.Module.StoreReviews.Controllers
{
    [Module(Type = "StoreReviews")]
    public class StoreReviewsController : ModuleController
    {
        public ActionResult Index(int page = 1)
        {
            var paging = new StoreReviewsPagingHandler(page).Get();
            if ((paging.Pager.TotalPages < paging.Pager.CurrentPage && paging.Pager.CurrentPage > 1) ||
                paging.Pager.CurrentPage < 0)
            {
                return Error404();
            }

            var blogTitle = ModuleSettingsProvider.GetSettingValue<string>("PageTitle", StoreReviews.ModuleID);
            var breadCrumbs = new List<BreadCrumbs>()
            {
                new BreadCrumbs(T("MainPage"), Url.AbsoluteRouteUrl("Home")),
                new BreadCrumbs(blogTitle, Url.AbsoluteRouteUrl("StoreReviewsHome"))
            };

            var model = new ReviewsModelViewModel
            {
                Pager = paging.Pager,
                Items = paging.Items,
                BreadCrumbs = breadCrumbs,
                Title = blogTitle,
                ShowRatio = ModuleSettingsProvider.GetSettingValue<bool>("ShowRatio", StoreReviews.ModuleID),
                ImageWidth = ModuleSettingsProvider.GetSettingValue<int>("MaxImageWidth", StoreReviews.ModuleID)
            };

            var meta = new MetaInfo()
            {
                Title =
                    HttpUtility.HtmlEncode(ModuleSettingsProvider.GetSettingValue<string>("PageTitle", StoreReviews.ModuleID)),
                MetaKeywords =
                    HttpUtility.HtmlEncode(ModuleSettingsProvider.GetSettingValue<string>("MetaKeyWords", StoreReviews.ModuleID)),
                MetaDescription =
                    HttpUtility.HtmlEncode(ModuleSettingsProvider.GetSettingValue<string>("MetaDescription", StoreReviews.ModuleID)),
                H1 =
                    HttpUtility.HtmlEncode(ModuleSettingsProvider.GetSettingValue<string>("PageTitle", StoreReviews.ModuleID)),
            };

            model.Username =
                (CustomerContext.CurrentCustomer.FirstName + " " +
                 CustomerContext.CurrentCustomer.LastName).Trim();
            model.Email = CustomerContext.CurrentCustomer.EMail;
            model.ModerateReviews = ModuleSettingsProvider.GetSettingValue<bool>("ActiveModerateStoreReviews", StoreReviews.ModuleID);
            model.AllowImageUploading = ModuleSettingsProvider.GetSettingValue<bool>("AllowImageUploading", StoreReviews.ModuleID);
            model.UseCaptcha = ModuleSettingsProvider.GetSettingValue<bool>("UseCaptcha", StoreReviews.ModuleID);

            SetMetaInformation(meta, meta.Title, page: page);

            return View("~/Modules/StoreReviews/Views/StoreReviews/Index.cshtml", model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddReviewPost(StoreReviewAddViewModel model)
        {
            var result = new AddStoreReviewHandler().Execute(model);

            if (!string.IsNullOrWhiteSpace(result.Errors))
            {
                ShowMessage(NotifyType.Error, result.Errors);
                TempData["StoreReviewAddViewModel"] = model;

                return RedirectToRoute("StoreReviewsHome");
            }

            var activeModerate = ModuleSettingsProvider.GetSettingValue<bool>("ActiveModerateStoreReviews", StoreReviews.ModuleID);
            ShowMessage(NotifyType.Success, activeModerate ? "Спасибо за отзыв! Он появится после проверки модератором" : "Спасибо за отзыв!");

            return RedirectToRoute("StoreReviewsHome");
        }

        public ActionResult AddReview()
        {
            var model = TempData["StoreReviewAddViewModel"] as StoreReviewAddViewModel ?? new StoreReviewAddViewModel();
            model.ShowRatio = ModuleSettingsProvider.GetSettingValue<bool>("ShowRatio", StoreReviews.ModuleID);
            model.UseCaptcha = ModuleSettingsProvider.GetSettingValue<bool>("UseCaptcha", StoreReviews.ModuleID);
            model.AllowImageUploading = ModuleSettingsProvider.GetSettingValue<bool>("AllowImageUploading", StoreReviews.ModuleID);
            model.IsShowUserAgreementText = SettingsCheckout.IsShowUserAgreementText;
            model.UserAgreementText = SettingsCheckout.UserAgreementText;

            return PartialView("~/Modules/StoreReviews/Views/StoreReviews/AddReview.cshtml", model);
        }

        // GET: /storereviews/add
        public JsonResult Add(List<HttpPostedFileBase> file, string data)
        {
            var model = System.Web.Helpers.Json.Decode<StoreReviewAddViewModel>(data);

            if (ModuleSettingsProvider.GetSettingValue<bool>("AllowImageUploading", StoreReviews.ModuleID) && file != null && file.Any())
            {
                model.ReviewerImage = file[0];
            }

            var result = new AddStoreReviewHandler().Execute(model);

            if (!string.IsNullOrWhiteSpace(result.Errors))
                return Json(new { error = true, errors = result.Errors });

            return Json(new
            {
                error = false,
                review = new
                {
                    ParentId = result.Review.ParentId,
                    ReviewId = result.Review.Id,
                    Name = result.Review.ReviewerName,
                    Text = result.Review.Review,
                    PhotoPath = !string.IsNullOrEmpty(result.Review.ReviewerImage)
                        ? StoreReviews.ImagePath + result.Review.ReviewerImage
                        : string.Empty
                }
            });
        }

        public JsonResult Delete(string reviewId)
        {
            return Json(false);
        }
    }
}