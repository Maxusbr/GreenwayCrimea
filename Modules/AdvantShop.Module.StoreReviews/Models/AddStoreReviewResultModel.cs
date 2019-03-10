using AdvantShop.Module.StoreReviews.Domain;

namespace AdvantShop.Module.StoreReviews.Models
{
    public class AddStoreReviewResultModel
    {
        public string Errors { get; set; }
        public StoreReview Review { get; set; }
    }
}
