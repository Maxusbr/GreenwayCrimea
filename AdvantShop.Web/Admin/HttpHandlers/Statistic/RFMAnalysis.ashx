<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Statistic.RfmAnalysis" %>

using System.Web;
using AdvantShop.Admin.Handlers.Statistics;
using AdvantShop.Core.HttpHandlers;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Statistic
{
    public class RfmAnalysis : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;
            
            context.Response.ContentType = "application/JSON";
            
            var handler = new RFMAnalysisHandler();
            var result = handler.GetData();

            context.Response.Write(JsonConvert.SerializeObject(result));
        }
    }
}