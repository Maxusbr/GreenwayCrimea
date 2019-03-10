using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.FilePath;
using AdvantShop.Repository.Currencies;
using AdvantShop.Saas;
using AdvantShop.Taxes;

namespace AdvantShop.Web.Admin.Models.Products
{
    public class AdminProductModel : IValidatableObject
    {
        public AdminProductModel()
        {
            BreadCrumbs = new List<BreadCrumbs>();

            var allCurrencies = CurrencyService.GetAllCurrencies(true);
            Currencies =
                allCurrencies
                    .Select(x => new SelectListItem() { Text = x.Name, Value = x.CurrencyId.ToString() })
                    .ToList();

            TabModules = new List<AdminProductTabItem>();
            LandingLinks = new List<string>();

            Taxes =
                TaxService.GetTaxes().Where(x => x.Enabled)
                    .Select(x => new SelectListItem() { Text = x.Name, Value = x.TaxId.ToString() })
                    .ToList();
        }

        public List<BreadCrumbs> BreadCrumbs { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public List<SelectListItem> Currencies { get; set; }


        public int ProductId { get; set; }
        public string ArtNo { get; set; }
        public string Name { get; set; }
        public string UrlPath { get; set; }
        public bool Enabled { get; set; }

        public string BriefDescription { get; set; }
        public string Description { get; set; }

        public bool Recomended { get; set; }
        public bool New { get; set; }
        public bool BestSeller { get; set; }
        public bool Sales { get; set; }


        public float Weight { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }


        public string Photo { get; set; }
        public string PhotoSrc()
        {
            return FoldersHelper.GetImageProductPath(ProductImageType.Small, Photo, false);
        }

        public float DiscountPercent { get; set; }
        public float DiscountAmount { get; set; }
        public DiscountType DiscountType { get; set; }


        public bool AllowPreOrder { get; set; }

        public string Unit { get; set; }
        public float? ShippingPrice { get; set; }

        public float? MinAmount { get; set; }
        public float? MaxAmount { get; set; }
        public float Multiplicity { get; set; }


        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public bool DefaultMeta { get; set; }
        public string SeoTitle { get; set; }
        public string SeoH1 { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoDescription { get; set; }


        public int ReviewsCount { get; set; }
        public List<string> Tags { get; set; }

        public bool IsTagsVisible
        {
            get { return !SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveTags; }
        }

        public bool HasCustomOptions { get; set; }

        public List<AdminProductTabItem> TabModules { get; set; }


        public bool IsLandingEnabled { get; set; }
        public List<string> LandingLinks { get; set; }
        public string LandingProductDescription { get; set; }

        public bool ShowGoogleImageSearch { get; set; }
        public bool ShowBingImageSearch { get; set; }

        public string BarCode { get; set; }

        public bool AccrueBonuses { get; set; }

        public int? TaxId { get; set; }
        public List<SelectListItem> Taxes { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Category.AdminCategoryModel.Error.Name"), new[] { "Name" });
            }

            if (string.IsNullOrWhiteSpace(ArtNo))
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Category.AdminCategoryModel.Error.ArtNo"), new[] { "ArtNo" });
            }
            else
            {
                var tempId = ProductService.GetProductId(ArtNo);
                if (tempId != 0 && tempId != ProductId)
                {
                    yield return new ValidationResult(LocalizationService.GetResource("Admin.Category.AdminCategoryModel.Error.ArtNoDublicate"), new[] { "ArtNo" });
                }
            }

            if (Multiplicity <= 0)
            {
                Multiplicity = 1;
            }

            if (string.IsNullOrEmpty(UrlPath))
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Category.AdminCategoryModel.Error.Url"), new[] { "Url" });
            }
            else
            {
                // if new product or urlpath != previous urlpath
                if (ProductId == 0 || (UrlService.GetObjUrlFromDb(ParamType.Product, ProductId) != UrlPath))
                {
                    if (!UrlService.IsValidUrl(UrlPath, ParamType.Product))
                    {
                        UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Product, UrlPath);
                    }
                }
            }
        }
    }
}
