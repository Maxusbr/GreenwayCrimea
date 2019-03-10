//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Configuration;
using Quartz;

namespace AdvantShop.Core.Scheduler.Jobs
{
    [DisallowConcurrentExecution]
    public class LicJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            System.Threading.Thread.Sleep(new System.Random().Next(20 * 60 * 1000));
            SettingsLic.Activate();
        }
    }
}