using System.IO;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.SEO;
using AdvantShop.Core.Services.SEO.MetaData;
using AdvantShop.Customers;
using AdvantShop.Extensions;
using AdvantShop.FilePath;
using AdvantShop.Handlers.ProductDetails;
using AdvantShop.Payment;
using AdvantShop.SEO;
using AdvantShop.Trial;
using AdvantShop.ViewModel.ProductDetails;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using AdvantShop.Core.Services.InplaceEditor;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public partial class ProductController : BaseClientProductController
    {
        #region Details page

        public ActionResult Index(string url, int? color, int? size, string v)
        {
            if (string.IsNullOrWhiteSpace(url))
                return Error404();

            var product = ProductService.GetProductByUrl(url);
            if (product == null || !product.Enabled || !product.CategoryEnabled)
                return Error404();
                        
            var model = new GetProductHandler(product, color, size, v).Get();

            model.BreadCrumbs =
                CategoryService.GetParentCategories(product.CategoryId)
                    .Reverse()
                    .Select(cat => new BreadCrumbs(cat.Name, Url.AbsoluteRouteUrl("Category", new { url = cat.UrlPath })))
                    .ToList();

            model.BreadCrumbs.Insert(0, new BreadCrumbs(T("MainPage"), Url.AbsoluteRouteUrl("Home")));
            model.BreadCrumbs.Add(new BreadCrumbs(product.Name, Url.AbsoluteRouteUrl("Product", new { url = product.UrlPath })));

            RecentlyViewService.SetRecentlyView(CustomerContext.CustomerId, product.ProductId);

            SetNgController(NgControllers.NgControllersTypes.ProductCtrl);

            var category = CategoryService.GetCategory(product.CategoryId);

            var productsArtNo = product.Offers.Select(x => x.ArtNo).ToList();

            SetMetaInformation(
                product.Meta, product.Name, category != null ? category.Name : string.Empty,
                product.Brand != null ? product.Brand.Name : string.Empty, 
                tags: product.Tags.Select(x => x.Name).ToList(),
                price: PriceFormatService.FormatPricePlain(model.FinalPrice, CurrencyService.CurrentCurrency),
                artNo: productsArtNo.Count > 0 ? string.Join(", ",productsArtNo) : product.ArtNo);

            var tagManager = GoogleTagManagerContext.Current;
            if (tagManager.Enabled)
            {
                tagManager.PageType = ePageType.product;
                tagManager.ProdId = model.Offer != null ? model.Offer.ArtNo : model.Product.ArtNo;
                tagManager.ProdName = model.Product.Name;
                tagManager.ProdValue = model.Offer != null ? model.Offer.RoundedPrice : 0;
                tagManager.CatCurrentId = model.Product.MainCategory.ID;
                tagManager.CatCurrentName = model.Product.MainCategory.Name;
            }

            MetaDataContext.CurrentObject = new OpenGraphModel
            {
                Type = OpenGraphType.Product,
                Images = product.ProductPhotos.OrderByDescending(x => x.Main)
                    .ThenBy(x => x.PhotoSortOrder)
                    .Take(5)
                    .Select(photo => photo.ImageSrcMiddle()).ToList()
            };

            WriteLog(model.Product.Name, Url.AbsoluteRouteUrl("Product", new { url = product.UrlPath }), ePageType.product);

            return CustomView(model);
        }

        public ActionResult ProductQuickView(int productId, int? color, int? size)
        {
            var product = ProductService.GetProduct(productId);
            if (product == null || !product.Enabled || !product.CategoryEnabled)
                return Error404();

            var model = new GetProductHandler(product, color, size, null).Get();
            model.AllowReviews = false;
            model.ShowBriefDescription = true;

            RecentlyViewService.SetRecentlyView(CustomerContext.CustomerId, product.ProductId);

            SetMetaInformation(
                product.Meta, product.Name, CategoryService.GetCategory(product.CategoryId).Name,
                product.Brand != null ? product.Brand.Name : string.Empty, tags: product.Tags.Select(x => x.Name).ToList());

            return View(model);
        }


        [ChildActionOnly]
        public ActionResult ProductPhotos(ProductDetailsViewModel productModel, bool enabledModalPreview = true)
        {
            var product = productModel.Product;

            var model = new ProductPhotosViewModel()
            {
                Product = product,
                Discount = productModel.FinalDiscount, // todo: Check it
                ProductModel = productModel,
                Photos = product.ProductPhotos,
                EnabledModalPreview = enabledModalPreview,
                EnabledZoom = SettingsDesign.EnableZoom,

                ActiveThreeSixtyView = productModel.Product.ActiveView360 && product.ProductPhotos360.Any(),
                Photos360 = product.ProductPhotos360,
                Photos360Ext = product.ProductPhotos360.Any() ? Path.GetExtension(product.ProductPhotos360.First().PhotoName) : string.Empty,
                CustomViewPath = productModel.CustomViewPath
            };

            foreach (var photo in model.Photos)
            {
                photo.Title =
                    photo.Alt =
                        !string.IsNullOrWhiteSpace(photo.Description)
                            ? photo.Description
                            : product.Name + " - " + T("Product.ProductPhotos.AltText") + " " + photo.PhotoId;
            }

            if (productModel.Offer != null && productModel.Offer.Photo != null) //&& productModel.Offer.Photo.PhotoName.IsNotEmpty())
            {
                model.MainPhoto = productModel.Offer.Photo;
            }
            else
            {
                model.MainPhoto =
                    model.Photos.OrderByDescending(item => item.Main)
                                .ThenBy(item => item.PhotoSortOrder)
                                .FirstOrDefault(item => item.Main);
            }
            
            if (model.MainPhoto == null)
            {
                model.MainPhoto = new ProductPhoto(product.ProductId, PhotoType.Product, "");
            }

            model.MainPhoto.Title =
                model.MainPhoto.Alt =
                    !string.IsNullOrWhiteSpace(model.MainPhoto.Description)
                        ? model.MainPhoto.Description
                        : product.Name + " - " + T("Product.ProductPhotos.AltText") + " " + model.MainPhoto.PhotoId;

            model.Video = product.ProductVideos.FirstOrDefault();

            var customLabels = new List<ProductLabel>();

            foreach (var labelModule in AttachedModules.GetModules<ILabel>())
            {
                var classInstance = (ILabel)Activator.CreateInstance(labelModule);
                var label = classInstance.GetLabel();
                if (label != null)
                {
                    customLabels.Add(label);
                }

                var labels = classInstance.GetLabels();
                if (labels != null)
                {
                    customLabels.AddRange(labels);
                }
            }

            model.Labels = customLabels.Where(l => l.ProductIds.Contains(product.ProductId)).Select(l => l.LabelCode).ToList();

            model.CarouselPhotoHeight = SettingsPictureSize.XSmallProductImageHeight;
            model.CarouselPhotoWidth = SettingsPictureSize.XSmallProductImageWidth;
            model.PreviewPhotoHeight = SettingsPictureSize.MiddleProductImageHeight;
            model.PreviewPhotoWidth = SettingsPictureSize.MiddleProductImageWidth;

            return CustomPartialView(model);
        }

        [ChildActionOnly]
        public ActionResult ProductInfo(ProductDetailsViewModel productInfoModel)
        {
            return CustomPartialView(productInfoModel);
        }

        [ChildActionOnly]
        public ActionResult ProductTabs(ProductDetailsViewModel productModel)
        {
            var model = new ProductTabsViewModel()
            {
                ProductModel = productModel
            };

            foreach (var tabsModule in AttachedModules.GetModules<IProductTabs>())
            {
                var classInstance = (IProductTabs)Activator.CreateInstance(tabsModule, null);
                model.Tabs.AddRange(classInstance.GetProductDetailsTabsCollection(productModel.Product.ProductId));
            }

            if (SettingsSEO.ProductAdditionalDescription.IsNotEmpty())
            {
                model.AdditionalDescription =
                    GlobalStringVariableService.TranslateExpression(
                        SettingsSEO.ProductAdditionalDescription, MetaType.Product, productModel.Product.Name,
                        CategoryService.GetCategory(productModel.Product.CategoryId).Name,
                        productModel.Product.Brand != null ? productModel.Product.Brand.Name : string.Empty,
                        price: PriceFormatService.FormatPricePlain(productModel.FinalPrice, CurrencyService.CurrentCurrency),
                        tags: productModel.Product.Tags.Select(x => x.Name).ToList().AggregateString(" "), 
                        productArtNo: productModel.Product.ArtNo);
            }

            model.UseStandartReviews = !AttachedModules.GetModules<IModuleReviews>().Any();

            model.ReviewsCount = model.UseStandartReviews
                ? SettingsCatalog.ModerateReviews
                    ? ReviewService.GetCheckedReviewsCount(productModel.Product.ProductId, EntityType.Product)
                    : ReviewService.GetReviewsCount(productModel.Product.ProductId, EntityType.Product)
                : 0;

            return PartialView(model);
        }

        //[ChildActionOnly]
        public ActionResult ProductProperties(ProductDetailsViewModel productModel, int productId = 0, bool renderInplaceBlock = true)
        {
            var showInPlaceEditor = (CustomerContext.CurrentCustomer.IsAdmin || TrialService.IsTrialEnabled) &&
                                    SettingsMain.EnableInplace;

            var propertyValues = new List<PropertyValue>();
            var prodPropertyValues = productId == 0
                                        ? productModel.ProductProperties.Where(v => v.Property.UseInDetails).ToList()
                                        : PropertyService.GetPropertyValuesByProductId(productId).Where(v => v.Property.UseInDetails).ToList();

            if (!showInPlaceEditor)
            {
                foreach (var value in prodPropertyValues.Where(propValue => propertyValues.All(x => x.PropertyId != propValue.PropertyId)))
                {
                    propertyValues.Add(new PropertyValue()
                    {
                        Property = value.Property,
                        PropertyId = value.PropertyId,
                        PropertyValueId = value.PropertyValueId,
                        SortOrder = value.SortOrder,
                        Value = String.Join(", ", prodPropertyValues.Where(x => x.PropertyId == value.PropertyId).Select(x => x.Value))
                    });
                }
            }
            else
            {
                propertyValues = prodPropertyValues;
            }

            if (propertyValues.Count == 0 && !showInPlaceEditor)
                return new EmptyResult();

            var model = new PropductPropertiesViewModel()
            {
                ProductId = productId == 0 ? productModel.Product.ProductId : productId,
                PropertyValues = propertyValues,
                ShowInPlaceEditor = showInPlaceEditor,
                RenderInplaceAddBlock = showInPlaceEditor && renderInplaceBlock,
                CustomViewPath = productModel.CustomViewPath
            };

            return CustomPartialView(model);
        }

        [ChildActionOnly]
        public ActionResult ProductVideos(int productId)
        {
            if (!ProductVideoService.HasVideo(productId))
                return new EmptyResult();

            return PartialView(productId);
        }
        
        [ChildActionOnly]
        public ActionResult ProductReviews(ProductDetailsViewModel productModel, int productId, bool reviewsReadonly = false, string headerText = "")
        {
            var reviews =
                ReviewService.GetReviews(productId, EntityType.Product)
                    .Where(review => review.Checked || !SettingsCatalog.ModerateReviews).ToList();

            var customer = CustomerContext.CurrentCustomer;

            var model = new ProductReviewsViewModel()
            {
                EntityId = productId,
                EntityType = (int)EntityType.Product,
                ModerateReviews = SettingsCatalog.ModerateReviews,
                IsAdmin = customer.IsAdmin,
                Reviews = reviews,
                UserName = customer.RegistredUser ? customer.FirstName + " " + customer.LastName : string.Empty,
                Email = customer.EMail,
                CustomViewPath = productModel.CustomViewPath,
                ReviewsReadonly = reviewsReadonly,
                HeaderText = headerText,
                DisplayImage = SettingsCatalog.DisplayReviewsImage
            };

            return CustomPartialView(model);
        }

        [ChildActionOnly]
        public ActionResult SizeColorPicker(Product product)
        {
            var offers = product.Offers;

            var colors = offers.Where(o => o.Color != null && (o.Amount > 0 || product.AllowPreOrder || !SettingsCatalog.ShowOnlyAvalible))
                        .OrderBy(o => o.Color.SortOrder)
                        .Select(x => new
                        {
                            x.Color.ColorId,
                            x.Color.ColorName,
                            x.Color.ColorCode,
                            x.Color.IconFileName.PhotoName
                        })
                        .Distinct();

            var sizes = offers.Where(o => o.Size != null && (o.Amount > 0 || product.AllowPreOrder|| !SettingsCatalog.ShowOnlyAvalible))
                        .OrderBy(o => o.Size.SortOrder)
                        .Select(x => new
                        {
                            x.Size.SizeId,
                            x.Size.SizeName
                        })
                        .Distinct();

            var model = new SizeColorPickerViewModel
            {
                Colors = colors.Any() ? JsonConvert.SerializeObject(colors) : string.Empty,
                Sizes = sizes.Any() ? JsonConvert.SerializeObject(sizes) : string.Empty,
                ColorIconWidthDetails = SettingsPictureSize.ColorIconWidthDetails,
                ColorIconHeightDetails = SettingsPictureSize.ColorIconHeightDetails,
                SizesHeader = SettingsCatalog.SizesHeader,
                ColorsHeader = SettingsCatalog.ColorsHeader
            };

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult CustomOptions(int productId)
        {
            if (productId == 0)
                return new EmptyResult();

            return PartialView(productId);
        }

        [ChildActionOnly]
        public ActionResult RelatedProducts(Product product, string type, bool enabledCarousel = true)
        {
            if (product == null || string.IsNullOrEmpty(type))
                return new EmptyResult();

            RelatedType relatedType;
            Enum.TryParse(type, true, out relatedType);

            var model = new GetRelatedProductsHandler(product, relatedType).Get();

            if (model == null || !model.IsNotEmpty)
                return new EmptyResult();

            model.EnabledCarousel = enabledCarousel;

            return PartialView(relatedType + "Products", model);
        }

        [ChildActionOnly]
        public ActionResult ProductGifts(ProductDetailsViewModel productModel)
        {
            if (productModel.Gifts == null || productModel.Gifts.Count == 0)
                return new EmptyResult();

            var model = new ProductGiftsViewModel()
             {
                 Gifts = productModel.Gifts,
                 CustomViewPath = productModel.CustomViewPath,
             };

            return CustomPartialView(model);
        }

        [HttpPost]
        public JsonResult GetShippings(int offerId, float amount, string customOptions, string zip)
        {
            var model = new GetShippingsHandler(offerId, amount, customOptions, zip).Get();
            return Json(model);
        }

        public JsonResult GetOffers(int productId, int? colorId, int? sizeId)
        {
            var product = ProductService.GetProduct(productId);
            if (product == null)
                return Json(null);

            var offers = product.Offers;
            if (offers == null || offers.Count == 0)
                return Json(null);

            var offerSelected = OfferService.GetMainOffer(offers, product.AllowPreOrder, colorId, sizeId);
            var amountBuy = product.MinAmount == null
                                ? product.Multiplicity
                                : product.Multiplicity > product.MinAmount
                                    ? product.Multiplicity
                                    : product.MinAmount.Value;

            var obj = new
            {
                Offers = offers.Select(
                    offer => new
                    {
                        offer.OfferId,
                        offer.ProductId,
                        offer.ArtNo,
                        Color = offer.Color,
                        Size = offer.Size,
                        offer.RoundedPrice,
                        offer.Product.Discount,
                        offer.Amount,
                        AmountBuy = amountBuy,
                        offer.Main,
                        IsAvailable = RoundsMin(offer.Amount, product.Multiplicity) > 0.0f,
                        Available = string.Format("{0}{1}",
                        offer.Amount > 0 ? T("Product.Available") : T("Product.NotAvailable"),
                        offer.Amount > 0 && SettingsCatalog.ShowStockAvailability
                            ? string.Format(" (<div class=\"details-avalable-text {2}\" {3}>{0}</div> <div class=\"details-avalable-unit {2}\" {4}>{1}</div>)",
                            RoundsMin(offer.Amount, product.Multiplicity).ToString(),
                            product.Unit,
                            InplaceEditorService.CanUseInplace(RoleAction.Catalog) ? "inplace-offset inplace-obj inplace-rich-simple" : string.Empty,
                            InplaceExtensions.InplaceOfferAmount(offer.OfferId),
                            InplaceExtensions.InplaceProductUnit(offer.ProductId, ProductInplaceField.Unit))
                            : string.Empty)
                    }),
                StartOfferIdSelected = offerSelected.OfferId,
                product.Unit,
                ShowStockAvailability = SettingsCatalog.ShowStockAvailability,
                AllowPreOrder = product.AllowPreOrder
            };
            return Json(obj);
        }

        private float RoundsMin(float Amount, float Multiplicity)
         {
            int t = Multiplicity.ToString().Remove(0, Multiplicity.ToString().IndexOf(",") + 1).Length;
            var round = (float)Math.Round(Amount, t);
            var multiplicity = (float)Math.Round(Multiplicity, t);
            
            round = round > Amount ? round - multiplicity : round;
            
            return round;
        }

        [HttpPost]
        public JsonResult GetOfferPrice(int offerId, string attributesXml)
        {
            var offer = OfferService.GetOffer(offerId);
            if (offer == null)
                return Json(new { PriceString = 0F, PriceNumber = "", Bonuses = "" });

            var customer = CustomerContext.CurrentCustomer;
            var bonusPrice = string.Empty;

            var customOptionsPrice = CustomOptionsService.GetCustomOptionPrice(offer.RoundedPrice, HttpUtility.UrlDecode(attributesXml), offer.Product.Currency.Rate);

            var price = offer.RoundedPrice + customOptionsPrice;
            var totalDiscount = PriceService.GetFinalDiscount(price, offer.Product.Discount, offer.Product.Currency.Rate, customer.CustomerGroup, offer.ProductId);

            var startPrice = PriceService.GetFinalPrice(price);
            var finalPrice = PriceService.GetFinalPrice(price, totalDiscount);

            var priceHtml = PriceFormatService.FormatPrice(startPrice, finalPrice, totalDiscount, true, true);

            if (BonusSystem.IsActive && offer.RoundedPrice > 0 && offer.Product.AccrueBonuses)
            {
                var bonusCard = BonusSystemService.GetCard(customer.Id);
                if (bonusCard != null && bonusCard.Blocked)
                {
                    bonusPrice = null;
                }
                else if (bonusCard != null)
                {
                    bonusPrice = PriceFormatService.RenderBonusPrice((float)bonusCard.Grade.BonusPercent, finalPrice, totalDiscount);
                }
                else if (BonusSystem.BonusFirstPercent != 0)
                {
                    bonusPrice = 
                        //BonusSystem.BonusesForNewCard +
                        PriceFormatService.RenderBonusPrice((float)BonusSystem.BonusFirstPercent, finalPrice, totalDiscount);
                }
            }

            return Json(new
            {
                PriceString = priceHtml, //for details page 
                PriceNumber = finalPrice,
                PriceOldNumber = startPrice,
                Bonuses = bonusPrice,
            });
        }

        public JsonResult GetFirstPaymentPrice(float price, float discount, float discountAmount)
        {
            var creditPayment = PaymentService.GetCreditPaymentMethods().FirstOrDefault();
            if (creditPayment == null)
                return null;

            var result = "";
            var finalPrice = PriceService.GetFinalPrice(price, new Discount(discount, discountAmount));

            if (price >= PriceService.RoundPrice(creditPayment.MinimumPrice))
            {
                result = creditPayment.FirstPayment > 0
                    ? PriceFormatService.FormatPrice(finalPrice * creditPayment.FirstPayment / 100, true) + "*"
                    : T("Product.WithoutFirstPayment");
            }

            return Json(result);
        }

        public JsonResult GetVideos(int productId)
        {
            return Json(ProductVideoService.GetProductVideos(productId));
        }

        public JsonResult GetPhotos(int productId)
        {
            return Json(PhotoService.GetPhotos(productId, PhotoType.Product).Select(photo => new
            {
                PathXSmall = FoldersHelper.GetImageProductPath(ProductImageType.XSmall, photo.PhotoName, false),
                PathSmall = FoldersHelper.GetImageProductPath(ProductImageType.Small, photo.PhotoName, false),
                PathMiddle = FoldersHelper.GetImageProductPath(ProductImageType.Middle, photo.PhotoName, false),
                photo.ColorID,
                photo.PhotoId,
                photo.Description,
                SettingsPictureSize.XSmallProductImageHeight,
                SettingsPictureSize.XSmallProductImageWidth,
                SettingsPictureSize.SmallProductImageHeight,
                SettingsPictureSize.SmallProductImageWidth,
                SettingsPictureSize.MiddleProductImageWidth,
                SettingsPictureSize.MiddleProductImageHeight
            }).ToList(),
            JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomOptions(int productId)
        {
            var customOptions = CustomOptionsService.GetCustomOptionsByProductId(productId);
            return Json(customOptions);
        }

        public JsonResult CustomOptionsXml(int productId, string selectedOptions)
        {
            var attributesXml = new GetProductCustomOptionsHandler(productId, selectedOptions).Get();
            return Json(HttpUtility.UrlEncode(attributesXml));
        }

        public JsonResult AddRating(int objid, int rating)
        {
            float newRating = 0;

            if (objid != 0 && rating != 0)
                newRating = RatingService.Vote(objid, rating);

            return Json(newRating);
        }

        [HttpGet]
        public JsonResult GetPropertiesNames(string q)
        {
            return Json(PropertyService.GetPropertiesByName(q).ToList());
        }

        [HttpGet]
        public JsonResult GetPropertiesValues(string q, int productId, int propertyId = 0)
        {
            return Json(PropertyService.GetPropertiesValuesByNameEndProductId(q, productId, propertyId).ToList());
        }

        #endregion
    }
}