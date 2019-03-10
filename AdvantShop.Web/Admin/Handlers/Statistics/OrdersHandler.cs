using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using Resources;

namespace AdvantShop.Admin.Handlers.Statistics
{
    public class OrderStatictsHandler
    {
        #region Properties

        private readonly DateTime _dateFrom;
        private readonly DateTime _dateTo;
        private readonly int? _statusId;
        private readonly bool? _paied;
        
        private EGroupDateBy _groupBy;
        private readonly string _groupFormatString;
        private readonly bool _useShippingCost;
        private readonly float _currencyValue = CurrencyService.CurrentCurrency.Rate;

        #endregion

        public OrderStatictsHandler(DateTime dateFrom, DateTime dateTo, int? statusId, bool? paied, bool useShippingCost, string groupFormatString)
        {
            _dateFrom = dateFrom;
            _dateTo = dateTo;
            _statusId = statusId;
            _paied = paied;
            _useShippingCost = useShippingCost;
            _groupFormatString = groupFormatString;
        }


        public object GetOrdersSum()
        {
            Filter();

            return new
            {
                chart = RenderOrdersSumGraph(_dateFrom, _dateTo),
                options =
                    string.Format("{{xaxis : {{ mode: 'time', timeformat: '%d %b', min: {0}, max: {1}}} }}",
                        GetTimestamp(_dateFrom), GetTimestamp(_dateTo))
            };
        }

        public object GetOrdersCount()
        {
            Filter();

            return new
            {
                chart = RenderOrdersCountGraph(_dateFrom, _dateTo),
                options =
                    string.Format("{{xaxis : {{ mode: 'time', timeformat: '%d %b', min: {0}, max: {1}}} }}",
                        GetTimestamp(_dateFrom), GetTimestamp(_dateTo))
            };
        }

        public object GetOrderReg()
        {
            Filter();

            return new
            {
                chart = RenderOrdersRegGraph(_dateFrom, _dateTo),
                options =
                    string.Format("{{xaxis : {{ mode: 'time', timeformat: '%d %b', min: {0}, max: {1}}} }}",
                        GetTimestamp(_dateFrom), GetTimestamp(_dateTo))
            };
        }

        
        #region Render methods

        private string RenderOrdersSumGraph(DateTime dateFrom, DateTime dateTo)
        {
            var listSum = OrderStatisticsService.GetOrdersSum(_groupFormatString, dateFrom, dateTo, _paied, _statusId, _useShippingCost);

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

            return String.Format("[{{label: '{0}', data:[{1}]}}]", Resource.Admin_Default_Orders, data);
        }

        private string RenderOrdersCountGraph(DateTime dateFrom, DateTime dateTo)
        {
            var listSum = OrderStatisticsService.GetOrdersCount(_groupFormatString, dateFrom, dateTo, _paied, _statusId);

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

            return String.Format("[{{label: '{0}', data:[{1}]}}]", Resource.Admin_Statistics_OrdersByCount, data);
        }

        private string RenderOrdersRegGraph(DateTime dateFrom, DateTime dateTo)
        {
            var listSumForReg = OrderStatisticsService.GetOrdersReg(_groupFormatString, dateFrom, dateTo, true, _paied, _statusId);
            var listSumForUnReg = OrderStatisticsService.GetOrdersReg(_groupFormatString, dateFrom, dateTo, false, _paied, _statusId);

            var dataReg = "";
            var dataUnReg = "";
            switch (_groupBy)
            {
                case EGroupDateBy.Day:
                    dataReg = GetByDays(listSumForReg, dateFrom, dateTo);
                    dataUnReg = GetByDays(listSumForUnReg, dateFrom, dateTo);
                    break;
                case EGroupDateBy.Week:
                    dataReg = GetByWeeks(listSumForReg, dateFrom, dateTo);
                    dataUnReg = GetByWeeks(listSumForUnReg, dateFrom, dateTo);
                    break;
                case EGroupDateBy.Month:
                    dataReg = GetByMonths(listSumForReg, dateFrom, dateTo);
                    dataUnReg = GetByMonths(listSumForUnReg, dateFrom, dateTo);
                    break;
            }

            return String.Format("[{{label: '{0}', data:[{1}]}}, {{label: '{2}', data:[{3}]}}]",
                Resource.Admin_Statistics_Registered, dataReg,
                Resource.Admin_Statistics_UnRegistered, dataUnReg);
        }
        
        #endregion

        #region Help methods

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

            if (prevDate != endDate)
            {
                var lastValue = list.Where(x => x.Key > prevDate && x.Key <= endDate).Sum(x => x.Value);
                resultList.Add(string.Format("[{0},{1}]", GetTimestamp(endDate), (lastValue/_currencyValue).ToString("F2").Replace(",", ".")));
            }

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

            if (prevDate != endDate)
            {
                var lastValue = list.Where(x => x.Key > prevDate && x.Key <= endDate).Sum(x => x.Value);
                resultList.Add(string.Format("[{0},{1}]", GetTimestamp(endDate), (lastValue/_currencyValue).ToString("F2").Replace(",", ".")));
            }

            return String.Join(",", resultList);
        }

        #endregion
    }
}