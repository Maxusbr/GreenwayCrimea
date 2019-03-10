//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Mails;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Payment;
using AdvantShop.Repository.Currencies;
using AdvantShop.Taxes;
using System.Web.Mvc;
using System.Web;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.Services.Loging;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Core.Services.Loging.TrafficSource;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Repository;
using AdvantShop.Shipping;
using Newtonsoft.Json;

namespace AdvantShop.Orders
{
    public class OrderService
    {
        #region OrderInformation

        public static OrderInformation GetOrderHistoryFromReader(SqlDataReader reader)
        {
            return new OrderInformation
            {
                OrderID = SQLDataHelper.GetInt(reader, "OrderID"),
                OrderNumber = SQLDataHelper.GetString(reader, "Number"),
                ShippingMethod = SQLDataHelper.GetString(reader, "ShippingMethod"),
                ShippingMethodName = SQLDataHelper.GetString(reader, "ShippingMethodName"),
                ArchivedPaymentName = SQLDataHelper.GetString(reader, "PaymentMethodName"),
                PaymentMethodID = SQLDataHelper.GetInt(reader, "PaymentMethodID"),
                Status = SQLDataHelper.GetString(reader, "StatusName"),
                StatusID = SQLDataHelper.GetInt(reader, "OrderStatusID"),
                PreviousStatus = SQLDataHelper.GetString(reader, "PreviousStatus"),
                Sum = SQLDataHelper.GetFloat(reader, "Sum"),
                OrderDate = SQLDataHelper.GetDateTime(reader, "OrderDate"),
                Payed = SQLDataHelper.GetNullableDateTime(reader, "PaymentDate") != null,
                ProductsHtml = string.Empty,
                CurrencyCode = SQLDataHelper.GetString(reader, "CurrencyCode"),
                CurrencyValue = SQLDataHelper.GetFloat(reader, "CurrencyValue"),
                CurrencySymbol = SQLDataHelper.GetString(reader, "CurrencySymbol"),
                IsCodeBefore = SQLDataHelper.GetBoolean(reader, "IsCodeBefore"),
                ManagerId = SQLDataHelper.GetNullableInt(reader, "ManagerId"),
                ManagerName = SQLDataHelper.GetString(reader, "ManagerName")
            };
        }

        public static List<OrderInformation> GetCustomerOrderHistory(Guid customerId)
        {
            return SQLDataAccess.ExecuteReadList("[Order].[sp_GetCustomerOrderHistory]", CommandType.StoredProcedure,
                GetOrderHistoryFromReader, new SqlParameter("@CustomerID", customerId));
        }

        #endregion

        #region OrderItems

        private static OrderItem GetOrderItemFromReader(IDataReader reader)
        {
            return new OrderItem
            {
                OrderID = SQLDataHelper.GetInt(reader, "OrderId"),
                OrderItemID = SQLDataHelper.GetInt(reader, "OrderItemID"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                Price = SQLDataHelper.GetFloat(reader, "Price"),
                Amount = SQLDataHelper.GetFloat(reader, "Amount"),
                ProductID = SQLDataHelper.GetNullableInt(reader, "ProductID"),
                ArtNo = SQLDataHelper.GetString(reader, "ArtNo"),
                SupplyPrice = SQLDataHelper.GetFloat(reader, "SupplyPrice"),
                Weight = SQLDataHelper.GetFloat(reader, "Weight"),
                Color = SQLDataHelper.GetString(reader, "Color", null),
                Size = SQLDataHelper.GetString(reader, "Size", null),
                IsCouponApplied = SQLDataHelper.GetBoolean(reader, "IsCouponApplied"),
                PhotoID = SQLDataHelper.GetNullableInt(reader, "PhotoID"),
                DecrementedAmount = SQLDataHelper.GetFloat(reader, "DecrementedAmount"),
                IgnoreOrderDiscount = SQLDataHelper.GetBoolean(reader, "IgnoreOrderDiscount"),
                AccrueBonuses = SQLDataHelper.GetBoolean(reader, "AccrueBonuses"),
                TaxId = SQLDataHelper.GetNullableInt(reader, "TaxId"),
                TaxName = SQLDataHelper.GetString(reader, "TaxName"),
                TaxType = (TaxType)SQLDataHelper.GetInt(reader, "TaxType"),
                TaxRate = SQLDataHelper.GetNullableFloat(reader, "TaxRate"),
                TaxShowInPrice = SQLDataHelper.GetNullableBoolean(reader, "TaxShowInPrice"),
                Width = SQLDataHelper.GetFloat(reader, "Width"),
                Length = SQLDataHelper.GetFloat(reader, "Length"),
                Height = SQLDataHelper.GetFloat(reader, "Height"),
            };
        }

        public static List<OrderItem> GetOrderItems(int orderId)
        {
            var result =
                SQLDataAccess.ExecuteReadList("[Order].[sp_GetOrderItems]", CommandType.StoredProcedure,
                    GetOrderItemFromReader,
                    new SqlParameter("@OrderID", orderId));

            foreach (OrderItem orditm in result)
            {
                orditm.SelectedOptions = SQLDataAccess.ExecuteReadList(
                    "[Order].[sp_GetSelectedOptionsByOrderItemId]",
                                                                        CommandType.StoredProcedure,
                                                                        reader => new EvaluatedCustomOptions
                                                                        {
                                                                            CustomOptionId = SQLDataHelper.GetInt(reader, "CustomOptionId"),
                                                                            CustomOptionTitle = SQLDataHelper.GetString(reader, "CustomOptionTitle"),
                                                                            OptionId = SQLDataHelper.GetInt(reader, "OptionId"),
                                                                            OptionPriceBc = SQLDataHelper.GetFloat(reader, "OptionPriceBC"),
                                                                            OptionPriceType = (OptionPriceType)SQLDataHelper.GetInt(reader, "OptionPriceType"),
                                                                            OptionTitle = SQLDataHelper.GetString(reader, "OptionTitle")
                    }, 
                    new SqlParameter("@OrderItemId", orditm.OrderItemID));
            }
            return result;
        }

        private static void UpdateOrderedItem(int orderId, OrderItem item)
        {
            SQLDataAccess.ExecuteNonQuery("[Order].[sp_UpdateOrderItem]", CommandType.StoredProcedure,
                new SqlParameter("@OrderItemID", item.OrderItemID),
                new SqlParameter("@OrderID", orderId),
                new SqlParameter("@Name", item.Name),
                new SqlParameter("@Price", item.Price),
                new SqlParameter("@Amount", item.Amount),
                new SqlParameter("@ProductID", item.ProductID ?? (object) DBNull.Value),
                new SqlParameter("@ArtNo", item.ArtNo),
                new SqlParameter("@SupplyPrice", item.SupplyPrice),
                new SqlParameter("@Weight", item.Weight),
                new SqlParameter("@IsCouponApplied", item.IsCouponApplied),
                new SqlParameter("@Color", item.Color ?? (object) DBNull.Value),
                new SqlParameter("@Size", item.Size ?? (object) DBNull.Value),
                new SqlParameter("@DecrementedAmount", item.DecrementedAmount),
                new SqlParameter("@PhotoID", item.PhotoID != 0 && item.PhotoID != null ? item.PhotoID : (object) DBNull.Value),
                new SqlParameter("@IgnoreOrderDiscount", item.IgnoreOrderDiscount),
                new SqlParameter("@AccrueBonuses", item.AccrueBonuses),
                new SqlParameter("@TaxId", item.TaxId ?? (object) DBNull.Value),
                new SqlParameter("@TaxName", item.TaxName ?? (object) DBNull.Value),
                new SqlParameter("@TaxType", item.TaxType != null ? (int)item.TaxType : (object) DBNull.Value),
                new SqlParameter("@TaxRate", item.TaxRate ?? (object) DBNull.Value),
                new SqlParameter("@TaxShowInPrice", item.TaxShowInPrice ?? (object) DBNull.Value),
                new SqlParameter("@Width", item.Width),
                new SqlParameter("@Length", item.Length),
                new SqlParameter("@Height", item.Height)
                );
        }

        private static void AddOrderedItem(int orderId, OrderItem item)
        {
            item.OrderItemID =
                SQLDataAccess.ExecuteScalar<int>("[Order].[sp_AddOrderItem]", CommandType.StoredProcedure,
                    new SqlParameter("@OrderID", orderId),
                    new SqlParameter("@Name", item.Name ?? (object) DBNull.Value),
                    new SqlParameter("@Price", item.Price),
                    new SqlParameter("@Amount", item.Amount),
                    new SqlParameter("@ProductID", item.ProductID ?? (object) DBNull.Value),
                    new SqlParameter("@ArtNo", item.ArtNo ?? (object) DBNull.Value),
                    new SqlParameter("@SupplyPrice", item.SupplyPrice),
                    new SqlParameter("@Weight", item.Weight),
                    new SqlParameter("@IsCouponApplied", item.IsCouponApplied),
                    new SqlParameter("@Color", item.Color ?? (object) DBNull.Value),
                    new SqlParameter("@Size", item.Size ?? (object) DBNull.Value),
                    new SqlParameter("@DecrementedAmount", item.DecrementedAmount),
                    new SqlParameter("@PhotoID", item.PhotoID != 0 && item.PhotoID != null ? item.PhotoID : (object) DBNull.Value),
                    new SqlParameter("@IgnoreOrderDiscount", item.IgnoreOrderDiscount),
                    new SqlParameter("@AccrueBonuses", item.AccrueBonuses),
                    new SqlParameter("@TaxId", item.TaxId ?? (object)DBNull.Value),
                    new SqlParameter("@TaxName", item.TaxName ?? (object)DBNull.Value),
                    new SqlParameter("@TaxType", item.TaxType != null ? (int)item.TaxType : (object)DBNull.Value),
                    new SqlParameter("@TaxRate", item.TaxRate ?? (object)DBNull.Value),
                    new SqlParameter("@TaxShowInPrice", item.TaxShowInPrice ?? (object)DBNull.Value),
                    new SqlParameter("@Width", item.Width),
                    new SqlParameter("@Height", item.Height),
                    new SqlParameter("@Length", item.Length)
                    );

            if (item.SelectedOptions != null)
            {
                foreach (EvaluatedCustomOptions evco in item.SelectedOptions)
                {
                    SQLDataAccess.ExecuteNonQuery("[Order].[sp_AddOrderCustomOptions]", CommandType.StoredProcedure,
                                                  new SqlParameter("@CustomOptionId", evco.CustomOptionId),
                                                  new SqlParameter("@OptionId", evco.OptionId),
                                                  new SqlParameter("@CustomOptionTitle", evco.CustomOptionTitle),
                                                  new SqlParameter("@OptionTitle", evco.OptionTitle),
                                                  new SqlParameter("@OptionPriceBC", evco.OptionPriceBc),
                                                  new SqlParameter("@OptionPriceType", evco.OptionPriceType),
                                                  new SqlParameter("@OrderItemID", item.OrderItemID));
                }
            }
        }

        private static void AddUpdateOrderedItem(int orderId, OrderItem item, OrderChangedBy changedBy = null, bool ignoreHistory = false)
        {
            var items = GetOrderItems(orderId);

            var oldItem = items.Find(x => x.OrderItemID == item.OrderItemID);
            if (oldItem != null)
            {
                if (!ignoreHistory)
                    OrderHistoryService.ChangingOrderItem(orderId, oldItem, item, changedBy);

                UpdateOrderedItem(orderId, item);
            }
            else
            {
                if (!ignoreHistory)
                    OrderHistoryService.ChangingOrderItem(orderId, null, item, changedBy);

                AddOrderedItem(orderId, item);
            }
        }

        //public static bool AddUpdateOrderItems(List<OrderItem> items, int orderId, OrderChangedBy changedBy = null, bool trackChanges = true)
        //{
        //    foreach (OrderItem orderItem in items)
        //    {
        //        AddUpdateOrderedItem(orderId, orderItem, changedBy, !trackChanges);
        //    }
        //    RefreshTotal(orderId);
        //    return false;
        //}

        public static bool AddUpdateOrderItems(List<OrderItem> items, List<OrderItem> olditems, int orderId,
                                                OrderChangedBy changedBy = null, bool trackChanges = true, 
                                                bool updateModules = true)
        {
            var itemsToDelete = new List<OrderItem>();
            foreach (OrderItem oldOrderItem in olditems)
            {
                bool isfound = items.Any(orderItem => orderItem.OrderItemID == oldOrderItem.OrderItemID);

                if (!isfound)
                {
                    oldOrderItem.Amount = 0;
                    AddUpdateOrderedItem(orderId, oldOrderItem, changedBy, !trackChanges);
                    itemsToDelete.Add(oldOrderItem);
                }
            }

            if (itemsToDelete.Count > 0)
            {
                IncrementProductsCountAccordingOrder(orderId);
                foreach (var item in itemsToDelete)
                {
                    DeleteOrderItem(orderId, item, changedBy, trackChanges);
                }
            }

            foreach (OrderItem orderItem in items)
            {
                AddUpdateOrderedItem(orderId, orderItem, changedBy, !trackChanges);
            }

            var order = GetOrder(orderId);
            RefreshTotal(order, !trackChanges, changedBy, updateModules);

            return false;
        }


        public static void DeleteOrderItem(int orderId, OrderItem item, OrderChangedBy changedBy = null, bool trackChanges = true)
        {
            if (trackChanges)
                OrderHistoryService.ChangingOrderItem(orderId, item, null, changedBy);

            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Order].[OrderItems] WHERE [OrderItemID] = @OrderItemID",
                                          CommandType.Text,
                                          new SqlParameter("@OrderID", orderId),
                                          new SqlParameter("@OrderItemID", item.OrderItemID));
        }

