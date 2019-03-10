using System;
using System.Linq;
using System.Collections.Generic;

using AdvantShop.ExportImport;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Web.Admin.Models.ExportCategories;
using Newtonsoft.Json;

namespace AdvantShop.Web.Admin.Handlers.ExportCategories
{
    public class GetExportCategoriesHandler
    {
        public ExportCategoriesModel Execute()
        {
            var csvEnconingList = new Dictionary<string, string>();

            foreach (EncodingsEnum enumItem in Enum.GetValues(typeof(EncodingsEnum)))
            {
                csvEnconingList.Add(enumItem.ToString(), enumItem.StrName());
            }

            var csvSeparatorList = new Dictionary<string, string>();
            foreach (var enumItem in (SeparatorsEnum[])Enum.GetValues(typeof(SeparatorsEnum)))
            {
                csvSeparatorList.Add(enumItem.Localize(), enumItem.StrName());
            }

            var model = new ExportCategoriesModel
            {
                CsvEncoding = ExportFeedCsvCategorySettings.CsvEnconing ?? EncodingsEnum.Utf8.StrName(),
                CsvSeparator = string.IsNullOrEmpty(ExportFeedCsvCategorySettings.CsvSeparator) ? SeparatorsEnum.SemicolonSeparated.StrName() : ExportFeedCsvCategorySettings.CsvSeparator,
                FieldMapping = ExportFeedCsvCategorySettings.FieldMapping ?? new List<CategoryFields>(),

                AllFields = GetAllFields(),
                DefaultExportFields = JsonConvert.SerializeObject(Enum.GetNames(typeof(CategoryFields)).Where(item => item != CategoryFields.None.ToString()).ToList()),
                CsvSeparatorList = csvSeparatorList,
                CsvEnconingList = csvEnconingList
            };

            return model;
        }

        private Dictionary<string, string> GetAllFields()
        {
            var result = new Dictionary<string, string>();

            foreach (CategoryFields item in Enum.GetValues(typeof(CategoryFields)))
            {
                result.Add(item.ToString(), item.Localize());
            }
            return result;
        }
    }
}
