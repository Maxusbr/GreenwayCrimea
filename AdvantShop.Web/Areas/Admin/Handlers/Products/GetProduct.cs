using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Repository.Currencies;
using AdvantShop.SEO;
using AdvantShop.Web.Admin.Models.Products;

namespace AdvantShop.Web.Admin.Handlers.Products
{
    public class GetProduct
    {
        private readonly int _productId;

        public GetProduct(int productId)
        {
            _productId = productId;
        }

        public AdminProductModel Execute()
        {
            var product = ProductService.GetProduct(_productId);
            if (product == null)
                return null;

            var model = new AdminProductModel()
            {
                ProductId = product.ProductId,
                Name = product.Name,
                ArtNo = product.ArtNo,
                Enabled = product.Enabled,
                UrlPath = product.UrlPath,

                BestSeller = product.BestSeller,
                Recomended = product.Recomended,
                New = product.New,
                Sales = product.OnSale,

                Weight = product.Weight,
                Width = product.Width,
                Height = product.Height,
                Length = product.Length,
                
                CurrencyId = product.CurrencyID,
                DiscountPercent = product.Discount.Percent,
                DiscountAmount = product.Discount.Amount,
                DiscountType = product.Discount.Type,
                AllowPreOrder = product.AllowPreOrder,

                Unit = product.Unit,
                MinAmount = product.MinAmount,
                MaxAmount = product.MaxAmount,
                Multiplicity = product.Multiplicity,
                ShippingPrice = product.ShippingPrice,

                Photo = product.Photo,
                BriefDescription = product.BriefDescription,
                Description = product.Description,

                BrandId = product.BrandId,
                Brand = product.Brand,

                ReviewsCount = ReviewService.GetReviewsCount(product.ProductId, EntityType.Product),
                BarCode = product.BarCode,
                AccrueBonuses = product.AccrueBonuses,

                TaxId = product.TaxId,
            };

            var currency = model.Currencies.Find(x => x.Value == model.CurrencyId.ToString());
            if (currency != null)
                currency.Selected = true;

            model.Currency = CurrencyService.GetAllCurrencies(true).Find(x => x.CurrencyId == product.CurrencyID) ??
                             CurrencyService.CurrentCurrency;

            var meta = MetaInfoService.GetMetaInfo(product.ProductId, MetaType.Product);
            if (meta == null)
            {
                model.DefaultMeta = true;
            }
            else
            {
                model.DefaultMeta = false;
                model.SeoTitle = meta.Title;
                model.SeoH1 = meta.H1;
                model.SeoKeywords = meta.MetaKeywords;
                model.SeoDescription = meta.MetaDescription;
            }

            //var options = CustomOptionsService.GetCustomOptionsByProductId(product.ProductId);
            //model.HasCustomOptions = options != null && options.Count > 0;


            if (product.CategoryId != 0)
            {
                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

                model.BreadCrumbs =
                    CategoryService.GetParentCategories(product.CategoryId)
                        .Select(x => new BreadCrumbs(x.Name, urlHelper.Action("Index", "Catalog", new { categoryId = x.CategoryId })))
                        .Reverse()
                        .ToList();
            }
            model.BreadCrumbs.Insert(0, new BreadCrumbs(LocalizationService.GetResource("Admin.Catalog.Index.CatalogTitle"), "catalog"));

            // modules
            foreach (var module in AttachedModules.GetModules<IAdminProductTabs>())
            {
                var classInstance = (IAdminProductTabs)Activator.CreateInstance(module);
                model.TabModules.AddRange(classInstance.GetAdminProductTabs(model.ProductId));
            }

            // landing
            model.IsLandingEnabled = SettingsLandingPage.ActiveLandingPage;
            model.LandingProductDescription = ProductLandingPageService.GetDescriptionByProductId(model.ProductId, true);

            var path = HostingEnvironment.MapPath("~/landings/templates/");
            if (Directory.Exists(path))
                model.LandingLinks.AddRange(Directory.GetDirectories(path).Select(p => new DirectoryInfo(p).Name));

            model.ShowGoogleImageSearch = AttachedModules.GetModules<IPhotoSearcher>().Any(x=>x.Name== "GoogleImagesSearchModule");
            model.ShowBingImageSearch = AttachedModules.GetModules<IPhotoSearcher>().Any(x => x.Name== "BingImagesSearchModule");


            var modifiedDate = ProductService.GetModifiedDate(product.ProductId);
            if (modifiedDate != null)
                model.ModifiedDate = ((DateTime)modifiedDate).ToString("dd.MM.yy hh:mm");
            
            Guid modifiedById;
            if (!string.IsNullOrEmpty(product.ModifiedBy) && Guid.TryParse(product.ModifiedBy, out modifiedById))
            {
                var modifiedByCustomer = CustomerService.GetCustomer(modifiedById);
                if (modifiedByCustomer != null)
                    model.ModifiedBy = modifiedByCustomer.GetShortName();
            }
            else
            {
                model.ModifiedBy = product.ModifiedBy;
            }

            return model;
        }
    }
}
