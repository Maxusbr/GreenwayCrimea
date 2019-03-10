using AdvantShop.Core.Scheduler;
using Quartz;

namespace AdvantShop.Module.Resellers.Domain
{
    [DisallowConcurrentExecution]
    public class ImportJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (!context.CanStart()) return;
            context.WriteLastRun();

            TaskManager.TaskManagerInstance().AddTask(() => ProcessFile.Import(false));
        }
    }
}