using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.Models.Common;

namespace AdvantShop.Web.Admin.Handlers.Analytics.Reports
{
    public class OrderStatictsHandler : AnalyticsBaseHandler
    {
        private readonly DateTime _dateFrom;
        private readonly DateTime _dateTo;
        private readonly int? _statusId;
        private readonly bool? _paid;
        
        private EGroupDateBy _groupBy;
        private readonly string _groupFormatString;
        private readonly bool _useShippingCost;


        public OrderStatictsHandler(DateTime dateFrom, DateTime dateTo, int? statusId, bool? paid, bool useShippingCost, string groupFormatString)
        {
            _dateFrom = dateFrom;
            _dateTo = dateTo;
            _statusId = statusId;
            _paid = paid;
            _useShippingCost = useShippingCost;
            _groupFormatString = groupFormatString ?? "dd";
            
            _groupBy = Filter(_groupFormatString);
        }
        

        public ChartDataJsonModel GetOrderReg()
        {
            var listSumForReg = OrderStatisticsService.GetOrdersReg(_groupFormatString, _dateFrom, _dateTo, true, _paid, _statusId);
            var listSumForUnReg = OrderStatisticsService.GetOrdersReg(_groupFormatString, _dateFrom, _dateTo, false, _paid, _statusId);

            var dataReg = new Dictionary<DateTime, float>();
            var dataUnReg = new Dictionary<DateTime, float>();

            switch (_groupBy)
            {
                case EGroupDateBy.Day:
                    dataReg = GetByDays(listSumForReg, _dateFrom, _dateTo);
                    dataUnReg = GetByDays(listSumForUnReg, _dateFrom, _dateTo);
                    break;
                case EGroupDateBy.Week:
                    dataReg = GetByWeeks(listSumForReg, _dateFrom, _dateTo);
                    dataUnReg = GetByWeeks(listSumForUnReg, _dateFrom, _dateTo);
                    break;
                case EGroupDateBy.Month:
                    dataReg = GetByMonths(listSumForReg, _dateFrom, _dateTo);
                    dataUnReg = GetByMonths(listSumForUnReg, _dateFrom, _dateTo);
                    break;
            }

            return new ChartDataJsonModel()
            {
                Data = new List<object>()
                {
                    dataReg.Values.Select(x => x),
                    dataUnReg.Values.Select(x => x)
                },
                Labels = dataReg.Keys.Select(x => x.ToString("d MMM")).ToList(),
                Series = new List<string>() {"reg", "unreg"}
            };
        }

        public ChartDataJsonModel GetOrdersSum()
        {
            var listSum = OrderStatisticsService.GetOrdersSum(_groupFormatString, _dateFrom, _dateTo, _paid, _statusId, _useShippingCost);

            var data = new Dictionary<DateTime, float>();
            switch (_groupBy)
            {
                case EGroupDateBy.Day:
                    data = GetByDays(listSum, _dateFrom, _dateTo);
                    break;
                case EGroupDateBy.Week:
                    data = GetByWeeks(listSum, _dateFrom, _dateTo);
                    break;
                case EGroupDateBy.Month:
                    data = GetByMonths(listSum, _dateFrom, _dateTo);
                    break;
            }

            return new ChartDataJsonModel()
            {
                Data = new List<object>(){data.Values.Select(x => x)},
                Labels = data.Keys.Select(x => x.ToString("d MMM")).ToList(),
                Series = new List<string>() { "Заказы" }
            };
        }

        public ChartDataJsonModel GetOrdersCount()
        {
            var listSum = OrderStatisticsService.GetOrdersCount(_groupFormatString, _dateFrom, _dateTo, _paid, _statusId);

            var data = new Dictionary<DateTime, float>();
            switch (_groupBy)
            {
                case EGroupDateBy.Day:
                    data = GetByDays(listSum, _dateFrom, _dateTo);
                    break;
                case EGroupDateBy.Week:
                    data = GetByWeeks(listSum, _dateFrom, _dateTo);
                    break;
                case EGroupDateBy.Month:
                    data = GetByMonths(listSum, _dateFrom, _dateTo);
                    break;
            }

            return new ChartDataJsonModel()
            {
                Data = new List<object>() { data.Values.Select(x => x) },
                Labels = data.Keys.Select(x => x.ToString("d MMM")).ToList(),
                Series = new List<string>() { "Количество заказов" }
            };
        }
    }
}