using System;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using Newtonsoft.Json;

namespace AdvantShop.Orders
{
    public class OrderConfirmationService
    {
        public static bool IsExist(Guid customerId)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "Select Count(CustomerId) from [Order].OrderConfirmation where CustomerId=@CustomerId", CommandType.Text,
                new SqlParameter("@CustomerId", customerId)) > 0;
        }

        public static CheckoutData Get(Guid customerId)
        {
            return SQLDataAccess.ExecuteReadOne("Select * from [Order].OrderConfirmation where CustomerId=@CustomerId",
                CommandType.Text, GetFromReader,
                new SqlParameter("@CustomerId", customerId));
        }

        public static void Add(Guid customerId, CheckoutData item)
        {
            var set = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            };

            SQLDataAccess.ExecuteNonQuery(
                "Insert into [Order].OrderConfirmation ([CustomerId],[OrderConfirmationData],LastUpdate) values (@CustomerId,@OrderConfirmationData,GetDate())",
                CommandType.Text,
                new SqlParameter("@CustomerId", customerId),
                new SqlParameter("@OrderConfirmationData", JsonConvert.SerializeObject(item, set)));
        }

        public static void Update(Guid customerId, CheckoutData item)
        {
            var set = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
            };

            SQLDataAccess.ExecuteNonQuery(
                "Update [Order].OrderConfirmation set OrderConfirmationData=@OrderConfirmationData,LastUpdate=GetDate() where CustomerId=@CustomerId",
                CommandType.Text,
                new SqlParameter("@CustomerId", customerId),
                new SqlParameter("@OrderConfirmationData", JsonConvert.SerializeObject(item, set)));
        }

        public static void Delete(Guid customerId)
        {
            SQLDataAccess.ExecuteNonQuery("Delete from [Order].OrderConfirmation Where CustomerId=@CustomerId",
                CommandType.Text, new SqlParameter("@CustomerId", customerId));
        }

        public static void DeleteExpired()
        {
            SQLDataAccess.ExecuteNonQuery(
                "Delete from [Order].OrderConfirmation Where DATEADD(month, 1, LastUpdate) < GetDate()",
                CommandType.Text);
        }

        private static CheckoutData GetFromReader(SqlDataReader reader)
        {
            return
                JsonConvert.DeserializeObject<CheckoutData>(
                    SQLDataHelper.GetString(reader, "OrderConfirmationData"),
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });

        }
    }
}