using System;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.Web.Admin.Models.Home;
using AdvantShop.Web.Admin.ViewModels.Home;

namespace AdvantShop.Web.Admin.Handlers.Home
{
    public class GetStatisticsDasboard
    {
        public StatisticsDashboardViewModel Execute()
        {
            var model = new StatisticsDashboardViewModel();

            var now = DateTime.Now;
            var currency = SettingsCatalog.DefaultCurrency;
            var settings = StatisticsDashboardSetting.Settings;

            if (settings.ShowTotalProducts)
            {
                model.AnyActive = true;
                model.ShowTotalProducts = true;
                model.TotalProductsCount = StatisticService.GetProductsCount();
            }

            if (settings.ShowTotalOrdersToday)
            {
                model.AnyActive = true;
                model.ShowTotalOrdersToday = true;
                model.TotalOrdersTodayCount = StatisticService.GetOrdersCountByDate(now);
                model.TotalOrdersTodayPrice = StatisticService.GetOrdersSumByDate(now).FormatPrice(currency);
            }

            if (settings.ShowTotalOrdersYesterday)
            {
                model.AnyActive = true;
                model.ShowTotalOrdersYesterday = true;
                model.TotalOrdersYesterdayCount = StatisticService.GetOrdersCountByDate(now.AddDays(-1));
                model.TotalOrdersYesterdayPrice = StatisticService.GetOrdersSumByDate(now.AddDays(-1)).FormatPrice(currency);
            }

            if (settings.ShowTotalOrdersMonth)
            {
                model.AnyActive = true;
                model.ShowTotalOrdersMonth = true;
                model.TotalOrdersMonthCount = StatisticService.GetOrdersCountByDateRange(new DateTime(now.Year, now.Month, 1), now);
                model.TotalOrdersMonthPrice = StatisticService.GetOrdersSumByDateRange(new DateTime(now.Year, now.Month, 1), now).FormatPrice(currency);
            }

            if (settings.ShowTotalOrdersAllTime)
            {
                model.AnyActive = true;
                model.ShowTotalOrdersAllTime = true;
                model.TotalOrdersAllTimeCount = StatisticService.GetOrdersCountByDateRange(now.AddYears(-100), now);
                model.TotalOrdersAllTimePrice = StatisticService.GetOrdersSumByDateRange(now.AddYears(-100), now).FormatPrice(currency);
            }


            // leads
            if (settings.ShowTotalLeadsToday)
            {
                model.AnyActive = true;
                model.ShowTotalLeadsToday = true;
                model.TotalLeadsTodayCount = StatisticService.GetLeadsCountByDate(now);
                model.TotalLeadsTodayPrice = StatisticService.GetLeadsSumByDate(now).FormatPrice(currency);
            }

            if (settings.ShowTotalLeadsYesterday)
            {
                model.AnyActive = true;
                model.ShowTotalLeadsYesterday = true;
                model.TotalLeadsYesterdayCount = StatisticService.GetLeadsCountByDate(now.AddDays(-1));
                model.TotalLeadsYesterdayPrice = StatisticService.GetLeadsSumByDate(now.AddDays(-1)).FormatPrice(currency);
            }

            if (settings.ShowTotalLeadsMonth)
            {
                model.AnyActive = true;
                model.ShowTotalLeadsMonth = true;
                model.TotalLeadsMonthCount = StatisticService.GetLeadsCountByDateRange(new DateTime(now.Year, now.Month, 1), now);
                model.TotalLeadsMonthPrice = StatisticService.GetLeadsSumByDateRange(new DateTime(now.Year, now.Month, 1), now).FormatPrice(currency);
            }


            // reviews
            if (settings.ShowTotalReviewsToday)
            {
                model.AnyActive = true;
                model.ShowTotalReviewsToday = true;
                model.TotalReviewsTodayCount = StatisticService.GetReviewsCountByDate(now);
            }

            if (settings.ShowTotalReviewsYesterday)
            {
                model.AnyActive = true;
                model.ShowTotalReviewsYesterday = true;
                model.TotalReviewsYesterdayCount = StatisticService.GetReviewsCountByDate(now.AddDays(-1));
            }

            if (settings.ShowTotalReviewsMonth)
            {
                model.AnyActive = true;
                model.ShowTotalReviewsMonth = true;
                model.TotalReviewsMonthCount = StatisticService.GetReviewsCountByDateRange(new DateTime(now.Year, now.Month, 1), now);
            }


            // calls
            if (settings.ShowTotalCallsToday)
            {
                model.AnyActive = true;
                model.ShowTotalCallsToday = true;
                model.TotalCallsTodayCount = StatisticService.GetCallsCountByDate(now);
            }

            if (settings.ShowTotalCallsYesterday)
            {
                model.AnyActive = true;
                model.ShowTotalCallsYesterday = true;
                model.TotalCallsYesterdayCount = StatisticService.GetCallsCountByDate(now.AddDays(-1));
            }

            if (settings.ShowTotalCallsMonth)
            {
                model.AnyActive = true;
                model.ShowTotalCallsMonth = true;
                model.TotalCallsMonthCount = StatisticService.GetCallsCountByDateRange(new DateTime(now.Year, now.Month, 1), now);
            }


            model.TelephonyConfigured = SettingsTelephony.CurrentIPTelephonyOperatorType != Core.Services.IPTelephony.EOperatorType.None;

            return model;
        }
    }
}
