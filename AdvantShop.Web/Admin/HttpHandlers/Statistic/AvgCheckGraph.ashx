<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Statistic.AvgCheckGraph" %>

using System;
using System.Web;
using AdvantShop.Admin.Handlers.Statistics;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Statistic
{
    public class AvgCheckGraph : AdminHandler, IHttpHandler
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
            
            int? statusId = null;
            bool? paied = null;

            if (context.Request["statuses"].IsNotEmpty())
                statusId = context.Request["statuses"].TryParseInt();
            
            if (context.Request["paied"].IsNotEmpty())
                paied = context.Request["paied"] == "any"
                    ? default(bool?)
                    : context.Request["paied"] == "yes";

            var groupFormatString = context.Request["groupby"].IsNotEmpty()
                                        ? context.Request["groupby"]
                                        : "dd";
            
            object result = null;

            var handler = new AvgCheckHandler(datefrom, dateto, statusId, paied, groupFormatString);
            switch (type)
            {
                case "avgvalue":
                    result = handler.GetAvgCheckValue();
                    break;
                case "avg":
                    result = handler.GetAvgCheck();
                    break;
                case "city":
                    result = handler.GetAvgCheckByCity();
                    break;
            }

            context.Response.Write(JsonConvert.SerializeObject(result));
        }
    }
}