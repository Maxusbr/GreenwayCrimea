using AdvantShop.Core.Modules;
using AdvantShop.Core.Scheduler;
using Quartz;
using System.Net;
using System.Web;
using System;

namespace AdvantShop.Module.LastOrder.Service
{
    [DisallowConcurrentExecution]
    public class FNPJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (ModulesRepository.IsActiveModule(LastOrder.ModuleStringId))
            {
                if (!context.CanStart()) return;
                ModuleSettings.LastClearNotifications = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                FNPService.ClearNotifications();
                context.WriteLastRun();
            }
        }
    }
}
