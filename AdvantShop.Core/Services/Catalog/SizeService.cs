using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.Caching;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.Catalog
{
    public class SizeService
    {
        private const string SizeCacheKey = "Size_";

        public static Size GetSize(int? sizeId)
        {
            if (!sizeId.HasValue)
                return null;

            return
                CacheManager.Get(SizeCacheKey + sizeId.Value, () =>
                    SQLDataAccess.ExecuteReadOne<Size>("Select * from Catalog.Size where sizeID=@sizeID",
                        CommandType.Text, GetFromReader, new SqlParameter("@sizeID", sizeId)));
        }

        public static List<Size> GetAllSizes()
        {
            return SQLDataAccess.ExecuteReadList<Size>("Select * from Catalog.Size Order by SortOrder, SizeName", CommandType.Text, GetFromReader);
        }


        public static Size GetSize(string sizeName)
        {
            return SQLDataAccess.ExecuteReadOne<Size>("Select Top 1 * from Catalog.Size where sizeName=@sizeName order by SortOrder",
                                                       CommandType.Text,
                                                       GetFromReader, new SqlParameter("@sizeName", sizeName));
        }

        private static Size GetFromReader(SqlDataReader reader)
        {
            return new Size()
            {
                SizeId = SQLDataHelper.GetInt(reader, "sizeID"),
                SizeName = SQLDataHelper.GetString(reader, "SizeName"),
                SortOrder = SQLDataHelper.GetInt(reader, "SortOrder")
            };
        }

        public static int AddSize(Size size)
        {
            if (size == null)
                throw new ArgumentNullException("size");
            
            return SQLDataAccess.ExecuteScalar<int>("[Catalog].[sp_AddSize]", CommandType.StoredProcedure,
                                                        new SqlParameter("@SizeName", size.SizeName),
                                                        new SqlParameter("@SortOrder", size.SortOrder)
                                                        );
        }

        public static void UpdateSize(Size size)
        {
            CacheManager.RemoveByPattern(SizeCacheKey + size.SizeId);

            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdateSize]", CommandType.StoredProcedure,
                                                 new SqlParameter("@SizeId", size.SizeId),
                                                 new SqlParameter("@SizeName", size.SizeName),
                                                 new SqlParameter("@SortOrder", size.SortOrder));
        }

        public static void DeleteSize(int sizeId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM Catalog.Size WHERE SizeID = @SizeId", CommandType.Text, new SqlParameter("@SizeId", sizeId));
            CacheManager.RemoveByPattern(SizeCacheKey + sizeId);
        }

        public static List<Size> GetSizesByCategoryID(int categoryId, bool inDepth, bool onlyAvaliable)
        {
            return SQLDataAccess.ExecuteReadList<Size>("Catalog.sp_GetSizesByCategory", CommandType.StoredProcedure,
                                                         GetFromReader,
                                                        new SqlParameter("@CategoryID", categoryId),
                                                        new SqlParameter("@inDepth", inDepth),
                                                        new SqlParameter("@OnlyAvailable", onlyAvaliable));
        }

        public static bool IsSizeUsed(int sizeId)
        {
            return Convert.ToInt32(SQLDataAccess.ExecuteScalar("Select Count(SizeId) From Catalog.Offer WHERE SizeID = @SizeId", CommandType.Text, 
                                                                    new SqlParameter("@SizeId", sizeId))) > 0;
        }
    }
}