//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Core.Services.Crm.BusinessProcesses;
using Quartz;

namespace AdvantShop.Core.Scheduler.Jobs
{
    [DisallowConcurrentExecution]
    public class DeferredTasksJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            System.Threading.Thread.Sleep(new System.Random().Next(20 * 60 * 1000));

            BizProcessExecuter.ProcessDeferredTasks();

            context.WriteLastRun();
        }
    }
}