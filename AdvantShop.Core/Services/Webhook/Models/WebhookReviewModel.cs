using AdvantShop.CMS;

namespace AdvantShop.Core.Services.Webhook.Models
{
    public class WebhookReviewModel : WebhookModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public static explicit operator WebhookReviewModel(Review review)
        {
            return new WebhookReviewModel
            {
                Id = review.ReviewId,
                Email = review.Email,
                Name = review.Name,
            };
        }
    }
}
