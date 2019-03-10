using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Areas.AdminMobile.Models.Orders;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL2;
using AdvantShop.Customers;
using AdvantShop.Mails;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Saas;
using AdvantShop.Web.Admin.Filters;
using Resources;

namespace AdvantShop.Areas.AdminMobile.Controllers
{
    [SaasFeature(ESaasProperty.HaveCrm)]
    [Auth(RoleAction.Orders)]
    public class OrdersController : BaseAdminMobileController
    {
        public ActionResult Index(int? statusId)
        {
            SetMetaInformation(Resource.AdminMobile_Orders);
            return View();
        }
        
        public ActionResult OrderItem(int orderId)
        {
            var order = OrderService.GetOrder(orderId);
            if (order == null || !OrderService.CheckAccess(order))
                return new EmptyResult();
            
            var model = new OrderItemModel
            {
                Order = order,
                Statuses = OrderStatusService.GetOrderStatuses()
                    .Select(os => new SelectListItem
                    {
                        Value = os.StatusID.ToString(),
                        Text = os.StatusName,
                        Selected = order.OrderStatusId == os.StatusID
                    }).ToList(),
            };

            SetMetaInformation(Resource.AdminMobile_Order + " №" + order.OrderID);

            return View(model);
        }

        public JsonResult SearchOrder(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return null;

            var orders = OrderService.GetOrdersForAutocomplete(q).Select(x => new OrderModel()
            {
                OrderId = x.OrderID,
                Number = x.Number,
                FirstName = x.FirstName,
                LastName = x.LastName,
                OrderDate = x.OrderDate,
                Sum = x.Sum
            });

            return Json(orders);
        } 

        public JsonResult GetLastOrders(int page, int? statusId, string number)
        {
            if (page == 0)
                page = 1;

            var paging = new SqlPaging(page, 10);
            paging.Select(
                "[Order].[OrderID]",
                "[Order].[Number]",
                "[Order].[Sum]",
                "[OrderCustomer].[FirstName]",
                "[OrderCustomer].[LastName]",
                "[OrderStatus].[StatusName]",
                "[OrderStatus].[Color]"
                );

            paging.From("[Order].[Order]");
            paging.Left_Join("[Order].[OrderCustomer] ON [OrderCustomer].[OrderId] = [Order].[OrderID]");
            paging.Left_Join("[Order].[OrderStatus] ON [OrderStatus].[OrderStatusId] = [Order].[OrderStatusID]");

            if (statusId.HasValue)
            {
                paging.Where("[Order].[OrderStatusID] = {0}", statusId);
            }
            
            if (!string.IsNullOrEmpty(number))
            {
                paging.Where(
                    (statusId.HasValue ? "AND " : string.Empty) +
                    "[Order].[Number] LIKE '%' + {0} + '%'", number);
            }

            var customer = CustomerContext.CurrentCustomer;

            if (customer.IsModerator)
            {
                var manager = ManagerService.GetManager(customer.Id);
                if (manager != null && manager.Active)
                {
                    if (SettingsManager.ManagersOrderConstraint == ManagersOrderConstraint.Assigned)
                    {
                        paging.Where("[Order].ManagerId = {0}", manager.ManagerId);
                    }
                    else if (SettingsManager.ManagersOrderConstraint == ManagersOrderConstraint.AssignedAndFree)
                    {
                        paging.Where("([Order].ManagerId = {0} or [Order].ManagerId is null)", manager.ManagerId);
                    }
                }
            }

            paging.OrderByDesc("OrderDate".AsSqlField("OrderDate"));

            var items = paging.PageItemsList<OrderModel>();

            return Json(items);
        }

        //Оставил paging потому что высока вероятность того что попросят искать по еще какому-то полю кроме Id
        public JsonResult GetOrders(string number)
        {
            var paging = new SqlPaging();

            paging.Select(
                "[Order].[OrderID]",
                "[Order].[Number]",
                "[Order].[Sum]",
                "[OrderCustomer].[FirstName]",
                "[OrderCustomer].[LastName]",
                "[OrderStatus].[StatusName]",
                "[OrderStatus].[Color]"
                );

            paging.From("[Order].[Order]");
            paging.Left_Join("[Order].[OrderCustomer] ON [OrderCustomer].[OrderId] = [Order].[OrderID]");
            paging.Left_Join("[Order].[OrderStatus] ON [OrderStatus].[OrderStatusId] = [Order].[OrderStatusID]");

            if (!string.IsNullOrEmpty(number))
            {
                paging.Where("[Order].[Number] LIKE '%' + {0} + '%'", number);
            }

            paging.OrderByDesc("OrderDate".AsSqlField("OrderDate"));

            var items = paging.PageItemsList<OrderModel>();

            return Json(items);
        }

