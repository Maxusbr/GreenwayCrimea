using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Areas.AdminMobile.Models.Home;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Statistic;
using AdvantShop.FilePath;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.SEO;
using AdvantShop.Customers;

namespace AdvantShop.Areas.AdminMobile.Controllers
{
    public partial class HomeController : BaseAdminMobileController
    {
        // GET: AdminMobile/
        public ActionResult Index()
        {
            var plannedSales = OrderStatisticsService.SalesPlan;
            var sales = OrderStatisticsService.GetMonthProgress().Key;
            var planPercent = (float) Math.Round(sales/(plannedSales/100));
            var statsDate = DateTime.Now;

            var model = new HomeOrdersModel()
            {
                CurrentCurrency = CurrencyService.CurrentCurrency,
                Sales = sales.FormatPrice(),
                PlanPercent = planPercent,
            };

            if (HasRole(RoleAction.Orders))
            {
                model.OrderStatuses = OrderStatusService.GetOrderStatuses();
                model.DailyOrdersSum =
                    StatisticService.GetOrdersSumByDate(statsDate)
                        .FormatPrice()
                        .Trim(CurrencyService.CurrentCurrency.Symbol.ToCharArray());
                model.DailyOrdersCount = StatisticService.GetOrdersCountByDate(statsDate);
                model.DailyVisitors = "N/A";
                model.AllOrdersCount = StatisticService.GetOrdersCount();
            }
            
            if (SettingsSEO.GoogleAnalyticsApiEnabled)
            {
                var data = GoogleAnalyticsService.GetData();
                if (data != null && data.ContainsKey(statsDate.Date))
                {
                    model.DailyVisitors = data[statsDate.Date].Visitors.ToString();
                }
            }

            SetMetaInformation("Панель администрирования");

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult Logo()
        {
            var model = new LogoAdminMobileModel()
            {
                Link = Url.RouteUrl("AdminMobile_Home"),
                ImageSrc = SettingsMain.LogoImageName.IsNullOrEmpty()
                                ? "images/nophoto-logo.png"
                                : FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.LogoImageName, false)
            };

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Header(string className, string href, string title)
        {
            var model = new HeaderModel()
            {
                ClassName = className,
                Href = href,
                Title = title,
                AnyNews =
                    CustomerContext.CurrentCustomer.IsManager &&
                    (ManagerTaskService.GetManagerTasksCountByManagerId(ManagerTaskStatus.Opened,
                        ManagerService.GetManager(CustomerContext.CurrentCustomer.Id).ManagerId) > 0 ||
                     OrderStatusService.GetOrderCountByStatusIdAndManagerId(OrderStatusService.DefaultOrderStatus,
                         ManagerService.GetManager(CustomerContext.CurrentCustomer.Id).ManagerId) > 0)
            };
            return PartialView("_Header", model);
        }
    }
}