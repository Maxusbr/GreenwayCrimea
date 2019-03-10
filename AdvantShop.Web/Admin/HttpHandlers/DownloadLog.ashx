<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.DownloadLog" %>

using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Helpers;
using AdvantShop.Statistic;

namespace Admin.HttpHandlers
{
    public class DownloadLog : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;
            
            if (!System.IO.File.Exists(CommonStatistic.FileLog))
            {
                FileHelpers.CreateFile(CommonStatistic.FileLog);
            }
            CommonHelper.WriteResponseFile(CommonStatistic.FileLog, CommonStatistic.VirtualFileLogPath);
        }
    }
}