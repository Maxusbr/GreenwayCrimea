using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.Caching;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.Catalog
{
    public class ColorService
    {
        private const string ColorCaheKey = "Color_";

        public static Color GetColor(int? colorId)
        {
            if (!colorId.HasValue)
                return null;

            return
                CacheManager.Get(ColorCaheKey + colorId.Value, () =>
                    SQLDataAccess.ExecuteReadOne(
                        "Select * From Catalog.Color Left Join Catalog.Photo On Photo.ObjId=Color.ColorId and Type=@Type Where Color.ColorID=@colorId",
                        CommandType.Text, GetFromReader,
                        new SqlParameter("@colorid", colorId),
                        new SqlParameter("@Type", PhotoType.Color.ToString())));
        }

        public static Color GetColor(string colorName)
        {
            return
                SQLDataAccess.ExecuteReadOne(
                    "Select Top 1 * from Catalog.Color Left Join Catalog.Photo On Photo.ObjId=Color.ColorId and Type=@Type Where ColorName=@colorName Order by SortOrder", CommandType.Text,
                    GetFromReader, 
                    new SqlParameter("@colorName", colorName),
                    new SqlParameter("@Type", PhotoType.Color.ToString()));
        }

        public static List<Color> GetAllColors()
        {
            return
                SQLDataAccess.ExecuteReadList<Color>(
                    "Select * From Catalog.Color Left Join Catalog.Photo On Photo.ObjId=Color.ColorId and Type=@Type Order by SortOrder, ColorName",
                    CommandType.Text, GetFromReader, new SqlParameter("@Type", PhotoType.Color.ToString()));
        }


        private static Color GetFromReader(SqlDataReader reader)
        {
            return new Color()
            {
                ColorId = SQLDataHelper.GetInt(reader, "ColorId"),
                ColorCode = SQLDataHelper.GetString(reader, "ColorCode"),
                ColorName = SQLDataHelper.GetString(reader, "ColorName"),
                SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                IconFileName =
                    new ColorPhoto(SQLDataHelper.GetInt(reader, "PhotoId"), SQLDataHelper.GetInt(reader, "ObjId"))
                    {
                        PhotoName = SQLDataHelper.GetString(reader, "PhotoName")
                    },
            };
        }

        public static int AddColor(Color color)
        {
            if (color == null)
                throw new ArgumentNullException("color");
            
            return SQLDataAccess.ExecuteScalar<int>("[Catalog].[sp_AddColor]", CommandType.StoredProcedure,
                                                        new SqlParameter("@ColorName", color.ColorName),
                                                        new SqlParameter("@ColorCode", color.ColorCode),
                                                        new SqlParameter("@SortOrder", color.SortOrder));
        }

        public static void UpdateColor(Color color)
        {
            CacheManager.RemoveByPattern(ColorCaheKey + color.ColorId);

            SQLDataAccess.ExecuteNonQuery("[Catalog].[sp_UpdateColor]", CommandType.StoredProcedure,
                                                 new SqlParameter("@ColorId", color.ColorId),
                                                 new SqlParameter("@ColorName", color.ColorName),
                                                 new SqlParameter("@ColorCode", color.ColorCode),
                                                 new SqlParameter("@SortOrder", color.SortOrder));
        }

        public static void DeleteColor(int colorId)
        {
            PhotoService.DeletePhotos(colorId, PhotoType.Color);
            SQLDataAccess.ExecuteNonQuery("DELETE FROM Catalog.Color WHERE ColorID = @ColorId", CommandType.Text, new SqlParameter("@ColorId", colorId));
            CacheManager.RemoveByPattern(ColorCaheKey + colorId);
        }

        public static List<Color> GetColorsByCategoryId(int categoryId, bool inDepth, bool onlyAvaliable)
        {
            return SQLDataAccess.ExecuteReadList("Catalog.sp_GetColorsByCategory", CommandType.StoredProcedure,
                                                GetFromReader,
                                                new SqlParameter("@CategoryID", categoryId),
                                                new SqlParameter("@Indepth", inDepth),
                                                new SqlParameter("@Type", PhotoType.Color.ToString()),
                                                new SqlParameter("@OnlyAvailable", onlyAvaliable));
        }

        public static bool IsColorUsed(int colorId)
        {
            return Convert.ToInt32(
                SQLDataAccess.ExecuteScalar("Select Count(ColorID) from Catalog.Offer where ColorID=@ColorID", CommandType.Text,
                    new SqlParameter("@ColorID", colorId))) > 0;
        }
    }
}