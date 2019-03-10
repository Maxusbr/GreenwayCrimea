using AdvantShop.Core.Scheduler;
using Quartz;

namespace AdvantShop.Modules.RetailCRM
{
    [DisallowConcurrentExecution]
    public class OrderHistoryJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (!context.CanStart()) return;
            context.WriteLastRun();

            string error = "";
            RetailCRMService.GetOrderHistory(out error);
        }
    }
}