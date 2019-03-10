using AdvantShop.Core.Modules;
using AdvantShop.Core.Scheduler;
using Quartz;
using System.Net;
using System.Web;

namespace AdvantShop.Module.SimaLand.Service
{
    [DisallowConcurrentExecution]
    public class UpdateBalanceJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (ModulesRepository.IsActiveModule(SimaLand.ModuleStringId))
            {
                if (!context.CanStart()) return;
                if (!SimalandImportStatistic.Process)
                {
                    UpdateBalanceService.UpdateBalancePriceJob();
                    context.WriteLastRun();
                }
            }
        }
    }
}
