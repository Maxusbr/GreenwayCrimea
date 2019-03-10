using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Orders;

namespace AdvantShop.Web.Admin.ViewModels.Analytics
{
    public class AnalyticsReportModel
    {
        public AnalyticsReportModel()
        {
            var now = DateTime.Now;
            var date = new DateTime(now.Year, now.Month, now.Day);

            DateFrom = date.AddMonths(-1);
            DateTo = date;

            PaidItems = new List<SelectListItem>()
            {
                new SelectListItem() {Text = "Любой", Value = "null", Selected = true},
                new SelectListItem() {Text = "Да", Value = "true"},
                new SelectListItem() {Text = "Нет", Value = "false"},
            };
            Paid = PaidItems.First(x => x.Selected).Value;

            OrderStatuses =
                OrderStatusService.GetOrderStatuses()
                    .Select(x => new SelectListItem() {Text = x.StatusName, Value = x.StatusID.ToString()})
                    .ToList();

            OrderStatuses.Insert(0, new SelectListItem() {Text = "Любой", Value = "null", Selected = true});

            OrderStatus = OrderStatuses.First(x => x.Selected).Value;
        }

        public List<SelectListItem> PaidItems { get; set; }
        
        public List<SelectListItem> OrderStatuses { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Paid { get; set; }
        public string OrderStatus { get; set; }
    }
}
