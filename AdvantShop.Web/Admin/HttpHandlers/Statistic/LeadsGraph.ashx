<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Statistic.LeadsGraph" %>

using System;
using System.Web;
using AdvantShop.Admin.Handlers.Statistics;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Statistic
{
    public class LeadsGraph : AdminHandler, IHttpHandler
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

            var handler = new LeadsHandler(datefrom, dateto, groupFormatString);
            switch (type)
            {
                case "count":
                    result = handler.LoadLeadsCountGraph();
                    break;
                case "status":
                    result = handler.LoadLeadsByStatusGraph();
                    break;
            }

            context.Response.Write(JsonConvert.SerializeObject(result));
        }
    }
}