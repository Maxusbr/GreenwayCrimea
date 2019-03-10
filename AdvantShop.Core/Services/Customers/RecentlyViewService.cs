//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.SQL;

namespace AdvantShop.Customers
{
    public class RecentlyViewService
    {
        private const string RecentlyViewCacheKey = "RecentlyView_";

        /// <summary>
        /// Загружает данные о просмотрах для текущего пользователя
        /// </summary>
        /// <remarks></remarks>
        public static List<ProductModel> LoadViewDataByCustomer(Guid customerId, int rowsCount)
        {
            return
                CacheManager.Get(RecentlyViewCacheKey + customerId, () =>
                    SQLDataAccess.Query<ProductModel>("[Customers].[sp_GetRecentlyView]",
                        new {customerId, rowsCount, Type = PhotoType.Product.ToString()}, CommandType.StoredProcedure)
                        .ToList());
        }

        public static void SetRecentlyView(Guid customerId, int productId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "if Exists (SELECT 1 FROM [Customers].[RecentlyViewsData] WHERE CustomerID=@CustomerId AND ProductID=@ProductId) " +
                "begin UPDATE [Customers].[RecentlyViewsData] SET ViewDate = GetDate() WHERE (CustomerID = @CustomerId) AND (ProductID = @ProductId) end " +
                "else begin INSERT INTO [Customers].[RecentlyViewsData](CustomerID, ProductID, ViewDate) VALUES (@CustomerId, @ProductId, GetDate()) end",
                CommandType.Text,
                new SqlParameter("@CustomerId", customerId),
                new SqlParameter("@ProductId", productId));

            CacheManager.Remove(RecentlyViewCacheKey + customerId);
        }

        public static void DeleteExpired()
        {
            //SQLDataAccess.ExecuteNonQuery("Delete from [Customers].[RecentlyViewsData] where GetDate() > DATEADD(week, 1, ViewDate)", CommandType.Text);

            using (var db = new SQLDataAccess())
            {
                db.cmd.CommandTimeout = 60 * 10; // 10 mins
                db.cmd.CommandText = "Delete from [Customers].[RecentlyViewsData] where ViewDate < @Date";
                db.cmd.Parameters.Clear();
                db.cmd.Parameters.Add(new SqlParameter("@Date", DateTime.Now.AddDays(-7)));

                db.cnOpen();
                db.cmd.ExecuteNonQuery();
                db.cnClose();
            }

            CacheManager.RemoveByPattern(RecentlyViewCacheKey);
        }
    }
}