<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Statistic.GetNoticeStatistic" %>

using System;
using System.Text;
using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.Customers;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Statistic
{
    public class GetNoticeStatistic : AdminHandler, IHttpHandler
    {

        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;
            
            context.Response.ContentType = "application/JSON";
            context.Response.ContentEncoding = Encoding.UTF8;

            context.Response.Write(JsonConvert.SerializeObject(new
                {
                    LastOrdersCount = StatisticService.GetLastOrdersCount(),
                    LastReviews = StatisticService.GetLastReviewsCount(),
                    LastLeadsCount = CustomerContext.CurrentCustomer.IsAdmin || CustomerContext.CurrentCustomer.ManagerId == null
                                        ? LeadService.GetLeadsCount(default(int?))
                                        : LeadService.GetLeadsCount(Convert.ToInt32(CustomerContext.CurrentCustomer.ManagerId))
                }));
        }
    }
}