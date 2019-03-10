using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using AdvantShop.Core.Caching;
using AdvantShop.Core.SQL;
using AdvantShop.Orders;

namespace AdvantShop.Shipping.Sdek
{
    public class SdekService
    {
        public static int GetSdekCityId(string cityName, string regionName)
        {
            return CacheManager.Get("Sdek_GetSdekCityId_" + cityName + "_" + regionName, 2, () =>
            {
                var id = 0;
                if (!string.IsNullOrEmpty(regionName))
                {
                    id = SQLDataAccess.ExecuteScalar<int>(
                        "SELECT TOP 1 [Id] FROM [Shipping].[SdekCities] WHERE CityName = @CityName and OblName Like @RegionName + '%'",
                        CommandType.Text,
                        new SqlParameter("@CityName", cityName),
                        new SqlParameter("@RegionName",
                            regionName.Split(new[] { ' ', ',', '.' }, StringSplitOptions.RemoveEmptyEntries)[0]));
                }
                if (id == 0)
                {
                    id = SQLDataAccess.ExecuteScalar<int>(
                        "SELECT TOP 1 [Id] FROM [Shipping].[SdekCities] WHERE CityName = @CityName",
                        CommandType.Text,
                        new SqlParameter("@CityName", cityName));
                }
                if (id == 0)
                {
                    id = SQLDataAccess.ExecuteScalar<int>(
                        "SELECT TOP 1 [Id] FROM [Shipping].[SdekCities] WHERE CityName Like @CityName + '%'",
                        CommandType.Text,
                        new SqlParameter("@CityName", cityName));
                }
                return id;
            });
        }

        public static float GetActualWeght(OrderItem item, float defaultLength, float defaultWidth, float defaultHeight, float defaultWeight)
        {

            var result = 0f;
            var length = defaultLength;
            var width = defaultWidth;
            var height = defaultHeight;
            var weight = item.Weight == 0 ? defaultWeight : item.Weight;

            if (item.Size != null)
            {
                var sizes = item.Size.Split(new[] { 'x' }, StringSplitOptions.RemoveEmptyEntries);
                if (sizes.Length == 3)
                {
                    float.TryParse(sizes[0], out length);
                    float.TryParse(sizes[1], out width);
                    float.TryParse(sizes[2], out height);
                }
            }

            var volumeWeight = (length / 10) * (width / 10) * (height / 10) / 5000;
            result = Math.Max(weight, volumeWeight);

            return result * 1000;
        }

        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
