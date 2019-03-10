using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Hosting;
using AdvantShop.Core.Common.Captcha;
using AdvantShop.Core.Modules;
using AdvantShop.Module.StoreReviews.Domain;
using AdvantShop.Module.StoreReviews.Models;
using AdvantShop.Web.Infrastructure.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Configuration;

namespace AdvantShop.Module.StoreReviews.Handlers
{
    public class AddStoreReviewHandler
    {
        public AddStoreReviewResultModel Execute(StoreReviewAddViewModel model)
        {
            var result = new AddStoreReviewResultModel();

            var errList = new StringBuilder();

            var email = HttpUtility.HtmlEncode(model.Email);
            var name = HttpUtility.HtmlEncode(model.Name);
            var review = HttpUtility.HtmlEncode(model.Text);

            if (string.IsNullOrEmpty(email))
                errList.Append("Поле \"Email\" обязательное для заполнения. ");

            if (model.Agreement != SettingsCheckout.IsShowUserAgreementText)
                errList.Append(LocalizationService.GetResource("Js.Subscribe.ErrorAgreement"));

            if (string.IsNullOrEmpty(name))
                errList.Append("Поле \"Имя\" обязательное для заполнения. ");

            if (string.IsNullOrEmpty(review))
                errList.Append("Поле \"Отзыв\" обязательное для заполнения. ");

            if (ModuleSettingsProvider.GetSettingValue<bool>("UseCaptcha", StoreReviews.ModuleID) &&
                !CaptchaService_old.IsValidCode(model.CaptchaCode, model.CaptchaSource))
            {
                errList.Append("Введенный код не совпадает. ");
            }

            if (model.ReviewerImage.HasFile() && !StoreReviewRepository.CheckImageExtension(model.ReviewerImage.FileName))
                errList.Append("Неверный формат изображения. ");

            if (errList.Length > 0)
            {
                result.Errors = errList.ToString();
                return result;
            }

            result.Review = new StoreReview
            {
                Moderated = false,
                Rate = model.Rate,
                ParentId = model.ParentId,
                ReviewerEmail = email,
                ReviewerName = name,
                Review = review,
                DateAdded = DateTime.Now
            };

            StoreReviewRepository.AddStoreReview(result.Review);

            var imagePath = HostingEnvironment.MapPath(StoreReviews.ImagePath);
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            if (model.ReviewerImage.HasFile())
            {
                var imageName = result.Review.Id + Path.GetExtension(model.ReviewerImage.FileName);
                StoreReviewRepository.SaveAndResizeImage(Image.FromStream(model.ReviewerImage.InputStream),
                    imagePath + imageName);

                if (File.Exists(imagePath + imageName))
                {
                    result.Review.ReviewerImage = imageName;
                    StoreReviewRepository.UpdateStoreReview(result.Review);
                }
            }

            return result;
        }
    }
}
