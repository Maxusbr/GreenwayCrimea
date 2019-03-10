using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.SessionState;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.ExportImport;
using AdvantShop.ExportImport.Excel;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Analytics;
using AdvantShop.Web.Admin.Handlers.Analytics.Statistics;
using AdvantShop.Web.Admin.Handlers.Analytics.Reports;
using AdvantShop.Web.Admin.Models.Analytics;
using AdvantShop.Web.Admin.ViewModels.Analytics;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Shipping;
using AdvantShop.Repository;

namespace AdvantShop.Web.Admin.Controllers.Marketing
{
    [Auth(RoleAction.Marketing)]
    [SaasFeature(Saas.ESaasProperty.DeepAnalytics)]
    public partial class AnalyticsController : BaseAdminController
    {
        #region Analytics report

        public ActionResult Index()
        {
            var model = new AnalyticsReportModel();

            SetMetaInformation("Аналитика. Сводный отчет.");
            SetNgController(NgControllers.NgControllersTypes.AnalyticsReportCtrl);

            return View(model);
        }


        public JsonResult GetVortex(string datefrom, string dateto)
        {
            var from = datefrom.TryParseDateTime();
            var to = dateto.TryParseDateTime();
            to = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59);

            var result = new VortexHandler(from, to).Execute();
            return Json(result);
        }

        public JsonResult GetProfit(string type, string datefrom, string dateto, bool? paid, int? orderStatus, bool useShippingCost, string groupFormatString)
        {
            var from = datefrom.TryParseDateTime();
            var to = dateto.TryParseDateTime();
            to = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59);

            object result = null;

