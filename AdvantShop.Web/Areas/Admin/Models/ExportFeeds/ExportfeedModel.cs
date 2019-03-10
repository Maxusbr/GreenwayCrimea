using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Services.Localization;
using AdvantShop.ExportImport;

namespace AdvantShop.Web.Admin.Models.ExportFeeds
{
    public class ExportFeedModel : IValidatableObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public EExportFeedType Type { get; set; }
        
        public string Description { get; set; }

        public DateTime? LastExport { get; set; }

        public string LastExportFileFullName { get; set; }

        public bool ExportAllProducts { get; set; }

        public ExportFeedSettingsModel ExportFeedSettings { get; set; }
        
        public bool HaveProducts { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Category.AdminCategoryModel.Error.Name"), new[] { "Name" });
            }            
        }
    }
}
