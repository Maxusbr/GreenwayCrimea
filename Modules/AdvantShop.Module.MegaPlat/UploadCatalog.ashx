<%@ WebHandler Language="C#" Class="Advantshop.UserControls.Modules.UploadCatalog" %>

using System.IO;
using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.ExportImport;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Trial;

namespace Advantshop.UserControls.Modules
{
    public class UploadCatalog : IHttpHandler
    {
        private const string _moduleName = "MegaPlat";

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            if (context.Request.Files.Count < 1 || context.Request.Files[0].FileName.IsNullOrEmpty())
            {
                context.Response.Write("Нет файла");
                return;
            }
            
            if (context.Request["apikey"] != ModuleSettingsProvider.GetSettingValue<string>("MegaPlatApiKey", _moduleName))
            {
                context.Response.Write("Неверный apikey");
                return;
            }

            HttpPostedFile pf = context.Request.Files[0];

            FileHelpers.CreateDirectory(FoldersHelper.GetPathAbsolut(FolderType.ImageTemp));
            var path = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp, pf.FileName);

            if (File.Exists(path))
                File.Delete(path);

            pf.SaveAs(path);

            string logPath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp, "StatisticLog.txt");
            
            if (File.Exists(logPath))
            {
                File.Delete(logPath);
            }

            CsvImport.Factory(path, true, false, SeparatorsEnum.SemicolonSeparated.StrName(), EncodingsEnum.Utf8.StrName(), null, ",", ":", true).Process().Wait();
            
            if (File.Exists(logPath))
            {
                string log;
                using (TextReader reader = new StreamReader(logPath))
                {
                    log = reader.ReadToEnd();
                }
                context.Response.Write(log.IsNullOrEmpty() ? "OK" : log);
            }
            else
            {
                context.Response.Write("OK");
            }
        }


        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}