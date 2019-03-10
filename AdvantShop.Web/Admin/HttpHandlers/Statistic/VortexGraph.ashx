<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Statistic.VortexGraph" %>

using System;
using System.Web;
using AdvantShop.Admin.Handlers.Statistics;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Statistic
{
    public class VortexGraph : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;
            
            context.Response.ContentType = "application/JSON";
            
            var datefrom = context.Request["datefrom"].TryParseDateTime();
            var dateto = context.Request["dateto"].TryParseDateTime();
            dateto = new DateTime(dateto.Year, dateto.Month, dateto.Day, 23, 59, 59);
            
            var handler = new VortexHandler(datefrom, dateto);
            var result = handler.GetVortex();

            context.Response.Write(JsonConvert.SerializeObject(result));
        }
    }
}