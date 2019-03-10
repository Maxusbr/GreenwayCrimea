using System.IO;
using System.Collections.Generic;

using AdvantShop.ExportImport;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Statistic;
using AdvantShop.Catalog;

namespace AdvantShop.Web.Admin.Handlers.Import
{
    public class StartImportCategoriesHandler
    {
        private readonly string _inputFilePath;
        private readonly string _columnSeparator;        
        private readonly string _encoding;
        private readonly bool _haveHeader;        

        private readonly Dictionary<string, int> _fieldMapping;


        public StartImportCategoriesHandler(List<string> selectedCategoryFields, string inputFilePath, string columnSeparator,  string encoding, bool haveHeader)
        {
            _inputFilePath = inputFilePath;

            _columnSeparator = columnSeparator;            
            _encoding = encoding;
            _haveHeader = haveHeader;
            
            _fieldMapping = new Dictionary<string, int>();
            for (var index = 0; index < selectedCategoryFields.Count; index++)
            {
                _fieldMapping.Add(selectedCategoryFields[index], index);
            }
        }

        public object Execute()
        {
            if (!_fieldMapping.ContainsKey(ProductFields.Sku.StrName()) && !_fieldMapping.ContainsKey(ProductFields.Name.StrName()))
            {
                return new { Result = false, error = "Resource.Admin_ImportCsv_SelectNameOrSKU" };
            }
            
            if (!File.Exists(_inputFilePath))
            {
                return new { Result = false, error = "Файл не найден" };
            }

            if (CommonStatistic.IsRun)
            {
                return new { };
            }

            CommonStatistic.Init();
            CommonStatistic.CurrentProcess = "import/importCategories";
            CommonStatistic.CurrentProcessName = "Загрузка категорий";
            
            CommonStatistic.IsRun = true;
            var importCategories = new CsvImportCategories(_inputFilePath, _haveHeader, _columnSeparator, _encoding, _fieldMapping);
            importCategories.Process();

            CategoryService.SetCategoryHierarchicallyEnabled(0);
            CategoryService.RecalculateProductsCountManual();            

            return new { Result = true };
        }
    }
}
