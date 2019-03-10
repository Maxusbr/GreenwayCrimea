using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Areas.Mobile.Models.ProductDetails;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Customers;
using AdvantShop.Handlers.ProductDetails;
using AdvantShop.SEO;
using AdvantShop.ViewModel.ProductDetails;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.SEO;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Areas.Mobile.Controllers
{
    public class ProductController : BaseMobileController
    {
        public ActionResult Index(string url, int? color, int? size)
        {
            if (string.IsNullOrWhiteSpace(url))
                return Error404();

            var product = ProductService.GetProductByUrl(url);
            if (product == null || !product.Enabled || !product.CategoryEnabled)
                return Error404();

            var model = new GetProductHandler(product, color, size, null).Get();

            model.BreadCrumbs =
                CategoryService.GetParentCategories(product.CategoryId)
                    .Reverse()
                    .Select(cat => new BreadCrumbs(cat.Name, Url.AbsoluteRouteUrl("Category", new { url = cat.UrlPath })))
                    .ToList();

            model.BreadCrumbs.Insert(0, new BreadCrumbs(T("MainPage"), Url.AbsoluteRouteUrl("Home")));
            model.BreadCrumbs.Add(new BreadCrumbs(product.Name, Url.AbsoluteRouteUrl("Product", new { url = product.UrlPath })));

            RecentlyViewService.SetRecentlyView(CustomerContext.CustomerId, product.ProductId);

            SetTitle(T("Catalog.Index.CatalogTitle"));
            SetNgController(NgControllers.NgControllersTypes.ProductCtrl);

            var category = CategoryService.GetCategory(product.CategoryId);

            var productsArtNo = product.Offers.Select(x => x.ArtNo).ToList();

            SetMetaInformation(
                product.Meta, product.Name, category != null ? category.Name : string.Empty,
                product.Brand != null ? product.Brand.Name : string.Empty,
                tags: product.Tags.Select(x => x.Name).ToList(),
                price: PriceFormatService.FormatPricePlain(model.FinalPrice, CurrencyService.CurrentCurrency),
                artNo: productsArtNo.Count > 0 ? string.Join(", ", productsArtNo) : product.ArtNo);
            
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

            return View(model);
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

        [ChildActionOnly]
        public ActionResult ProductPhotos(ProductDetailsViewModel productModel)
        {
            var product = productModel.Product;

            var model = new ProductPhotosMobileViewModel()
            {
                Product = product,
                Discount = productModel.FinalDiscount, // todo: Check it
                ProductModel = productModel,
                Photos =
                    product.ProductPhotos.OrderByDescending(item => item.Main)
                        .ThenBy(item => item.PhotoSortOrder)
                        .ToList(),
            };

            foreach (var photo in model.Photos)
            {
                photo.Title =
                    photo.Alt =
                        !string.IsNullOrWhiteSpace(photo.Description)
                            ? photo.Description
                            : product.Name + " - " + T("Product.ProductPhotos.AltText") + " " + photo.PhotoId;
            }

            return PartialView(model);
        }
    }
}