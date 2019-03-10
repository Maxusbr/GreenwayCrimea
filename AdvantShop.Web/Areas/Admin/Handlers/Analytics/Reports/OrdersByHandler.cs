using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Web.Admin.Models.Common;

namespace AdvantShop.Web.Admin.Handlers.Analytics.Reports
{
    public class OrdersByHandler : AnalyticsBaseHandler
    {
        private readonly DateTime _dateFrom;
        private readonly DateTime _dateTo;
        private readonly int? _statusId;
        private readonly bool? _paied;
        
        private EGroupDateBy _groupBy;
        private readonly string _groupFormatString;
               

        public OrdersByHandler(DateTime dateFrom, DateTime dateTo, int? statusId, bool? paied, string groupFormatString)
        {
            _dateFrom = dateFrom;
            _dateTo = dateTo;
            _statusId = statusId;
            _paied = paied;
            _groupFormatString = groupFormatString ?? "dd";

            _groupBy = Filter(_groupFormatString);
        }

        public ChartDataJsonModel GetPayments()
        {
            var payments =
                OrderStatisticsService.GetPayments(_dateFrom, _dateTo, _paied, _statusId)
                    .OrderByDescending(x => x.Sum)
                    .ToList();

            var paymentStat = payments.Take(9).ToList();

            if (payments.Count >= 10)
                paymentStat.Add(new StatisticsDataItem()
                {
                    Name = "Другие",
                    Count = payments.Skip(9).Sum(x => x.Count),
                    Sum = payments.Skip(9).Sum(x => x.Sum)
                });

            //return String.Format("[ {{ 'key': '{0}', 'values': [{1}], {2} }} ]",
            //    "",
            //    paymentStat.Aggregate("",
            //        (current, payment) =>
            //            current +
            //            string.Format("{{ 'label': \"{0}\",  'value':{1}, 'count':'{2}'}},",
            //                string.IsNullOrEmpty(payment.Name) ? "n/a" : payment.Name.Reduce(30), 
            //                payment.Sum.ToInvariantString(),
            //                payment.Count)),
            //   _format);
            return new ChartDataJsonModel()
            {
                Data = new List<object>() { paymentStat.Select(x => x.Sum) },
                Labels = paymentStat.Select(x => !string.IsNullOrWhiteSpace(x.Name) ? x.Name.Reduce(30) : "n/a").ToList(),
                Series = new List<string>() { "Методы оплаты" }
            };
        }

        public ChartDataJsonModel GetShippings()
        {
            var shippings =
                OrderStatisticsService.GetShippings(_dateFrom, _dateTo, _paied, _statusId)
                    .OrderByDescending(x => x.Sum)
                    .ToList();

            var shippingStat = shippings.Take(9).ToList();

            if (shippings.Count >= 10)
                shippingStat.Add(new StatisticsDataItem()
                {
                    Name = "Другие",
                    Count = shippings.Skip(9).Sum(x => x.Count),
                    Sum = shippings.Skip(9).Sum(x => x.Sum)
                });

            //return String.Format("[ {{ 'key': '{0}', 'values': [{1}], {2} }} ]",
            //    "",
            //    shippingStat.Aggregate("",
            //        (current, shipping) =>
            //            current +
            //            string.Format("{{ 'label': '{0}',  'value':{1}, 'count':'{2}'}},",
            //                string.IsNullOrEmpty(shipping.Name) ? "n/a" : shipping.Name.Reduce(30),
            //                shipping.Sum.ToInvariantString(),
            //                shipping.Count)),
            //    _format);

            return new ChartDataJsonModel()
            {
                Data = new List<object>() { shippingStat.Select(x => x.Sum) },
                Labels = shippingStat.Select(x => !string.IsNullOrWhiteSpace(x.Name) ? x.Name.Reduce(30) : "n/a").ToList(),
                Series = new List<string>() { "Методы доставки" }
            };
        }

        public ChartDataJsonModel GetStatuses()
        {
            var statuses =
                OrderStatisticsService.GetOrdersByStatus(_dateFrom, _dateTo, _paied, _statusId)
                    .OrderByDescending(x => x.Value);

            //return String.Format("[ {{ 'key': '{0}', 'values': [{1}] }} ]",
            //    "",
            //    statuses.Aggregate("",
            //        (current, status) =>
            //            current + string.Format("{{ 'label': '{0}',  'value':{1}}},", status.Key.Reduce(30), status.Value)));
            return new ChartDataJsonModel()
            {
                Data = new List<object>() { statuses.Select(x => x.Value) },
                Labels = statuses.Select(x => !string.IsNullOrWhiteSpace(x.Key) ? x.Key.Reduce(30) : "n/a").ToList(),
                Series = new List<string>() { "Статусы заказов" }
            };
        }

        public ChartDataJsonModel GetCities()
        {
            var cities = OrderStatisticsService.GetTopCities().OrderByDescending(x => x.Value);

            //return String.Format("[ {{ 'key': '{0}', 'values': [{1}] }} ]",
            //    "",
            //    cities.Aggregate("",
            //        (current, city) =>
            //            current + string.Format("{{ 'label': '{0}',  'value':{1}}},", city.Key.Reduce(30), city.Value)));
            return new ChartDataJsonModel()
            {
                Data = new List<object>() { cities.Select(x => x.Value) },
                Labels = cities.Select(x => !string.IsNullOrWhiteSpace(x.Key) ? x.Key.Reduce(30) : "n/a").ToList(),
                Series = new List<string>() { "Города" }
            };
        }

        public object GetOrderTypes()
        {
            var types =
                OrderStatisticsService.GetOrdersByOrderType(_dateFrom, _dateTo, _paied, _statusId)
                    .OrderByDescending(x => x.Value);

            //return String.Format("[ {{ 'key': '{0}', 'values': [{1}] }} ]",
            //    "",
            //    types.Aggregate("",
            //        (current, type) =>
            //            current +
            //            string.Format("{{ 'label': '{0}',  'value':{1}}},",
            //                type.Key.Reduce(30), type.Value)));
            return new ChartDataJsonModel()
            {
                Data = new List<object>() { types.Select(x => x.Value) },
                Labels = types.Select(x => !string.IsNullOrWhiteSpace(x.Key) ? x.Key.Reduce(30) : "n/a").ToList(),
                Series = new List<string>() { "Способы оформления" }
            };
        }

        public object GetRepeatOrders()
        {
            //Filter();

            //return new
            //{
            //    chart = RenderRepeatOrdersGraph(),
            //    options =
            //        string.Format("{{xaxis : {{ mode: 'time', timeformat: '%d %b', min: {0}, max: {1}}} }}",
            //            GetTimestamp(_dateFrom), GetTimestamp(_dateTo))
            //};
            var list = OrderStatisticsService.GetRepeatOrders(_groupFormatString, _dateFrom, _dateTo, _paied, _statusId);

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
                Series = new List<string>() { "Повторные заказы" }
            };
        }
    }
}