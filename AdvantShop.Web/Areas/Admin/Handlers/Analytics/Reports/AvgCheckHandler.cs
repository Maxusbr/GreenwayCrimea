using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.Models.Common;

namespace AdvantShop.Web.Admin.Handlers.Analytics.Reports
{
    public class AvgChartDataJsonModel : ChartDataJsonModel
    {
        public string AvgCheck { get; set; }
    }

    public class AvgCheckHandler : AnalyticsBaseHandler
    {
        private readonly DateTime _dateFrom;
        private readonly DateTime _dateTo;
        private readonly int? _statusId;
        private readonly bool? _paied;
        
        private EGroupDateBy _groupBy;
        private readonly string _groupFormatString;
        

        public AvgCheckHandler(DateTime dateFrom, DateTime dateTo, int? statusId, bool? paied, string groupFormatString)
        {
            _dateFrom = dateFrom;
            _dateTo = dateTo;
            _statusId = statusId;
            _paied = paied;
            _groupFormatString = groupFormatString ?? "dd";

            _groupBy = Filter(_groupFormatString);
        }

        public ChartDataJsonModel GetAvgCheck()
        {
            var list = OrderStatisticsService.GetOrdersAvgCheck(_groupFormatString, _dateFrom, _dateTo, _paied, _statusId);

            var data = new Dictionary<DateTime, float>();
            switch (_groupBy)
            {
                case EGroupDateBy.Day:
                    data = GetByDays(list, _dateFrom, _dateTo);
                    break;
                case EGroupDateBy.Week:
                    data = GetByWeeks(list, _dateFrom, _dateTo);
                    break;
                case EGroupDateBy.Month:
                    data = GetByMonths(list, _dateFrom, _dateTo);
                    break;
            }

            var avgCheckValue = OrderStatisticsService.GetOrdersAvgCheckValue(_groupFormatString, _dateFrom, _dateTo, _paied, _statusId);

            return new AvgChartDataJsonModel()
            {
                AvgCheck = avgCheckValue.FormatPrice(),

                Data = new List<object>() { data.Values.Select(x => x) },
                Labels = data.Keys.Select(x => x.ToString("d MMM")).ToList(),
                Series = new List<string>() { "Средний чек" }
            };
        }

        public ChartDataJsonModel GetAvgCheckByCity()
        {
            var items =
                OrderStatisticsService.GetOrdersAvgCheckByCity(_groupFormatString, _dateFrom, _dateTo, _paied, _statusId)
                    .OrderByDescending(x => x.Sum)
                    .ToList();

            var itemsStat = items.Take(15).ToList();
            if (items.Count > 15)
                itemsStat.Add(new StatisticsDataItem()
                {
                    Name = "Другие",
                    Sum = items.Skip(15).Sum(x => x.Sum),
                    Count = items.Skip(15).Sum(x => x.Count)
                });

            return new ChartDataJsonModel()
            {
                Data = new List<object>() { itemsStat.Select(x => x.Sum) },
                Labels = itemsStat.Select(x => !string.IsNullOrWhiteSpace(x.Name) ? x.Name.Reduce(30) : "n/a").ToList(),
                Series = new List<string>() { "Средний чек" }
            };
        }
    }
}