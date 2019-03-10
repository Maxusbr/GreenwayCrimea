//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.ExportImport;
using AdvantShop.Helpers;
using AdvantShop.Statistic;
using AdvantShop.Trial;
using Resources;
using AdvantShop.Core.Common.Attributes;

namespace Admin
{
    public partial class ExportFeedProgress : AdvantShopAdminPage
    {
        public static string PhysicalAppPath { get; set; }

        private List<ExportFeed> _exportFeeds;

        public int ExportFeedId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            _exportFeeds = new List<ExportFeed>();
            ExportFeedId = Request["feedId"].TryParseInt();
            var typeExportFeed = Request["type"].TryParseEnum<EExportFeedType>();
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_ExportFeed_Yandex_aspx));

            if (!(CommonStatistic.IsRun))
            {
                TrialService.TrackEvent(TrialEvents.ExportProductsToFeed, "");
                CommonStatistic.Init();
            }

            if (ExportFeedId != 0)
            {
                var exportFeed = ExportFeedService.GetExportFeed(ExportFeedId);
                if (exportFeed == null)
                {
                    Response.Redirect("exportfeed.aspx");
                    return;
                }

                ltrlHead.Text = exportFeed.Name;
                PageSubheader.Visible = true;
                ltrlSubHead.Text = exportFeed.Type.Localize();

                OutDiv.Visible = true;
                PhysicalAppPath = Request.PhysicalApplicationPath;
                _exportFeeds.Add(exportFeed);
            }
            else if (typeExportFeed != EExportFeedType.None)
            {
                _exportFeeds = ExportFeedService.GetExportFeeds(typeExportFeed);
            }
            else
            {
                Response.Redirect("exportfeed.aspx");
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {

            if (_exportFeeds != null)
            {
                ExportSelection(_exportFeeds, Request.Url.PathAndQuery);
            }

        }

        public static void ExportSelection(List<ExportFeed> exportFeeds, string requestUrlPathAndQuery)
        {
            if (CommonStatistic.IsRun) return;
            CommonStatistic.Init();
            CommonStatistic.IsRun = true;
            CommonStatistic.CurrentProcess = requestUrlPathAndQuery;

            var ctx = HttpContext.Current;
            if (exportFeeds != null && exportFeeds.Any())
            {
                CommonStatistic.StartNew(() =>
                {
                    HttpContext.Current = ctx;
                    StartExport(exportFeeds, PhysicalAppPath);
                });
            }
        }

        private static void StartExport(IEnumerable<ExportFeed> exportFeeds, string applicationPath)
        {
            try
            {
                foreach (var exportFeed in exportFeeds)
                {
                    CommonStatistic.CurrentProcess = "";
                    CommonStatistic.CurrentProcessName = Resource.Admin_ExportFeed_PageSubHeader + " " + exportFeed.Name;
                    CommonStatistic.RowPosition = 0;

                    var fileName = MakeExportFile(exportFeed, applicationPath);

                    if (string.IsNullOrEmpty(fileName)) continue;

                    //CommonStatistic.FileName = "../" + fileName;
                }
            }
            catch (Exception ex)
            {
                AdvantShop.Diagnostics.Debug.Log.Error("on MakeExportFile in exportFeed", ex);
            }
            finally
            {
                CommonStatistic.IsRun = false;
            }
        }

        private static string MakeExportFile(ExportFeed exportFeed, string applicationPath)
        {
            var exportFeedSettings = ExportFeedSettingsProvider.GetSettings(exportFeed.Id);
            if (exportFeedSettings == null)
            {
                return string.Empty;
            }

            var type = ReflectionExt.GetTypeByAttributeValue<ExportFeedKeyAttribute>(typeof(BaseExportFeed), atr => atr.Value, exportFeed.Type.ToString());
            var currentExportFeed = (BaseExportFeed)Activator.CreateInstance(type);

            var exportFile = new FileInfo(exportFeedSettings.FileFullPath);
            
            //var filePath = exportFeedSettings.FileFullPath;

            if (!string.IsNullOrEmpty(exportFile.Directory.FullName))
            {
                FileHelpers.CreateDirectory(exportFile.Directory.FullName);
            }

            FileHelpers.DeleteFile(exportFile.FullName);

            var exportFilePath = SettingsGeneral.AbsolutePath + currentExportFeed.Export(exportFeed.Id);

            exportFeed.LastExport = DateTime.Now;
            exportFeed.LastExportFileFullName = exportFeedSettings.FileFullName;// exportFilePath;
            ExportFeedService.UpdateExportFeed(exportFeed);
            
            //var  currentExportFeed.GetDownloadableExportFeedFileLink

            return exportFilePath;
        }
    }
}