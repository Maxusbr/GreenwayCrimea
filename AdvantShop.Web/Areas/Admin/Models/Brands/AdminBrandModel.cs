using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Repository;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.FilePath;

namespace AdvantShop.Web.Admin.Models.Brands
{
    public class AdminBrandModel : IValidatableObject
    {
        public AdminBrandModel()
        {
            Picture = new BrandPhoto();

            _countries = new List<Country>() {new Country() {Name = "Все страны"}};
            _countries.AddRange(CountryService.GetAllCountries());
        }

        public bool IsEditMode { get; set; }

        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string Description { get; set; }
        public string BriefDescription { get; set; }

        public bool Enabled { get; set; }
        public int SortOrder { get; set; }
        public string BrandSiteUrl { get; set; }

        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public string UrlPath { get; set; }

        public BrandPhoto Picture { get; set; }

        public int PhotoId { get; set; }
        public string PhotoName { get; set; }
        public string PhotoSrc
        {
            get { return !string.IsNullOrEmpty(PhotoName) ? FoldersHelper.GetPath(FolderType.BrandLogo, PhotoName, true) : "../images/nophoto_small.jpg"; }
        }

        public bool DefaultMeta { get; set; }
        public string SeoTitle { get; set; }
        public string SeoH1 { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoDescription { get; set; }

        private List<Country> _countries; 
        public List<Country> Countries
        {
            get { return _countries; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(BrandName))
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Category.AdminCategoryModel.Error.Name"), new[] { "Name" });
            }

            if (string.IsNullOrEmpty(UrlPath))
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Category.AdminCategoryModel.Error.Url"), new[] { "Url" });
            }
            else
            {
                // if new category or urlpath != previous urlpath
                if (BrandId == 0 || (UrlService.GetObjUrlFromDb(ParamType.Brand, BrandId) != UrlPath))
                {
                    if (!UrlService.IsValidUrl(UrlPath, ParamType.Brand))
                    {
                        UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Brand, UrlPath);
                    }
                }
            }
        }
    }
}
