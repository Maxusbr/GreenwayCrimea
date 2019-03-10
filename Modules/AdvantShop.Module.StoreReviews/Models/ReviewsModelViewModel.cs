using System.Collections.Generic;
using AdvantShop.CMS;
using AdvantShop.Core.Models;
using AdvantShop.Module.StoreReviews.Domain;

namespace AdvantShop.Module.StoreReviews.Models
{
    public class ReviewsModelViewModel
    {
        public List<StoreReview> Items { get; set; }

        public Pager Pager { get; set; }

        public List<BreadCrumbs> BreadCrumbs { get; set; }

        public string Title { get; set; }
        public bool ShowRatio { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public bool ModerateReviews { get; set; }
        public int ImageWidth { get; set; }
        public bool AllowImageUploading { get; set; }
        public bool UseCaptcha { get; set; }
    }
}
