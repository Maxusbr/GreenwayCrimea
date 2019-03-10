//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;

using Newtonsoft.Json;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Statistic;

namespace AdvantShop.ExportImport
{
    [ExportFeedKey("Csv")]
    public class ExportFeedCsv : BaseExportFeed
    {
        private readonly IEnumerable<ExportFeedProductModel> _products;
        private readonly ExportFeedCsvOptions _exportFeedCsvOptions;
        private readonly int _categoriesCount;
        private readonly int _productsCount;

        public ExportFeedCsv() { }

        public ExportFeedCsv(IEnumerable<ExportFeedCategories> categories, IEnumerable<ExportFeedProductModel> products, ExportFeedSettings options, int categoriesCount, int productsCount)
            : base(categories, products, options, categoriesCount, productsCount)
        {
            _products = products;
            _exportFeedCsvOptions = (ExportFeedCsvOptions)options;
            _productsCount = productsCount;
            _categoriesCount = categoriesCount;
        }

        private List<ProductDiscount> _productDiscountModels = null;

        [Obsolete("This method is obsolete. Use Export method", false)]
        public override void Build()
        {
            try
            {
                var discountModule = AttachedModules.GetModules<IDiscount>().FirstOrDefault();
                if (discountModule != null)
                {
                    var classInstance = (IDiscount)Activator.CreateInstance(discountModule);
                    _productDiscountModels = classInstance.GetProductDiscountsList();
                }

                var fileName = _exportFeedCsvOptions.FileName + "." + _exportFeedCsvOptions.FileExtention;
                var filePath = SettingsGeneral.AbsolutePath + "/" + fileName;
                var directory = filePath.Substring(0, filePath.LastIndexOf('/'));


                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                FileHelpers.DeleteFile(filePath);
                CsvExport.Factory(_products, _exportFeedCsvOptions, _productsCount, _categoriesCount).Process().Wait();

            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }


        public override string Export(int exportFeedId)
        {
            var commonSettings = ExportFeedSettingsProvider.GetSettings(exportFeedId);
            var advancedSettings = JsonConvert.DeserializeObject<ExportFeedCsvOptions>(commonSettings.AdvancedSettings);

            return Export(
                ExportFeedCsvService.GetCategories(exportFeedId),
                ExportFeedCsvService.GetProducts(exportFeedId, commonSettings, advancedSettings),
                commonSettings,
                ExportFeedCsvService.GetCategoriesCount(exportFeedId),
                ExportFeedCsvService.GetProductsCount(exportFeedId, commonSettings, advancedSettings));

        }

        public override string Export(IEnumerable<ExportFeedCategories> categories, IEnumerable<ExportFeedProductModel> products, ExportFeedSettings options, int categoriesCount, int productsCount)
        {
            var advancedSettings = JsonConvert.DeserializeObject<ExportFeedCsvOptions>(options.AdvancedSettings);

            try
            {
                var discountModule = AttachedModules.GetModules<IDiscount>().FirstOrDefault();
                if (discountModule != null)
                {
                    var classInstance = (IDiscount)Activator.CreateInstance(discountModule);
                    _productDiscountModels = classInstance.GetProductDiscountsList();
                }

                //var fileName = options.FileFullName;
                var exportFile = new FileInfo(options.FileFullPath);
                if (!string.IsNullOrEmpty(exportFile.Directory.FullName))
                {
                    FileHelpers.CreateDirectory(exportFile.Directory.FullName);
                }
                FileHelpers.DeleteFile(exportFile.FullName);
                
                CommonStatistic.FileName = "../" + options.FileFullName;
                
                CsvExport.Factory(products, options, productsCount, categoriesCount).Process().Wait();
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return options.FileFullName;
        }

        public override void SetDefaultSettings(int exportFeedId)
        {
            ExportFeedSettingsProvider.SetSettings(exportFeedId,
                  new ExportFeedSettings()
                  {
                      Interval = 1,
                      IntervalType = Core.Scheduler.TimeIntervalType.Hours,
                      Active = false,

                      PriceMargin = 0,
                      FileName = File.Exists(SettingsGeneral.AbsolutePath + "/export/catalog.csv") ? "export/catalog" + exportFeedId : "catalog",
                      FileExtention = "csv",
                      
                      AdvancedSettings = JsonConvert.SerializeObject(
                          new ExportFeedCsvOptions()
                          {
                              CsvSeparator = ";",
                              CsvColumSeparator = ";",
                              CsvPropertySeparator = ":",
                              CsvEnconing = EncodingsEnum.Utf8.StrName(),

                              FieldMapping = new List<ProductFields>(Enum.GetValues(typeof(ProductFields)).OfType<ProductFields>().Where(item => item != ProductFields.None && item != ProductFields.Sorting).ToList()),
                          }),
                      AdditionalUrlTags = string.Empty
                  });
        }

        public override List<string> GetAvailableVariables()
        {
            return new List<string> { "#STORE_NAME#", "#STORE_URL#", "#PRODUCT_NAME#", "#PRODUCT_ID#", "#PRODUCT_ARTNO#" };
        }

        public override List<ExportFeedCategories> GetCategories(int exportFeedId)
        {
            throw new NotImplementedException();
        }

        public override List<ExportFeedProductModel> GetProducts(int exportFeedId)
        {
            throw new NotImplementedException();
        }

        public override int GetProductsCount(int exportFeedId)
        {
            var commonSettings = ExportFeedSettingsProvider.GetSettings(exportFeedId);
            var advancedSettings = JsonConvert.DeserializeObject<ExportFeedCsvOptions>(commonSettings.AdvancedSettings);

            return ExportFeedCsvService.GetProductsCount(exportFeedId,commonSettings,advancedSettings);
        }

        public override int GetCategoriesCount(int exportFeedId)
        {
            return ExportFeedCsvService.GetCategoriesCount(exportFeedId);
        }

        public override List<string> GetAvailableFileExtentions()
        {
            return new List<string> { "csv", "txt" };
        }
    }
}