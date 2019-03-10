//using System.Web.Hosting;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Scheduler;
using AdvantShop.ExportImport;
using AdvantShop.Repository.Currencies;
using Quartz;

namespace AdvantShop.Modules.RetailCRM
{
    [DisallowConcurrentExecution]
    public class ExportFeedJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //var fileName = "retailcrm_" + ModuleSettingsProvider.GetSettingValue<string>("ApiKey", RetailCRMModule.ModuleStringId).Md5() + ".xml";
            //var filePath = HostingEnvironment.MapPath("~/") + "\\" + fileName;
            if (!context.CanStart()) return;
            context.WriteLastRun();

			var currency = CurrencyService.GetCurrency(ModuleSettingsProvider.GetSettingValue<int>("CurrencyIDExport", RetailCRMModule.ModuleStringId));

            var options = new ExportFeedYandexOptions
            {
                //ExportNotActiveProducts = true,
                //ExportNotAmountProducts = true,
                ExportNotAvailable = true,
                FileName = "retailcrm_" + ModuleSettingsProvider.GetSettingValue<string>("ApiKey", RetailCRMModule.ModuleStringId).Md5(),
                FileExtention = "xml",
                Delivery = true,
                ColorSizeToName = false,
				Currency = currency != null ? currency.Iso3 : "RUB",
                RemoveHtml = true,
                ProductDescriptionType = "full",

            };
            //TODO EXPORTFEED
            var exportFeedModule = new ExportFeedRetailCRM(
                RetailCRMService.GetCategories(),
                RetailCRMService.GetProducts(options),
                options,
                0,
                0
            );
            exportFeedModule.Build();
        }
    }
}