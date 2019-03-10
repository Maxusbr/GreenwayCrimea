using AdvantShop.Core.Scheduler;
using Quartz;

namespace AdvantShop.Module.YandexMarketImport.Domain
{
    [DisallowConcurrentExecution]
    public class YandexMarketImportJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (!context.CanStart()) return;
            context.WriteLastRun();

            TaskManager.TaskManagerInstance().AddTask(YandexMarketImportProcessFile.ProcessYmlInJob);
        }
    }
}