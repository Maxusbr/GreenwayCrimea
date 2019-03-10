<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Statistic.SalesGraph" %>

using System;
using System.Web;
using AdvantShop.Admin.Handlers.Statistics;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Statistic
{
    public class SalesGraph : AdminHandler, IHttpHandler
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

            var useShippingCost = context.Request["useshippings"].IsNotEmpty() && context.Request["useshippings"].TryParseBool();

            var groupFormatString = context.Request["groupby"].IsNotEmpty()
                                        ? context.Request["groupby"]
                                        : "dd";

            object result = null;

            var handler = new OrderStatictsHandler(datefrom, dateto, statusId, paied, useShippingCost, groupFormatString);
            switch (type)
            {
                case "sum":
                    result = handler.GetOrdersSum();
                    break;
                case "count":
                    result = handler.GetOrdersCount();
                    break;
               
            }

            context.Response.Write(JsonConvert.SerializeObject(result));
        }
    }
}