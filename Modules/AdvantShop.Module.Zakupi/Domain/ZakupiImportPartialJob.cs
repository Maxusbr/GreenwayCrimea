using AdvantShop.Core.Scheduler;
using Quartz;

namespace AdvantShop.Module.ZakupiImport.Domain
{
    [DisallowConcurrentExecution]
    public class ZakupiImportPartialJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (!context.CanStart()) return;
            context.WriteLastRun();

            TaskManager.TaskManagerInstance().AddTask(ZakupiImportProcessFile.ProcessPartialYmlInJob);
        }
    }
}