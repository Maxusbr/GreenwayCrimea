//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Text;
using AdvantShop.Configuration;
using AdvantShop.ExportImport;
using Quartz;

namespace AdvantShop.Core.Scheduler.Jobs
{
    [DisallowConcurrentExecution]
    public class GenerateHtmlMapJob : IJob
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

            new ExportHtmlMap(SettingsGeneral.AbsolutePath + "sitemap.html", Encoding.UTF8).Create();

            context.WriteLastRun();
        }
    }
}