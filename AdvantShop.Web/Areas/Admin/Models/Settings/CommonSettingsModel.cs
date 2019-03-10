using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Web.Admin.Models.Settings
{
    public class CommonSettingsModel : IValidatableObject
    {
        // Common settings
        public string StoreUrl { get; set; }
        public string StoreName { get; set; }
        public string LogoImgSrc { get; set; }
        public string LogoImgAlt { get; set; }
        public string FaviconSrc { get; set; }

        public int CountryId { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public int RegionId { get; set; }
        public List<SelectListItem> Regions { get; set; }
        public bool HasRegions { get; set; }
        
        public string City { get; set; }

        public string Phone { get; set; }
        

        // Sales plan settings
        public float SalesPlan { get; set; }
        public float ProfitPlan { get; set; }

        public bool IsTrial { get; set; }

        // Validation
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StoreName.IsNullOrEmpty())
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Settings.Index.Error.ShopName"), new[] { "ShopName" });
            }

            if (!Trial.TrialService.IsTrialEnabled && StoreUrl.IsNullOrEmpty())
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Settings.Index.Error.SiteUrl"), new[] { "SiteUrl" });
            }
        }
    }
}
