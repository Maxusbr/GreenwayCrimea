using System;
using System.Collections.Generic;

using AdvantShop.Web.Admin.Models.ExportFeeds;
using AdvantShop.ExportImport;
using AdvantShop.Core.Scheduler;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Web.Admin.Handlers.ExportFeeds
{
    public class GetExportFeedSettings
    {
        private ExportFeedSettings _exportFeedSettings;
        private EExportFeedType _exportFeedType;

        public GetExportFeedSettings(ExportFeedSettings exportFeedSettings, EExportFeedType exportFeedType)
        {
            _exportFeedSettings = exportFeedSettings;
            _exportFeedType = exportFeedType;
        }

        public ExportFeedSettingsModel Execute()
        {
            var intervalTypeList = new Dictionary<TimeIntervalType, string>();
            foreach (TimeIntervalType timeIntervalType in Enum.GetValues(typeof(TimeIntervalType)))
            {
                if (timeIntervalType == TimeIntervalType.Minutes || timeIntervalType == TimeIntervalType.None)
                {
                    continue;
                }
                intervalTypeList.Add(timeIntervalType, timeIntervalType.Localize());
            }

            var type = ReflectionExt.GetTypeByAttributeValue<ExportFeedKeyAttribute>(typeof(BaseExportFeed), atr => atr.Value, _exportFeedType.ToString());
            var currentExportFeedInstance = (BaseExportFeed)Activator.CreateInstance(type);

            var fileExtentions = new Dictionary<string, string>();
            foreach (var fileExtention in currentExportFeedInstance.GetAvailableFileExtentions())
            {
                fileExtentions.Add(fileExtention, fileExtention);
            }

            var model = new ExportFeedSettingsModel
            {
                FileName = _exportFeedSettings.FileName,
                FileExtention = _exportFeedSettings.FileExtention,
                PriceMargin = _exportFeedSettings.PriceMargin,
                AdditionalUrlTags = _exportFeedSettings.AdditionalUrlTags,

                Active = _exportFeedSettings.Active,
                IntervalType = _exportFeedSettings.IntervalType,
                Interval = _exportFeedSettings.Interval,
                
                JobStartHour = _exportFeedSettings.JobStartTime.Hour,
                JobStartMinute = _exportFeedSettings.JobStartTime.Minute,

                IntervalTypeList = intervalTypeList,
                FileExtentions = fileExtentions,

                ExportAllProducts = _exportFeedSettings.ExportAllProducts,

                AdvancedSettings = _exportFeedSettings.AdvancedSettings

                //JobStartTime = _exportFeedSettings.JobStartTime,
            };

            return model;
        }
    }
}
