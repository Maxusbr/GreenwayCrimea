using AdvantShop.CMS;
using AdvantShop.Core.Services.InplaceEditor;

namespace AdvantShop.Handlers.Inplace
{
    public class InplaceReviewHandler
    {
        public bool Execute(int id, string content, ReviewInplaceField field)
        {
            var review = ReviewService.GetReview(id);
            if (review == null)
                return false;

            switch (field)
            {
                case ReviewInplaceField.Message:
                    review.Text = content;
                    break;
                case ReviewInplaceField.Name:
                    review.Name = content;
                    break;
            }

            ReviewService.UpdateReview(review);

            return true;
        }
    }
}