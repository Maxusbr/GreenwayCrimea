using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;


using Newtonsoft.Json;
using AdvantShop.Core.Caching;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Repository;
using AdvantShop.Shipping.DDelivery;
using AdvantShop.Catalog;
using AdvantShop.Shipping;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.SQL;

namespace AdvantShop.Core.Services.Shipping.DDelivery
{
    public class DDeliveryService
    {
        public string GetDDeliveryOrderNumber(int orderId)
        {
            return SQLDataAccess.ExecuteScalar<string>(
                "SELECT TOP 1 [DDeliveryOrder] FROM [Shipping].[DDeliveryOrders] WHERE OrderId = @OrderId",
                CommandType.Text,
                new SqlParameter("@OrderId", orderId));
        }

        public void AddDDeliveryOrder(int orderId, string dDeliveryOrderId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "INSERT INTO  [Shipping].[DDeliveryOrders] (OrderId, DDeliveryOrder) VALUES (@OrderId, @DDeliveryOrder)",
                CommandType.Text,
                new SqlParameter("@OrderId", orderId),
                new SqlParameter("@DDeliveryOrder", dDeliveryOrderId));
        }

        public void DeleteDDeliveryOrder(int orderId, string dDeliveryOrderId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM [Shipping].[DDeliveryOrders] WHERE OrderId=@OrderId",
                CommandType.Text,
                new SqlParameter("@OrderId", orderId));
        }
    }
}
