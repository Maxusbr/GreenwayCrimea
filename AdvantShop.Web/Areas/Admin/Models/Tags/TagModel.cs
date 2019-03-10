using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.UrlRewriter;

namespace AdvantShop.Web.Admin.Models.Tags
{
    public partial class TagModel : IValidatableObject
    {
        public bool IsEditMode { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlPath { get; set; }
        public bool Enabled { get; set; }
        public bool VisibilityForUsers { get; set; }
        public int SortOrder { get; set; }
        public string Description { get; set; }
        public string BriefDescription { get; set; }
        public bool DefaultMeta { get; set; }
        public string SeoTitle { get; set; }
        public string SeoH1 { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoDescription { get; set; }
        

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Category.AdminCategoryModel.Error.Name"), new[] { "Name" });
            }

            if (string.IsNullOrEmpty(UrlPath))
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Category.AdminCategoryModel.Error.Url"), new[] { "Url" });
            }
            else
            {
                // if new or urlpath != previous urlpath
                if (Id == 0 || (UrlService.GetObjUrlFromDb(ParamType.Tag, Id) != UrlPath))
                {
                    var reg = new Regex("^[a-zA-Z0-9_-]*$");
                    if (!reg.IsMatch(UrlPath))
                    {
                        UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Tag, UrlPath);
                    }

                    if (!UrlService.IsAvailableUrl(ParamType.Tag, UrlPath))
                    {
                        UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Tag, UrlPath);
                    }
                }
            }
        }
    }
}