        public static void ClearOrderItems(int orderId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Order].[OrderItems] WHERE [OrderID] = @OrderID",
                                          CommandType.Text, new SqlParameter("@OrderID", orderId));
        }

        #endregion

        public static Order GetOrderFromReader(IDataReader reader)
        {
            return new Order
            {
                OrderID = SQLDataHelper.GetInt(reader, "OrderID"),
                Number = SQLDataHelper.GetString(reader, "Number"),
                Code = SQLDataHelper.GetGuid(reader, "Code"),
                OrderStatusId = SQLDataHelper.GetInt(reader, "OrderStatusID"),
                StatusComment = SQLDataHelper.GetString(reader, "StatusComment"),
                AdditionalTechInfo = SQLDataHelper.GetString(reader, "AdditionalTechInfo"),
                AdminOrderComment = SQLDataHelper.GetString(reader, "AdminOrderComment"),
                Sum = SQLDataHelper.GetFloat(reader, "Sum"),
                ShippingCost = SQLDataHelper.GetFloat(reader, "ShippingCost"),
                PaymentCost = SQLDataHelper.GetFloat(reader, "PaymentCost"),
                OrderDiscount = SQLDataHelper.GetFloat(reader, "OrderDiscount"),
                OrderDiscountValue = SQLDataHelper.GetFloat(reader, "OrderDiscountValue"),
                DiscountCost = SQLDataHelper.GetFloat(reader, "DiscountCost"),
                TaxCost = SQLDataHelper.GetFloat(reader, "TaxCost"),
                OrderDate = SQLDataHelper.GetDateTime(reader, "OrderDate"),
                SupplyTotal = SQLDataHelper.GetFloat(reader, "SupplyTotal"),
                ShippingMethodId = SQLDataHelper.GetInt(reader, "ShippingMethodID"),
                PaymentMethodId = SQLDataHelper.GetInt(reader, "PaymentMethodId"),
                AffiliateID = SQLDataHelper.GetInt(reader, "AffiliateID"),
                CustomerComment = SQLDataHelper.GetString(reader, "CustomerComment"),
                Decremented = SQLDataHelper.GetBoolean(reader, "Decremented"),
                PaymentDate =
                    SQLDataHelper.GetDateTime(reader, "PaymentDate") == DateTime.MinValue
                        ? null
                        : (DateTime?)SQLDataHelper.GetDateTime(reader, "PaymentDate"),
                ArchivedPaymentName = SQLDataHelper.GetString(reader, "PaymentMethodName"),
                ArchivedShippingName = SQLDataHelper.GetString(reader, "ShippingMethodName"),

                GroupName = SQLDataHelper.GetString(reader, "GroupName"),
                GroupDiscount = SQLDataHelper.GetFloat(reader, "GroupDiscount"),
                Certificate = SQLDataHelper.GetString(reader, "CertificateCode").IsNotEmpty()
                                    ? new OrderCertificate
                                    {
                                        Code = SQLDataHelper.GetString(reader, "CertificateCode"),
                                        Price = SQLDataHelper.GetFloat(reader, "CertificatePrice")
                                    }
                                    : null,
                Coupon = SQLDataHelper.GetString(reader, "CouponCode").IsNotEmpty()
                                ? new OrderCoupon
                                {
                                    Type = (CouponType)SQLDataHelper.GetInt(reader, "CouponType"),
                                    Code = SQLDataHelper.GetString(reader, "CouponCode"),
                                    Value = SQLDataHelper.GetFloat(reader, "CouponValue")
                                }
                                : null,
                BonusCost = SQLDataHelper.GetFloat(reader, "BonusCost"),
                BonusCardNumber = SQLDataHelper.GetNullableLong(reader, "BonusCardNumber"),
                ManagerId = SQLDataHelper.GetNullableInt(reader, "ManagerId"),

                UseIn1C = SQLDataHelper.GetBoolean(reader, "UseIn1C"),
                ModifiedDate = SQLDataHelper.GetDateTime(reader, "ModifiedDate"),
                ManagerConfirmed = SQLDataHelper.GetBoolean(reader, "ManagerConfirmed"),
                PreviousStatus = SQLDataHelper.GetString(reader, "PreviousStatus"),

                OrderSourceId = SQLDataHelper.GetInt(reader, "OrderSourceId"),
                CustomData = SQLDataHelper.GetString(reader, "CustomData"),
                IsDraft = SQLDataHelper.GetBoolean(reader, "IsDraft"),
                DeliveryDate = SQLDataHelper.GetNullableDateTime(reader, "DeliveryDate"),
                DeliveryTime = SQLDataHelper.GetString(reader, "DeliveryTime"),
                TrackNumber = SQLDataHelper.GetString(reader, "TrackNumber"),
                IsFromAdminArea = SQLDataHelper.GetBoolean(reader, "IsFromAdminArea"),
                LeadId = SQLDataHelper.GetNullableInt(reader, "LeadId"),
                ShippingTaxType = (TaxType)SQLDataHelper.GetInt(reader, "ShippingTaxType"),
            };
        }

        public static OrderAutocomplete GetOrderForAutocompleteFromReader(IDataReader reader)
        {
            return new OrderAutocomplete
            {
                OrderID = SQLDataHelper.GetInt(reader, "OrderID"),
                Number = SQLDataHelper.GetString(reader, "Number"),
                FirstName = SQLDataHelper.GetString(reader, "FirstName"),
                LastName = SQLDataHelper.GetString(reader, "LastName"),
                Email = SQLDataHelper.GetString(reader, "Email"),
                MobilePhone = SQLDataHelper.GetString(reader, "Phone"),
                OrderDate = SQLDataHelper.GetDateTime(reader, "OrderDate"),
                Sum = SQLDataHelper.GetFloat(reader, "Sum"),
                StatusName = SQLDataHelper.GetString(reader, "StatusName")
            };
        }


        public static List<Order> GetAllOrders()
        {
            return SQLDataAccess.ExecuteReadList<Order>("SELECT * FROM [Order].[Order] Where IsDraft <> 1", CommandType.Text, GetOrderFromReader);
        }


        public static Dictionary<Guid, long> GetAllOrdersPhones()
        {
            var dict = new Dictionary<Guid, long>();
            dict.AddRange(
                SQLDataAccess.ExecuteReadIEnumerable<KeyValuePair<Guid, long>>(
                    "SELECT DISTINCT CustomerID, (select TOP(1) StandardPhone FROM [Order].[OrderCustomer] WHERE CustomerID = tbl.CustomerID ORDER BY OrderId DESC) AS StandardPhone FROM [Order].[OrderCustomer] AS tbl WHERE StandardPhone IS NOT NULL",
                    CommandType.Text,
                    reader =>
                        new KeyValuePair<Guid, long>(SQLDataHelper.GetGuid(reader, "CustomerID"),
                            SQLDataHelper.GetLong(reader, "StandardPhone"))));
            return dict;
        }

        public static List<Order> GetOrders(string email)
        {
            return
                SQLDataAccess.ExecuteReadList<Order>(
                    "SELECT * FROM [Order].[Order] inner join [Order].[OrderCustomer] on [OrderCustomer].[OrderID] = [Order].[OrderID] Where Email=@Email order by OrderDate desc",
                    CommandType.Text, GetOrderFromReader, new SqlParameter("@email", email));
        }

        public static List<Order> GetOrdersByPhone(string phone)
        {
            return
                SQLDataAccess.ExecuteReadList<Order>(
                    "SELECT * FROM [Order].[Order] inner join [Order].[OrderCustomer] on [OrderCustomer].[OrderID] = [Order].[OrderID] Where Phone=@Phone or StandardPhone=@Phone",
                    CommandType.Text, GetOrderFromReader, new SqlParameter("@Phone", phone));
        }

        public static int GetOrdersCountByCustomer(Guid customerId)
        {
            return Convert.ToInt32(
                SQLDataAccess.ExecuteScalar(
                    "SELECT Count([Order].[OrderId]) FROM [Order].[Order] inner join [Order].[OrderCustomer] on [OrderCustomer].[OrderID] = [Order].[OrderID] Where CustomerId=@CustomerId",
                    CommandType.Text, new SqlParameter("@CustomerId", customerId)));
        }


        public static List<OrderAutocomplete> GetOrdersForAutocomplete(string query)
        {
            if (query.IsDecimal())
            {
                return SQLDataAccess.ExecuteReadList<OrderAutocomplete>(
                "SELECT [Order].[OrderID], Number, FirstName, LastName, Email, Phone, OrderDate, [Order].[Sum], StatusName " +
                "FROM [Order].[Order] " +
                "INNER JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderID] " +
                "INNER JOIN [Order].[OrderStatus] ON [OrderStatus].[OrderStatusID] = [Order].[OrderStatusId] " +
                "WHERE [Number] LIKE '%' + @q + '%' " +
                "OR [Email] LIKE @q + '%' " +
                (query.Length >= 6  ?
                    "OR [Phone] LIKE '%' + @q + '%' " +
                    "OR [StandardPhone] LIKE '%' + @q + '%'" 
                : ""),
                CommandType.Text, GetOrderForAutocompleteFromReader, new SqlParameter("@q", query));
            }
            else
            {
                var translitKeyboard = StringHelper.TranslitToRusKeyboard(query);

                return SQLDataAccess.ExecuteReadList<OrderAutocomplete>(
                    "SELECT [Order].[OrderID], Number, FirstName, LastName, Email, Phone, OrderDate, [Order].[Sum], StatusName " +
                    "FROM [Order].[Order] " +
                    "INNER JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderID] " +
                    "INNER JOIN [Order].[OrderStatus] ON [OrderStatus].[OrderStatusID] = [Order].[OrderStatusId] " +
                    "WHERE [Number] LIKE '%' + @q + '%' " +
                    "OR [Email] LIKE @q + '%' " +
                    "OR [FirstName] LIKE @q + '%' OR [FirstName] like @qtr + '%' " +
                    "OR [LastName] LIKE @q + '%' OR [LastName] like @qtr + '%' " +
                    "OR [Phone] LIKE '%' + @q + '%'",
                    CommandType.Text, GetOrderForAutocompleteFromReader, new SqlParameter("@q", query), new SqlParameter("@qtr", translitKeyboard));
            }
           
        }

        public static List<LastOrdersItem> GetLastOrders(int count)
        {
            return SQLDataAccess.Query<LastOrdersItem>(
                "SELECT TOP(@count) [Order].[OrderId], Number, OrderDate, Sum, [OrderCustomer].[CustomerId], FirstName, LastName, Patronymic,  [OrderCurrency].*, StatusName, Color " +
                "FROM [Order].[Order] " +
                "LEFT JOIN [Order].[OrderStatus] ON [OrderStatus].[OrderStatusID] = [Order].[OrderStatusID] " +
                "LEFT JOIN [Order].[OrderCurrency] ON [OrderCurrency].[OrderId] = [Order].[OrderId] " +
                "LEFT JOIN [Order].[OrderCustomer] ON [OrderCustomer].[OrderId] = [Order].[OrderId] " +
                "WHERE IsDraft = 0 " +
                "ORDER BY orderdate desc",
                new {count}).ToList();

        }

        public static List<LastOrdersItem> GetLastOrders(int count, int? managerId)
        {
            return SQLDataAccess.Query<LastOrdersItem>(
                    "SELECT TOP(@count) [Order].[OrderId], Number, OrderDate, Sum, [OrderCustomer].[CustomerId], FirstName, LastName, Patronymic,  [OrderCurrency].*, StatusName, Color " +
                    "FROM [Order].[Order] " +
                    "LEFT JOIN [Order].[OrderStatus] ON [OrderStatus].[OrderStatusID] = [Order].[OrderStatusID] " +
                    "LEFT JOIN [Order].[OrderCurrency] ON [OrderCurrency].[OrderId] = [Order].[OrderId] " +
                    "LEFT JOIN [Order].[OrderCustomer] ON [OrderCustomer].[OrderId] = [Order].[OrderId] " +
                    "WHERE IsDraft = 0 " + (managerId != null ? " and [ManagerId] = @ManagerId " : " and [ManagerId] IS NULL ") +
                    "ORDER BY orderdate desc",
                    new { count, managerId }).ToList();
        }

        public static List<string> GetShippingMethods()
        {
            List<string> result = SQLDataAccess.ExecuteReadList("SELECT Name FROM [Order].[ShippingMethod]", CommandType.Text, reader => SQLDataHelper.GetString(reader, "Name").Trim());
            return result;
        }

        public static List<string> GetShippingMethodNamesFromOrder()
        {
            List<string> result = SQLDataAccess.ExecuteReadList("SELECT distinct ShippingMethodName FROM [Order].[Order]", CommandType.Text, reader => SQLDataHelper.GetString(reader, "ShippingMethodName").Trim());
            return result;
        }

        public static void DeleteOrder(int orderId)
        {
            var order = GetOrder(orderId);
            if (order == null) return;
            var prevStatus = order.OrderStatus;
            var user = CustomerContext.CurrentCustomer;
            var history = new OrderStatusHistory()
            {
                OrderID = orderId,
                CustomerID = user.IsAdmin || user.IsManager || user.IsModerator ? user.Id : (Guid?)null,
                CustomerName = user.IsAdmin || user.IsManager || user.IsModerator ? user.FirstName + " " + user.LastName : string.Empty,
                PreviousStatus = prevStatus != null ? prevStatus.StatusName : string.Empty,
                NewStatus = "Удален",
            };

            SQLDataAccess.ExecuteNonQuery("[Order].[sp_DeleteOrder]", CommandType.StoredProcedure, new SqlParameter { ParameterName = "@OrderID", Value = orderId });

            if (Settings1C.Enabled)
                SQLDataAccess.ExecuteNonQuery(
                    "Insert Into [Order].[DeletedOrders] ([OrderId],[DateTime]) Values (@OrderId, Getdate())", CommandType.Text,
                    new SqlParameter("@OrderId", orderId));
            
            OrderStatusService.AddOrderStatusHistory(history);
            OrderHistoryService.DeleteOrder(order, null);

            if (BonusSystem.IsActive)
            {
                BonusSystemService.CancelPurchase(order.BonusCardNumber, order.Number, orderId);
            }

            ModulesExecuter.OrderDeleted(orderId);
            ModulesExecuter.SendNotificationsOnOrderDeleted(orderId);
        }

        public static int AddOrder(Order order, OrderChangedBy changedBy = null, bool trackChanges = true)
        {
            AddOrderMain(order);
            if (order.OrderID != 0)
            {
                AddOrderCustomer(order.OrderID, order.OrderCustomer);
                
                AddOrderCurrency(order.OrderID, order.OrderCurrency);

                if (order.PaymentDetails != null)
                    AddPaymentDetails(order.OrderID, order.PaymentDetails);

                if (order.OrderPickPoint != null)
                    AddUpdateOrderPickPoint(order.OrderID, order.OrderPickPoint);

                if (order.OrderItems != null)
                    foreach (var row in order.OrderItems)
                    {
                        AddUpdateOrderedItem(order.OrderID, row, ignoreHistory: true);
                    }

                if (order.OrderCertificates != null)
                    foreach (var certificate in order.OrderCertificates)
                    {
                        certificate.OrderId = order.OrderID;
                        GiftCertificateService.AddCertificate(certificate);
                    }

                RefreshTotal(order, updateModules:false);
            }

            order.Number = GenerateNumber(order.OrderID);
            UpdateNumber(order.OrderID, order.Number);

            if (trackChanges)
                OrderHistoryService.NewOrder(order, changedBy);

            ModulesExecuter.OrderAdded(order);

            if (!order.IsDraft)
            {
                ModulesExecuter.SendNotificationsOnOrderAdded(order);
                BizProcessExecuter.OrderAdded(order);
            }

            var loger = LogingManager.GetTrafficSourceLoger();
            loger.LogOrderTafficSource(order.OrderID, TrafficSourceType.Order, order.IsFromAdminArea);

            return order.OrderID;
        }

        private static void AddOrderCurrency(int orderId, OrderCurrency orderCurrency)
        {
            SQLDataAccess.ExecuteNonQuery(
                "INSERT INTO [Order].[OrderCurrency] (OrderID, CurrencyCode, CurrencyNumCode, CurrencyValue, CurrencySymbol, IsCodeBefore, RoundNumbers, EnablePriceRounding) VALUES (@OrderID, @CurrencyCode, @CurrencyNumCode, @CurrencyValue, @CurrencySymbol, @IsCodeBefore, @RoundNumbers, @EnablePriceRounding)",
                CommandType.Text,
                new SqlParameter("@OrderID", orderId),
                new SqlParameter("@CurrencyCode", orderCurrency.CurrencyCode),
                new SqlParameter("@CurrencyNumCode", orderCurrency.CurrencyNumCode),
                new SqlParameter("@CurrencyValue", orderCurrency.CurrencyValue),
                new SqlParameter("@CurrencySymbol", orderCurrency.CurrencySymbol),
                new SqlParameter("@IsCodeBefore", orderCurrency.IsCodeBefore),
                new SqlParameter("@EnablePriceRounding", orderCurrency.EnablePriceRounding),
                new SqlParameter("@RoundNumbers", orderCurrency.RoundNumbers));
        }

        public static void AddUpdateOrderPickPoint(int orderId, OrderPickPoint pickPoint)
        {
            SQLDataAccess.ExecuteNonQuery(
                @"if (select count(orderid) from [Order].[OrderPickPoint] where orderid=@orderid) = 0
                begin 
                    INSERT INTO [Order].[OrderPickPoint] (OrderID, PickPointId, PickPointAddress, AdditionalData) VALUES (@OrderID, @PickPointId, @PickPointAddress, @AdditionalData) 
                end
                else
                begin
                    Update [Order].[OrderPickPoint] set PickPointId=@PickPointId, PickPointAddress=@PickPointAddress, AdditionalData=@AdditionalData Where OrderID=@OrderID
                end",
                CommandType.Text,
                new SqlParameter("@OrderID", orderId),
                new SqlParameter("@PickPointId", pickPoint.PickPointId ?? string.Empty),
                new SqlParameter("@PickPointAddress", pickPoint.PickPointAddress ?? string.Empty),
                new SqlParameter("@AdditionalData", pickPoint.AdditionalData ?? string.Empty));
        }

        public static void DeleteOrderPickPoint(int orderID)
        {
            SQLDataAccess.ExecuteNonQuery("Delete from [Order].[OrderPickPoint] where OrderID= @OrderID",
               CommandType.Text,
               new SqlParameter("@OrderID", orderID));
        }
        
