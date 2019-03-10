using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.Core.Services.Orders
{
    public class OrderStatus1CService
    {
        public static void AddOrUpdateOrderStatus1C(OrderStatus1C status)
        {
            SQLDataAccess.ExecuteNonQuery(
                @"If ((Select Count(OrderId) From [Order].[OrderStatus1C] Where OrderId = @OrderId) > 0) 
                begin 
                    Update [Order].[OrderStatus1C] Set Status1C=@Status1C, OrderId1C=@OrderId1C, OrderDate=@OrderDate Where OrderId=@OrderId
                end 
                Else 
                begin 
                    Insert Into [Order].[OrderStatus1C] ([OrderId],[Status1C],[OrderId1C],[OrderDate]) Values (@OrderId, @Status1C, @OrderId1C, @OrderDate); 
                end ",
                CommandType.Text,
                new SqlParameter("@OrderId", status.OrderId),
                new SqlParameter("@Status1C", status.Status1C),
                new SqlParameter("@OrderId1C", status.OrderId1C),
                new SqlParameter("@OrderDate", status.OrderDate));
        }

        public static OrderStatus1C GetStatus1C(int orderId)
        {
            return
                SQLDataAccess.ExecuteReadOne(
                    "Select * From [Order].[OrderStatus1C] Where OrderId = @OrderId", CommandType.Text,
                    reader => new OrderStatus1C()
                    {
                        OrderId = SQLDataHelper.GetInt(reader, "OrderId"),
                        Status1C = SQLDataHelper.GetString(reader, "Status1C"),
                        OrderId1C = SQLDataHelper.GetString(reader, "OrderId1C"),
                        OrderDate = SQLDataHelper.GetString(reader, "OrderDate"),
                    },
                    new SqlParameter("@OrderId", orderId));
        }
    }
}
