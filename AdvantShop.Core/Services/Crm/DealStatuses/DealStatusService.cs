using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.Caching;
using AdvantShop.Core.SQL;

namespace AdvantShop.Core.Services.Crm.DealStatuses
{
    public static class DealStatusService
    {
        private const string DealStatusCacheKey = "DealStatus_";

        public static List<DealStatus> GetList()
        {
            return CacheManager.Get(DealStatusCacheKey + "list",
                () => SQLDataAccess.Query<DealStatus>("Select * From CRM.DealStatus Order by SortOrder").ToList());
        }

        public static DealStatus Get(int id)
        {
            return CacheManager.Get(DealStatusCacheKey + id,
                () =>
                    SQLDataAccess.Query<DealStatus>("Select * From CRM.DealStatus Where Id = @id", new {id})
                        .FirstOrDefault());
        }

        public static int Add(DealStatus status)
        {
            CacheManager.RemoveByPattern(DealStatusCacheKey);
            return SQLDataAccess.ExecuteScalar<int>(
                "INSERT INTO CRM.DealStatus (Name, SortOrder) VALUES (@Name, @SortOrder); SELECT SCOPE_IDENTITY()",
                CommandType.Text,
                new SqlParameter("@Name", status.Name),
                new SqlParameter("@SortOrder", status.SortOrder));
        }

        public static int Update(DealStatus status)
        {
            CacheManager.RemoveByPattern(DealStatusCacheKey);

            return SQLDataAccess.ExecuteScalar<int>(
                "UPDATE CRM.DealStatus SET Name = @Name, SortOrder = @SortOrder WHERE Id = @Id",
                CommandType.Text,
                new SqlParameter("@Id", status.Id),
                new SqlParameter("@Name", status.Name),
                new SqlParameter("@SortOrder", status.SortOrder));
        }

        public static void Delete(int id)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM CRM.DealStatus WHERE Id = @Id", CommandType.Text, new SqlParameter("@Id", id));
            CacheManager.RemoveByPattern(DealStatusCacheKey);
        }
    }
}
