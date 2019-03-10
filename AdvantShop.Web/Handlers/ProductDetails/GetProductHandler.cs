using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.InplaceEditor;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Extensions;
using AdvantShop.Payment;
using AdvantShop.ViewModel.ProductDetails;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Handlers.ProductDetails
{
    public class GetProductHandler
    {
        private readonly Product _product;
        private readonly int? _color;
        private readonly int? _size;
        private readonly string _view;

        public GetProductHandler(Product product, int? color, int? size, string view)
        {
            _product = product;
            _color = color;
            _size = size;
            _view = view;
        }

        public ProductDetailsViewModel Get()
        {
            var offer = OfferService.GetMainOffer(_product.Offers, _product.AllowPreOrder, _color, _size);
            var customer = CustomerContext.CurrentCustomer;

            var model = new ProductDetailsViewModel
            {
                IsAdmin = customer.IsAdmin || customer.IsManager,
                Product = _product,
                Offer = offer,
                ColorId = _color,
                SizeId = _size
            };

            var isAvailable = model.IsAvailable = offer != null && offer.Amount > 0;

            model.Availble = string.Format("{0}{1}",
                isAvailable ? LocalizationService.GetResource("Product.Available") : LocalizationService.GetResource("Product.NotAvailable"),
                isAvailable && SettingsCatalog.ShowStockAvailability
                    ? string.Format(
                        " (<div class=\"details-avalable-text inplace-offset inplace-rich-simple inplace-obj\" {1}>{0}</div> <div class=\"details-avalable-unit inplace-offset inplace-rich-simple inplace-obj\" {3}>{2}</div>)",
                        offer.Amount,
                        InplaceExtensions.InplaceOfferAmount(offer.OfferId),
                        _product.Unit,
                        _product.Unit.IsNotEmpty()
                            ? InplaceExtensions.InplaceProductUnit(offer.ProductId, ProductInplaceField.Unit).ToString()
                            : string.Empty)
                    : string.Empty);

            if (offer != null)
            {
                model.ShowAddButton = true;
                model.ShowPreOrderButton = _product.AllowPreOrder;
                model.ShowBuyOneClick = SettingsCheckout.BuyInOneClick;
                model.HasCustomOptions = CustomOptionsService.GetCustomOptionsByProductId(_product.ProductId).Any();
                
                var customOptionsPrice =
                    CustomOptionsService.GetCustomOptionPrice(offer.RoundedPrice,
                        new GetProductCustomOptionsHandler(_product.ProductId, string.Empty).Get(),
                        _product.Currency.Rate);
                
                var price = (offer.RoundedPrice + customOptionsPrice).RoundPrice(CurrencyService.CurrentCurrency.Rate);

                model.FinalDiscount = PriceService.GetFinalDiscount(price, _product.Discount, _product.Currency.Rate, customer.CustomerGroup, _product.ProductId);

                model.FinalPrice = PriceService.GetFinalPrice(price, model.FinalDiscount);
                model.PreparedPrice = PriceFormatService.FormatPrice(price, model.FinalPrice, model.FinalDiscount, true, true);
                model.BonusPrice = GetBonusCardPrice(_product, model.FinalPrice);
                
                var currencyIso3 = CurrencyService.CurrentCurrency.Iso3;
                model.MicrodataOffers = new List<MicrodataOffer>();

                foreach (var itemOffer in _product.Offers.OrderByDescending(x => x.OfferId == offer.OfferId))
                {
                    var offerPrice = PriceService.GetFinalPrice(itemOffer.RoundedPrice + customOptionsPrice, model.FinalDiscount);

                    model.MicrodataOffers.Add(new MicrodataOffer()
                    {
                        Name = itemOffer.ArtNo,
                        Price = offerPrice.ToInvariantString(),
                        Currency = currencyIso3,
                        Available = offerPrice > 0 && itemOffer.Amount > 0
                    });
                }

                if (SettingsDesign.ShowShippingsMethodsInDetails != SettingsDesign.eShowShippingsInDetails.Never)
                {
                    model.RenderShippings = true;
                    model.ShowShippingsMethods = SettingsDesign.ShowShippingsMethodsInDetails;
                }

                var creditPayment = PaymentService.GetCreditPaymentMethods().FirstOrDefault();
                if (creditPayment != null)
                {
                    model.ShowCreditButton = true;
                    model.FirstPaymentId = creditPayment.PaymentMethodId;
                    model.FirstPaymentMinPrice = PriceService.RoundPrice(creditPayment.MinimumPrice);
                    model.FirstPaymentPrice = creditPayment.FirstPayment > 0
                        ? (model.FinalPrice * creditPayment.FirstPayment / 100).FormatPrice(true, false) + "*"
                        : LocalizationService.GetResource("Product.WithoutFirstPayment");
                }
            }

            model.ProductProperties = PropertyService.GetPropertyValuesByProductId(_product.ProductId);
            model.BriefProperties = model.ProductProperties.Where(prop => prop.Property.UseInBrief)
                                        .GroupBy(x => new { x.PropertyId })
                                        .Select(x => new PropertyValue
                                        {
                                            PropertyId = x.Key.PropertyId,
                                            Property = x.First(y => y.PropertyId == x.Key.PropertyId).Property,
                                            PropertyValueId = x.First(y => y.PropertyId == x.Key.PropertyId).PropertyValueId,
                                            SortOrder = x.First(y => y.PropertyId == x.Key.PropertyId).SortOrder,
                                            Value = String.Join(", ", x.Where(y => y.PropertyId == x.Key.PropertyId).Select(v => v.Value))
                                        }).ToList();

            model.Gifts = OfferService.GetProductGifts(_product.ProductId);

            model.MinimumOrderPrice = CustomerGroupService.GetMinimumOrderPrice();

            model.RatingReadOnly = RatingService.DoesUserVote(_product.ProductId, customer.Id);

            var reviewsCountString = string.Empty;
            var modules = AttachedModules.GetModules<IModuleReviews>();
            if (modules.Any())
            {
                foreach (var module in modules)
                {
                    var instance = (IModuleReviews)Activator.CreateInstance(module);
                    reviewsCountString += instance.GetReviewsCount(HttpContext.Current.Request.Url.AbsoluteUri);
                }
            }
            else
            {
                var reviewsCount = SettingsCatalog.ModerateReviews
                    ? ReviewService.GetCheckedReviewsCount(_product.ProductId, EntityType.Product)
                    : ReviewService.GetReviewsCount(_product.ProductId, EntityType.Product);
                reviewsCountString = string.Format("{0} {1}", reviewsCount,
                    Strings.Numerals(reviewsCount,
                        LocalizationService.GetResource("Product.Reviews0"),
                        LocalizationService.GetResource("Product.Reviews1"),
                        LocalizationService.GetResource("Product.Reviews2"),
                        LocalizationService.GetResource("Product.Reviews5")));
            }

            model.ReviewsCount = reviewsCountString;
            model.AllowReviews = SettingsCatalog.AllowReviews;
            model.ShowBriefDescription = false;

            model.CustomViewPath = _view;

            model.PreOrderButtonText = SettingsCatalog.PreOrderButtonText;

            return model;
        }

        private string GetBonusCardPrice(Product product, float productPrice)
        {
            if (!BonusSystem.IsActive || productPrice <= 0 || !product.AccrueBonuses)
                return null;

            var bonusCard = BonusSystemService.GetCard(CustomerContext.CurrentCustomer.Id);
            if (bonusCard != null && bonusCard.Blocked)
                return null;

            if (bonusCard != null)
                return PriceService.GetBonusPrice((float)bonusCard.Grade.BonusPercent, productPrice).FormatPrice();

            return (//BonusSystem.BonusesForNewCard + 
                    PriceService.GetBonusPrice((float)BonusSystem.BonusFirstPercent, productPrice)).FormatPrice();
        }
    }
}