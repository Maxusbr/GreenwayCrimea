using System;
using System.Collections.Generic;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using Resources;

namespace Admin.UserControls.Dashboard
{
    public partial class BigOrdersChart : System.Web.UI.UserControl
    {
        protected DateTime Now = DateTime.Now;
        
        private string GetByDays(Dictionary<DateTime, float> list, DateTime startDate, DateTime endDate)
        {
            var resultList = new List<string>();

            var valCurrency = CurrencyService.CurrentCurrency.Rate;
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

                resultList.Add(string.Format("[{0},{1}]", GetTimestamp(profit.Key), profit.Value/valCurrency));
                tempDate = profit.Key;
            }

            if (tempDate == DateTime.MinValue)
                tempDate = startDate;

            var endDayOffset = (endDate - tempDate).Days;
            for (var i = 1; i <= endDayOffset; i++)
            {
                resultList.Add(string.Format("[{0},{1}]", GetTimestamp(tempDate.AddDays(i)), 0));
            }

            return String.Join(",", resultList);
        }

        private string GetByMonths(Dictionary<DateTime, float> list, DateTime startDate, DateTime endDate)
        {
            var resultList = new List<string>();

            var valCurrency = CurrencyService.CurrentCurrency.Rate;
            var tempDate = DateTime.MinValue;

            startDate = new DateTime(startDate.Year, startDate.Month, 1);
            endDate = new DateTime(endDate.Year, endDate.Month, 1);

            foreach (var profit in list)
            {
                if (tempDate != DateTime.MinValue)
                {
                    var dayOffset = (profit.Key - tempDate).Days/30;
                    for (var i = 1; i < dayOffset; i++)
                    {
                        resultList.Add(string.Format("[{0},{1}]", GetTimestamp(tempDate.AddMonths(i)), 0));
                    }
                }
                else
                {
                    var dayOffset = (profit.Key - startDate).Days/30;
                    for (var i = 1; i < dayOffset; i++)
                    {
                        resultList.Add(string.Format("[{0},{1}]", GetTimestamp(startDate.AddMonths(i)), 0));
                    }
                }

                resultList.Add(string.Format("[{0},{1}]", GetTimestamp(profit.Key), profit.Value / valCurrency));
                tempDate = profit.Key;
            }

            if (tempDate == DateTime.MinValue)
                tempDate = startDate;

            var endDayOffset = (endDate - tempDate).Days/30;
            for (var i = 1; i <= endDayOffset; i++)
            {
                resultList.Add(string.Format("[{0},{1}]", GetTimestamp(tempDate.AddMonths(i)), 0));
            }

            return String.Join(",", resultList);
        }

        protected string RenderDataByDays(DateTime date)
        {
            var listProfit = OrderStatisticsService.GetOrdersProfitByDays(date, Now);
            var listSum = OrderStatisticsService.GetOrdersSumByDays(date, Now);

            return String.Format("[{{label: '{0}', data:[{1}]}}, {{label: '{2}', data:[{3}]}}]",
                                    Resource.Admin_Chart_Profit, GetByDays(listProfit, date, Now),
                                    Resource.Admin_Default_Orders, GetByDays(listSum, date, Now));
        }

        protected string RenderDataByMonths(DateTime date)
        {
            var listProfit = OrderStatisticsService.GetOrdersProfitByDays(date, Now);
            var listSum = OrderStatisticsService.GetOrdersSumByDays(date, Now);

            return String.Format("[{{label: '{0}', data:[{1}]}}, {{label: '{2}', data:[{3}]}}]",
                                    Resource.Admin_Chart_Profit, GetByMonths(listProfit, date, Now),
                                    Resource.Admin_Default_Orders, GetByMonths(listSum, date, Now));
        }

        protected static long GetTimestamp(DateTime date)
        {
            TimeSpan span = (date - new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (long)(span.TotalSeconds * 1000);
        }
    }
}