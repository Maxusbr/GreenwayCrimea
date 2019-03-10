using AdvantShop.Core.Scheduler;
using Quartz;

namespace AdvantShop.Module.ZakupiImport.Domain
{
    [DisallowConcurrentExecution]
    public class ZakupiImportJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (!context.CanStart()) return;
            context.WriteLastRun();

            TaskManager.TaskManagerInstance().AddTask(ZakupiImportProcessFile.ProcessYmlInJob);
        }
    }
}