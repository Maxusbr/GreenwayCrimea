<%@ WebHandler Language="C#" Class="Advantshop.UserControls.Modules.DownloadCatalog" %>
using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Threading.Tasks;

using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.ExportImport;
using AdvantShop.Helpers;
using Newtonsoft.Json;

namespace Advantshop.UserControls.Modules
{
    public class DownloadCatalog : IHttpHandler
    {
        private const string _moduleName = "MegaPlat";

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            if (context.Request["apikey"] !=
                ModuleSettingsProvider.GetSettingValue<string>("MegaPlatApiKey", _moduleName))
            {
                context.Response.Write("Неверный apikey");
                return;
            }

            List<ProductFields> productFields = new List<ProductFields>()
            {
                ProductFields.Sku,
                ProductFields.Name,
                ProductFields.ParamSynonym,
                ProductFields.Category,
                ProductFields.Sorting,
                ProductFields.Enabled,
                ProductFields.Price,
                ProductFields.PurchasePrice,
                ProductFields.Amount,
                ProductFields.MultiOffer,
                ProductFields.Unit,
                ProductFields.Discount,
                ProductFields.ShippingPrice,
                ProductFields.Weight,
                ProductFields.Size,
                ProductFields.BriefDescription,
                ProductFields.Description,
                ProductFields.Title,
                ProductFields.MetaKeywords,
                ProductFields.MetaDescription,
                ProductFields.H1,
                ProductFields.Photos,
                ProductFields.Videos,
                ProductFields.Markers,
                ProductFields.Properties,
                ProductFields.Producer,
                ProductFields.OrderByRequest,
                ProductFields.SalesNotes,
                ProductFields.Related,
                ProductFields.Alternative,
                ProductFields.CustomOption,
                ProductFields.Gtin,
                ProductFields.GoogleProductCategory,
                ProductFields.Adult
            };


            string fileName = "export";
            string filePath = context.Server.MapPath("~/modules/megaplat/" + fileName + ".csv");
            if (File.Exists(filePath))
                File.Delete(filePath);

            var advantsedSettings = new ExportFeedCsvOptions
            {

                CsvEnconing = EncodingsEnum.Utf8.StrName(),
                CsvSeparator = ";",
                CsvColumSeparator = ",",
                CsvPropertySeparator = ":",
                FieldMapping = productFields,
                CsvExportNoInCategory = true
            };

            var commonSettings = new ExportFeedSettings
            {
                FileName = "modules/megaplat/" + fileName,
                FileExtention = "csv",
                AdvancedSettings = JsonConvert.SerializeObject(advancedSettings)
            };




            CsvExport.Factory(
                        ExportFeedCsvService.GetAllProducts(commonSettings, advantsedSettings),
                        commonSettings,
                        ExportFeedCsvService.GetAllCsvProductsCount(),
                        0).Process().Wait();


            CommonHelper.WriteResponseFile(commonSettings.FileFullPath, fileName + ".csv");

            if (File.Exists(fileName + ".csv"))
                File.Delete(fileName + ".csv");
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