        public JsonResult ChangeOrderStatus(int orderId, int statusId)
        {
            var status = OrderStatusService.GetOrderStatus(statusId);
            if (status != null)
            {
                OrderStatusService.ChangeOrderStatus(orderId, statusId, "Из мобильной админки");
                var order = OrderService.GetOrder(orderId);
                if (order != null && order.OrderCustomer.Email.IsNotEmpty())
                {
                    var orderItemsHtml = OrderService.GenerateOrderItemsHtml(order.OrderItems, order.OrderCurrency,
                                                        order.Sum,
                                                        order.OrderDiscount, order.OrderDiscountValue,
                                                        order.Coupon, order.Certificate,
                                                        order.TotalDiscount,
                                                        order.ShippingCost, order.PaymentCost,
                                                        order.TaxCost,
                                                        order.BonusCost,
                                                        0);

                    var mailTemplate = new OrderStatusMailTemplate(order.OrderStatus.StatusName,
                                                                   order.StatusComment.Replace("\r\n", "<br />"),
                                                                   order.Number, orderItemsHtml, !string.IsNullOrEmpty(order.TrackNumber) ? order.TrackNumber : string.Empty);
                    mailTemplate.BuildMail();
                    SendMail.SendMailNow(order.OrderCustomer.CustomerID, order.OrderCustomer.Email, mailTemplate.Subject, mailTemplate.Body, true);
                }
                return Json(new
                {
                    ResultCode = 0,
                    status.Color,
                    status.StatusName,
                    UserNotified = order != null && order.OrderCustomer.Email.IsNotEmpty()
                });
            }

            return null;
        }

        public JsonResult SetOrderPaid(int orderId, int paid)
        {
            OrderService.PayOrder(orderId, paid == 1);
            return Json(new {ResultCode = 0});
        }

        public ActionResult Graphics(GraphicModel graphic)
        {
            var now = DateTime.Now;
            var from = now.AddMonths(-1);
            var to = now;

            if (!String.IsNullOrWhiteSpace(graphic.DateFrom) && !String.IsNullOrWhiteSpace(graphic.DateTo))
            {
                DateTime.TryParseExact(graphic.DateFrom, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out from);
                DateTime.TryParseExact(graphic.DateTo, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out to);
            }

            var model = GetGraphictsModel(from, to, graphic.Paid);
            SetMetaInformation(Resource.AdminMobile_Graphics);

            return View(model);
        }

        [HttpGet]
        public ActionResult GetGraphics(GraphicModel graphic)
        {
            if (String.IsNullOrWhiteSpace(graphic.DateFrom) || String.IsNullOrWhiteSpace(graphic.DateTo))
            {
                return null;
            }

            DateTime from = DateTime.Parse(graphic.DateFrom, null, DateTimeStyles.RoundtripKind);
            DateTime to = DateTime.Parse(graphic.DateTo, null, DateTimeStyles.RoundtripKind);

            var model = GetGraphictsModel(from, to, graphic.Paid);
            return Json(model);
        }

        #region Help method

        private GraphicViewModel GetGraphictsModel(DateTime from, DateTime to, bool? paid)
        {
            var model = new GraphicViewModel();

            var listSum = OrderStatisticsService.GetOrdersSum("dd", from, to, paid);
            var data = GetByDays(listSum, from, to);

            model.DataChart = String.Format("[{{label: '{0}', data:[{1}]}}]", Resource.Admin_Default_Orders, data);
            model.Min = GetTimestamp(from).ToString();
            model.Max = GetTimestamp(to).ToString();
            model.DateFrom = from;
            model.DateTo = to;

            return model;
        }

        private string GetByDays(Dictionary<DateTime, float> list, DateTime startDate, DateTime endDate)
        {
            var resultList = new List<string>();
            var tempDate = DateTime.MinValue;

            var currencyValue = CurrencyService.CurrentCurrency.Rate;

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

                resultList.Add(string.Format("[{0},{1}]", GetTimestamp(profit.Key), (profit.Value / currencyValue).ToString("F2").Replace(",", ".")));
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

        private static long GetTimestamp(DateTime date)
        {
            TimeSpan span = (date - new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (long)(span.TotalSeconds * 1000);
        }

        #endregion
    }
}