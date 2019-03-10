using AdvantShop.Core.Scheduler;
using AdvantShop.Diagnostics;
using AdvantShop.Module.VkMarket.Domain;
using Quartz;

namespace AdvantShop.Module.VkMarket.Services
{
    [DisallowConcurrentExecution]
    public class VkMarketExportJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (!context.CanStart() || VkMarketExportState.IsRun || !VkMarketExportSettings.ExportOnShedule)
                return;

            Debug.Log.Info("VkMarketExportJob start");

            var exportSerivice = new VkMarketExportService();
            exportSerivice.StartExport();

            Debug.Log.Info("VkMarketExportJob end");
        }
    }
}