public static void AddUpdateOrderAdditionalData(int orderId, string key, string value)
        {
            SQLDataAccess.ExecuteNonQuery(
                @"IF NOT EXISTS(SELECT 1 FROM [Order].[OrderAdditionalData] WHERE [OrderID] = @OrderID AND [Name] = @Name)
                begin 
                    INSERT INTO [Order].[OrderAdditionalData] ([OrderID],[Name],[Value]) VALUES (@OrderID, @Name, @Value) 
                end
                else
                begin
                    Update [Order].[OrderAdditionalData] set [Value]=@Value Where [OrderID] = @OrderID AND [Name] = @Name
                end",
                CommandType.Text,
                new SqlParameter("@OrderID", orderId),
                new SqlParameter("@Name", key),
                new SqlParameter("@Value", value ?? (object)DBNull.Value));
        }
        
        public static Dictionary<string, string> GetOrderAdditionalData(int orderId)
        {
            return SQLDataAccess.ExecuteReadDictionary<string, string>(
                "SELECT [Name], [Value] FROM [Order].[OrderAdditionalData] WHERE [OrderID] = @OrderID",
                CommandType.Text,
                "Name",
                "Value",
                new SqlParameter("@OrderID", orderId));
        }
        
        public static string GetOrderAdditionalData(int orderId, string key)
        {
            return SQLDataAccess.ExecuteReadOne(
                "SELECT [Value] FROM [Order].[OrderAdditionalData] WHERE [OrderID] = @OrderID AND [Name] = @Name",
                CommandType.Text,
                reader =>
                    SQLDataHelper.GetString(reader, "Value"),
                new SqlParameter("@OrderID", orderId),
                new SqlParameter("@Name", key));
        }

        public static void DeleteOrderAdditionalData(int orderId)
        {
            SQLDataAccess.ExecuteNonQuery("Delete from [Order].[OrderAdditionalData] where OrderID= @OrderID",
                CommandType.Text,
                new SqlParameter("@OrderID", orderId));
        }

        public static void DeleteOrderAdditionalData(int orderId, string key)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Delete from [Order].[OrderAdditionalData] where [OrderID] = @OrderID AND [Name] = @Name",
                CommandType.Text,
                new SqlParameter("@OrderID", orderId),
                new SqlParameter("@Name", key));
        }
        
        public static void AddOrderCustomer(int orderId, OrderCustomer customer)
        {
            SQLDataAccess.ExecuteNonQuery(
                "INSERT INTO [Order].[OrderCustomer] ([OrderId],[CustomerID],[CustomerIP],[FirstName],[LastName],[Patronymic],[Email],[Phone],[StandardPhone],Country,Region,City,Zip,CustomField1,CustomField2,CustomField3,Street,House,Apartment,Structure,Entrance,Floor) " +
                " VALUES (@OrderId,@CustomerID,@CustomerIP,@FirstName,@LastName, @Patronymic,@Email,@Phone,@StandardPhone,@Country,@Region,@City,@Zip,@CustomField1,@CustomField2,@CustomField3,@Street,@House,@Apartment,@Structure,@Entrance,@Floor)",
                CommandType.Text,
                new SqlParameter("@OrderID", orderId),
                new SqlParameter("@CustomerID", customer.CustomerID),
                new SqlParameter("@CustomerIP", customer.CustomerIP ?? string.Empty),
                new SqlParameter("@FirstName", customer.FirstName ?? string.Empty),
                new SqlParameter("@LastName", customer.LastName ?? string.Empty),
                new SqlParameter("@Patronymic", customer.Patronymic ?? string.Empty),
                new SqlParameter("@Email", customer.Email ?? string.Empty),
                new SqlParameter("@Phone", customer.Phone ?? string.Empty),
                new SqlParameter("@StandardPhone", customer.StandardPhone ?? (object)DBNull.Value),


                new SqlParameter("@Country", customer.Country ?? string.Empty),
                new SqlParameter("@Region", customer.Region ?? string.Empty),
                new SqlParameter("@City", customer.City ?? string.Empty),
                new SqlParameter("@Zip", customer.Zip ?? string.Empty),
                new SqlParameter("@CustomField1", customer.CustomField1 ?? string.Empty),
                new SqlParameter("@CustomField2", customer.CustomField2 ?? string.Empty),
                new SqlParameter("@CustomField3", customer.CustomField3 ?? string.Empty),

                new SqlParameter("@Street", customer.Street ?? string.Empty),
                new SqlParameter("@House", customer.House ?? string.Empty),
                new SqlParameter("@Apartment", customer.Apartment ?? string.Empty),
                new SqlParameter("@Structure", customer.Structure ?? string.Empty),
                new SqlParameter("@Entrance", customer.Entrance ?? string.Empty),
                new SqlParameter("@Floor", customer.Floor ?? string.Empty)
                );

            var modules = AttachedModules.GetModules<ISendMails>();
            foreach (var moduleType in modules)
            {
                var moduleObject = (ISendMails)Activator.CreateInstance(moduleType, null);
                moduleObject.SubscribeEmail(new Subscription
                {
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Phone = customer.Phone,
                    CustomerType = EMailRecipientType.OrderCustomer
                });
            }
        }

        public static void CreateCustomerByOrderCustomer(OrderCustomer orderCustomer)
        {
            var customer = CustomerService.GetCustomer(orderCustomer.CustomerID);
            if (customer != null)
                return;

            if (!string.IsNullOrWhiteSpace(orderCustomer.Email))
                customer = CustomerService.GetCustomerByEmail(orderCustomer.Email);

            if (customer != null)
                return;

            var c = (Customer) orderCustomer;

            CustomerService.InsertNewCustomer(c);

            if (c.Contacts != null && c.Contacts.Count > 0)
                CustomerService.AddContact(c.Contacts[0], c.Id);
        }

        public static void UpdateOrderCustomer(OrderCustomer customer, OrderChangedBy changedBy = null, bool trackChanges = true)
        {
            if (trackChanges)
                OrderHistoryService.ChangingCustomer(customer, changedBy);

            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Order].[OrderCustomer] " +
                "SET [CustomerID] = @CustomerID, [FirstName] = @FirstName, LastName=@LastName, Patronymic=@Patronymic, Email=@Email, Phone=@Phone, StandardPhone=@StandardPhone, " +
                "Country = @Country, Region = @Region, City = @City, Zip = @Zip, CustomField1 = @CustomField1, CustomField2 = @CustomField2, CustomField3 = @CustomField3, " +
                "Street = @Street, House = @House, Apartment = @Apartment, Structure = @Structure, Entrance = @Entrance, Floor = @Floor " +
                "WHERE [OrderID] = @OrderID",
                CommandType.Text,
                new SqlParameter("@OrderID", customer.OrderID),
                new SqlParameter("@CustomerID", customer.CustomerID),
                new SqlParameter("@FirstName", customer.FirstName ?? string.Empty),
                new SqlParameter("@LastName", customer.LastName ?? string.Empty),
                new SqlParameter("@Patronymic", customer.Patronymic ?? string.Empty),
                new SqlParameter("@Email", customer.Email ?? string.Empty),
                new SqlParameter("@Phone", customer.Phone ?? string.Empty),
                new SqlParameter("@StandardPhone", customer.StandardPhone ?? (object)DBNull.Value),

                new SqlParameter("@Country", customer.Country ?? string.Empty),
                new SqlParameter("@Region", customer.Region ?? string.Empty),
                new SqlParameter("@City", customer.City ?? string.Empty),
                new SqlParameter("@Zip", customer.Zip ?? string.Empty),
                new SqlParameter("@CustomField1", customer.CustomField1 ?? string.Empty),
                new SqlParameter("@CustomField2", customer.CustomField2 ?? string.Empty),
                new SqlParameter("@CustomField3", customer.CustomField3 ?? string.Empty),

                new SqlParameter("@Street", customer.Street ?? string.Empty),
                new SqlParameter("@House", customer.House ?? string.Empty),
                new SqlParameter("@Apartment", customer.Apartment ?? string.Empty),
                new SqlParameter("@Structure", customer.Structure ?? string.Empty),
                new SqlParameter("@Entrance", customer.Entrance ?? string.Empty),
                new SqlParameter("@Floor", customer.Floor ?? string.Empty)
                );
        }


        private static void AddOrderMain(Order ord)
        {
            if (ord.Code == Guid.Empty)
                ord.Code = Guid.NewGuid();

            ord.OrderID = SQLDataAccess.ExecuteScalar<int>(
                        "INSERT INTO [Order].[Order] " +
                            "([Number], [ShippingMethodID], [PaymentMethodID], [AffiliateID], " +
                             "[OrderDate], [PaymentDate], [CustomerComment], [StatusComment], " +
                             "[AdditionalTechInfo],[AdminOrderComment], [ShippingCost],[PaymentCost], [OrderStatusID], " +
                             "[ShippingMethodName],[PaymentMethodName], [GroupName], [GroupDiscount], [OrderDiscount], " +
                             "[CertificateCode], [CertificatePrice], [CouponCode], [CouponType], [CouponValue], [BonusCost], [BonusCardNumber], " +
                             "[ManagerId],  [UseIn1C], [ModifiedDate], [Code], [ManagerConfirmed], [OrderSourceId], CustomData, IsDraft, " +
                             "DeliveryDate, DeliveryTime, TrackNumber, IsFromAdminArea, OrderDiscountValue, LeadId, ShippingTaxType) " +
                        "VALUES " +
                            "(@Number, @ShippingMethodID, @PaymentMethodID, @AffiliateID, " +
                             "@OrderDate, null, @CustomerComment, @StatusComment, " +
                             "@AdditionalTechInfo, @AdminOrderComment, @ShippingCost,@PaymentCost, @OrderStatusID, " +
                             "@ShippingMethodName, @PaymentMethodName, @GroupName, @GroupDiscount,@OrderDiscount, " +
                             "@CertificateCode, @CertificatePrice, @CouponCode, @CouponType, @CouponValue, @BonusCost, @BonusCardNumber, " +
                             "@ManagerId,  @UseIn1C, Getdate(), @Code, @ManagerConfirmed, @OrderSourceId, @CustomData, @IsDraft, " +
                             "@DeliveryDate, @DeliveryTime, @TrackNumber, @IsFromAdminArea, @OrderDiscountValue, @LeadId, @ShippingTaxType); " +
                        "SELECT scope_identity();",
                CommandType.Text,

                new SqlParameter("@Number", ord.Number ?? string.Empty),
                new SqlParameter("@ShippingMethodID", ord.ShippingMethodId != 0 ? ord.ShippingMethodId : (object)DBNull.Value),
                new SqlParameter("@PaymentMethodID", ord.PaymentMethodId != 0 ? ord.PaymentMethodId : (object)DBNull.Value),
                new SqlParameter("@ShippingMethodName", ord.ArchivedShippingName ?? string.Empty),
                new SqlParameter("@ShippingTaxType", (int)ord.ShippingTaxType),
                new SqlParameter("@PaymentMethodName", ord.PaymentMethodName ?? string.Empty),
                new SqlParameter("@OrderStatusID", ord.OrderStatusId),
                new SqlParameter("@AffiliateID", ord.AffiliateID),
                new SqlParameter("@ShippingCost", ord.ShippingCost),
                new SqlParameter("@PaymentCost", ord.PaymentCost),
                new SqlParameter("@OrderDate", ord.OrderDate.AddTicks(-(ord.OrderDate.Ticks % TimeSpan.TicksPerSecond))), 
                new SqlParameter("@CustomerComment", ord.CustomerComment ?? string.Empty),
                new SqlParameter("@StatusComment", ord.StatusComment ?? string.Empty),
                new SqlParameter("@AdditionalTechInfo", ord.AdditionalTechInfo ?? string.Empty),
                new SqlParameter("@AdminOrderComment", ord.AdminOrderComment ?? string.Empty),
                new SqlParameter("@GroupName", ord.GroupName ?? (CustomerGroupService.GetCustomerGroup() != null  ? CustomerGroupService.GetCustomerGroup().GroupName : string.Empty)),
                new SqlParameter("@GroupDiscount", ord.GroupDiscount),
                new SqlParameter("@OrderDiscount", ord.OrderDiscount),
                new SqlParameter("@OrderDiscountValue", ord.OrderDiscountValue),
                new SqlParameter("@CertificatePrice", ord.Certificate != null ? (object)ord.Certificate.Price : DBNull.Value),
                new SqlParameter("@CertificateCode", ord.Certificate != null ? (object)ord.Certificate.Code : DBNull.Value),
                new SqlParameter("@CouponCode", ord.Coupon != null ? (object)ord.Coupon.Code : DBNull.Value),
                new SqlParameter("@CouponType", ord.Coupon != null ? (object)ord.Coupon.Type : DBNull.Value),
                new SqlParameter("@CouponValue", ord.Coupon != null ? (object)ord.Coupon.Value : DBNull.Value),
                new SqlParameter("@BonusCost", ord.BonusCost),
                new SqlParameter("@BonusCardNumber", ord.BonusCardNumber ?? (object)DBNull.Value),
                new SqlParameter("@ManagerId", ord.ManagerId ?? (object)DBNull.Value),
                //new SqlParameter("@OrderType", ord.OrderType.ToString()),
                new SqlParameter("@UseIn1C", ord.UseIn1C),
                new SqlParameter("@Code", ord.Code),
                new SqlParameter("@ManagerConfirmed", ord.ManagerConfirmed),
                new SqlParameter("@OrderSourceId", ord.OrderSourceId),
                new SqlParameter("@CustomData", ord.CustomData ?? (object)DBNull.Value),
                new SqlParameter("@IsDraft", ord.IsDraft),
                new SqlParameter("@DeliveryDate", ord.DeliveryDate ?? (object)DBNull.Value),
                new SqlParameter("@DeliveryTime", ord.DeliveryTime ?? string.Empty),
                new SqlParameter("@TrackNumber", ord.TrackNumber ?? (object)DBNull.Value),
                new SqlParameter("@IsFromAdminArea", ord.IsFromAdminArea),
                new SqlParameter("@LeadId", ord.LeadId ?? (object)DBNull.Value)
                );
        }

        public static void AddPaymentDetails(int orderId, PaymentDetails details)
        {
            if (details != null)
                SQLDataAccess.ExecuteNonQuery("[Order].[sp_AddPaymentDetails]", CommandType.StoredProcedure,
                                                new SqlParameter("@OrderID", orderId),
                                                new SqlParameter("@CompanyName", details.CompanyName ?? string.Empty),
                                                new SqlParameter("@INN", details.INN ?? string.Empty),
                                                new SqlParameter("@Phone", details.Phone ?? string.Empty));
        }

        public static void UpdatePaymentDetails(int orderId, PaymentDetails details, OrderChangedBy changedBy = null, bool trackChanges = true)
        {
            if (details == null)
                return;

            if (trackChanges)
                OrderHistoryService.ChangingPaymentDetails(orderId, details, changedBy);

            SQLDataAccess.ExecuteNonQuery(
                "if(select count (orderid) from [Order].[PaymentDetails] where OrderID=@OrderID) > 0 " +
                " Update [Order].[PaymentDetails] Set CompanyName=@CompanyName, INN=@INN, phone=@phone Where OrderID=@OrderID" +
                " else " +
                " insert into [Order].[PaymentDetails] (OrderID, CompanyName,  INN, phone) values(@OrderID, @CompanyName, @INN, @phone) ", CommandType.Text,
                new SqlParameter("@OrderID", orderId),
                new SqlParameter("@CompanyName", details.CompanyName ?? string.Empty),
                new SqlParameter("@INN", details.INN ?? string.Empty),
                new SqlParameter("@Phone", details.Phone ?? string.Empty));
        }

        public static void UpdateNumber(int id, string number)
        {
            SQLDataAccess.ExecuteNonQuery("[Order].[sp_UpdateOrderNumber]", CommandType.StoredProcedure, new SqlParameter("@OrderID", id), new SqlParameter("@Number", number));
        }

        public static void UpdateAdminOrderComment(int id, string adminOrderComment, OrderChangedBy changedBy = null, bool trackChanges = true)
        {
            if (trackChanges)
                OrderHistoryService.ChangingAdminComment(id, adminOrderComment, changedBy);

            SQLDataAccess.ExecuteNonQuery("[Order].[sp_UpdateOrderAdminOrderComment]", CommandType.StoredProcedure, new SqlParameter("@OrderID", id),
                                        new SqlParameter("@AdminOrderComment", adminOrderComment ?? string.Empty));
        }

        public static void UpdateStatusComment(int id, string statusComment, OrderChangedBy changedBy = null, bool trackChanges = true)
        {
            if (trackChanges)
                OrderHistoryService.ChangingStatusComment(id, statusComment, changedBy);

            SQLDataAccess.ExecuteNonQuery("[Order].[sp_UpdateOrderStatusComment]", CommandType.StoredProcedure,
                                            new SqlParameter("@OrderID", id), new SqlParameter("@StatusComment", statusComment ?? string.Empty));
        }

        public static StatusInfo GetStatusInfo(string orderNum)
        {
            return SQLDataAccess.ExecuteReadOne("[Order].[sp_GetOrderStatusInfo]", CommandType.StoredProcedure,
                reader => new StatusInfo()
                {
                    StatusComment = SQLDataHelper.GetString(reader, "StatusComment"),
                    StatusName = SQLDataHelper.GetString(reader, "StatusName")
                },
                new SqlParameter("@OrderNum", orderNum));
        }

        public static void PayOrder(int orderId, bool pay, bool updateModules = true, bool trackChanges = true)
        {
            var order = GetOrder(orderId);
            if (order == null)
                throw new Exception("Can't pay empty order");

            if (pay && order.Payed || !pay && !order.Payed)
                return;

            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Order].[Order] SET [PaymentDate] = @PaymentDate, ModifiedDate = Getdate() WHERE [OrderID] = @OrderID", CommandType.Text,
                new SqlParameter("@OrderID", orderId),
                new SqlParameter("@PaymentDate", pay ? DateTime.Now : (object)DBNull.Value));

            var payOrderTemplate =
                new PayOrderTemplate(order.OrderID.ToString(), order.Number,
                    pay
                        ? LocalizationService.GetResource("Core.Orders.Order.PaySpend").ToLower()
                        : LocalizationService.GetResource("Core.Orders.Order.PayCancel").ToLower(),
                    order.Sum.ToString());
            payOrderTemplate.BuildMail();

            if (!payOrderTemplate.Body.IsNullOrEmpty() && !payOrderTemplate.Subject.IsNullOrEmpty())
            {
                SendMail.SendMailNow(Guid.Empty, SettingsMail.EmailForOrders, payOrderTemplate.Subject, payOrderTemplate.Body, true);
            }
            

            foreach (var certificate in GiftCertificateService.GetOrderCertificates(orderId))
            {
                GiftCertificateService.SendCertificateMails(certificate);
            }

            if (pay && BonusSystem.IsActive)
            {
                BonusSystemService.Confirm(order.BonusCardNumber, order.Number, orderId);
            }

            if (trackChanges)
                OrderHistoryService.ChangingPayDate(order, pay);

            if (updateModules)
            {
                ModulesExecuter.PayOrder(orderId, pay);
                ModulesExecuter.SendNotificationsOnPayOrder(orderId, pay);
            }
        }

        public static bool ManagerConfirmOrder(int orderId, bool confirm)
        {
            var order = GetOrder(orderId);
            if (order == null)
                throw new Exception("Can't pay empty order");

            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Order].[Order] SET [ManagerConfirmed] = @ManagerConfirmed, ModifiedDate = Getdate() WHERE [OrderID] = @OrderID", CommandType.Text,
                new SqlParameter("@OrderID", orderId),
                new SqlParameter("@ManagerConfirmed", confirm));

            return true;

        }

        public static bool ManagerConfirmOrders(bool confirm)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Order].[Order] SET [ManagerConfirmed] = @ManagerConfirmed, ModifiedDate = Getdate()",
                CommandType.Text,
                new SqlParameter("@ManagerConfirmed", confirm));

            return true;

        }

        public static PaymentDetails GetPaymentDetails(int orderId)
        {
            return SQLDataAccess.ExecuteReadOne("[Order].[sp_GetPaymentDetails]", CommandType.StoredProcedure,
                reader => new PaymentDetails
                {
                    CompanyName = SQLDataHelper.GetString(reader, "CompanyName"),
                    INN = SQLDataHelper.GetString(reader, "INN"),
                    Phone = SQLDataHelper.GetString(reader, "Phone")
                },
                new SqlParameter("@OrderID", orderId));
        }

        public static Order GetOrder(int orderId)
        {
            return SQLDataAccess.ExecuteReadOne<Order>(
                "SELECT * FROM [Order].[Order] WHERE [OrderID] = @OrderID",
                CommandType.Text,
                GetOrderFromReader,
                new SqlParameter("@OrderID", orderId));
        }

        public static int GetOrderIdByNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return 0;

            return SQLDataAccess.ExecuteScalar<int>("[Order].[sp_GetOrderIdByNumber]", CommandType.StoredProcedure,
                new SqlParameter("@Number", number));
        }

        public static int GetOrderIdByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return 0;

            return SQLDataAccess.ExecuteScalar<int>("SELECT [OrderID] FROM [Order].[Order] WHERE [Code] = @Code",
                CommandType.Text, new SqlParameter("@Code", code));
        }

        public static int GetOrderIdByLeadId(int leadId)
        {
            return SQLDataAccess.ExecuteScalar<int>("SELECT top(1) [OrderID] FROM [Order].[Order] WHERE [LeadId] = @leadId",
                CommandType.Text, new SqlParameter("@leadId", leadId));
        }

        public static string GetOrderNumberById(int orderId)
        {
            return SQLDataAccess.ExecuteScalar<string>(
             "SELECT [Number] FROM [Order].[Order] WHERE [OrderId] = @OrderId",
             CommandType.Text,
             new SqlParameter("@OrderId", orderId));
        }

        public static int GetCountOrder(string number)
        {
            int retCount = 0;

            if (!string.IsNullOrEmpty(number))
            {
                retCount = SQLDataAccess.ExecuteScalar<int>("[Order].[sp_GetCountOrderByNumber]", CommandType.StoredProcedure, new SqlParameter("@Number", number));
            }

            return retCount;
        }

        public static string GenerateNumber(int orderId)
        {
            var currentNumber = GetOrderNumberById(orderId);

            if (currentNumber.IsNotEmpty())
                return currentNumber;

            var format = SettingsCheckout.OrderNumberFormat;

            if (string.IsNullOrWhiteSpace(format))
                return orderId.ToString();

            if (format.Contains("#NUMBER#"))
                format = format.Replace("#NUMBER#", orderId.ToString());

            if (format.Contains("#YEAR#") || format.Contains("#MONTH#") || format.Contains("#DAY#"))
            {
                var now = DateTime.Now;
                format =
                    format.Replace("#YEAR#", now.ToString("yy"))
                          .Replace("#MONTH#", now.ToString("MM"))
                          .Replace("#DAY#", now.ToString("dd"));
            }

            if (format.Contains("#RRR#"))
            {
                var random = new Random();
                var result = "";
                var arr = format.Split("#RRR#");

                for (int i = 0; i < arr.Length - 1; i++)
                {
                    result += arr[i] + random.Next(0, 1000).ToString("###");
                }
                result += arr[arr.Length - 1];

                format = result;
            }

            return format;
        }


        public static void SerializeToXml(List<Order> orders, TextWriter baseWriter, bool isAdvanced = false)
        {
            using (var writer = XmlWriter.Create(baseWriter, new XmlWriterSettings { Encoding = Encoding.UTF8, Indent = true }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Orders");
                foreach (var order in orders)
                {
                    SerializeToXml(order, writer, isAdvanced);
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
            }
        }

        public static void SerializeToXml(Order order, TextWriter baseWriter)
        {
            SerializeToXml(new List<Order> { order }, baseWriter);
        }

        private static void SerializeToXml(Order order, XmlWriter writer, bool isAdvanced = false)
        {
            var customer = order.OrderCustomer;
            var currency = order.OrderCurrency;

            if (currency == null)
            {
                Debug.Log.Error("Order SerializeToXml currency is null");
                return;
            }

            var totalDiscount = order.TotalDiscount;
            var currencyValue = order.OrderCurrency.CurrencyValue;

            writer.WriteStartElement("Order");
            writer.WriteAttributeString("OrderID", order.OrderID.ToString());
            writer.WriteAttributeString("CustomerEmail",
                customer != null && customer.Email.IsNotEmpty() ? customer.Email : string.Empty);
            writer.WriteAttributeString("OfferType", "Obsolete");
            writer.WriteAttributeString("Date", order.OrderDate.ToString());
            writer.WriteAttributeString("IsPaid", order.PaymentDate != null ? "1" : "0");
            writer.WriteAttributeString("PaymentDate", order.PaymentDate != null ? order.PaymentDate.ToString() : string.Empty);

            writer.WriteAttributeString("Comments", order.CustomerComment);
            writer.WriteAttributeString("DiscountPercent", (order.OrderDiscount * currencyValue).ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("DiscountValue", (totalDiscount * currencyValue + order.OrderDiscountValue).ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("ShippingCost", (order.ShippingCost * currencyValue).ToString(CultureInfo.InvariantCulture));
            writer.WriteAttributeString("CustomerIP", customer != null ? order.OrderCustomer.CustomerIP : string.Empty);
            writer.WriteAttributeString("ShippingMethod", order.ArchivedShippingName);
            writer.WriteAttributeString("BillingMethod", order.PaymentMethodName);

            if (isAdvanced)
            {
                writer.WriteAttributeString("PaymentCost", (order.PaymentCost * currencyValue).ToString(CultureInfo.InvariantCulture));
                writer.WriteAttributeString("BonusCost", (order.BonusCost * currencyValue).ToString(CultureInfo.InvariantCulture));
                writer.WriteAttributeString("TaxCost",
                    (order.Taxes != null ? order.Taxes.Sum(tax => tax.Sum) * currencyValue : 0).ToString(CultureInfo.InvariantCulture));
                writer.WriteAttributeString("TaxInPrice",
                    (order.Taxes != null && order.Taxes.Count > 0
                        ? Convert.ToInt32(order.Taxes.FirstOrDefault().ShowInPrice)
                        : 0).ToString(CultureInfo.InvariantCulture));
                writer.WriteAttributeString("ModifiedDate", order.ModifiedDate.ToString());
            }

            var orderItemsCount = order.OrderItems.Count;
            //var discountOnProduct = (int)(totalDiscount / orderItemsCount);
            var bonusOnProduct = (int)(order.BonusCost / orderItemsCount);
            var paymentCostOnProduct = (int)(order.PaymentCost / orderItemsCount);
            var discountSum = 0f;
            var bonusSum = 0;
            var paymentSum = 0;
            var count = 0;

            var orderItemsSum = order.OrderItems.Sum(x => x.Amount * x.Price);

            if (order.OrderItems.Count != 0)
            {
                writer.WriteStartElement("Products");
                foreach (OrderItem item in order.OrderItems)
                {
                    writer.WriteStartElement("Product");
                    writer.WriteAttributeString("ID", item.ArtNo ?? item.ProductID.ToString());
                    writer.WriteAttributeString("Name", item.Name);
                    writer.WriteAttributeString("Цвет", item.Color);
                    writer.WriteAttributeString("Размер", item.Size);
                    writer.WriteAttributeString("Amount", item.Amount.ToString());
                    writer.WriteAttributeString("Price", (item.Price * currencyValue).ToString("F2", CultureInfo.InvariantCulture));
                    //for 1c, because this filds 1c are waiting
                    writer.WriteAttributeString("Currency", string.Empty);

                    if (isAdvanced)
                    {
                        var discountOnProduct =
                            (float)Math.Round(item.Amount * item.Price / orderItemsSum * totalDiscount, 2);

                        writer.WriteAttributeString("Discount",
                            ((orderItemsCount != count + 1 ? discountOnProduct : totalDiscount - discountSum) * currencyValue).ToString("F2", CultureInfo.InvariantCulture));

                        writer.WriteAttributeString("Bonus",
                            ((orderItemsCount != count + 1 ? bonusOnProduct : order.BonusCost - bonusSum) * currencyValue).ToString(CultureInfo.InvariantCulture));

                        writer.WriteAttributeString("Payment",
                            ((orderItemsCount != count + 1 ? paymentCostOnProduct : order.PaymentCost - paymentSum) * currencyValue).ToString(CultureInfo.InvariantCulture));

                        discountSum += discountOnProduct;
                        bonusSum += bonusOnProduct;
                        paymentSum += paymentCostOnProduct;
                    }

                    if (item.ProductID != null)
                    {
                        var product = ProductService.GetProduct((int)item.ProductID);
                        writer.WriteAttributeString("Unit", product == null ? "" : product.Unit);
                    }

                    writer.WriteEndElement();
                    count++;
                }
                writer.WriteEndElement();
            }

            if (customer != null)
            {
                var customerName = StringHelper.AggregateStrings(" ", customer.LastName, customer.FirstName, customer.Patronymic);

                writer.WriteStartElement("Customer");

                writer.WriteAttributeString("Surname", customer.LastName ?? string.Empty);
                writer.WriteAttributeString("Name", customer.FirstName ?? string.Empty);
                writer.WriteAttributeString("Email", customer.Email ?? string.Empty);
                writer.WriteAttributeString("CustomerType", string.Empty);
                
                writer.WriteAttributeString("City", string.Empty);
                writer.WriteAttributeString("Address", string.Empty);
                writer.WriteAttributeString("Zip", string.Empty);

                //for 1c, because this filds 1c are waiting
                writer.WriteAttributeString("Phone", customer.Phone);
                writer.WriteAttributeString("Fax", string.Empty);
                writer.WriteAttributeString("ShippingName", customerName);
                writer.WriteAttributeString("BillingName", customerName);
                writer.WriteAttributeString("ContactName", customerName);

                //for 1c, because this filds 1c are waiting
                writer.WriteAttributeString("ShippingEmail", string.Empty);
                writer.WriteAttributeString("BillingEmail", string.Empty);

                //writer.WriteAttributeString("BillingEmail", order.BillingContact.Name);

                writer.WriteAttributeString("ShippingCountry", customer.Country ?? "");
                writer.WriteAttributeString("BillingCountry", customer.Country ?? "");
                writer.WriteAttributeString("ShippingZone", customer.Region ?? "");
                writer.WriteAttributeString("BillingZone", customer.Region ?? "");

                writer.WriteAttributeString("ShippingCity", customer.City ?? "");
                writer.WriteAttributeString("BillingCity", customer.City ?? "");
                writer.WriteAttributeString("ShippingAddress", customer.GetCustomerAddress());
                writer.WriteAttributeString("BillingAddress", customer.GetCustomerAddress());
                writer.WriteAttributeString("ShippingZip", customer.Zip ?? "");
                writer.WriteAttributeString("BillingZip", customer.Zip ?? "");
                //for 1c, because this filds 1c are waiting
                writer.WriteAttributeString("ShippingPhone", string.Empty);
                writer.WriteAttributeString("BillingPhone", string.Empty);
                writer.WriteAttributeString("ShippingFax", string.Empty);
                writer.WriteAttributeString("BillingFax", string.Empty);

                writer.WriteEndElement();

            }

            writer.WriteEndElement();
        }

        public static List<OrderPriceDiscount> GetOrderPricesDiscounts()
        {
            var result = new List<OrderPriceDiscount>();

            if (SettingsCheckout.EnableDiscountModule)
            {
                result = CacheManager.Get(CacheNames.GetOrderPriceDiscountCacheObjectName(), 20,
                    () =>
                        SQLDataAccess.ExecuteReadList(
                            "SELECT PriceRange, PercentDiscount FROM [Order].OrderPriceDiscount ORDER BY PriceRange",
                            CommandType.Text,
                            reader => new OrderPriceDiscount
                            {
                                PercentDiscount = SQLDataHelper.GetDouble(reader, "PercentDiscount"),
                                PriceRange = SQLDataHelper.GetFloat(reader, "PriceRange")
                            })
                        ?? new List<OrderPriceDiscount>());
            }
            return result;
        }

        public static float GetDiscount(float price)
        {
            return GetDiscount(GetOrderPricesDiscounts(), price);
        }

        public static float GetDiscount(List<OrderPriceDiscount> table, float price)
        {
            var currency = CurrencyService.CurrentCurrency;

            return table == null
                       ? 0
                       : (float)
                         table.Where(dr => dr.PriceRange / currency.Rate <= price).OrderBy(dr => dr.PriceRange).DefaultIfEmpty(
                             new OrderPriceDiscount { PercentDiscount = 0 }).Last().PercentDiscount;
        }

        public static bool? IsDecremented(int orderId)
        {
            return SQLDataAccess.ExecuteScalar<bool>("[Order].[sp_IsDecremented]", CommandType.StoredProcedure, new SqlParameter("@OrderID", orderId));
        }

        public static Order RefreshTotal(Order order, bool ignoreHistory = true, OrderChangedBy changedBy = null, bool updateModules = true)
        {
            float totalPrice = 0;
            float totalProductsPrice = 0;
            float productsIgnoreDiscountPrice = 0;
            float totalDiscount = 0;
            float supplyTotal = 0;
            float bonusPrice = order.BonusCost;

            if (order.OrderItems.Count > 0)
            {
                totalProductsPrice = order.OrderItems.Sum(item => item.Price * item.Amount);
                productsIgnoreDiscountPrice = order.OrderItems.Where(item => item.IgnoreOrderDiscount).Sum(item => item.Price * item.Amount);
                supplyTotal = order.OrderItems.Sum(item => item.SupplyPrice * item.Amount);
            }
            else if (order.OrderCertificates.Count > 0)
            {
                totalProductsPrice = order.OrderCertificates.Sum(item => item.Sum);
            }

            totalDiscount += order.OrderDiscount > 0 ? (order.OrderDiscount * (totalProductsPrice - productsIgnoreDiscountPrice) / 100).RoundPrice(order.OrderCurrency) : 0;
            totalDiscount += order.OrderDiscountValue;

            if (order.Certificate != null)
            {
                totalDiscount += order.Certificate.Price != 0 ? order.Certificate.Price : 0;
            }

            if (order.Coupon != null)
            {
                switch (order.Coupon.Type)
                {
                    case CouponType.Fixed:
                        var productsPrice =
                            order.OrderItems.Where(item => item.IsCouponApplied).Sum(p => p.Price * p.Amount);
                        totalDiscount += productsPrice >= order.Coupon.Value ? order.Coupon.Value : productsPrice;
                        break;
                    case CouponType.Percent:
                        totalDiscount +=
                            order.OrderItems.Where(item => item.IsCouponApplied)
                                 .Sum(item => order.Coupon.Value * item.Price / 100 * item.Amount);
                        break;
                }
            }

            if (order.BonusCost > 0)
            {
                bonusPrice = BonusSystemService.GetBonusCost(totalProductsPrice - totalDiscount + order.ShippingCost, totalProductsPrice - totalDiscount, order.BonusCost);
            }
            
            totalDiscount = totalDiscount.RoundPrice(order.OrderCurrency);

            totalPrice = (totalProductsPrice - totalDiscount - bonusPrice + order.ShippingCost + order.PaymentCost).RoundPrice(order.OrderCurrency.CurrencyValue, order.OrderCurrency);

            if (totalPrice < 0) totalPrice = 0;

            order.Sum = totalPrice;
            order.SupplyTotal = supplyTotal;
            order.BonusCost = bonusPrice;
            order.DiscountCost = totalDiscount;
            order.TaxCost = 0;

            if (!ignoreHistory)
            {
                var refreshTotalOrder = new OnRefreshTotalOrder()
                {
                    Sum = order.Sum,
                    TaxCost = order.TaxCost,
                    BonusCost = order.BonusCost
                };
                OrderHistoryService.ChangingOrderTotal(order.OrderID, refreshTotalOrder, changedBy);
            }

            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Order].[Order] SET [Sum] = @Sum, [SupplyTotal] = @SupplyTotal, [BonusCost] = @BonusCost, [DiscountCost] = @DiscountCost WHERE [OrderID] = @OrderID",
                CommandType.Text,
                new SqlParameter("@OrderID", order.OrderID),
                new SqlParameter("@Sum", order.Sum),
                new SqlParameter("@SupplyTotal", order.SupplyTotal),
                new SqlParameter("@BonusCost", order.BonusCost),
                new SqlParameter("@DiscountCost", order.DiscountCost));

            if (updateModules)
                ModulesExecuter.OrderUpdated(order);

            return order;
        }

        public static int GetLastOrderId()
        {
            return SQLDataAccess.ExecuteScalar<int>("SELECT TOP 1 OrderID FROM [Order].[Order] order by OrderDate desc", CommandType.Text);
        }

        public static int GetLastDbOrderId()
        {
            return SQLDataHelper.GetInt(SQLDataAccess.ExecuteScalar("SELECT IDENT_CURRENT('[Order].[Order]')", CommandType.Text));
        }

        public static void ResetOrderID(int newOrderId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DBCC CHECKIDENT ('Order.Order', RESEED, @OrderId);", CommandType.Text,
                new SqlParameter("@OrderId", newOrderId));
        }

        //public static bool UpdateOrderContacts(int orderId, OrderContact shippingContact, OrderContact billingContact, OrderChangedBy changedBy = null, bool trackChanges = true)
        //{
        //    if (trackChanges)
        //        OrderHistoryService.ChangingContacts(orderId, shippingContact, billingContact, changedBy);

        //    bool res = true;
        //    ClearOrderContacts(orderId);
        //    if (shippingContact == billingContact)
        //        res &= AddOrderContacts(orderId, shippingContact);
        //    else
        //        res &= AddOrderContacts(orderId, shippingContact, billingContact);
        //    if (res)
        //        return RefreshTotal(orderId);

        //    return false;
        //}

        //private static void ClearOrderContacts(int orderId)
        //{
        //    SQLDataAccess.ExecuteNonQuery(
        //        @"with temp (ShippingContactID, BillingContactID) as 
        //        (SELECT ShippingContactID, BillingContactID FROM [Order].[Order] WHERE [OrderID] = @OrderID) 
        //        DELETE FROM [Order].[OrderContact] 
        //        WHERE [OrderContactID] = (select top(1) ShippingContactID from temp) 
        //        OR  [OrderContactID] = (select top(1) BillingContactID from temp); 
        //        UPDATE [Order].[Order] SET ShippingContactID = 0, BillingContactID = 0 WHERE [OrderID] = @OrderID",
        //        CommandType.Text,
        //        new SqlParameter("@OrderID", orderId));
        //}

        public static void UpdateOrderMain(Order order, bool updateModules = true, OrderChangedBy changedBy = null, bool trackChanges = true)
        {
            if (trackChanges)
                OrderHistoryService.ChangingOrderMain(order, changedBy);

            SQLDataAccess.ExecuteNonQuery(
                @"UPDATE [Order].[Order]
                       SET [Number] = @Number
                          ,[ShippingMethodID] = @ShippingMethodID
                          ,[PaymentMethodID] = @PaymentMethodID
                          ,[AffiliateID] = @AffiliateID
                          ,[OrderDiscount] = @OrderDiscount
                          ,[CustomerComment] = @CustomerComment
                          ,[StatusComment] = @StatusComment
                          ,[AdditionalTechInfo] = @AdditionalTechInfo
                          ,[AdminOrderComment] = @AdminOrderComment
                          ,[Decremented] = @Decremented
                          ,[ShippingCost] = @ShippingCost
                          ,[PaymentCost] = @PaymentCost
                          ,[TaxCost] = @TaxCost
                          ,[SupplyTotal] = @SupplyTotal
                          ,[Sum] = @Sum
                          ,[OrderStatusID] = @OrderStatusID
                          ,[ShippingMethodName] = @ShippingMethodName
                          ,[PaymentMethodName] = @PaymentMethodName
                          ,[GroupName] = @GroupName
                          ,[GroupDiscount] = @GroupDiscount
                          ,[OrderDate] = @OrderDate
                          ,[CertificateCode] = @CertificateCode
                          ,[CertificatePrice] = @CertificatePrice
                          ,[ManagerId] = @ManagerId                          
                          ,[UseIn1C] = @UseIn1C
                          ,[ModifiedDate] = Getdate()
                          ,[ManagerConfirmed] = @ManagerConfirmed
                          ,[OrderSourceId] = @OrderSourceId
                          ,[BonusCost] = @BonusCost
                          ,[BonusCardNumber] = @BonusCardNumber
                          ,[CustomData] = @CustomData
                          ,[IsDraft] = @IsDraft
                          ,[DeliveryDate] = @DeliveryDate
                          ,[DeliveryTime] = @DeliveryTime
                          ,[TrackNumber] = @TrackNumber
                          ,[OrderDiscountValue] = @OrderDiscountValue
                          ,[ShippingTaxType] = @ShippingTaxType
                     WHERE [OrderID] = @OrderID",
                CommandType.Text,
                new SqlParameter("@Number", order.Number),
                new SqlParameter("@ShippingMethodID", order.ShippingMethodId == 0 ? (object)DBNull.Value : order.ShippingMethodId),
                new SqlParameter("@PaymentMethodID", order.PaymentMethodId == 0 ? (object)DBNull.Value : order.PaymentMethodId),
                new SqlParameter("@AffiliateID", order.AffiliateID),
                new SqlParameter("@OrderDiscount", order.OrderDiscount),
                new SqlParameter("@CustomerComment", order.CustomerComment ?? string.Empty),
                new SqlParameter("@StatusComment", order.StatusComment ?? string.Empty),
                new SqlParameter("@AdditionalTechInfo", order.AdditionalTechInfo ?? string.Empty),
                new SqlParameter("@AdminOrderComment", order.AdminOrderComment ?? string.Empty),
                new SqlParameter("@Decremented", order.Decremented),
                new SqlParameter("@ShippingCost", order.ShippingCost),
                new SqlParameter("@PaymentCost", order.PaymentCost),
                new SqlParameter("@TaxCost", order.TaxCost),
                new SqlParameter("@SupplyTotal", order.SupplyTotal),
                new SqlParameter("@Sum", order.Sum),
                new SqlParameter("@OrderStatusID", order.OrderStatusId),
                new SqlParameter("@OrderID", order.OrderID),
                new SqlParameter("@ShippingMethodName", order.ArchivedShippingName),
                new SqlParameter("@PaymentMethodName", order.PaymentMethodName),
                new SqlParameter("@GroupName", order.GroupName),
                new SqlParameter("@GroupDiscount", order.GroupDiscount),
                new SqlParameter("@OrderDate", order.OrderDate),
                new SqlParameter("@CertificateCode", order.Certificate != null ? (object)order.Certificate.Code : DBNull.Value),
                new SqlParameter("@CertificatePrice", order.Certificate != null ? (object)order.Certificate.Price : DBNull.Value),
                new SqlParameter("@ManagerId", order.ManagerId ?? (object)DBNull.Value),
                //new SqlParameter("@OrderType", order.OrderType.ToString()),
                new SqlParameter("@UseIn1C", order.UseIn1C),
                new SqlParameter("@ManagerConfirmed", order.ManagerConfirmed),
                new SqlParameter("@OrderSourceId", order.OrderSourceId),
                new SqlParameter("@BonusCost", order.BonusCost),
                new SqlParameter("@BonusCardNumber", order.BonusCardNumber ?? (object)DBNull.Value),
                new SqlParameter("@CustomData", order.CustomData ?? (object)DBNull.Value),
                new SqlParameter("@IsDraft", order.IsDraft),
                new SqlParameter("@DeliveryDate", order.DeliveryDate ?? (object)DBNull.Value),
                new SqlParameter("@DeliveryTime", order.DeliveryTime ?? string.Empty),
                new SqlParameter("@TrackNumber", order.TrackNumber ?? (object)DBNull.Value),
                new SqlParameter("@OrderDiscountValue", order.OrderDiscountValue),
                new SqlParameter("@ShippingTaxType", (int)order.ShippingTaxType)
                );

            if (updateModules)
            {
                ModulesExecuter.OrderUpdated(order);
                ModulesExecuter.SendNotificationsOnOrderUpdated(order);
            }
        }

        public static void DecrementProductsCountAccordingOrder(int ordId)
        {
            if (Settings1C.Enabled && Settings1C.DisableProductsDecremention)
                return;

            SQLDataAccess.ExecuteNonQuery("[Order].[sp_DecrementProductsCountAccordingOrder]",
                                            CommandType.StoredProcedure,
                                            new SqlParameter("@orderId", ordId));

            foreach (var orderitem in GetOrderItems(ordId).Where(orderitem => orderitem.ProductID.HasValue))
            {
                ProductService.PreCalcProductParams((int)orderitem.ProductID);
            }
        }

        public static void IncrementProductsCountAccordingOrder(int ordId)
        {
            if (Settings1C.Enabled && Settings1C.DisableProductsDecremention)
                return;

            SQLDataAccess.ExecuteNonQuery("[Order].[sp_IncrementProductsCountAccordingOrder]",
                                            CommandType.StoredProcedure,
                                            new SqlParameter("@orderId", ordId));

            foreach (var orderitem in GetOrderItems(ordId).Where(orderitem => orderitem.ProductID.HasValue))
            {
                ProductService.PreCalcProductParams((int)orderitem.ProductID);
            }
        }

        public static OrderCustomer GetOrderCustomer(int orderId)
        {
            return
                SQLDataAccess.Query<OrderCustomer>("SELECT * FROM [Order].[OrderCustomer] WHERE [OrderID] = @orderId",
                    new {orderId}).FirstOrDefault();
        }

        public static OrderCustomer GetOrderCustomer(string orderNumber)
        {
            return
                SQLDataAccess.Query<OrderCustomer>("SELECT * FROM [Order].[OrderCustomer] WHERE [OrderID] = (Select OrderID from [Order].[Order] Where Number=@orderNumber)",
                    new { orderNumber }).FirstOrDefault();
        }
        
        public static List<string> GetOrderCustomersEmails()
        {
            return SQLDataAccess.ExecuteReadColumn<string>(
                "SELECT DISTINCT [Email] FROM [Order].[OrderCustomer] WHERE [Email] IS NOT NULL AND [Email] <> ''",
                CommandType.Text,
                "Email");
        }

        public static OrderCurrency GetOrderCurrency(int orderId)
        {
            return SQLDataAccess.ExecuteReadOne(
                "SELECT * FROM [Order].[OrderCurrency] WHERE [OrderID] = @OrderID",
                CommandType.Text,
                reader =>
                new OrderCurrency
                {
                    CurrencyCode = SQLDataHelper.GetString(reader, "CurrencyCode"),
                    CurrencyNumCode = SQLDataHelper.GetInt(reader, "CurrencyNumCode"),
                    CurrencyValue = SQLDataHelper.GetFloat(reader, "CurrencyValue"),
                    CurrencySymbol = SQLDataHelper.GetString(reader, "CurrencySymbol"),
                    IsCodeBefore = SQLDataHelper.GetBoolean(reader, "IsCodeBefore"),
                    EnablePriceRounding = SQLDataHelper.GetBoolean(reader, "EnablePriceRounding"),
                    RoundNumbers = SQLDataHelper.GetFloat(reader, "RoundNumbers"),
                }, new SqlParameter("@OrderID", orderId));
        }

        public static OrderPickPoint GetOrderPickPoint(int orderId)
        {
            return SQLDataAccess.ExecuteReadOne(
                "SELECT * FROM [Order].[OrderPickPoint] WHERE [OrderID] = @OrderID",
                CommandType.Text,
                reader =>
                new OrderPickPoint
                {
                    OrderId = orderId,
                    PickPointId = SQLDataHelper.GetString(reader, "PickPointId"),
                    PickPointAddress = SQLDataHelper.GetString(reader, "PickPointAddress"),
                    AdditionalData = SQLDataHelper.GetString(reader, "AdditionalData", ""),
                }, new SqlParameter("@OrderID", orderId));
        }
        

        public static void UpdateOrderCurrency(int orderId, string currencyCode, float currencyValue, OrderChangedBy changedBy = null, bool trackChanges = true)
        {
            if (trackChanges)
                OrderHistoryService.ChangingCurrency(orderId, currencyCode, currencyValue, changedBy);

            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Order].[OrderCurrency] SET [CurrencyCode] = @CurrencyCode, [CurrencyValue] = @CurrencyValue WHERE [OrderID] = @OrderID",
                CommandType.Text,
                new SqlParameter("@OrderID", orderId),
                new SqlParameter("@CurrencyCode", currencyCode),
                new SqlParameter("@CurrencyValue", currencyValue));
        }

        

        public static Order GetOrderByNumber(string orderNumber)
        {
            return GetOrder(GetOrderIdByNumber(orderNumber));
        }

        public static Order GetOrderByCode(string code)
        {
            return GetOrder(GetOrderIdByCode(code));
        }

        public static string ProcessOrder(Order order, PageWithPaymentButton page,
                                            PaymentMethod customPaymentMethod = null)
        {
            if (order == null)
                return string.Empty;

            var paymentMethod = customPaymentMethod ?? order.PaymentMethod;
            if (paymentMethod == null)
                return string.Empty;
            return paymentMethod.Process(order, page);
        }

        public static void CancelOrder(int orderID)
        {
            OrderStatusService.ChangeOrderStatus(orderID, OrderStatusService.CanceledOrderStatus, LocalizationService.GetResource("Core.Orders.Order.OrderCanceled"));
            UpdateStatusComment(orderID, LocalizationService.GetResource("Core.Orders.Order.UserCanceledOrder"));
        }


        public static void SendOrderMail(Order order, float totalDiscount, float bonusPlus, 
                                         string shippingName, string paymentName)
        {
            var orderItemsHtml = GenerateOrderItemsHtml(order.OrderItems, order.OrderCurrency,
                                                        order.OrderItems.Sum(oi => oi.Price * oi.Amount),
                                                        order.OrderDiscount, order.OrderDiscountValue,
                                                        order.Coupon, order.Certificate,
                                                        totalDiscount,
                                                        order.ShippingCost, order.PaymentCost,
                                                        order.TaxCost,
                                                        order.BonusCost,
                                                        bonusPlus);

            const string format = "<div class='l-row'><div class='l-name vi cs-light' style='display: inline-block; margin: 5px 0; padding-right: 15px; width: 150px;'>{0}:</div><div class='l-value vi' style='display: inline-block; margin: 5px 0;'>{1}</div></div>";

            var customer = order.OrderCustomer;

            // Build a new mail
            var customerSb = new StringBuilder();
            customerSb.AppendFormat(format, SettingsCheckout.CustomerFirstNameField, customer.FirstName);

            if (SettingsCheckout.IsShowLastName && !string.IsNullOrEmpty(customer.LastName))
                customerSb.AppendFormat(format, LocalizationService.GetResource("User.Registration.LastName"), customer.LastName);

            if (SettingsCheckout.IsShowPatronymic && !string.IsNullOrEmpty(customer.Patronymic))
                customerSb.AppendFormat(format, LocalizationService.GetResource("User.Registration.Patronymic"), customer.Patronymic);

            if (SettingsCheckout.IsShowPhone && !string.IsNullOrEmpty(customer.Phone))
                customerSb.AppendFormat(format, SettingsCheckout.CustomerPhoneField, customer.Phone);

            if (SettingsCheckout.IsShowCountry && !string.IsNullOrEmpty(customer.Country))
                customerSb.AppendFormat(format, LocalizationService.GetResource("User.Registration.Country"), customer.Country);

            if (SettingsCheckout.IsShowState && customer.Region.IsNotEmpty())
                customerSb.AppendFormat(format, LocalizationService.GetResource("User.Registration.Region"), customer.Region);

            if (SettingsCheckout.IsShowCity && !string.IsNullOrEmpty(customer.City))
                customerSb.AppendFormat(format, LocalizationService.GetResource("User.Registration.City"), customer.City);

            if (SettingsCheckout.IsShowZip && !string.IsNullOrEmpty(customer.Zip))
                customerSb.AppendFormat(format, LocalizationService.GetResource("User.Registration.Zip"), customer.Zip);

            if (SettingsCheckout.IsShowAddress)
            {
                var address = !SettingsCheckout.IsShowFullAddress
                    ? customer.Street
                    : customer.GetCustomerAddress();

                customerSb.AppendFormat(format, LocalizationService.GetResource("User.Registration.Address"),
                    string.IsNullOrEmpty(address)
                        ? LocalizationService.GetResource("User.Registration.AddressEmpty")
                        : address);
            }

            if (SettingsCheckout.IsShowCustomShippingField1 && customer.CustomField1.IsNotEmpty())
                customerSb.AppendFormat(format, SettingsCheckout.CustomShippingField1, customer.CustomField1);

            if (SettingsCheckout.IsShowCustomShippingField2 && customer.CustomField2.IsNotEmpty())
                customerSb.AppendFormat(format, SettingsCheckout.CustomShippingField2, customer.CustomField2);

            if (SettingsCheckout.IsShowCustomShippingField3 && customer.CustomField3.IsNotEmpty())
                customerSb.AppendFormat(format, SettingsCheckout.CustomShippingField3, customer.CustomField3);


            var email = customer.Email;
            var orderMailTemplate = new NewOrderMailTemplate(order.Number, email, customerSb.ToString(),
                                                            shippingName + (order.OrderPickPoint != null ? "<br />" + order.OrderPickPoint.PickPointAddress : ""),
                                                            paymentName,
                                                            orderItemsHtml,
                                                            order.OrderCurrency.CurrencyCode,
                                                            order.Sum.ToString(),
                                                            order.CustomerComment,
                                                            GetBillingLinkHash(order),
                                                            customer.FirstName,
                                                            customer.LastName);

            orderMailTemplate.BuildMail();

            if (!string.IsNullOrWhiteSpace(email))
            {
                SendMail.SendMailNow(customer.CustomerID, email, orderMailTemplate.Subject, orderMailTemplate.Body, true);
            }
            SendMail.SendMailNow(Guid.Empty, SettingsMail.EmailForOrders, orderMailTemplate.Subject, orderMailTemplate.Body, true, email);
        }

        public static string GenerateOrderItemsHtml(List<OrderItem> orderItems, Currency currency, float productsPrice, 
                                                    float orderDiscountPercent, float orderDiscountValue, OrderCoupon coupon, OrderCertificate certificate, 
                                                    float totalDiscount, float shippingPrice,float paymentPrice, float taxesTotal, float bonusPrice, 
                                                    float newBonus)
        {
            var orderItemsHtml = new StringBuilder();


            orderItemsHtml.Append("<table class='orders-table' style='border-collapse: collapse; width: 100%;'>");
            orderItemsHtml.Append("<tr class='orders-table-header'>");
            orderItemsHtml.AppendFormat("<th class='photo' style='border-bottom: 1px solid #e3e3e3; padding: 20px 5px 20px 0; text-align: left;'>{0}</th>", LocalizationService.GetResource("Core.Orders.Order.Letter.Goods"));
            orderItemsHtml.Append("<th class='name' style='border-bottom: 1px solid #e3e3e3; padding: 20px 0; text-align: left; width: 50%;'></th>");
            orderItemsHtml.AppendFormat("<th class='price' style='border-bottom: 1px solid #e3e3e3; padding: 20px 5px 20px 0; text-align: center;'>{0}</th>", LocalizationService.GetResource("Core.Orders.Order.Letter.Price"));
            orderItemsHtml.AppendFormat("<th class='amount' style='border-bottom: 1px solid #e3e3e3; padding: 20px 5px 20px 0; text-align: center; white-space:nowrap;'>{0}</th>", LocalizationService.GetResource("Core.Orders.Order.Letter.Count"));
            orderItemsHtml.AppendFormat("<th class='total-price' style='border-bottom: 1px solid #e3e3e3; padding: 20px 0 20px 0; text-align: center;'>{0}</th>", LocalizationService.GetResource("Core.Orders.Order.Letter.Cost"));
            orderItemsHtml.Append("</tr>");

            //var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

            // Добавление заказанных товаров
            foreach (var item in orderItems)
            {
                orderItemsHtml.Append("<tr>");

                Photo photo;
                if (item.PhotoID.HasValue && item.PhotoID != 0 && (photo = PhotoService.GetPhoto((int)item.PhotoID)) != null)
                {
                    orderItemsHtml.AppendFormat(
                        "<td class='photo' style='border-bottom: 1px solid #e3e3e3; margin-right: 15px; padding: 20px 5px 20px 0; padding-left: 20px; text-align: left; width:{1}px;'><img style='border:none;display:block;outline:none;text-decoration:none;max-width:100%;height:auto;' src='{0}' /></td>",
                        FoldersHelper.GetImageProductPath(ProductImageType.XSmall, photo.PhotoName, false), SettingsPictureSize.XSmallProductImageWidth);
                }
                else
                {
                    orderItemsHtml.AppendFormat("<td>&nbsp;</td>");
                }

                var product = item.ProductID.HasValue ? ProductService.GetProduct(item.ProductID.Value) : null;

                orderItemsHtml.AppendFormat("<td class='name' style='border-bottom: 1px solid #e3e3e3; padding: 20px 5px 20px 0; text-align: left; min-width:150px; width: 50%;'>" +
                                                    "<div class='description' style='display: inline-block;'>" +
                                                        "<div class='prod-name' style='font-size: 14px; margin-bottom: 5px;'><a href='{0}' class='cs-link' style='color: #0764c3; text-decoration: none;'>{1}</a></div> " +
                                                        "{2} {3} {4}" +
                                                    "</div>" +
                                            "</td>",
                                            product != null ?
                                                        SettingsMain.SiteUrl.Trim('/') + "/" + UrlService.GetLink(ParamType.Product, product.UrlPath, product.ProductId)
                                                        : "",
                                            item.ArtNo + ", " + item.Name,
                                            item.Color.IsNotEmpty() ? "<div class='prod-option' style='margin-bottom: 5px;'><span class='cs-light' style='color: #acacac;'>" + SettingsCatalog.ColorsHeader + ":</span><span class='value cs-link' style='padding-left: 10px;'>" + item.Color + "</span></div>" : "",
                                            item.Size.IsNotEmpty() ? "<div class='prod-option' style='margin-bottom: 5px;'><span class='cs-light' style='color: #acacac;'>" + SettingsCatalog.SizesHeader + ":</span><span class='value cs-link' style='padding-left: 10px;'>" + item.Size + "</span></div>" : "",
                                            RenderSelectedOptions(item.SelectedOptions, currency));
                orderItemsHtml.AppendFormat("<td class='price' style='border-bottom: 1px solid #e3e3e3; padding: 20px 5px 20px 0; text-align: center; white-space: nowrap;'>{0}</td>", item.Price.FormatPrice(currency));
                orderItemsHtml.AppendFormat("<td class='amount' style='border-bottom: 1px solid #e3e3e3; padding: 20px 5px 20px 0; text-align: center;'>{0}</td>", item.Amount);
                orderItemsHtml.AppendFormat("<td class='total-price' style='border-bottom: 1px solid #e3e3e3; padding: 20px 0 20px 0; text-align: center;  white-space: nowrap;'>{0}</td>", (item.Price * item.Amount).FormatPrice(currency));
                orderItemsHtml.Append("</tr>");
            }

            const string footerFormat = "<tr>" +
                                            "<td class='footer-name' colspan='4' style='border-bottom: none; padding: 5px; text-align: right;'>{0}:</td>" +
                                            "<td class='footer-value' style='border-bottom: none; padding: 5px 0; text-align: center;'>{1}</td>" +
                                        "</tr>";

            const string footerMinusFormat = "<tr>" +
                                                "<td class='footer-name' colspan='4' style='border-bottom: none; padding: 5px; text-align: right;'>{0}:</td>" +
                                                "<td class='footer-value' style='border-bottom: none; padding: 5px 0; text-align: center;'>-{1}</td>" +
                                            "</tr>";

            // Стоимость заказа
            orderItemsHtml.AppendFormat(footerFormat, LocalizationService.GetResource("Core.Orders.Order.Letter.OrderCost"), productsPrice.FormatPrice(currency));

            if (orderDiscountPercent != 0 || orderDiscountValue != 0)
            {
                var productsIgnoreDiscountPrice = orderItems.Where(item => item.IgnoreOrderDiscount).Sum(item => item.Price * item.Amount);
                orderItemsHtml.AppendFormat(footerMinusFormat,
                    LocalizationService.GetResource("Core.Orders.Order.Letter.Discount"), PriceFormatService.FormatDiscountPercent(productsPrice - productsIgnoreDiscountPrice, orderDiscountPercent, orderDiscountValue, false));
            }

            if (bonusPrice != 0)
                orderItemsHtml.AppendFormat(footerMinusFormat,
                    LocalizationService.GetResource("Core.Orders.Order.Letter.Bonuses"), bonusPrice.FormatPrice(currency));

            if (certificate != null)
                orderItemsHtml.AppendFormat(footerMinusFormat,
                    LocalizationService.GetResource("Core.Orders.Order.Letter.Certificate"), certificate.Price.FormatPrice(currency));

            if (coupon != null)
            {
                float couponValue;
                string couponString = null;
                switch (coupon.Type)
                {
                    case CouponType.Fixed:
                        var productPrice = orderItems.Where(p => p.IsCouponApplied).Sum(p => p.Price * p.Amount);
                        couponValue = productPrice >= coupon.Value ? coupon.Value : productPrice;
                        couponString = String.Format("-{0} ", PriceFormatService.FormatPrice(couponValue.RoundPrice(currency.Rate, currency), currency));
                        break;
                    case CouponType.Percent:
                        couponValue = orderItems.Where(p => p.IsCouponApplied).Sum(p => coupon.Value * p.Price / 100 * p.Amount);
                        couponString = String.Format("-{0} ({1}%)", PriceFormatService.FormatPrice(couponValue.RoundPrice(currency.Rate, currency), currency),
                                                       PriceFormatService.FormatPriceInvariant(coupon.Value));
                        break;
                }
                orderItemsHtml.AppendFormat(footerMinusFormat,
                    LocalizationService.GetResource("Core.Orders.Order.Letter.Coupon"), couponString);//coupon.Value.FormatPriceInvariant());
            }

            // Стоимость доставки
            if (shippingPrice != 0)
                orderItemsHtml.AppendFormat(footerFormat,
                    LocalizationService.GetResource("Core.Orders.Order.Letter.ShippingCost"), shippingPrice.FormatPrice(currency));

            if (paymentPrice != 0)
                orderItemsHtml.AppendFormat(footerFormat,
                    paymentPrice > 0 ? LocalizationService.GetResource("Core.Orders.Order.Letter.PaymentCost") : LocalizationService.GetResource("Core.Orders.Order.Letter.PaymentDiscount"), paymentPrice.FormatPrice(currency));

            var total = productsPrice - totalDiscount - bonusPrice + shippingPrice + paymentPrice;
            if (total < 0) total = 0;

            // Итого
            orderItemsHtml.AppendFormat(footerFormat,
                "<b>" + LocalizationService.GetResource("Core.Orders.Order.Letter.Total") + "</b>",
                "<b>" + total.FormatPrice(currency) + "</b>");

            if (newBonus > 0)
            {
                orderItemsHtml.AppendFormat(footerFormat,
                    LocalizationService.GetResource("Core.Orders.Order.Letter.NewBonus"), newBonus.ToString("F2"));
            }

            orderItemsHtml.Append("</table>");

            return orderItemsHtml.ToString();
        }

        public static string GenerateCertificatesHtml(List<GiftCertificate> orderCetificates, Currency currency, float paymentPrice, float taxesTotal)
        {
            var certificatesHtml = new StringBuilder();

            certificatesHtml.Append("<table class='orders-table' style='border-collapse: collapse; width: 100%;'>");
            certificatesHtml.Append("<tr>");
            certificatesHtml.AppendFormat("<td style='border-bottom: 1px solid #e3e3e3; padding: 20px 0; text-align: left;'>{0}</td>", LocalizationService.GetResource("Core.Orders.Order.Letter.Certificate"));
            certificatesHtml.AppendFormat("<td style='border-bottom: 1px solid #e3e3e3; padding: 20px 0; text-align: center; width: 150px;'>{0}</td>", LocalizationService.GetResource("Core.Orders.Order.Letter.Price"));
            certificatesHtml.Append("</tr>");

            // Добавление заказанных сертификатов
            foreach (var item in orderCetificates)
            {
                certificatesHtml.Append("<tr>");
                certificatesHtml.AppendFormat("<td style='border-bottom: 1px solid #e3e3e3; padding: 20px 0; text-align: center;'>{0}</td>", item.CertificateCode);
                certificatesHtml.AppendFormat("<td style='border-bottom: 1px solid #e3e3e3; padding: 20px 0; text-align: center;'>{0}</td>", item.Sum.FormatPrice(currency));
                certificatesHtml.Append("</tr>");
            }

            const string footerFormat = "<tr>" +
                                            "<td style='border-bottom: none; padding: 5px 0; text-align: right;'>{0}:</td>" +
                                            "<td style='border-bottom: none; padding: 5px 0; text-align: center;'>{1}</td>" +
                                        "</tr>";

            // Налоги
            var taxes = TaxService.CalculateCertificateTaxes(orderCetificates.Sum(cert => cert.Sum));
            float taxesExcluded = taxes.Where(tax => tax.Key.ShowInPrice).Sum(tax => tax.Value);
            foreach (var tax in taxes)
            {
                certificatesHtml.AppendFormat(footerFormat,
                    (tax.Key.ShowInPrice ? LocalizationService.GetResource("Core.Tax.IncludeTax") : "") + " " + tax.Key.Name,
                    (tax.Key.ShowInPrice ? "" : "+") + tax.Value.FormatPrice(currency));
            }

            if (paymentPrice != 0)
            {
                certificatesHtml.AppendFormat(footerFormat,
                    paymentPrice > 0
                        ? LocalizationService.GetResource("Core.Orders.Order.Letter.PaymentCost")
                        : LocalizationService.GetResource("Core.Orders.Order.Letter.PaymentDiscount"),
                    paymentPrice.FormatPrice(currency));
            }

            // Итого
            certificatesHtml.AppendFormat(footerFormat,
                "<b>" + LocalizationService.GetResource("Core.Orders.Order.Letter.Total") + "</b>",
                "<b>" + (orderCetificates.Sum(cert => cert.Sum) + paymentPrice + taxesExcluded).FormatPrice(currency) + "</b>");

            certificatesHtml.Append("</table>");

            return certificatesHtml.ToString();
        }

        public static string GenerateCustomerContactsHtml(OrderCustomer customer)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(customer.FirstName))
                sb.AppendFormat("Имя" + " {0}<br/>", customer.FirstName);

            if (!string.IsNullOrEmpty(customer.LastName))
                sb.AppendFormat("Фамилия" + " {0}<br/>", customer.LastName);

            if (!string.IsNullOrEmpty(customer.Country))
                sb.AppendFormat("Страна" + " {0}<br/>", customer.Country);

            if (!string.IsNullOrEmpty(customer.Region))
                sb.AppendFormat("Регион" + " {0}<br/>", customer.Region);

            if (!string.IsNullOrEmpty(customer.City))
                sb.AppendFormat("Город" + " {0}<br/>", customer.City);

            if (!string.IsNullOrEmpty(customer.Zip))
                sb.AppendFormat("Индекс" + " {0}<br/>", customer.Zip);

            if (!string.IsNullOrEmpty(customer.GetCustomerAddress()))
                sb.AppendFormat("Адрес" + ": {0}<br/>", customer.GetCustomerAddress());

            return sb.ToString();
        }

        public static string RenderSelectedOptions(List<EvaluatedCustomOptions> evlist, Currency currency)
        {
            if (evlist == null || evlist.Count == 0)
                return string.Empty;

            var res = new StringBuilder("<div class=\"customoptions\">");

            foreach (EvaluatedCustomOptions evco in evlist)
            {
                res.Append(evco.CustomOptionTitle + ": " + evco.OptionTitle + " ");
                if (evco.OptionPriceBc > 0)
                {
                    res.Append(evco.OptionPriceType == OptionPriceType.Fixed
                        ? "+" + evco.OptionPriceBc.FormatPrice(currency)
                        : evco.FormatPrice);
                }
                res.Append("<br />");
            }
            res.Append("</div>");

            return res.ToString();
        }

        public static bool IsPaidOrder(int orderId)
        {
            return Convert.ToInt32(
                SQLDataAccess.ExecuteScalar(
                "Select COUNT([PaymentDate]) FROM [Order].[Order] WHERE OrderID = @OrderID AND [PaymentDate] is not null",
                CommandType.Text,
                new SqlParameter("@OrderID", orderId))) > 0;
        }

        public static string GetBillingLinkHash(Order order)
        {
            if (order == null || order.OrderCustomer == null)
                return string.Empty;

            return (order.OrderID + order.Number + order.OrderCustomer.CustomerID).Md5(false).ToString();
        }

        public static void UpdateOrderManager(int orderId, int? managerId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE [Order].[Order] SET [ManagerId] = @ManagerId WHERE [OrderID] = @OrderID",
                CommandType.Text,
                new SqlParameter("@OrderID", orderId),
                new SqlParameter("@ManagerId", managerId ?? (object)DBNull.Value));
        }

        [Obsolete("Использовать UpdateOrder")]
        public static void ChangeUseIn1C(int orderId, bool useIn1C)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [Order].[Order] Set UseIn1C = @UseIn1C, ModifiedDate = Getdate() Where OrderId=@OrderId",
                CommandType.Text,
                new SqlParameter("@OrderId", orderId),
                new SqlParameter("@UseIn1C", useIn1C));
        }

        public static List<int> GetDeletedOrders(DateTime? from, DateTime? to)
        {
            var query = "SELECT OrderId FROM [Order].[DeletedOrders]";
            var queryParams = new List<SqlParameter>();

            if (from != null && to != null)
            {
                query += " Where [DateTime] >= @From and [DateTime] <= @To";
                queryParams.Add(new SqlParameter("@From", from));
                queryParams.Add(new SqlParameter("@To", to));
            }

            return SQLDataAccess.ExecuteReadList(query, CommandType.Text, reader => SQLDataHelper.GetInt(reader, "OrderId"), queryParams.ToArray());
        }

        public static List<Order> GetOrdersFor1C(DateTime from, DateTime to, bool onlyUseIn1C)
        {
            var query = "SELECT * FROM [Order].[Order] WHERE IsDraft <> 1 and ";
            var queryParams = new List<SqlParameter>();

            if (onlyUseIn1C)
            {
                query += "[UseIn1C] = 1 and ";
            }

            query += "([OrderDate] >= @From and [OrderDate] <= @To or [ModifiedDate] >= @From and [ModifiedDate] <= @To)";
            queryParams.Add(new SqlParameter("@From", from));
            queryParams.Add(new SqlParameter("@To", to));

            return SQLDataAccess.ExecuteReadList(query, CommandType.Text, GetOrderFromReader, queryParams.ToArray());
        }

        public static Order CreateOrder(Lead lead)
        {
            try
            {
                var order = new Order()
                {
                    OrderCurrency = (Currency) lead.LeadCurrency,
                    OrderStatusId = OrderStatusService.DefaultOrderStatus,
                    CustomerComment = lead.Comment,
                    ManagerId = lead.ManagerId,
                    OrderDate = DateTime.Now,
                    OrderDiscount = lead.Discount,
                    OrderDiscountValue = lead.DiscountValue,
                    OrderSourceId = lead.OrderSourceId,

                    DeliveryDate = lead.DeliveryDate,
                    DeliveryTime = lead.DeliveryTime,
                    ShippingMethodId = lead.ShippingMethodId,
                    ArchivedShippingName = lead.ShippingName,
                    ShippingCost = lead.ShippingCost,
                    OrderPickPoint = !string.IsNullOrEmpty(lead.ShippingPickPoint)
                        ? JsonConvert.DeserializeObject<OrderPickPoint>(lead.ShippingPickPoint)
                        : null,

                    LeadId = lead.Id,
                    IsFromAdminArea = true
                };

                if (SettingsCrm.OrderStatusIdFromLead != 0 &&
                    OrderStatusService.GetOrderStatus(SettingsCrm.OrderStatusIdFromLead) != null)
                {
                    order.OrderStatusId = SettingsCrm.OrderStatusIdFromLead;
                }

                if (lead.CustomerId != null && lead.Customer != null)
                {
                    order.OrderCustomer = (OrderCustomer) lead.Customer;
                    var contact = lead.Customer.Contacts.FirstOrDefault();

                    if (contact != null)
                    {
                        order.OrderCustomer.Country = contact.Country;
                        order.OrderCustomer.City = contact.City;
                        order.OrderCustomer.Region = contact.Region;
                        order.OrderCustomer.Zip = contact.Zip;
                        order.OrderCustomer.Apartment = contact.Apartment;
                        order.OrderCustomer.Entrance = contact.Entrance;
                        order.OrderCustomer.Floor = contact.Floor;
                        order.OrderCustomer.House = contact.House;
                        order.OrderCustomer.Street = contact.Street;
                        order.OrderCustomer.Structure = contact.Structure;
                    }
                }
                else
                {
                    order.OrderCustomer = new OrderCustomer()
                    {
                        CustomerID = Guid.NewGuid(),
                        FirstName = lead.FirstName,
                        LastName = lead.LastName,
                        Patronymic = lead.Patronymic,
                        Email = lead.Email,
                        Phone = lead.Phone,
                        StandardPhone = !string.IsNullOrWhiteSpace(lead.Phone) ? StringHelper.ConvertToStandardPhone(lead.Phone): null
                    };
                }

                foreach (var item in lead.LeadItems)
                    order.OrderItems.Add((OrderItem)item);
                
                order.OrderID = AddOrder(order);

                // Update lead status
                if (SettingsCrm.FinalDealStatusId != 0)
                {
                    lead.DealStatusId = SettingsCrm.FinalDealStatusId;
                    LeadService.UpdateLead(lead);

                    LeadEventService.AddEvent(new LeadEvent()
                    {
                        LeadId = lead.Id,
                        Message = string.Format("Создан заказ <a target=\"_blank\" href=\"orders/edit/{0}\">{1}</a>", order.OrderID, order.Number),
                        Type = LeadEventType.None,
                        CreatedBy = CustomerContext.CurrentCustomer != null ? CustomerContext.CurrentCustomer.GetShortName() : ""
                    });
                }

                if (order.OrderCustomer != null && !string.IsNullOrWhiteSpace(order.OrderCustomer.Email))
                {
                    SendOrderMail(order, order.TotalDiscount, 0, order.ArchivedShippingName, order.PaymentMethodName);
                }

                return order;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return null;
        }

        public static bool CheckAccess(Order order)
        {
            var customer = CustomerContext.CurrentCustomer;
            if (customer.IsModerator)
            {
                var manager = ManagerService.GetManager(customer.Id);
                if (manager != null && manager.Active)
                {
                    if (SettingsManager.ManagersOrderConstraint == ManagersOrderConstraint.Assigned &&
                        order.ManagerId != manager.ManagerId)
                        return false;

                    if (SettingsManager.ManagersOrderConstraint == ManagersOrderConstraint.AssignedAndFree &&
                        order.ManagerId != manager.ManagerId && order.ManagerId != null)
                        return false;
                }
            }
            return true;
        }


        public static List<OrderItem> RecalculateItemsPriceIncludingAllDiscounts(List<OrderItem> oldItems, float shipping, float total)
        {
            if (oldItems == null || !oldItems.Any() || oldItems.Sum(item => item.Amount * item.Price) == 0)
                return oldItems;

            var newItems = oldItems.Where(x => x.Price > 0).ToList().DeepClone();

            var productsTotal = oldItems.Sum(item => item.Amount * item.Price);

            var div = total - shipping - productsTotal;

            foreach (var item in newItems)
            {
                item.Price = (float)Math.Round(item.Price + (item.Price / productsTotal * div), 2);
            }

            var newTotal = newItems.Sum(item => item.Amount * item.Price);
            if (newTotal != total - shipping)
            {
                var item = newItems.FirstOrDefault(x => x.Amount == 1f) ?? newItems.Last();
                item.Price = (float)Math.Round(item.Price + ((total - shipping - newTotal) / item.Amount), 2);
            }

            return newItems;
        }
    }
}