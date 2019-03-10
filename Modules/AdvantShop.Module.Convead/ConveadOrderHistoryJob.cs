using AdvantShop.Core.Scheduler;
using Quartz;

namespace AdvantShop.Modules.Convead
{
    [DisallowConcurrentExecution]
    public class ConveadOrderHistoryJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (!context.CanStart()) return;
            context.WriteLastRun();

            string error = "";
            ConveadService.GetOrderHistory(out error);
        }
    }
}