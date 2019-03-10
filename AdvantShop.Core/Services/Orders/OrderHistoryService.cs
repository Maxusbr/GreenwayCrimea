using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Payment;

namespace AdvantShop.Core.Services.Orders
{
    public class OrderHistoryService
    {
        public static void NewOrder(Order order, OrderChangedBy changedBy)
        {
            Add(new OrderHistory(changedBy)
            {
                OrderId = order.OrderID,
                Parameter = LocalizationService.GetResource("Core.Orders.OrderHistory.OrderCreated") + order.OrderID
            });
        }

        public static void DeleteOrder(Order order, OrderChangedBy changedBy)
        {
            Add(new OrderHistory(changedBy)
            {
                OrderId = order.OrderID,
                Parameter = LocalizationService.GetResource("Core.Orders.OrderHistory.OrderDeleted")
            });
        }

        public static bool ChangingOrderMain(Order newOrder, OrderChangedBy changedBy)
        {
            var oldOrder = OrderService.GetOrder(newOrder.OrderID);
            if (oldOrder == null)
                return false;

            var history = GetChangesHistory(newOrder.OrderID, oldOrder, newOrder, changedBy);

            Add(history);

            return true;
        }
        

        public static bool ChangingCurrency(int orderId, string currencyCode, float currencyValue, OrderChangedBy changedBy)
        {
            var oldCurrency = OrderService.GetOrderCurrency(orderId);
            if (oldCurrency == null)
                return false;

            var newCurrency = new OrderCurrency()
            {
                CurrencyCode = currencyCode,
                CurrencyValue = currencyValue,
            };

            var history = GetChangesHistory(orderId, oldCurrency, newCurrency, changedBy);

            Add(history);

            return true;
        }


        public static bool ChangingCustomer(OrderCustomer customer, OrderChangedBy changedBy)
        {
            var oldCustomer = OrderService.GetOrderCustomer(customer.OrderID);
            if (oldCustomer == null)
                return false;
            
            var history = GetChangesHistory(customer.OrderID, oldCustomer, customer, changedBy);

            foreach (var orderHistory in history)
            {
                orderHistory.ParameterType = OrderHistoryParameterType.Customer;
                orderHistory.ParameterValue = customer.CustomerID.ToString();
            }

            Add(history);

            return true;
        }

        public static bool ChangingPaymentDetails(int orderId, PaymentDetails details, OrderChangedBy changedBy)
        {
            var oldDetails = OrderService.GetPaymentDetails(orderId);
            if (oldDetails == null)
                return false;
            
            var history = GetChangesHistory(orderId, oldDetails, details, changedBy);

            Add(history);

            return true;
        }

        public static bool ChangingOrderItem(int orderId, OrderItem oldItem, OrderItem newItem, OrderChangedBy changedBy)
        {
            var history = new List<OrderHistory>();

            if (oldItem != null && newItem != null)
            {
                history = GetChangesHistory(orderId, oldItem, newItem, changedBy);
            }
            else if (newItem == null && oldItem != null)
            {
                history.Add(new OrderHistory(changedBy)
                {
                    OrderId = orderId,
                    Parameter = oldItem.ProductID != null
                        ? LocalizationService.GetResource("Core.Orders.OrderHistory.DeletedProduct")
                        : LocalizationService.GetResource("Core.Orders.OrderHistory.DeletedCertificate"),
                });
            }
            else if (oldItem == null && newItem != null)
            {
                history.Add(new OrderHistory(changedBy)
                {
                    OrderId = orderId,
                    Parameter = newItem.ProductID != null
                        ? LocalizationService.GetResource("Core.Orders.OrderHistory.AddedProduct")
                        : LocalizationService.GetResource("Core.Orders.OrderHistory.AddedCertificate")
                });
            }

            if (history.Count > 0)
            {
                var description = newItem != null ? (newItem.Name + " " + newItem.Size + " " + newItem.Color) : "";
                var value = newItem != null ? (newItem.ProductID ?? newItem.CertificateID) : null;

                foreach (var orderHistory in history)
                {
                    orderHistory.ParameterDescription = description;
                    orderHistory.ParameterType = OrderHistoryParameterType.OrderItem;
                    orderHistory.ParameterValue = value != null ? value.Value.ToString() : null;
                }
            }

            Add(history);

            return true;
        }

        public static bool ChangingOrderTotal(int orderId, OnRefreshTotalOrder newRefreshTotalOrder, OrderChangedBy changedBy)
        {
            var oldOrder = OrderService.GetOrder(orderId);
            if (oldOrder == null)
                return false;

            var oldRefreshTotalOrder = new OnRefreshTotalOrder()
            {
                Sum = oldOrder.Sum,
                TaxCost = oldOrder.TaxCost,
                BonusCost = oldOrder.BonusCost
            };

            var history = GetChangesHistory(orderId, oldRefreshTotalOrder, newRefreshTotalOrder, changedBy);

            Add(history);

            return true;
        }

