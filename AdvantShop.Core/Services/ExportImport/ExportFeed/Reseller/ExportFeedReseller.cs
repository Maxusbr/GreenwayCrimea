
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
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Statistic;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.ExportImport
{
    [ExportFeedKey("Reseller")]
    public class ExportFeedReseller : BaseExportFeed
    {
        private readonly IEnumerable<ExportFeedProductModel> _products;

        private readonly ExportFeedResellerOptions _exportFeedResellerOptions;

        private readonly int _categoriesCount;
        private readonly int _productsCount;

        public ExportFeedReseller() { }

        public ExportFeedReseller(IEnumerable<ExportFeedCategories> categories, IEnumerable<ExportFeedProductModel> products, ExportFeedSettings options, int categoriesCount, int productsCount)
            : base(categories, products, options, categoriesCount, productsCount)
        {
            _products = products;

            _exportFeedResellerOptions = (ExportFeedResellerOptions)options;

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

                var fileName = _exportFeedResellerOptions.FileName + "." + _exportFeedResellerOptions.FileExtention;
                var filePath = SettingsGeneral.AbsolutePath + "/" + fileName;
                var directory = filePath.Substring(0, filePath.LastIndexOf('/'));


                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                FileHelpers.DeleteFile(filePath);

                CsvExport.Factory(_products, _exportFeedResellerOptions, _productsCount, _categoriesCount).Process().Wait();
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        public override string Export(int exportFeedId)
        {
            var commonSettings = ExportFeedSettingsProvider.GetSettings(exportFeedId);
            var advancedSettings = JsonConvert.DeserializeObject<ExportFeedResellerOptions>(commonSettings.AdvancedSettings);

            advancedSettings.CsvSeparator = ";";
            advancedSettings.CsvColumSeparator = ";";
            advancedSettings.CsvPropertySeparator = ":";
            advancedSettings.CsvEnconing = EncodingsEnum.Utf8.StrName();
            advancedSettings.CsvCategorySort = true;

            return Export(
                ExportFeedResellerService.GetCategories(exportFeedId),
                ExportFeedResellerService.GetProducts(exportFeedId, commonSettings, advancedSettings),
                commonSettings,
                ExportFeedResellerService.GetCategoriesCount(exportFeedId),
                ExportFeedResellerService.GetProductsCount(exportFeedId, commonSettings, advancedSettings));
        }

        public override string Export(IEnumerable<ExportFeedCategories> categories, IEnumerable<ExportFeedProductModel> products, ExportFeedSettings options, int categoriesCount, int productsCount)
        {

            try
            {
                var discountModule = AttachedModules.GetModules<IDiscount>().FirstOrDefault();
                if (discountModule != null)
                {
                    var classInstance = (IDiscount)Activator.CreateInstance(discountModule);
                    _productDiscountModels = classInstance.GetProductDiscountsList();
                }

                var exportFile = new FileInfo(options.FileFullPath);
                if (!string.IsNullOrEmpty(exportFile.Directory.FullName))
                {
                    FileHelpers.CreateDirectory(exportFile.Directory.FullName);
                }
                FileHelpers.DeleteFile(exportFile.FullName);

                CommonStatistic.FileName = GetDownloadableExportFeedFileLink(JsonConvert.DeserializeObject<ExportFeedResellerOptions>(options.AdvancedSettings));

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
            var resellerCode = Guid.NewGuid();

            ExportFeedSettingsProvider.SetSettings(exportFeedId,
                  new ExportFeedSettings()
                  {
                      Interval = 1,
                      IntervalType = Core.Scheduler.TimeIntervalType.Hours,
                      Active = false,

                      PriceMargin = 0,
                      FileName = "export/resellers/" + resellerCode,
                      FileExtention = "csv",

                      AdvancedSettings = JsonConvert.SerializeObject(
                          new ExportFeedResellerOptions()
                          {
                              ResellerCode = resellerCode.ToString(),

                              CsvSeparator = ";",
                              CsvColumSeparator = ";",
                              CsvPropertySeparator = ":",
                              CsvEnconing = EncodingsEnum.Utf8.StrName(),
                              CsvCategorySort = true,

                              FieldMapping = new List<ProductFields>(Enum.GetValues(typeof(ProductFields)).OfType<ProductFields>().Where(item => item != ProductFields.None).ToList()),
                          }),
                      AdditionalUrlTags = string.Empty
                  });

            ExportFeedService.InsertCategory(exportFeedId, 0);
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
            var advancedSettings = JsonConvert.DeserializeObject<ExportFeedResellerOptions>(commonSettings.AdvancedSettings);

            return ExportFeedResellerService.GetProductsCount(exportFeedId, commonSettings, advancedSettings);
        }

        public override int GetCategoriesCount(int exportFeedId)
        {
            return ExportFeedResellerService.GetCategoriesCount(exportFeedId);
        }

        public override List<string> GetAvailableFileExtentions()
        {
            return new List<string> { "csv" };
        }

        public override string GetDownloadableExportFeedFileLink(int exportFeedId)
        {
            var commonSettings = ExportFeedSettingsProvider.GetSettings(exportFeedId);
            var advancedSettings = JsonConvert.DeserializeObject<ExportFeedResellerOptions>(commonSettings.AdvancedSettings);

            return GetDownloadableExportFeedFileLink(advancedSettings);
        }

        private string GetDownloadableExportFeedFileLink(ExportFeedResellerOptions advancedSettings)
        {
            return SettingsMain.SiteUrl + "/api/resellers/catalog?id=" + advancedSettings.ResellerCode;
        }
    }
}