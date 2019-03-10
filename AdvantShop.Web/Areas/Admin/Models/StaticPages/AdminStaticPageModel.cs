using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.CMS;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.UrlRewriter;
using System;

namespace AdvantShop.Web.Admin.Models.StaticPages
{
    public class AdminStaticPageModel : IValidatableObject
    {
        public bool IsEditMode { get; set; }

        public int StaticPageId { get; set; }

        public string PageName { get; set; }

        public string PageText { get; set; }

        public int SortOrder { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime ModifyDate { get; set; }

        public bool IndexAtSiteMap { get; set; }

        public bool Enabled { get; set; }

        private bool? _hasChildren;

        public bool HasChildren
        {
            get { return _hasChildren ?? (bool)(_hasChildren = (bool?)StaticPageService.CheckChilds(StaticPageId)); }
            set { _hasChildren = value; }
        }
        public string UrlPath { get; set; }

        public int ParentId { get; set; }

        public string ParentPageName { get; set; }
        

        public bool DefaultMeta { get; set; }
        public string SeoTitle { get; set; }
        public string SeoH1 { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoDescription { get; set; }
        
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(PageName))
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Category.AdminStaticPageModel.Error.Name"), new[] { "Name" });
            }

            if (string.IsNullOrEmpty(UrlPath))
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Category.AdminStaticPageModel.Error.Url"), new[] { "Url" });
            }
            else
            {
                if (StaticPageId == 0 || (UrlService.GetObjUrlFromDb(ParamType.StaticPage, StaticPageId) != UrlPath))
                {
                    if (!UrlService.IsValidUrl(UrlPath, ParamType.StaticPage))
                    {
                        UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.StaticPage, UrlPath);
                    }
                }
            }
        }
    }
}
