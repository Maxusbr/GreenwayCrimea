//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Configuration;
using AdvantShop.ExportImport;
using AdvantShop.Helpers;
using Quartz;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Core.Scheduler.Jobs
{
    public class GenerateExportFeedJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (!context.CanStart()) return;

            JobDataMap dataMap = context.JobDetail.JobDataMap;
            var jobData = dataMap.Get(TaskManager.DataMap) as TaskSetting;

            if (jobData.TimeType == TimeIntervalType.Hours)
            {
                System.Threading.Thread.Sleep(new System.Random().Next(10 * 60 * 1000));
            }

            var exportFeedId = 0;
            if (jobData.DataMap == null || !int.TryParse(jobData.DataMap.ToString(), out exportFeedId))
            {
                return;
            }

            var exportFeed = ExportFeedService.GetExportFeed(exportFeedId);

            if (exportFeed == null)
            {
                return;
            }
            var exportFeedSettings = ExportFeedSettingsProvider.GetSettings(exportFeed.Id);
            if (exportFeedSettings == null)
            {
                return;
            }

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

            exportFeed.LastExport = DateTime.Now;
            exportFeed.LastExportFileFullName = exportFeedSettings.FileFullName;// filePath;
            ExportFeedService.UpdateExportFeed(exportFeed);

            context.WriteLastRun();
        }
    }
}