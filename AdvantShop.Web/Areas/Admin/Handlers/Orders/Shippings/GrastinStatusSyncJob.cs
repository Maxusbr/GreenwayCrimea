using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Services.Shipping.Grastin.Api;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using Quartz;

namespace AdvantShop.Web.Admin.Handlers.Orders.Shippings
{
    [DisallowConcurrentExecution]
    public class GrastinStatusSyncJob : IJob
    {
        private static readonly object Sync = new object();

        public void Execute(IJobExecutionContext context)
        {
            lock (Sync)
            {
                try
                {
                    var grastinTypeKey =
                    ((ShippingKeyAttribute)
                        typeof (Shipping.Grastin.Grastin).GetCustomAttributes(typeof (ShippingKeyAttribute), false)
                            .First()).Value;

                    var orderStatuses = OrderStatusService.GetOrderStatuses();

                    var shippings = ShippingMethodService.GetShippingMethodByType(grastinTypeKey, false);

                    foreach (var shippingMethod in shippings)
                    {
                        var grastinMethod = new Shipping.Grastin.Grastin(shippingMethod, null);

                        if (grastinMethod.StatusesSync)
                        {
                            var service = new GrastinApiService(grastinMethod.ApiKey);
                            var statusesReference = grastinMethod.StatusesReference;
                            var orderPrefix = grastinMethod.OrderPrefix;

                            var orders = GetOrdersShippingGrastinAndSendToGrastin(shippingMethod.ShippingMethodId);
                            OrderInfoContainer orderInfoContainer = new OrderInfoContainer() {Orders = new List<string>()};

                            foreach (var order in orders)
                            {
                                var orderStatus = orderStatuses.FirstOrDefault(x => x.StatusID == order.OrderStatusId);

                                if (orderStatus != null && !orderStatus.IsCanceled && !orderStatus.IsCompleted)
                                    orderInfoContainer.Orders.Add(string.Format("{0}{1}", orderPrefix, order.Number));
                                else
                                    continue;

                                if (orderInfoContainer.Orders.Count >= 10)
                                {
                                    SyncStatuses(service, orderInfoContainer, orderPrefix, orders, statusesReference, orderStatuses);
                                    orderInfoContainer.Orders.Clear();
                                }
                            }
                            SyncStatuses(service, orderInfoContainer, orderPrefix, orders, statusesReference, orderStatuses);
                        }
                    }
                }
                catch (BlException ex)
                {
                    Debug.Log.Error(ex);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }
            }
        }

        private void SyncStatuses(GrastinApiService service, OrderInfoContainer orderInfoContainer, string orderPrefix,
            List<Order> orders, Dictionary<string, int?> statusesReference, List<OrderStatus> orderStatuses)
        {
            if (orderInfoContainer.Orders.Count > 0)
            { 
                var ordersInfo = service.GetOrderInfo(orderInfoContainer);
                if (ordersInfo != null)
                {
                    foreach (var orderInfo in ordersInfo)
                    {
                        orderInfo.Number = orderInfo.Number.Substring(orderPrefix.Length);
                        var currentOrder = orders.First(x => x.Number.Equals(orderInfo.Number, StringComparison.OrdinalIgnoreCase));

                        var grastingOrderStatus = statusesReference.ContainsKey(orderInfo.Status)
                            ? statusesReference[orderInfo.Status]
                            : null;

                        if (grastingOrderStatus.HasValue &&
                            currentOrder.OrderStatusId != grastingOrderStatus.Value &&
                            orderStatuses.Exists(x => x.StatusID == grastingOrderStatus.Value))
                        {
                            var lastOrderStatusHistory =
                                OrderStatusService.GetOrderStatusHistory(currentOrder.OrderID)
                                    .OrderByDescending(item => item.Date).First();

                            if (lastOrderStatusHistory != null &&
                                lastOrderStatusHistory.Date < orderInfo.StatusDateTime)
                            {
                                OrderStatusService.ChangeOrderStatus(currentOrder.OrderID,
                                    grastingOrderStatus.Value, "Синхронизация статусов для Grastin");
                            }
                        }
                    }
                }
            }
        }

        private static List<Order> GetOrdersShippingGrastinAndSendToGrastin(int shippingMethodId)
        {
            return SQLDataAccess.ExecuteReadList(
                "SELECT [OrderID], [Number], OrderStatusID FROM [Order].[Order] WHERE [ShippingMethodID] = @ShippingMethodID" +
                " AND EXISTS(SELECT 1 FROM [Order].[OrderAdditionalData] WHERE [OrderAdditionalData].[OrderID] = [Order].[OrderID] AND [Name] = @Name)",
                CommandType.Text,
                reader => new Order()
                {
                    OrderID = SQLDataHelper.GetInt(reader, "OrderID"),
                    Number = SQLDataHelper.GetString(reader, "Number"),
                    OrderStatusId = SQLDataHelper.GetInt(reader, "OrderStatusID")
                },
                new SqlParameter("@ShippingMethodID", shippingMethodId),
                new SqlParameter("@Name", "GrastinSendOrder"));
        }
    }
}
