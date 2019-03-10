using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.Models.Common;

namespace AdvantShop.Web.Admin.Handlers.Analytics.Reports
{
    public class TelephonyHandler : AnalyticsBaseHandler
    {
        private readonly DateTime _dateFrom;
        private readonly DateTime _dateTo;

        private readonly string _groupFormatString;
        private EGroupDateBy _groupBy;
        

        public TelephonyHandler(DateTime dateFrom, DateTime dateTo, string groupFormatString)
        {
            _dateFrom = dateFrom;
            _dateTo = dateTo;
            _groupFormatString = groupFormatString ?? "dd";

            _groupBy = Filter(_groupFormatString);
        }

        public ChartDataJsonModel GetCallsCount(ECallType type)
        {
            var list = OrderStatisticsService.GetCallsCount(_groupFormatString, type, _dateFrom, _dateTo);

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
            
            return new ChartDataJsonModel()
            {
                Data = new List<object>() { data.Values.Select(x => x) },
                Labels = data.Keys.Select(x => x.ToString("d MMM")).ToList(),
                Series = new List<string>() { "Входящие вызовы" }
            };
        }

        public ChartDataJsonModel GetAvgDuration()
        {
            var list = OrderStatisticsService.GetAvgDurationOfCalls(_groupFormatString, _dateFrom, _dateTo);

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

            return new ChartDataJsonModel()
            {
                Data = new List<object>() { data.Values.Select(x => x) },
                Labels = data.Keys.Select(x => x.ToString("d MMM")).ToList(),
                Series = new List<string>() { "Средняя продолжительность разговора (сек.)" }
            };
        }
    }
}