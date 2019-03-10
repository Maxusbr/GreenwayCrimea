using AdvantShop.Core.Modules;
using AdvantShop.Core.Scheduler;
using Quartz;
using System.Net;
using System.Web;

namespace AdvantShop.Module.SimaLand.Service
{
    [DisallowConcurrentExecution]
    public class JobService : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (ModulesRepository.IsActiveModule(SimaLand.ModuleStringId))
            {
                if (!context.CanStart()) return;
                if (!SimalandImportStatistic.Process)
                {
                    SimalandProductService.ParseProducts();
                    context.WriteLastRun();
                }
            }
        }
    }
}
