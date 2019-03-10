//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Configuration;
using AdvantShop.ExportImport;
using Quartz;
using System.Text;

namespace AdvantShop.Core.Scheduler.Jobs
{
    [DisallowConcurrentExecution]
    public class SiteMapJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
                        
            new ExportHtmlMap(SettingsGeneral.AbsolutePath + "sitemap.html", Encoding.UTF8).Create();
            new ExportXmlMap(SettingsGeneral.AbsolutePath + "sitemap.xml").Create();            

            context.WriteLastRun();
        }
    }
}