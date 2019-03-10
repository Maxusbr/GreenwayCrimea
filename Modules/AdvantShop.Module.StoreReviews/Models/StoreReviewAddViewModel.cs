using System.Web;

namespace AdvantShop.Module.StoreReviews.Models
{
    public class StoreReviewAddViewModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string CaptchaCode { get; set; }
        public string CaptchaSource { get; set; }
        public HttpPostedFileBase ReviewerImage { get; set; }
        public int Rate { get; set; }
        public int ParentId { get; set; }
        public bool ShowRatio{ get; set; }
        public bool UseCaptcha{ get; set; }
        public bool AllowImageUploading { get; set; }
        public bool IsShowUserAgreementText { get; set; }
        public string UserAgreementText { get; set; }
        public bool Agreement { get; set; }
    }
}
