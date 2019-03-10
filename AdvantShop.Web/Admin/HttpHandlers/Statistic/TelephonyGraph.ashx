<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Statistic.TelephonyGraph" %>

using System;
using System.Web;
using AdvantShop.Admin.Handlers.Statistics;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Services.IPTelephony;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Statistic
{
    public class TelephonyGraph : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;
            
            context.Response.ContentType = "application/JSON";

            var type = context.Request["type"];
            if (string.IsNullOrWhiteSpace(type))
                return;

            var datefrom = context.Request["datefrom"].TryParseDateTime();
            var dateto = context.Request["dateto"].TryParseDateTime();
            dateto = new DateTime(dateto.Year, dateto.Month, dateto.Day, 23, 59, 59);
            
            var groupFormatString = context.Request["groupby"].IsNotEmpty()
                                        ? context.Request["groupby"]
                                        : "dd";
            
            object result = null;

            var handler = new TelephonyHandler(datefrom, dateto, groupFormatString);
            switch (type)
            {
                case "in":
                    result = handler.GetCallsCount(ECallType.In);
                    break;
                case "missed":
                    result = handler.GetCallsCount(ECallType.Missed);
                    break;
                case "out":
                    result = handler.GetCallsCount(ECallType.Out);
                    break;
                case "avgtime":
                    result = handler.GetAvgDuration();
                    break;
            }

            context.Response.Write(JsonConvert.SerializeObject(result));
        }
    }
}