using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Configuration;

namespace AdvantShop.ViewModel.ProductDetails
{
    public class ProductDetailsViewModel : BaseProductViewModel
    {
        public ProductDetailsViewModel()
        {
            FinalDiscount = new Discount();
        }

        public Product Product { get; set; }

        public Offer Offer { get; set; }

        public List<BreadCrumbs> BreadCrumbs { get; set; }

        public bool IsAdmin { get; set; }
        
        public string Availble { get; set; }

        public bool IsAvailable { get; set; }

        public string Shippings { get; set; }

        public int FirstPaymentId { get; set; }

        public float FirstPayment { get; set; }

        public string FirstPaymentPrice { get; set; }

        public float FirstPaymentMinPrice { get; set; }

        public float MinimumOrderPrice { get; set; }

        public bool ShowAddButton { get; set; }

        public bool ShowPreOrderButton { get; set; }

        public bool ShowCreditButton { get; set; }

        public bool ShowBuyOneClick { get; set; }

        public float FinalPrice { get; set; }

        public Discount FinalDiscount { get; set; }

        public string PreparedPrice { get; set; }

        public string BonusPrice { get; set; }
        
        public string ReviewsCount { get; set; }

        public List<PropertyValue> ProductProperties { get; set; }

        public List<PropertyValue> BriefProperties { get; set; }

        public bool RatingReadOnly { get; set; }

        public SettingsDesign.eShowShippingsInDetails ShowShippingsMethods { get; set; }

        public bool RenderShippings { get; set; }

        public bool AllowReviews { get; set; }

        public bool ShowBriefDescription { get; set; }

        public bool HasCustomOptions { get; set; }

        public List<Offer> Gifts { get; set; }

        public List<MicrodataOffer> MicrodataOffers { get; set; }

        public int? ColorId { get; set; }

        public int? SizeId { get; set; }

        public string PreOrderButtonText { get; set; }

        public bool AllowBuyOutOfStockProducts = SettingsCheckout.OutOfStockAction == eOutOfStockAction.Cart;

    }

    public class MicrodataOffer
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Currency { get; set; }
        public bool Available { get; set; }
    }
}