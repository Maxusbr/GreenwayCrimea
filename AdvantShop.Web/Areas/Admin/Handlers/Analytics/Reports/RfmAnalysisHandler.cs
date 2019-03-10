using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Orders;

namespace AdvantShop.Web.Admin.Handlers.Analytics.Reports
{
    public class RfmAnalysisHandler
    {
        private readonly DateTime _from;
        private readonly DateTime _to;

        private class RfmDataItem
        {
            public string Count { get; set; }
            public string Percent { get; set; }
        }

        public RfmAnalysisHandler(DateTime from, DateTime to)
        {
            _from = from;
            _to = to;
        }

        public object GetData()
        {
            var data = GetDataItems();
            if (data == null || data.Count == 0)
                return null;

            var totalCount = data.Count;

            var arrRm = new RfmDataItem[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    var count = data.Count(x => x.R == i + 1 && x.M == j + 1);
                    var percent = ((float)count / totalCount * 100).ToString("F2");

                    arrRm[i, j] = new RfmDataItem {Count = Numerals(count), Percent = percent};
                }
            }

            var arrRf = new RfmDataItem[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    var count = data.Count(x => x.R == i + 1 && x.F == j + 1);
                    var percent = ((float)count / totalCount * 100).ToString("F2");

                    arrRf[i, j] = new RfmDataItem {Count = Numerals(count), Percent = percent};
                }
            }

            return new
            {
                Rm = arrRm,
                Rf = arrRf
            };
        }


        public List<RfmAnalysisDataItem> GetDataItems()
        {
            var dataItems = OrderStatisticsService.GetRfmAnalysisDataItems(_from, _to);
            if (dataItems == null)
                return null;

            var now = _to.AddDays(1);
            var maxDays = Int32.MinValue;
            var maxOrdersCount = Int32.MinValue;
            var maxOrdersSum = float.MinValue;


            foreach (var dataItem in dataItems)
            {
                dataItem.DaysCount = (now - dataItem.LastOrderDate).Days;

                if (maxDays < dataItem.DaysCount)
                    maxDays = dataItem.DaysCount;

                if (maxOrdersCount < dataItem.OrdersCount)
                    maxOrdersCount = dataItem.OrdersCount;

                if (maxOrdersSum < dataItem.OrdersSum)
                    maxOrdersSum = dataItem.OrdersSum;
            }

            foreach (var dataItem in dataItems)
            {
                if (dataItem.DaysCount <= 0.2 * maxDays)
                    dataItem.R = 5;
                else if (dataItem.DaysCount <= 0.4 * maxDays)
                    dataItem.R = 4;
                else if (dataItem.DaysCount <= 0.6 * maxDays)
                    dataItem.R = 3;
                else if (dataItem.DaysCount <= 0.8 * maxDays)
                    dataItem.R = 2;
                else
                    dataItem.R = 1;

                if (dataItem.OrdersCount <= 0.2 * maxOrdersCount)
                    dataItem.F = 1;
                else if (dataItem.OrdersCount <= 0.4 * maxOrdersCount)
                    dataItem.F = 2;
                else if (dataItem.OrdersCount <= 0.6 * maxOrdersCount)
                    dataItem.F = 3;
                else if (dataItem.OrdersCount <= 0.8 * maxOrdersCount)
                    dataItem.F = 4;
                else
                    dataItem.F = 5;

                if (dataItem.OrdersSum <= 0.2 * maxOrdersSum)
                    dataItem.M = 1;
                else if (dataItem.OrdersSum <= 0.4 * maxOrdersSum)
                    dataItem.M = 2;
                else if (dataItem.OrdersSum <= 0.6 * maxOrdersSum)
                    dataItem.M = 3;
                else if (dataItem.OrdersSum <= 0.8 * maxOrdersSum)
                    dataItem.M = 4;
                else
                    dataItem.M = 5;
            }

            return dataItems;
        }

        private string Numerals(float count)
        {
            return count + " " + Strings.Numerals(count, "клиентов", "клиент", "клиента", "клиентов");
        }
    }
}