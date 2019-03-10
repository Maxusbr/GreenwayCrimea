using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Saas;
using AdvantShop.CMS;

namespace AdvantShop.Web.Admin.Models.Categories
{
    public class AdminCategoryModel : IValidatableObject
    {
        public AdminCategoryModel()
        {
            DisplayStyles = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = ECategoryDisplayStyle.Tile.Localize(),
                    Value = ECategoryDisplayStyle.Tile.ToString()
                },
                new SelectListItem()
                {
                    Text = ECategoryDisplayStyle.List.Localize(),
                    Value = ECategoryDisplayStyle.List.ToString()
                },
                new SelectListItem()
                {
                    Text = ECategoryDisplayStyle.None.Localize(),
                    Value = ECategoryDisplayStyle.None.ToString()
                },
            };

            Sortings = new List<SelectListItem>();
            foreach (ESortOrder item in Enum.GetValues(typeof (ESortOrder)))
            {
                Sortings.Add(new SelectListItem() { Text = item.Localize(), Value = ((int)item).ToString() });
            }

            Picture = new CategoryPhoto();
            MiniPicture = new CategoryPhoto();
            Icon = new CategoryPhoto();
            BreadCrumbs = new List<BreadCrumbs>();
        }

        public bool IsEditMode { get; set; }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string UrlPath { get; set; }
        public string ParentCategoryName { get; set; }
        public int ParentCategoryId { get; set; }
        public int SortOrder { get; set; }
        public string Description { get; set; }
        public string BriefDescription { get; set; }

        public ECategoryDisplayStyle DisplayStyle { get; set; }
        public List<SelectListItem> DisplayStyles { get; set; }
        public ESortOrder Sorting { get; set; }
        public List<SelectListItem> Sortings { get; set; }

        public List<BreadCrumbs> BreadCrumbs { get; set; }

        public bool DisplayBrandsInMenu { get; set; }
        public bool DisplaySubCategoriesInMenu { get; set; }
        public bool Hidden { get; set; }
        public bool Enabled { get; set; }
        public List<string> Tags { get; set; }


        public CategoryPhoto Picture { get; set; }
        public int PictureId { get; set; }
        
        public CategoryPhoto Icon { get; set; }
        public int IconId { get; set; }

        public CategoryPhoto MiniPicture { get; set; }
        public int MiniPictureId { get; set; }


        public bool DefaultMeta { get; set; }
        public string SeoTitle { get; set; }
        public string SeoH1 { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoDescription { get; set; }

        public bool IsTagsVisible
        {
            get { return !SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveTags; }
        }

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
                // if new category or urlpath != previous urlpath
                if (CategoryId == 0 || (UrlService.GetObjUrlFromDb(ParamType.Category, CategoryId) != UrlPath))
                {
                    if (!UrlService.IsValidUrl(UrlPath, ParamType.Category))
                    {
                        UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Category, UrlPath);
                    }
                }
            }
        }
    }
}
