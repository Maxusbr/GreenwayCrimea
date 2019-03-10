using AdvantShop.Core.Scheduler;
using Quartz;

namespace AdvantShop.Module.SupplierOfHappiness.Domain
{
    [DisallowConcurrentExecution]
    public class SupplierOfHappinessFullJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (!context.CanStart()) return;
            context.WriteLastRun();

            TaskManager.TaskManagerInstance().AddTask(SupplierOfHappinessProcessFile.ProcessFileInJob);
        }
    }
}