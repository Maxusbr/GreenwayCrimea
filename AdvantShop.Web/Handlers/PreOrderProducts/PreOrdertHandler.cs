using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Mails;
using AdvantShop.Orders;
using AdvantShop.ViewModel.PreOrder;

namespace AdvantShop.Handlers.PreOrderProducts
{
    public class PreOrderHandler
    {
        private readonly UrlHelper _urlHelper;

        public PreOrderHandler()
        {
            _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        public PreOrderViewModel Get(PreOrderViewModel model)
        {
            Offer offer = null;

            if (model.OfferId != 0)
            {
                offer = OfferService.GetOffer(model.OfferId);
            }

            if (offer == null || !offer.Product.Enabled || !offer.Product.CategoryEnabled)
                return null;

            var customer = CustomerContext.CurrentCustomer;

            model.Offer = offer;
            model.ProductId = offer.ProductId;
            model.Name = model.Name ??
                         (customer.RegistredUser ? customer.FirstName + " " + customer.LastName : string.Empty);
            model.Email = model.Email ?? (customer.RegistredUser ? customer.EMail : string.Empty);
            model.Phone = model.Phone ?? (customer.RegistredUser ? customer.Phone : string.Empty);

            model.Amount = offer.Product.MinAmount.HasValue && model.Amount < offer.Product.MinAmount.Value
                ? offer.Product.MinAmount.Value
                : model.Amount;

            if (float.IsNaN(model.Amount))
                model.Amount = 1;

            model.CanOrderByRequest = offer.CanOrderByRequest;
            model.ManufacturerName = offer.Product.Brand != null ? offer.Product.Brand.Name : string.Empty;
            model.ManufacturerUrl = offer.Product.Brand != null ? offer.Product.Brand.UrlPath : string.Empty;
            model.Ratio = offer.Product.Ratio;

            if (model.EnabledReviewsCount = SettingsCatalog.AllowReviews)
            {
                int reviewsCount = ReviewService.GetReviewsCount(offer.Product.ProductId, EntityType.Product);
                model.ReviewsCount = string.Format("{0} {1}",
                    reviewsCount == 0 ? "" : reviewsCount.ToString(CultureInfo.InvariantCulture),
                    Strings.Numerals(reviewsCount,
                        LocalizationService.GetResource("Product.Reviews0"),
                            LocalizationService.GetResource("Product.Reviews1"),
                            LocalizationService.GetResource("Product.Reviews2"),
                            LocalizationService.GetResource("Product.Reviews5")));
            }


            var productPhotoName = string.Empty;
            if (offer.ColorID != null)
            {
                var photo =
                    PhotoService.GetPhotos(offer.Product.ProductId, PhotoType.Product)
                        .FirstOrDefault(item => item.ColorID == offer.ColorID);

                if (photo != null)
                    productPhotoName = photo.PhotoName;
            }
            
            model.ImageSrc =
                FoldersHelper.GetImageProductPath(ProductImageType.Middle,
                    string.IsNullOrEmpty(productPhotoName) ? offer.Product.Photo : productPhotoName,
                    false);

            float optionsPrice = 0;
            if (model.Options.IsNotEmpty())
            {
                var listOptions = CustomOptionsService.DeserializeFromXml(model.Options, offer.Product.Currency.Rate);
                model.OptionsRendered = OrderService.RenderSelectedOptions(listOptions, offer.Product.Currency);
                optionsPrice = CustomOptionsService.GetCustomOptionPrice(offer.RoundedPrice, listOptions);
            }

            var priceWithDiscount = PriceService.GetFinalPrice(offer, customer.CustomerGroup, optionsPrice);

            model.PreparedPrice = PriceFormatService.FormatPrice(offer.RoundedPrice + optionsPrice, priceWithDiscount, offer.Product.Discount, true);

            model.BreadCrumbs = new List<BreadCrumbs>()
            {
                new BreadCrumbs(LocalizationService.GetResource("MainPage"), _urlHelper.RouteUrl("Home")),
                new BreadCrumbs(LocalizationService.GetResource("PreOrder.Index.Header"), _urlHelper.RouteUrl("PreOrder"))
            };
            return model;
        }

        public bool Send(PreOrderViewModel model, Offer offer)
        {
            if (model.Options.IsNotEmpty())
            {
                var listOptions = CustomOptionsService.DeserializeFromXml(model.Options, offer.Product.Currency.Rate);
                if (listOptions == null)
                    model.Options = null;

                model.OptionsRendered = OrderService.RenderSelectedOptions(listOptions, offer.Product.Currency);
            }

            var customer = CustomerContext.CurrentCustomer;

            if (SettingsCheckout.OutOfStockAction == eOutOfStockAction.Preorder)
            {
                var orderByRequest = new OrderByRequest
                {
                    OfferId = offer.OfferId,
                    ProductId = offer.Product.ProductId,
                    ProductName = offer.Product.Name,
                    ArtNo = offer.ArtNo,
                    Quantity = model.Amount,
                    UserName = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    Comment = model.Comment,
                    IsComplete = false,
                    RequestDate = DateTime.Now,
                    Options = model.Options
                };

                OrderByRequestService.AddOrderByRequest(orderByRequest);

                var mailTemplate =
                    new OrderByRequestMailTemplate(
                        orderByRequest.OrderByRequestId.ToString(CultureInfo.InvariantCulture),
                        offer.ArtNo,
                        offer.Product.Name,
                        model.Amount.ToString(CultureInfo.InvariantCulture),
                        model.Name,
                        model.Email,
                        model.Phone,
                        model.Comment,
                        offer.Color != null ? offer.Color.ColorName : string.Empty,
                        offer.Size != null ? offer.Size.SizeName : string.Empty,
                        model.OptionsRendered.IsNotEmpty() ? model.OptionsRendered : string.Empty);

                mailTemplate.BuildMail();

                SendMail.SendMailNow(CustomerContext.CustomerId, model.Email, mailTemplate.Subject, mailTemplate.Body, true);
                SendMail.SendMailNow(Guid.Empty, SettingsMail.EmailForOrders, mailTemplate.Subject, mailTemplate.Body, true, model.Email);

                return true;
            }
            else if (SettingsCheckout.OutOfStockAction == eOutOfStockAction.Order || SettingsCheckout.OutOfStockAction == eOutOfStockAction.Lead)
            {
                var handler = new Checkout.BuyInOneClickHandler(new Models.Checkout.BuyOneInClickJsonModel
                {
                    Amount = model.Amount,
                    Email = model.Email,
                    Comment = model.Comment,
                    Name = model.Name,
                    OfferId = model.OfferId,
                    OrderType = Core.Services.Orders.OrderType.PreOrder,
                    Page = BuyInOneclickPage.PreOrder,
                    Phone = model.Phone,
                    ProductId = model.ProductId
                });

                handler.Create();
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}