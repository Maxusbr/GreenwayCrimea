<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.CommonStatisticData" %>

using System;
using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Statistic;
using Newtonsoft.Json;

namespace Admin.HttpHandlers
{
    public class CommonStatisticData : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;
            
            context.Response.Cache.SetLastModified(DateTime.UtcNow);

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(CommonStatistic.CurrentData));
            context.Response.End();
        }
    }
}