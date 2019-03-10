using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Models.Review;
using AdvantShop.Web.Infrastructure.Extensions;
using Newtonsoft.Json;
using AdvantShop.FilePath;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Controllers
{
    public partial class ReviewsController : BaseClientController
    {
        #region Help methods

        private bool IsValidData(ReviewModel review, out string errors)
        {
            errors = string.Empty;
            //try { _entityType = (EntityType)review.EntityType; }
            //catch { return false; }

            //if (!ReviewService.IsExistsEntity(review.EntityId, (EntityType)review.EntityType))
            //    return false;

            //if (!Int32.TryParse(context.Request["parentId"], out _parentId))
            //    return false;

            //_text = !string.IsNullOrEmpty(review.Text)
            //                ? HttpUtility.HtmlEncode(context.Request["text"].Trim()).Replace("\n", "<br />") : string.Empty;

            //if (_text.Length < 3)
            //    return false;

            //_name = !string.IsNullOrEmpty(context.Request["name"])
            //                ? HttpUtility.HtmlEncode(context.Request["name"].Trim()) : string.Empty;

            //if (_name.Length < 3)
            //    return false;

            //_email = !string.IsNullOrEmpty(context.Request["email"])
            //                ? HttpUtility.HtmlEncode(context.Request["email"].Trim()) : string.Empty;

            //if (_email.Length < 3 || !ValidationHelper.IsValidEmail(_email))
            //    return false;

            if (review.Agreement != SettingsCheckout.IsShowUserAgreementText)
            {
                errors = LocalizationService.GetResource("Js.Subscribe.ErrorAgreement");
                return false;
            }

            return true;
        }

        #endregion

        public JsonResult Add(List<HttpPostedFileBase> file, string data)
        {
            var error = string.Empty;
            var review = JsonConvert.DeserializeObject<ReviewModel>(data);
            if (!IsValidData(review, out error))
                return Json(new { error = true, errors = error });

            var ip = "local";
            if (Request.UserHostAddress != "::1" && Request.UserHostAddress != "127.0.0.1")
                ip = Request.UserHostAddress;

            var text = HttpUtility.HtmlEncode(review.Text.Trim()).Replace("\n", "<br />");
            var name = HttpUtility.HtmlEncode(review.Name.Trim());
            var email = HttpUtility.HtmlEncode(review.Email.Trim());

            var reviewItem = new Review
            {
                ParentId = review.ParentId,
                EntityId = review.EntityId,
                CustomerId = CustomerContext.CustomerId,
                Text = text,
                Type = (EntityType)review.EntityType,
                Name = name,
                Email = email,
                Ip = ip,
                AddDate = DateTime.Now
            };

            ReviewService.AddReview(reviewItem);

            if (SettingsCatalog.AllowReviewsImageUploading && file != null && file.Any() && FileHelpers.CheckFileExtension(file[0].FileName, EAdvantShopFileTypes.Image))
            {
                var photoName = PhotoService.AddPhoto(new Photo(0, reviewItem.ReviewId, PhotoType.Review)
                {
                    OriginName = file[0].FileName
                });

                if (!string.IsNullOrWhiteSpace(photoName))
                {
                    using (var image = Image.FromStream(file[0].InputStream))
                    {
                        FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.ReviewImage, photoName),
                            SettingsPictureSize.ReviewImageWidth, SettingsPictureSize.ReviewImageHeight, image);
                    }
                }
            }

            try
            {
                var p = ProductService.GetProduct(review.EntityId);
                if (p != null)
                {
                    var mailTemplate = new ProductDiscussMailTemplate(p.ArtNo, p.Name,
                        Url.AbsoluteRouteUrl("Product", new { url = p.UrlPath }), name, DateTime.Now.ToString(), text,
                        Url.AbsoluteRouteUrl("Product", new { url = p.UrlPath }), email);

                    mailTemplate.BuildMail();
                    SendMail.SendMailNow(Guid.Empty, SettingsMail.EmailForProductDiscuss, mailTemplate.Subject, mailTemplate.Body, true);

                    if (reviewItem.ParentId != 0)
                    {
                        var previousReview = ReviewService.GetReview(reviewItem.ParentId);
                        if (previousReview != null && !string.IsNullOrWhiteSpace(previousReview.Email))
                        {
                            var mailAnswerTemplate = new ProductDiscussAnswerMailTemplate(p.ArtNo, p.Name,
                                Url.AbsoluteRouteUrl("Product", new {url = p.UrlPath, tab = "tabReviews" }), name, DateTime.Now.ToString(),
                                previousReview.Text,
                                text);

                            mailAnswerTemplate.BuildMail();
                            SendMail.SendMailNow(Guid.Empty, previousReview.Email, mailAnswerTemplate.Subject, mailAnswerTemplate.Body, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return Json(new
            {
                error = false,
                review = new
                {
                    ParentId = reviewItem.ParentId,
                    ReviewId = reviewItem.ReviewId,
                    Name = reviewItem.Name,
                    Text = reviewItem.Text,
                    PhotoPath = reviewItem.Photo.PhotoName.IsNotEmpty()
                        ? "pictures/review/" + reviewItem.Photo.PhotoName
                        : string.Empty
                }
            });
        }

        public JsonResult Delete(int reviewId)
        {
            var customer = CustomerContext.CurrentCustomer;
            if (customer.IsAdmin || (customer.IsModerator && customer.HasRoleAction(RoleAction.Catalog)))
            {
                if (reviewId == 0)
                    return Json(false);

                ReviewService.DeleteReview(reviewId);
                return Json(true);
            }

            return Json(false);
        }

    }
}