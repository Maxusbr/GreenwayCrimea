using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AdvantShop.Core.Services.Localization;
using AdvantShop.ExportImport;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Web.Admin.Models.ExportFeeds
{
    public class ExportFeedSettingsCsvModel : IValidatableObject
    {
        public ExportFeedSettingsCsvModel(ExportFeedCsvOptions exportFeedCsvOptions)
        {
            CsvEnconing = exportFeedCsvOptions.CsvEnconing;
            CsvSeparator = exportFeedCsvOptions.CsvSeparator;
            CsvColumSeparator = exportFeedCsvOptions.CsvColumSeparator;
            CsvPropertySeparator = exportFeedCsvOptions.CsvPropertySeparator;
            CsvExportNoInCategory = exportFeedCsvOptions.CsvExportNoInCategory;
            CsvCategorySort = exportFeedCsvOptions.CsvCategorySort;
            AllOffersToMultiOfferColumn = exportFeedCsvOptions.AllOffersToMultiOfferColumn;
            FieldMapping = exportFeedCsvOptions.FieldMapping;
            ModuleFieldMapping = exportFeedCsvOptions.ModuleFieldMapping;
        }

        public string CsvEnconing { get; set; }
        public string CsvSeparator { get; set; }
        public string CsvColumSeparator { get; set; }
        public string CsvPropertySeparator { get; set; }
        public bool CsvExportNoInCategory { get; set; }
        public bool CsvCategorySort { get; set; }
        public bool AllOffersToMultiOfferColumn { get; set; }
        public List<ProductFields> FieldMapping { get; set; }
        public List<CSVField> ModuleFieldMapping { get; set; }

        public Dictionary<string, string> CsvSeparatorList
        {
            get
            {
                var csvSeparatorList = new Dictionary<string, string>();
                foreach (SeparatorsEnum csvSeparator in Enum.GetValues(typeof(SeparatorsEnum)))
                {
                    csvSeparatorList.Add(csvSeparator.StrName(), csvSeparator.Localize());
                }
                return csvSeparatorList;
            }
        }
        public Dictionary<string, string> CsvEnconingList
        {
            get
            {
                var csvEnconingList = new Dictionary<string, string>();
                foreach (EncodingsEnum csvEnconing in Enum.GetValues(typeof(EncodingsEnum)))
                {
                    csvEnconingList.Add(csvEnconing.StrName(), csvEnconing.StrName());
                }
                return csvEnconingList;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(CsvColumSeparator))
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Category.AdminCategoryModel.Error.Name"), new[] { "CsvColumSeparator" });
            }
            if (string.IsNullOrEmpty(CsvPropertySeparator))
            {
                yield return new ValidationResult(LocalizationService.GetResource("Admin.Category.AdminCategoryModel.Error.Name"), new[] { "CsvPropertySeparator" });
            }
        }
    }
}
