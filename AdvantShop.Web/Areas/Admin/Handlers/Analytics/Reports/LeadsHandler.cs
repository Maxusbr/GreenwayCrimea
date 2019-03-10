using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Web.Admin.Handlers.Analytics.Statistics
{
    public class LeadsHandler
    {
        #region Properties

        private readonly DateTime _dateFrom;
        private readonly DateTime _dateTo;

        private EGroupDateBy _groupBy;
        private readonly string _groupFormatString;
        private readonly float _currencyValue = CurrencyService.CurrentCurrency.Rate;
        
        #endregion

        public LeadsHandler(DateTime dateFrom, DateTime dateTo, string groupFormatString)
        {
            _dateFrom = dateFrom;
            _dateTo = dateTo;
            _groupFormatString = groupFormatString;
        }
        

        public object LoadLeadsCountGraph()
        {
            Filter();

            return new
            {
                chart = RenderLeadsCountGraph(_dateFrom, _dateTo),
                options =
                    string.Format("{{xaxis : {{ mode: 'time', timeformat: '%d %b', min: {0}, max: {1}}} }}",
                        GetTimestamp(_dateFrom), GetTimestamp(_dateTo))
            };
        }

        public object LoadLeadsByStatusGraph()
        {
            //var statuses = LeadService.GetLeadsStatusesStatistics(_dateFrom, _dateTo);

            var data = "";

            //foreach (var item in statuses)
            //{
            //    LeadStatus status;
            //    Enum.TryParse(item.Key, out status);

            //    data += (data != "" ? ", " : "") +
            //           string.Format("{{ 'label': '{0}', 'value': {1} }}", status.Localize(), item.Value);
            //}

            return String.Format("[ {{ 'key': '{0}', 'values': [{1}] }} ]", "Statuses", data);
        }

        private void Filter()
        {
            switch (_groupFormatString)
            {
                case "dd":
                    _groupBy = EGroupDateBy.Day;
                    break;
                case "wk":
                    _groupBy = EGroupDateBy.Week;
                    break;
                case "mm":
                    _groupBy = EGroupDateBy.Month;
                    break;
                default:
                    _groupBy = EGroupDateBy.Day;
                    break;
            }
        }

        private string RenderLeadsCountGraph(DateTime dateFrom, DateTime dateTo)
        {
            var listSum = LeadService.GetLeadsCountStatistics(_groupFormatString, dateFrom, dateTo);

            var data = "";
            switch (_groupBy)
            {
                case EGroupDateBy.Day:
                    data = GetByDays(listSum, dateFrom, dateTo);
                    break;
                case EGroupDateBy.Week:
                    data = GetByWeeks(listSum, dateFrom, dateTo);
                    break;
                case EGroupDateBy.Month:
                    data = GetByMonths(listSum, dateFrom, dateTo);
                    break;
            }

            return String.Format("[{{label: '{0}', data:[{1}]}}]", "Resource.Admin_Statistics_LeadsByCount", data);
        }

        #region Help methods

        private static long GetTimestamp(DateTime date)
        {
            TimeSpan span = (date - new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (long)(span.TotalSeconds * 1000);
        }

        private string GetByDays(Dictionary<DateTime, float> list, DateTime startDate, DateTime endDate)
        {
            var resultList = new List<string>();
            var tempDate = DateTime.MinValue;

            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day);

            foreach (var profit in list)
            {
                if (tempDate != DateTime.MinValue)
                {
                    var dayOffset = (profit.Key - tempDate).Days;
                    for (var i = 1; i < dayOffset; i++)
                    {
                        resultList.Add(string.Format("[{0},{1}]", GetTimestamp(tempDate.AddDays(i)), 0));
                    }
                }
                else
                {
                    var dayOffset = (profit.Key - startDate).Days;
                    for (var i = 1; i < dayOffset; i++)
                    {
                        resultList.Add(string.Format("[{0},{1}]", GetTimestamp(startDate.AddDays(i)), 0));
                    }
                }

                resultList.Add(string.Format("[{0},{1}]", GetTimestamp(profit.Key), (profit.Value / _currencyValue).ToString("F2").Replace(",", ".")));
                tempDate = profit.Key;
            }

            if (tempDate == DateTime.MinValue)
                tempDate = startDate;

            var endDayOffset = (endDate - tempDate).Days;
            for (var i = 1; i <= endDayOffset; i++)
            {
                resultList.Add(string.Format("[{0},'{1}']", GetTimestamp(tempDate.AddDays(i)), 0));
            }

            return String.Join(",", resultList);
        }

        private string GetByWeeks(Dictionary<DateTime, float> list, DateTime startDate, DateTime endDate)
        {
            var resultList = new List<string>();

            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day);

            var nextDate = startDate;
            var prevDate = DateTime.MinValue;

            while (nextDate <= endDate)
            {
                var value = list.Where(x => x.Key > prevDate && x.Key <= nextDate).Sum(x => x.Value);
                resultList.Add(string.Format("[{0},{1}]", GetTimestamp(nextDate), (value / _currencyValue).ToString("F2").Replace(",", ".")));

                prevDate = nextDate;
                nextDate = nextDate.DayOfWeek != 0
                            ? nextDate.AddDays(7 - (int)nextDate.DayOfWeek + 1)
                            : nextDate.AddDays(7);
            }

            var lastValue = list.Where(x => x.Key > prevDate && x.Key <= endDate).Sum(x => x.Value);
            resultList.Add(string.Format("[{0},{1}]", GetTimestamp(endDate), (lastValue / _currencyValue).ToString("F2").Replace(",", ".")));

            return String.Join(",", resultList);
        }

        private string GetByMonths(Dictionary<DateTime, float> list, DateTime startDate, DateTime endDate)
        {
            var resultList = new List<string>();

            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day);

            var nextDate = startDate;
            var prevDate = DateTime.MinValue;

            while (nextDate <= endDate)
            {
                var value = list.Where(x => x.Key > prevDate && x.Key <= nextDate).Sum(x => x.Value);
                resultList.Add(string.Format("[{0},{1}]", GetTimestamp(nextDate), (value / _currencyValue).ToString("F2").Replace(",", ".")));

                prevDate = nextDate;
                nextDate = nextDate.AddMonths(1);
                if (nextDate.Day != 1)
                    nextDate = new DateTime(nextDate.Year, nextDate.Month, 1);
            }

            var lastValue = list.Where(x => x.Key > prevDate && x.Key <= endDate).Sum(x => x.Value);
            resultList.Add(string.Format("[{0},{1}]", GetTimestamp(endDate), (lastValue / _currencyValue).ToString("F2").Replace(",", ".")));

            return String.Join(",", resultList);
        }

        #endregion
    }
}