using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.CMS;

namespace AdvantShop.ViewModel.PreOrder
{
    public class PreOrderViewModel
    {
        public Offer Offer { get; set; }

        public List<BreadCrumbs> BreadCrumbs { get; set; }

        public bool CanOrderByRequest { get; set; }
        
        public string ImageSrc { get; set; }

        public bool IsSuccess { get; set; }

        public string SuccessText { get; set; }

        public string PreparedPrice { get; set; }

        public string ReviewsCount { get; set; }

        public bool EnabledReviewsCount { get; set; }

        public string ManufacturerName { get; set; }

        public string ManufacturerUrl { get; set; }

        public double Ratio { get; set; }

        public string OptionsRendered { get; set; }


        public int ProductId { get; set; }

        public int OfferId { get; set; }

        public float Amount { get; set; }

        public string Options { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Comment { get; set; }

        public string CaptchaCode { get; set; }

        public string CaptchaSource { get; set; }

        public bool Agreement { get; set; }
    }
}