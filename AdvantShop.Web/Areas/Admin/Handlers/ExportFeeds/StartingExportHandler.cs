using System;

using AdvantShop.ExportImport;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Models.ExportFeeds;
using AdvantShop.Core.Common.Attributes;
using System.Collections.Generic;

namespace AdvantShop.Web.Admin.Handlers.ExportFeeds
{
    public class StartingExportHandler
    {
        private readonly int _exportFeedId;

        public StartingExportHandler(int exportFeedId)
        {
            _exportFeedId = exportFeedId;
        }

        public string Execute()
        {
            var exportFeed = ExportFeedService.GetExportFeed(_exportFeedId);
            var model = new ExportFeedProgressModel
            {
                Id = exportFeed.Id,
                Name = exportFeed.Name,
                Type = exportFeed.Type
            };

            return MakeExportFile(exportFeed);
        }

        private static string MakeExportFile(ExportFeed exportFeed)
        {
            var exportFeedSettings = ExportFeedSettingsProvider.GetSettings(exportFeed.Id);
            if (exportFeedSettings == null)
            {
                return string.Empty;
            }

            //if (exportFeedSettings.ExportAllProducts)
            //{
            //    ExportFeedService.InsertCategories(exportFeed.Id, new List<int>() { 0 });
            //}

            var type = ReflectionExt.GetTypeByAttributeValue<ExportFeedKeyAttribute>(typeof(BaseExportFeed), atr => atr.Value, exportFeed.Type.ToString());
            var currentExportFeed = (BaseExportFeed)Activator.CreateInstance(type);

            var filePath = exportFeedSettings.FileFullPath;
            var directory = filePath.Substring(0, filePath.LastIndexOf('\\'));

            if (!string.IsNullOrEmpty(directory))
            {
                FileHelpers.CreateDirectory(directory);
            }

            FileHelpers.DeleteFile(filePath);

            currentExportFeed.Export(exportFeed.Id);


            //if (exportFeedSettings.ExportAllProducts)
            //{
            //    ExportFeedService.DeleteCategory(exportFeed.Id, 0);
            //}

            exportFeed.LastExport = DateTime.Now;
            exportFeed.LastExportFileFullName = exportFeedSettings.FileFullName;
            ExportFeedService.UpdateExportFeed(exportFeed);

            return exportFeedSettings.FileFullName;
        }
    }
}