            var handler = new OrderStatictsHandler(from, to, orderStatus, paid, useShippingCost, groupFormatString);
            switch (type)
            {
                case "sum":
                    result = handler.GetOrdersSum();
                    break;
                case "count":
                    result = handler.GetOrdersCount();
                    break;

            }
            return Json(result);
        }

        public JsonResult GetAvgCheck(string type, string datefrom, string dateto, bool? paid, int? orderStatus, string groupFormatString)
        {
            var from = datefrom.TryParseDateTime();
            var to = dateto.TryParseDateTime();
            to = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59);

            object result = null;

            var handler = new AvgCheckHandler(from, to, orderStatus, paid, groupFormatString);
            switch (type)
            {
                case "avg":
                    result = handler.GetAvgCheck();
                    break;
                case "city":
                    result = handler.GetAvgCheckByCity();
                    break;
            }

            return Json(result);
        }

        public JsonResult GetOrders(string type, string datefrom, string dateto, bool? paid, int? orderStatus, string groupFormatString)
        {
            var from = datefrom.TryParseDateTime();
            var to = dateto.TryParseDateTime();
            to = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59);

            object result = null;

            var handler = new OrdersByHandler(from, to, orderStatus, paid, groupFormatString);
            switch (type)
            {
                case "payments":
                    result = handler.GetPayments();
                    break;
                case "shippings":
                    result = handler.GetShippings();
                    break;
                case "statuses":
                    result = handler.GetStatuses();
                    break;
                case "sources":
                    result = handler.GetOrderTypes();
                    break;
                case "repeatorders":
                    result = handler.GetRepeatOrders();
                    break;
            }

            return Json(result);
        }

        public JsonResult GetTelephony(string type, string datefrom, string dateto, string groupFormatString)
        {
            var from = datefrom.TryParseDateTime();
            var to = dateto.TryParseDateTime();
            to = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59);

            object result = null;

            var handler = new TelephonyHandler(from, to, groupFormatString);
            switch (type)
            {
                case "in":
                    result = handler.GetCallsCount(ECallType.In);
                    break;
                case "missed":
                    result = handler.GetCallsCount(ECallType.Missed);
                    break;
                case "out":
                    result = handler.GetCallsCount(ECallType.Out);
                    break;
                case "avgtime":
                    result = handler.GetAvgDuration();
                    break;
            }

            return Json(result);
        }

        public JsonResult GetRfm(string datefrom, string dateto)
        {
            var from = datefrom.TryParseDateTime();
            var to = dateto.TryParseDateTime();
            to = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59);

            var handler = new RfmAnalysisHandler(from, to);
            return Json(handler.GetData());
        }

        public JsonResult GetManagers(string datefrom, string dateto)
        {
            var from = datefrom.TryParseDateTime();
            var to = dateto.TryParseDateTime();
            to = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59);

            var result = new ManagersHandler(from, to).Execute();
            return Json(result);
        }

        public JsonResult GetAbcxyzAnalysis(string datefrom, string dateto)
        {
            var from = datefrom.TryParseDateTime();
            var to = dateto.TryParseDateTime();
            to = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59);

            var result = new AbcxyzAnalysisHandler(from, to).Execute();
            return Json(result);
        }


        #endregion

        #region Analytics Filter

        public ActionResult AnalyticsFilter(AnalyticsFilterModel data)
        {
            SetMetaInformation("Аналитика. Сводный отчет.");
            SetNgController(NgControllers.NgControllersTypes.AnalyticsFilterCtrl);

            return View(data);
        }

        public JsonResult GetAnalyticsFilterAbcxyz(AnalyticsFilterModel data, BaseFilterModel filter)
        {
            var from = data.From.TryParseDateTime();
            var to = data.To.TryParseDateTime();
            to = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59);

            var handler = new GetAnalyticsFilterAbcxyz(filter, from, to, data.Group);
            return Json(handler.Execute());
        }

        public JsonResult GetAnalyticsFilterRfm(AnalyticsFilterModel data, BaseFilterModel filter)
        {
            var from = data.From.TryParseDateTime();
            var to = data.To.TryParseDateTime();
            to = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59);

            var handler = new GetAnalyticsFilterRfm(new BaseFilterModel(), data.Group, from, to);
            return Json(handler.Execute());
        }

        #endregion

        #region ExportOrders

        public ActionResult ExportOrders()
        {
            SetNgController(NgControllers.NgControllersTypes.AnalyticsCtrl);
            SetMetaInformation(T("Admin.Marketing.Analytics.ExportOrders"));

            var model = new ExportOrdersModel();

            var encodings = new Dictionary<string, string>();
            foreach (var enumItem in (EncodingsEnum[])Enum.GetValues(typeof(EncodingsEnum)))
            {
                encodings.Add(enumItem.StrName(), enumItem.StrName());
            }

            var orderStatuses = new Dictionary<int, string>();
            foreach (var status in OrderStatusService.GetOrderStatuses())
            {
                orderStatuses.Add(status.StatusID, status.StatusName);
            }



            model.PaidStatuses = new Dictionary<bool, string>
            {
                {true, "Оплачен" },
                {false, "Не оплачен" }
            };

            model.Shippings = new Dictionary<int, string>();
            foreach (var shipping in ShippingMethodService.GetAllShippingMethods())
            {
                model.Shippings.Add(shipping.ShippingMethodId, shipping.Name);
            }

            model.Cities = new Dictionary<string, string>();
            foreach (var city in CityService.GetAll())
            {
                if (!model.Cities.Any(item => item.Key == city.Name))
                {
                    model.Cities.Add(city.Name, city.Name);
                }
            }         

            model.Paid = true;
            
            model.Encoding = EncodingsEnum.Windows1251.StrName();
            model.Encodings = encodings;
            model.OrderStatuses = orderStatuses;
            model.Status = orderStatuses != null && orderStatuses.Count > 0 ? orderStatuses.FirstOrDefault().Key : 0;

            return View(model);
        }

        [HttpPost]
        public JsonResult ExportOrders(ExportOrdersModel settings)
        {
            new ExportOrdersHandler(settings).Execute();
            return Json(new { result = true });
        }

        #endregion

        #region ExportOrder to xlsx
        [SaasFeature(Saas.ESaasProperty.HaveExcel)]
        public ActionResult ExportOrder(int orderId)
        {
            var order = OrderService.GetOrder(orderId);
            if (order == null)
                return Content("Заказ не найден");

            try
            {
                order.OrderItems = order.OrderItems.OrderByDescending(x => x.Name).ToList();
                if (order.OrderItems.Count > 1)
                {
                    var temp = order.OrderItems[order.OrderItems.Count - 1];
                    var orderItems = order.OrderItems.Where(x => x.OrderItemID != temp.OrderItemID).OrderByDescending(x => x.Name).ToList();
                    order.OrderItems.Clear();
                    order.OrderItems.Add(temp);
                    order.OrderItems.AddRange(orderItems.ToArray());
                }

                string strPath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
                FileHelpers.CreateDirectory(strPath);

                var filename = string.Format("Order{0}.xlsx", order.Number.RemoveSymbols());
                var templatePath = Server.MapPath(ExcelExport.templateSingleOrder);

                ExcelExport.SingleOrder(templatePath, strPath + filename, order);

                return File(strPath + filename, "application/octet-stream", filename);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return Content("error");
        }

        #endregion

        #region ExportProducts

        public ActionResult ExportProducts()
        {
            SetNgController(NgControllers.NgControllersTypes.AnalyticsCtrl);
            SetMetaInformation(T("Admin.Marketing.Analytics.ExportProducts"));

            var encodings = new Dictionary<string, string>();
            foreach (var enumItem in (EncodingsEnum[])Enum.GetValues(typeof(EncodingsEnum)))
            {
                encodings.Add(enumItem.StrName(), enumItem.StrName());
            }

            var separators = new Dictionary<string, string>();
            foreach (var enumItem in (SeparatorsEnum[])Enum.GetValues(typeof(SeparatorsEnum)))
            {
                separators.Add(enumItem.StrName(), enumItem.Localize());
            }

            return View(new ExportProductsModel
            {
                Encoding = EncodingsEnum.Windows1251.StrName(),
                Encodings = encodings,
                ColumnSeparator = SeparatorsEnum.SemicolonSeparated.StrName(),
                Separators = separators
            });
        }

        [HttpPost]
        public ActionResult ExportProducts(ExportProductsModel settings)
        {
            new ExportProductsHandler(settings).Execute();
            return Json(new { result = true });
        }

        #endregion

        #region ExportCustomers

        public ActionResult ExportCustomers()
        {
            SetNgController(NgControllers.NgControllersTypes.AnalyticsCtrl);
            SetMetaInformation(T("Admin.Marketing.Analytics.ExportCustomers"));

            var encodings = new Dictionary<string, string>();
            foreach (var enumItem in (EncodingsEnum[])Enum.GetValues(typeof(EncodingsEnum)))
            {
                encodings.Add(enumItem.StrName(), enumItem.StrName());
            }

            var customerGroups = new Dictionary<int, string>() { { -1, "Все" } };
            foreach (var group in CustomerGroupService.GetCustomerGroupList())
            {
                customerGroups.Add(group.CustomerGroupId, group.GroupName);
            }

            var separators = new Dictionary<string, string>();
            foreach (var enumItem in (SeparatorsEnum[])Enum.GetValues(typeof(SeparatorsEnum)))
            {
                separators.Add(enumItem.StrName(), enumItem.Localize());
            }

            return View(new ExportCustomersModel
            {
                Group = -1,
                Encoding = EncodingsEnum.Windows1251.StrName(),
                PropertySeparator = ";",
                ColumnSeparator = SeparatorsEnum.SemicolonSeparated.StrName(),

                Encodings = encodings,
                Groups = customerGroups,
                Separators = separators
            });
        }

        [HttpPost]
        public JsonResult ExportCustomers(ExportCustomersModel settings)
        {
            new ExportCustomersHandler(settings).Execute();
            return Json(new { result = true });
        }

        #endregion
    }
}
