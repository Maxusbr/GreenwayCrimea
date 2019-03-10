using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using AdvantShop.Saas;
using AdvantShop.ExportImport;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Statistic;
using AdvantShop.Trial;
using AdvantShop.Catalog;

namespace AdvantShop.Web.Admin.Handlers.Import
{
    public class StartImportProductsHandler
    {
        private readonly string _inputFilePath;

        private readonly string _columnSeparator;
        private readonly string _propertySeparator;
        private readonly string _propertyValueSeparator;
        private readonly string _encoding;
        private readonly bool _haveHeader;
        private readonly bool _disableProducts;

        private readonly Dictionary<string, int> _fieldMapping;


        public StartImportProductsHandler(List<string> selectedProductFields, string inputFilePath, string columnSeparator, string propertySeparator, string propertyValueSeparator, string encoding, bool haveHeader, bool disableProducts)
        {
            _inputFilePath = inputFilePath;

            _columnSeparator = columnSeparator;
            _propertySeparator = propertySeparator;
            _propertyValueSeparator = propertyValueSeparator;
            _encoding = encoding;
            _haveHeader = haveHeader;
            _disableProducts = disableProducts;

            _fieldMapping = new Dictionary<string, int>();
            for (var index = 0; index < selectedProductFields.Count; index++)
            {
                if (!_fieldMapping.ContainsKey(selectedProductFields[index]))
                    _fieldMapping.Add(selectedProductFields[index], index);
            }
        }

        public object Execute()
        {
            if (!_fieldMapping.ContainsKey(ProductFields.Sku.StrName()) && !_fieldMapping.ContainsKey(ProductFields.Name.StrName()))
            {
                return new { Result = false, error = "Resource.Admin_ImportCsv_SelectNameOrSKU" };
            }

            if (SaasDataService.IsSaasEnabled)
            {
                //divSaasPlanProducts.Visible = true;
                //hfProductsCount.Value = AdvantShop.Catalog.ProductService.GetProductsCount().ToString();
                //lTotalSaasPlanProducts.Text = SaasDataService.CurrentSaasData.ProductsCount.ToString();
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
            CommonStatistic.CurrentProcess = "import/importProducts";
            CommonStatistic.CurrentProcessName = "Загрузка каталога товаров";


            CommonStatistic.IsRun = true;
            CsvImport.Factory(_inputFilePath, _haveHeader, _disableProducts, _columnSeparator, _encoding, _fieldMapping, _propertySeparator, _propertyValueSeparator).Process();

            TrialService.TrackEvent(TrialEvents.MakeCSVImport, "");
            TrialService.TrackEvent(ETrackEvent.Trial_ImportCSV);

            CategoryService.SetCategoryHierarchicallyEnabled(0);
            CategoryService.RecalculateProductsCountManual();
            ProductService.PreCalcProductParamsMass();

            return new { Result = true };
        }
    }
}