        public static bool ChangingAdminComment(int orderId, string comment, OrderChangedBy changedBy)
        {
            var order = OrderService.GetOrder(orderId);
            if (order == null)
                return false;
            
            if (order.AdminOrderComment.Equals(comment))
                return false;

            Add(new OrderHistory(changedBy)
            {
                OrderId = order.OrderID,
                Parameter = LocalizationService.GetResource("Core.Orders.Order.AdminOrderComment"),
                OldValue = order.AdminOrderComment,
                NewValue = comment
            });
            
            return true;
        }

        public static bool ChangingStatusComment(int orderId, string comment, OrderChangedBy changedBy)
        {
            var order = OrderService.GetOrder(orderId);
            if (order == null)
                return false;
            
            if (order.StatusComment.Equals(comment))
                return false;

            Add(new OrderHistory(changedBy)
            {
                OrderId = order.OrderID,
                Parameter = LocalizationService.GetResource("Core.Orders.Order.StatusComment"),
                OldValue = order.StatusComment,
                NewValue = comment
            });

            return true;
        }

        public static bool ChangingPayDate(Order order, bool pay)
        {
            var customer = CustomerContext.CurrentCustomer;
            var changedBy = customer != null ? new OrderChangedBy(customer) : new OrderChangedBy("site");

            Add(new OrderHistory(changedBy)
            {
                OrderId = order.OrderID,
                Parameter = LocalizationService.GetResource("Core.Orders.Order.ChangingPaymentStatus"),
                OldValue = order.Payed ? LocalizationService.GetResource("Core.Orders.Order.OrderPaied") : "",
                NewValue = pay 
                    ? LocalizationService.GetResource("Core.Orders.Order.OrderPaied")
                    : LocalizationService.GetResource("Core.Orders.Order.OrderNotPaied"),
            });

            return true;
        }
        
        public static bool ChangingStatus(OrderStatusHistory statusHistory)
        {
            var changedBy = new OrderChangedBy(statusHistory.CustomerName) {CustomerId = statusHistory.CustomerID};

            Add(new OrderHistory(changedBy)
            {
                OrderId = statusHistory.OrderID,
                Parameter = LocalizationService.GetResource("Core.Orders.Order.ChangingStatus"),
                ParameterType = OrderHistoryParameterType.Status,
                ParameterDescription = statusHistory.Basis,
                OldValue = statusHistory.PreviousStatus,
                NewValue = statusHistory.NewStatus
            });

            return true;
        }

        private static List<OrderHistory> GetChangesHistory(int orderId, object oldObj, object newObj, OrderChangedBy changedBy)
        {
            var history = new List<OrderHistory>();

            try
            {
                var attributeType = typeof(CompareAttribute);
                var properties = oldObj.GetType().GetProperties().Where(x => x.IsDefined(attributeType, false));

                foreach (var property in properties)
                {
                    object oldValue = property.GetValue(oldObj);
                    object newValue = property.GetValue(newObj);

                    if (object.Equals(oldValue, newValue) || (newValue == null && oldValue is string && (string) oldValue == ""))
                        continue;

                    var attributes = property.GetCustomAttributes(attributeType, false);
                    if (attributes.Length == 0)
                        continue;

                    var attribute = attributes[0] as IAttribute<string>;
                    var parameter = attribute != null ? attribute.Value : property.Name;

                    history.Add(new OrderHistory(changedBy)
                    {
                        OrderId = orderId,
                        Parameter = parameter,
                        NewValue = newValue != null ? newValue.ToString() : "",
                        OldValue = oldValue != null ? oldValue.ToString() : "",
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error("OrderHistoryService", ex);
            }

            return history;
        }

        private static void Add(List<OrderHistory> records)
        {
            foreach (var record in records)
            {
                Add(record);
            }
        }

        private static void Add(OrderHistory record)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Insert Into [Order].[OrderHistory] (OrderId,Parameter,ParameterType,ParameterValue,ParameterDescription,OldValue,NewValue,ManagerName,ManagerId,ModificationTime) Values (@OrderId,@Parameter,@ParameterType,@ParameterValue,@ParameterDescription,@OldValue,@NewValue,@ManagerName,@ManagerId,@ModificationTime)",
                CommandType.Text,
                new SqlParameter("@OrderId", record.OrderId),
                new SqlParameter("@Parameter", record.Parameter ?? string.Empty),
                new SqlParameter("@ParameterType", (int) record.ParameterType),
                new SqlParameter("@ParameterValue", record.ParameterValue ?? (object)DBNull.Value),
                new SqlParameter("@ParameterDescription", record.ParameterDescription ?? (object)DBNull.Value),
                new SqlParameter("@OldValue", record.OldValue ?? string.Empty),
                new SqlParameter("@NewValue", record.NewValue ?? string.Empty),
                new SqlParameter("@ManagerName", record.ManagerName ?? string.Empty),
                new SqlParameter("@ManagerId", record.ManagerId ?? (object) DBNull.Value),
                new SqlParameter("@ModificationTime", record.ModificationTime)
                );
        }

        public static List<OrderHistory> GetList(int orderId)
        {
            return
                SQLDataAccess.Query<OrderHistory>(
                    "Select OrderHistory.*, CustomerRole From [Order].[OrderHistory] left join Customers.Customer on Customer.CustomerID = [OrderHistory].ManagerId Where OrderId = @orderid Order By ModificationTime Desc",
                    new {orderId}).ToList();
        }
    }
}
