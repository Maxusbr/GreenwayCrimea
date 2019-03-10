using AdvantShop.Core.Scheduler;
using Quartz;

namespace AdvantShop.Module.ReturnCustomer.Service
{
    [DisallowConcurrentExecution]
    public class RCJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (!context.CanStart())
            {
                return;
            }

            context.WriteLastRun();

            TaskManager.TaskManagerInstance().AddTask(RCService.CheckUserActions);
        }
    }
}