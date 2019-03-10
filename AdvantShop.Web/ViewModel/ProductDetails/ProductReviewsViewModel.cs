using System.Collections.Generic;
using AdvantShop.CMS;

namespace AdvantShop.ViewModel.ProductDetails
{
    public class ProductReviewsViewModel : BaseProductViewModel
    {
        public int EntityId { get; set; }

        public int EntityType { get; set; }

        public bool ModerateReviews { get; set; }

        public bool IsAdmin { get; set; }

        public List<Review> Reviews { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool ReviewsReadonly { get; set; }

        public string HeaderText { get; set; }

        public bool DisplayImage { get; set; }
    }
}