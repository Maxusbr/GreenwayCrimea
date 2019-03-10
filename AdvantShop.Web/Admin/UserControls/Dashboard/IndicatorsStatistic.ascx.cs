using System;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.Localization;
using AdvantShop.Core.Common.Extensions;

namespace Admin.UserControls.Dashboard
{
    public partial class Admin_UserControls_IndicatorsStatistic : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            lblSaleToday.Text = Convert.ToString(StatisticService.GetOrdersCountByDate(now));
            lSumToday.Text = PriceFormatService.FormatPrice(StatisticService.GetOrdersSumByDate(now), SettingsCatalog.DefaultCurrency);

            lblSaleYesterday.Text = Convert.ToString(StatisticService.GetOrdersCountByDate(now.AddDays(-1)));
            lSumYesterday.Text = PriceFormatService.FormatPrice(StatisticService.GetOrdersSumByDate(now.AddDays(-1)));

            lblSaleWeek.Text = Convert.ToString(
                    StatisticService.GetOrdersCountByDateRange(
                        now.StartOfWeek(Culture.Language == Culture.SupportLanguage.English
                                            ? DayOfWeek.Sunday
                                            : DayOfWeek.Monday), now));
            lSumWeek.Text =
                PriceFormatService.FormatPrice(
                    StatisticService.GetOrdersSumByDateRange(
                        now.StartOfWeek(Culture.Language == Culture.SupportLanguage.English
                                            ? DayOfWeek.Sunday
                                            : DayOfWeek.Monday), now), SettingsCatalog.DefaultCurrency);

            lblSaleMounth.Text = Convert.ToString(StatisticService.GetOrdersCountByDateRange(new DateTime(now.Year, now.Month, 1), now));
            lSumMonth.Text = PriceFormatService.FormatPrice(StatisticService.GetOrdersSumByDateRange(new DateTime(now.Year, now.Month, 1), now), SettingsCatalog.DefaultCurrency);

            lblSale.Text = Convert.ToString(StatisticService.GetOrdersCountByDateRange(now.AddYears(-100), now));
            lSaleSum.Text = PriceFormatService.FormatPrice(StatisticService.GetOrdersSumByDateRange(now.AddYears(-100), now), SettingsCatalog.DefaultCurrency);

            lblTotalProducts.Text = Convert.ToString(StatisticService.GetProductsCount());
        }
    }
}