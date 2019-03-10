using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.ExportImport;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Import;
using AdvantShop.Web.Admin.ViewModels.Import;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using System.IO;
using AdvantShop.Statistic;

namespace AdvantShop.Web.Admin.Controllers.Catalog
{

    public partial class ImportController : BaseAdminController
    {
        private readonly string _csvProductsFileName = "importCSV.csv";
        private readonly string _csvCategoriesFileName = "importCsvCategories.csv";
        private readonly string _csvCustomersFileName = "importCsvCustomers.csv";

        #region Import catalog actions

        [Auth(RoleAction.Catalog)]
        public ActionResult ImportProducts()
        {
            SetMetaInformation("Импорт товаров");
            SetNgController(NgControllers.NgControllersTypes.ImportCtrl);

            var encodings = new Dictionary<string, string>();
            foreach (var enumItem in (EncodingsEnum[])Enum.GetValues(typeof(EncodingsEnum)))
            {
                encodings.Add(enumItem.StrName(), enumItem.StrName());
            }

            var separators = new Dictionary<string, string>();
            foreach (var enumItem in (SeparatorsEnum[])Enum.GetValues(typeof(SeparatorsEnum)))
            {
                separators.Add(enumItem.StrName(), enumItem.Localize());
            }

            var model = new ImportProductsModel
            {
                HaveHeader = true,
                DisableProducts = false,
                PropertySeparator = ";",
                PropertyValueSeparator = ":",
                ColumnSeparator = SeparatorsEnum.SemicolonSeparated.StrName(),
                Encoding = EncodingsEnum.Windows1251.StrName(),
                Encodings = encodings,
                ColumnSeparators = separators,
                CurrentSaasData = SaasDataService.CurrentSaasData,
                IsStartExport = CommonStatistic.IsRun && CommonStatistic.CurrentProcess.Equals("import/importProducts")
            };

            return View(model);
        }

        [Auth(RoleAction.Catalog)]
        public ActionResult ImportCategories()
        {
            SetMetaInformation("Импорт категорий");
            SetNgController(NgControllers.NgControllersTypes.ImportCtrl);

            var encodings = new Dictionary<string, string>();
            foreach (var enumItem in (EncodingsEnum[])Enum.GetValues(typeof(EncodingsEnum)))
            {
                encodings.Add(enumItem.StrName(), enumItem.StrName());
            }

            var separators = new Dictionary<string, string>();
            foreach (var enumItem in (SeparatorsEnum[])Enum.GetValues(typeof(SeparatorsEnum)))
            {
                separators.Add(enumItem.StrName(), enumItem.Localize());
            }

            var model = new ImportCategoriesModel
            {
                HaveHeader = true,

                Encoding = EncodingsEnum.Windows1251.StrName(),
                Encodings = encodings,

                ColumnSeparator = SeparatorsEnum.SemicolonSeparated.StrName(),
                ColumnSeparators = separators,

                CurrentSaasData = SaasDataService.CurrentSaasData
            };

            return View(model);
        }

        [Auth(RoleAction.Catalog)]
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult StartProductsImport(List<string> selectedProductFields, string columnSeparator, string propertySeparator, string propertyValueSeparator, string encoding, bool haveHeader, bool disableProducts)
        {
            var inputFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp) + _csvProductsFileName;
            var result = new StartImportProductsHandler(selectedProductFields, inputFilePath, columnSeparator, propertySeparator, propertyValueSeparator, encoding, haveHeader, disableProducts).Execute();
            return Json(result);
        }

