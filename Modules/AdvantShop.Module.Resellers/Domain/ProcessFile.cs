//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Web.Hosting;
using AdvantShop.Catalog;
using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;
using AdvantShop.ExportImport;
using AdvantShop.Helpers;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Module.Resellers.Domain
{
    public class ProcessFile
    {
        public enum UpdateType
        {
            Full = 0,
            AmountAndPrice = 1
        }


        public static void Import(bool manual)
        {
            if (!ModuleSettingsProvider.GetSettingValue<bool>("AutoUpdate", Reseller.ModuleID) && !manual)
                return;

            var importDirectory = HostingEnvironment.MapPath("~/modules/reseller/temp/");
            var filename = importDirectory + "import.csv";
            FileHelpers.CreateDirectory(importDirectory);

            if (File.Exists(filename))
                File.Delete(filename);

            var fileUrl = string.Format("{0}/api/resellers/catalog/{1}",
                ModuleSettingsProvider.GetSettingValue<string>("SupplierSite", Reseller.ModuleID),
                ModuleSettingsProvider.GetSettingValue<string>("ResellerID", Reseller.ModuleID));

            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(fileUrl, filename);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return;
            }

            if (ModuleSettingsProvider.GetSettingValue<bool>("DeactivateProducts", Reseller.ModuleID))
            {
                ProductService.DisableAllProducts();
            }

            if (ModuleSettingsProvider.GetSettingValue<bool>("AmountNulling", Reseller.ModuleID))
            {
                ProductService.ClearAmountAllProducts();
            }

            var magrin = ModuleSettingsProvider.GetSettingValue<float>("Margin", Reseller.ModuleID);
            var columnSeparator = ModuleSettingsProvider.GetSettingValue<string>("ColumnSepaartor",
                Reseller.ModuleID);
            var propertySeparator = ModuleSettingsProvider.GetSettingValue<string>("PropertySeparator",
                Reseller.ModuleID);

            CsvImport.Factory(filename, true, false, ";", EncodingsEnum.Utf8.StrName(), null, ";", ":")
                .Process(product =>
                {
                    foreach (var offer in product.Offers)
                    {
                        offer.BasePrice = offer.BasePrice + offer.BasePrice * magrin / 100;
                    }
                    return product;
                }).Wait();

            ModuleSettingsProvider.SetSettingValue("LastUpdate", DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                Reseller.ModuleID);
        }
    }
}