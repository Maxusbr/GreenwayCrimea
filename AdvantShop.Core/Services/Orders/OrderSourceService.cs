using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Helpers;
using AdvantShop.Core.SQL;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Orders;

namespace AdvantShop.Orders
{
    public class OrderSourceService
    {
        public static OrderSource GetOrderSourceFromReader(SqlDataReader reader)
        {
            return new OrderSource
            {
                Id = SQLDataHelper.GetInt(reader, "Id"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                Main = SQLDataHelper.GetBoolean(reader, "Main"),
                Type = SQLDataHelper.GetString(reader, "Type").TryParseEnum<OrderType>()
            };
        }

        public static List<OrderSource> GetOrderSources()
        {
            return SQLDataAccess.ExecuteReadList<OrderSource>(
                "Select * From [Order].[OrderSource] Order By [SortOrder]",
                CommandType.Text,
                GetOrderSourceFromReader);
        }

        public static OrderSource GetOrderSource(int id)
        {
            return SQLDataAccess.ExecuteReadOne<OrderSource>(
                "Select * From [Order].[OrderSource] Where [Id] = @Id",
                CommandType.Text,
                GetOrderSourceFromReader,
                new SqlParameter("@Id", id));
        }

        public static OrderSource GetOrderSource(string name)
        {
            return SQLDataAccess.ExecuteReadOne<OrderSource>(
                "Select TOP(1) * From [Order].[OrderSource] Where [Name] = @Name",
                CommandType.Text,
                GetOrderSourceFromReader,
                new SqlParameter("@Name", name));
        }

        public static OrderSource GetOrderSource(OrderType type)
        {
            var orderSource = SQLDataAccess.ExecuteReadOne<OrderSource>(
                "Select TOP(1) * From [Order].[OrderSource] Where [Type] = @Type And [Main] = 1",
                CommandType.Text,
                GetOrderSourceFromReader,
                new SqlParameter("@Type", type.ToString()));

            if (orderSource == null)
            {
                var orderSourceId = AddOrderSource(new OrderSource
                {
                    Name = type.Localize(),
                    SortOrder = 0,
                    Type = type,
                    Main = true
                });
                orderSource = GetOrderSource(orderSourceId);
            }

            return orderSource;
        }

        public static int AddOrderSource(OrderSource orderSource)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "If @Main=1 Begin Update [Order].[OrderSource] Set [Main]=0 Where [Type]=@Type End;" +
                "Insert Into [Order].[OrderSource] ([Name],[SortOrder],[Main],[Type]) Values (@Name,@SortOrder,@Main,@Type); SELECT scope_identity();" +
                "If (Select Count(Id) From [Order].[OrderSource] Where [Type]=@Type And Main=1) = 0 Begin Update Top (1) [Order].[OrderSource] Set Main=1 Where [Type]=@Type And Id <> scope_identity() End",
                CommandType.Text,
                new SqlParameter("@Name", orderSource.Name),
                new SqlParameter("@SortOrder", orderSource.SortOrder),
                new SqlParameter("@Main", orderSource.Main),
                new SqlParameter("@Type", orderSource.Type.ToString()));
        }

        public static void UpdateOrderSource(OrderSource orderSource)
        {
            SQLDataAccess.ExecuteNonQuery(
                "If @Main=1 Begin Update [Order].[OrderSource] Set [Main]=0 Where [Type]=@Type End;" +
                "Update [Order].[OrderSource] Set [Name]=@Name, [SortOrder]=@SortOrder, [Main]=@Main, [Type]=@Type Where Id=@Id;" +
                "If (Select Count(Id) From [Order].[OrderSource] Where [Type]=@Type And Main=1) = 0 Begin Update Top (1) [Order].[OrderSource] Set Main=1 Where [Type]=@Type And Id<>@Id End",
                CommandType.Text,
                new SqlParameter("@Id", orderSource.Id),
                new SqlParameter("@Name", orderSource.Name),
                new SqlParameter("@SortOrder", orderSource.SortOrder),
                new SqlParameter("@Main", orderSource.Main),
                new SqlParameter("@Type", orderSource.Type.ToString()));
        }

        public static bool DeleteOrderSource(int id)
        {
            if (!CanBeDeleted(id))
                return false;

            var source = GetOrderSource(id);

            if (source != null)
                SQLDataAccess.ExecuteNonQuery(
                    "Delete From [Order].[OrderSource] Where Id=@Id; " +
                    "If (Select Count(Id) From [Order].[OrderSource] Where [Type]=@Type And Main=1) = 0 Begin Update Top (1) [Order].[OrderSource] Set Main=1 Where [Type]=@Type And Id<>@Id End",
                    CommandType.Text,
                    new SqlParameter("@Id", id),
                    new SqlParameter("@Type", source.Type.ToString()));
            
            return true;
        }

        public static bool CanBeDeleted(int id)
        {
            var ordersCount = SQLDataAccess.Query<int>("Select Count(OrderId) From [Order].[Order] Where OrderSourceId = @id", new {id}).FirstOrDefault();

            if (ordersCount != 0)
                return false;

            var leadsCount = SQLDataAccess.Query<int>("Select Count([Lead].Id) From [Order].[Lead] Where OrderSourceId = @id", new { id }).FirstOrDefault();

            if (leadsCount != 0)
                return false;

            return true;
        }
    }
}
