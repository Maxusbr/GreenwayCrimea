//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;
using Newtonsoft.Json;
using Quartz;

namespace AdvantShop.Core.Scheduler
{
    public class TaskSettings : List<TaskSetting>
    {
        public static TaskSettings Settings
        {
            get
            {
                var fromDb = SettingProvider.Items["TaskSqlSettings"];
                if (fromDb == null)
                    return new TaskSettings();

                var temp = JsonConvert.DeserializeObject<TaskSettings>(SQLDataHelper.GetString(fromDb));
                if (temp == null)
                    return new TaskSettings();

                return temp;
            }
            set { SettingProvider.Items["TaskSqlSettings"] = JsonConvert.SerializeObject(value); }
        }

        public static TaskSettings ExportFeedSettings
        {
            get
            {
                var fromDb = SettingProvider.Items["ExportFeedTaskSqlSettings"];
                if (fromDb == null)
                    return new TaskSettings();

                var temp = JsonConvert.DeserializeObject<TaskSettings>(SQLDataHelper.GetString(fromDb));
                if (temp == null)
                    return new TaskSettings();

                return temp;
            }
            set { SettingProvider.Items["ExportFeedTaskSqlSettings"] = JsonConvert.SerializeObject(value); }
        }
    }

    public class TaskSetting
    {
        public string JobType { get; set; }
        public bool Enabled { get; set; }
        public int TimeInterval { get; set; }
        public int TimeHours { get; set; }
        public int TimeMinutes { get; set; }
        public TimeIntervalType TimeType { get; set; }

        public object DataMap { get; set; }

        public string GetUniqueName()
        {
            return JobType + (DataMap != null ? "_" + DataMap.GetHashCode() : string.Empty);
        }
        
    }

    public static class JobExt
    {
        /// <summary>
        /// Can job start. Проверка работает только для задач TaskSetting
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool CanStart(this IJobExecutionContext obj)
        {
            var dataMap = obj.JobDetail.JobDataMap;
            var jobData = dataMap.Get(TaskManager.DataMap) as TaskSetting;
            if (jobData == null) return false;

            var dir = SettingsGeneral.AbsolutePath + "App_Data/Jobs/";
            FileHelpers.CreateDirectory(dir);

            var file = dir + jobData.GetUniqueName() + ".txt";
            var lasttime = System.IO.File.Exists(file) ? System.IO.File.ReadAllText(file).TryParseDateTime(true) : null;
            var currentTime = DateTime.Now;
            if (!lasttime.HasValue) return true;
            if (jobData.TimeType == TimeIntervalType.Days)
            {
                var lastDateTime = new DateTime(lasttime.Value.Year, lasttime.Value.Month, lasttime.Value.Day, 0, 0, 0);
                return (currentTime - lastDateTime).TotalDays >= jobData.TimeInterval;
            }
            if (jobData.TimeType == TimeIntervalType.Hours)
            {
                var lastDateTime = new DateTime(lasttime.Value.Year, lasttime.Value.Month, lasttime.Value.Day, lasttime.Value.Hour, 0, 0);
                return (currentTime - lastDateTime).TotalHours >= jobData.TimeInterval;
            }
            if (jobData.TimeType == TimeIntervalType.Minutes)
            {
                return (currentTime - lasttime.Value).TotalMinutes >= jobData.TimeInterval;
            }
            return true;
        }

        public static void WriteLastRun(this IJobExecutionContext obj)
        {
            var dataMap = obj.JobDetail.JobDataMap;
            var jobData = dataMap.Get(TaskManager.DataMap) as TaskSetting;

            var fileName = jobData == null ? obj.JobDetail.Key.Name : jobData.GetUniqueName();

            if (!string.IsNullOrEmpty(fileName))
            {
                var dir = SettingsGeneral.AbsolutePath + "App_Data/Jobs/";
                FileHelpers.CreateDirectory(dir);

                var file = dir + fileName + ".txt";
                System.IO.File.WriteAllText(file, DateTime.Now.ToString());
            }
        }
    }
}