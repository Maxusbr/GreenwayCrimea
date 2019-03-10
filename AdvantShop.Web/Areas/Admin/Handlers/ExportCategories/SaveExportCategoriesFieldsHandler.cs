using System;
using System.Collections.Generic;
using AdvantShop.ExportImport;

namespace AdvantShop.Web.Admin.Handlers.ExportCategories
{
    public class SaveExportCategoriesFieldsHandler
    {

        private readonly List<string> _exportCategoriesFields;
        private readonly string _separator;
        private readonly string _encoding;

        public SaveExportCategoriesFieldsHandler(string separator, string encoding, List<string> exportCategoriesFields)
        {
            _encoding = encoding;
            _separator = separator;
            _exportCategoriesFields = exportCategoriesFields;
        }

        public bool Execute()
        {
            if (_exportCategoriesFields == null)
            {
                return false;
            }

            var fieldMapping = new List<CategoryFields>();
            foreach (var field in _exportCategoriesFields)
            {
                CategoryFields currentField;
                if (Enum.TryParse(field, out currentField))
                {
                    if (!fieldMapping.Contains(currentField) && currentField != CategoryFields.None)
                    {
                        fieldMapping.Add(currentField);
                    }
                }
            }

            if (fieldMapping.Count == 0 || string.IsNullOrEmpty(_encoding) || string.IsNullOrEmpty(_separator))
            {
                return false;
            }

            ExportFeedCsvCategorySettings.CsvEnconing = _encoding;
            ExportFeedCsvCategorySettings.CsvSeparator = _separator;
            ExportFeedCsvCategorySettings.FieldMapping = fieldMapping;

            return true;
        }
    }
}
