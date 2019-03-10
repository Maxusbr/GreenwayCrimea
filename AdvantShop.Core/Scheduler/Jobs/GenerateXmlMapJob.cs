//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Configuration;
using AdvantShop.ExportImport;
using Quartz;

namespace AdvantShop.Core.Scheduler.Jobs
{
    [DisallowConcurrentExecution]
    public class GenerateXmlMapJob : IJob
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


            new ExportXmlMap(SettingsGeneral.AbsolutePath + "sitemap.xml").Create();

            context.WriteLastRun();
        }
    }
}