        [Auth(RoleAction.Catalog)]
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult StartCategoriesImport(List<string> selectedCategoryFields, string columnSeparator, string encoding, bool haveHeader)
        {
            var inputFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp) + _csvCategoriesFileName;
            var result = new StartImportCategoriesHandler(selectedCategoryFields, inputFilePath, columnSeparator, encoding, haveHeader).Execute();
            return Json(result);
        }

        [Auth(RoleAction.Catalog)]
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadCsvFile(HttpPostedFileBase file)
        {
            var filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            FileHelpers.CreateDirectory(filePath);
            var outputFilePath = filePath + _csvProductsFileName;

            var result = new UploadCsvFileHandler(file, outputFilePath).Execute();
            return Json(result);
        }

        [Auth(RoleAction.Catalog)]
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadCsvCategoriesFile(HttpPostedFileBase file)
        {
            var filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            FileHelpers.CreateDirectory(filePath);
            var outputFilePath = filePath + _csvCategoriesFileName;

            var result = new UploadCsvFileHandler(file, outputFilePath).Execute();
            return Json(result);
        }

        [Auth(RoleAction.Catalog)]
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadZipFile(HttpPostedFileBase file)
        {
            var result = new UploadImagesArchiveFileHandler(file).Execute();
            return Json(result);
        }

        [Auth(RoleAction.Catalog)]
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteZipFile()
        {
            var result = new DeleteImagesArchiveFileHandler().Execute();
            return Json(result);
        }

        [Auth(RoleAction.Catalog)]
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteCsvFile(HttpPostedFileBase file)
        {
            return Json(new { result = true });
        }

        [Auth(RoleAction.Catalog)]
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadCsvFileByLink()
        {
            return Json(new { result = true });
        }

        [Auth(RoleAction.Catalog)]
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult GetFiledsFromCsvFile(string columnSeparator, string propertySeparator, string propertyValueSeparator, string encoding, bool haveHeader)
        {
            var outputFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp) + _csvProductsFileName;
            var result = new GetFiledsFromCsvFile(outputFilePath, columnSeparator, propertySeparator, propertyValueSeparator, encoding, haveHeader).Execute();
            if (result.Result)
            {
                return JsonOk(result.Obj);
            }

            return JsonError(result.Message);
        }

        [Auth(RoleAction.Catalog)]
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult GetFiledsFromCategoriesCsvFile(string columnSeparator, string encoding, bool haveHeader)
        {
            var outputFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp) + _csvCategoriesFileName;
            var result = new GetFiledsFromCategoriesCsvFile(outputFilePath, columnSeparator, encoding, haveHeader).Execute();
            if (result.Result)
            {
                return JsonOk(result.Obj);
            }

            return JsonError(result.Message);
        }

        [Auth(RoleAction.Catalog)]
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult GetSaasBlockInformation()
        {
            var currentSaasData = SaasDataService.CurrentSaasData;
            var productsCount = ProductService.GetProductsCount();

            return Json(new { productsInTariff = currentSaasData.ProductsCount, productsCount, isSaas = SaasDataService.IsSaasEnabled });
        }

        #endregion


        #region Import customers actions

        [Auth(RoleAction.Customers)]
        public ActionResult ImportCustomers()
        {
            SetMetaInformation(T("Admin.Import.ImportCustomers.Title"));
            SetNgController(NgControllers.NgControllersTypes.ImportCtrl);

            var encodings = new Dictionary<string, string>();
            foreach (var enumItem in (EncodingsEnum[])Enum.GetValues(typeof(EncodingsEnum)))
            {
                encodings.Add(enumItem.StrName(), enumItem.StrName());
            }

            var separators = new Dictionary<string, string>();
            foreach (var enumItem in (SeparatorsEnum[])Enum.GetValues(typeof(SeparatorsEnum)))
            {
                separators.Add(enumItem.StrName(), enumItem.Localize());
            }

            var model = new ImportCustomersModel
            {
                HaveHeader = true,
                Encoding = EncodingsEnum.Windows1251.StrName(),
                Encodings = encodings,
                ColumnSeparator = SeparatorsEnum.SemicolonSeparated.StrName(),
                ColumnSeparators = separators
            };

            return View(model);
        }

        [Auth(RoleAction.Customers)]
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult GetFiledsFromCustomersCsvFile(string columnSeparator, string encoding, bool haveHeader)
        {
            var outputFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp) + _csvCustomersFileName;
            var result = new GetFiledsFromCustomersCsvFile(outputFilePath, columnSeparator, encoding, haveHeader).Execute();
            return Json(result);
        }

        [Auth(RoleAction.Customers)]
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult StartCustomersImport(List<string> selectedCustomerFields, string columnSeparator, string encoding, bool haveHeader)
        {
            var inputFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp) + _csvCustomersFileName;
            var result = new StartImportCustomersHandler(selectedCustomerFields, inputFilePath, columnSeparator, encoding, haveHeader).Execute();
            return Json(result);
        }

        [Auth(RoleAction.Customers)]
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadCsvCustomersFile(HttpPostedFileBase file)
        {
            var filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
            FileHelpers.CreateDirectory(filePath);
            var outputFilePath = filePath + _csvCustomersFileName;

            var result = new UploadCsvFileHandler(file, outputFilePath).Execute();
            return Json(result);
        }

        public FileStreamResult GetExampleCustomersFile(string columnSeparator, string encoding)
        {
            var outputFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp) + "exampleCustomersFile.csv";
            new GetExampleCustomersFileHandler(outputFilePath, columnSeparator, encoding).Execute();

            var exampleFile = new FileInfo(outputFilePath);
            if (exampleFile.Exists)
            {
                return File(exampleFile.OpenRead(), "text/plain");
            }

            return null;
        }

        #endregion
    }
}
