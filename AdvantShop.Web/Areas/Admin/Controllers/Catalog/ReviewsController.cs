using System;
using System.Drawing;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Reviews;
using AdvantShop.Web.Admin.Models.Reviews;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Catalog
{
    [Auth(RoleAction.Catalog)]
    public class ReviewsController : BaseAdminController
    {
        public ActionResult Index(ReviewsFilterModel model)
        {
            SetMetaInformation(T("Admin.Reviews.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.ReviewsCtrl);

            return View(model);
        }

        public JsonResult GetReviews(ReviewsFilterModel model)
        {
            return Json(new GetReviewsHandler(model).Execute());
        }

        #region Commands

        private void Command(ReviewsFilterModel model, Action<int, ReviewsFilterModel> func)
        {
            if (model.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in model.Ids)
                    func(id, model);
            }
            else
            {
                var ids = new GetReviewsHandler(model).GetItemsIds("ReviewId");
                foreach (int id in ids)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteReviews(ReviewsFilterModel model)
        {
            Command(model, (id, c) => ReviewService.DeleteReview(id));
            return JsonOk();
        }

        #endregion

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteReview(int reviewId)
        {
            ReviewService.DeleteReview(reviewId);
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Inplace(int reviewId, bool Checked)
        {
            var review = ReviewService.GetReview(reviewId);
            if (review == null)
                return JsonError();

            review.Checked = Checked;
            ReviewService.UpdateReview(review);

            return JsonOk();
        }


        #region Add | Update review

        [HttpGet]
        public JsonResult GetReview(int reviewId)
        {
            var review = ReviewService.GetReview(reviewId);
            var model = new ReviewItemModel()
            {
                ReviewId = review.ReviewId,
                Type = EntityType.Product,
                Checked = review.Checked,
                Name = review.Name,
                Email = review.Email,
                Text = review.Text,
                AddDate = review.AddDate,
                Ip = review.Ip
            };

            var product = ProductService.GetProduct(review.EntityId);
            if (product != null)
            {
                model.EntityId = product.ProductId.ToString();
                model.ArtNo = product.ArtNo;
                model.ProductName = product.Name;
                model.ProductUrl = UrlService.GetUrl(UrlService.GetLink(ParamType.Product, product.UrlPath, product.ProductId));
            }

            var photo = PhotoService.GetPhotoByObjId<ReviewPhoto>(review.ReviewId, PhotoType.Review);

            if (photo != null && !string.IsNullOrEmpty(photo.PhotoName))
            {
                model.PhotoName = photo.PhotoName;
            }

            return Json(model);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddReview(ReviewItemModel model, HttpPostedFileBase photoFile)
        {
            Product product;
            if (string.IsNullOrWhiteSpace(model.ArtNo) || (product = ProductService.GetProduct(model.ArtNo)) == null)
                return JsonError("Продукт с таким артикулом не найден");

            var review = new Review()
            {
                EntityId = product.ProductId,
                Type = EntityType.Product,
                CustomerId = CustomerContext.CustomerId,
                Name = HttpUtility.HtmlEncode(model.Name.DefaultOrEmpty()),
                Text = HttpUtility.HtmlEncode(model.Text.DefaultOrEmpty()),
                Checked = model.Checked,
                Email = HttpUtility.HtmlEncode(model.Email.DefaultOrEmpty()),
                AddDate = model.AddDate != null ? model.AddDate.Value : DateTime.Now,
                Ip =
                    Request.UserHostAddress != "::1" && Request.UserHostAddress != "127.0.0.1"
                        ? Request.UserHostAddress
                        : "local"
            };

            ReviewService.AddReview(review);

            if (photoFile != null && FileHelpers.CheckFileExtension(photoFile.FileName, EAdvantShopFileTypes.Image))
            {
                AddPhoto(review.ReviewId, photoFile);
            }

            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateReview(ReviewItemModel model, HttpPostedFileBase photoFile)
        {
            var review = ReviewService.GetReview(model.ReviewId);
            if (review == null)
                return JsonError("Отзыв не найден");

            review.Name = HttpUtility.HtmlEncode(model.Name.DefaultOrEmpty());
            review.Email = HttpUtility.HtmlEncode(model.Email.DefaultOrEmpty());
            review.Text = HttpUtility.HtmlEncode(model.Text.DefaultOrEmpty());
            review.Checked = model.Checked;
            review.AddDate = model.AddDate != null ? model.AddDate.Value : DateTime.Now;

            ReviewService.UpdateReview(review);

            if (photoFile != null && FileHelpers.CheckFileExtension(photoFile.FileName, EAdvantShopFileTypes.Image))
            {
                AddPhoto(review.ReviewId, photoFile);
            }

            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeletePhoto(int reviewId)
        {
            var review = ReviewService.GetReview(reviewId);
            if (review == null)
                return Json(new { result = false });

            PhotoService.DeletePhotos(reviewId, PhotoType.Review);

            return JsonOk();
        }

        private void AddPhoto(int reviewId, HttpPostedFileBase photoFile)
        {
            try
            {
                PhotoService.DeletePhotos(reviewId, PhotoType.Review);

                var photoName =
                    PhotoService.AddPhoto(new Photo(0, reviewId, PhotoType.Review) {OriginName = photoFile.FileName});
                
                using (Image image = Image.FromStream(photoFile.InputStream))
                {
                    FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.ReviewImage, photoName),
                        SettingsPictureSize.ReviewImageWidth, SettingsPictureSize.ReviewImageHeight, image);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex.Message + " at ReviewController AddPhoto", ex);
            }
        }

        #endregion

        [HttpPost]
        public JsonResult GetFormData()
        {
            return Json(new
            {
                filesHelpText = FileHelpers.GetFilesHelpText(EAdvantShopFileTypes.Image, "15MB")
            });
        }
    }
}
