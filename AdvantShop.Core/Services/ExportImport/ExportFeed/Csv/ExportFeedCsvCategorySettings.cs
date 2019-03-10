using System.Collections.Generic;
using AdvantShop.Configuration;
using Newtonsoft.Json;

namespace AdvantShop.ExportImport
{
    public class ExportFeedCsvCategorySettings
    {
        public static string CsvEnconing
        {
            get { return SettingProvider.Items["ExportFeedCsvCategorySettings_CsvEnconing"]; }
            set { SettingProvider.Items["ExportFeedCsvCategorySettings_CsvEnconing"] = value; }
        }

        public static string CsvSeparator
        {
            get { return SettingProvider.Items["ExportFeedCsvCategorySettings_CsvSeparator"]; }
            set { SettingProvider.Items["ExportFeedCsvCategorySettings_CsvSeparator"] = value; }
        }

        public static List<CategoryFields> FieldMapping
        {
            get
            {
                var data = SettingProvider.Items["ExportFeedCsvCategorySettings_FieldMapping"] ?? string.Empty;
                return JsonConvert.DeserializeObject<List<CategoryFields>>(data) ?? new List<CategoryFields>();
            }
            set
            {
                SettingProvider.Items["ExportFeedCsvCategorySettings_FieldMapping"] = JsonConvert.SerializeObject(value);
            }
        }
    }

    public class ImportCsvCategorySettings
    {
        public static string CsvEnconing
        {
            get { return SettingProvider.Items["ImportCsvCategorySettings_CsvEnconing"]; }
            set { SettingProvider.Items["ImportCsvCategorySettings_CsvEnconing"] = value; }
        }

        public static string CsvSeparator
        {
            get { return SettingProvider.Items["ImportCsvCategorySettings_CsvSeparator"]; }
            set { SettingProvider.Items["ExportFeedCsvCategorySettings_CsvSeparator"] = value; }
        }
    }
}