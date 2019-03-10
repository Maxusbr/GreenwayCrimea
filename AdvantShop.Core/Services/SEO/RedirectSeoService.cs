//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.Caching;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.SEO
{
    public static class RedirectSeoService
    {
        private const string RedirectsCahcheName = "redirectsCache";
        private const string RedirectsCahcheNameRedirects = RedirectsCahcheName + "Redirects";
        private const string RedirectsCahcheNameRedirectsWithStar = RedirectsCahcheName + "RedirectsWithStar";

        private static void CleanRedirectsCahche()
        {
            CacheManager.RemoveByPattern(RedirectsCahcheName);
        }

        public static RedirectSeo GetRedirectSeoById(int id)
        {
            return SQLDataAccess.ExecuteReadOne<RedirectSeo>(
                "SELECT * FROM [Settings].[Redirect] WHERE ID = @ID",
                CommandType.Text,
                GetRedirectSeoFromReader,
                new SqlParameter("@ID", id));
        }

        public static IEnumerable<RedirectSeo> GetRedirectsSeo()
        {
            return SQLDataAccess.ExecuteReadIEnumerable<RedirectSeo>(
                "SELECT * FROM [Settings].[Redirect]",
                CommandType.Text,
                GetRedirectSeoFromReader);
        }


        public static RedirectSeo GetByInputUrl(string relativeUrl, string absoluteUrl)
        {
            List<RedirectSeo> redirectsWithStar;
            Dictionary<string, RedirectSeo> redirects;

            if (CacheManager.Contains(RedirectsCahcheNameRedirects) && CacheManager.Contains(RedirectsCahcheNameRedirectsWithStar))
            {
                redirects = CacheManager.Get<Dictionary<string, RedirectSeo>>(RedirectsCahcheNameRedirects);
                redirectsWithStar = CacheManager.Get<List<RedirectSeo>>(RedirectsCahcheNameRedirectsWithStar);
            }
            else
            {
                redirects = new Dictionary<string, RedirectSeo>();
                redirectsWithStar = new List<RedirectSeo>();

                var allRedirects = SQLDataAccess.ExecuteReadIEnumerable("SELECT * FROM [Settings].[Redirect]", CommandType.Text, GetRedirectSeoFromReader);

                foreach (var redirectSeo in allRedirects)
                {
                    if (!redirectSeo.RedirectFrom.Contains('*'))
                    {
                        if (!redirects.ContainsKey(redirectSeo.RedirectFrom))
                            redirects.Add(redirectSeo.RedirectFrom, redirectSeo);
                    }
                    else
                    {
                        redirectsWithStar.Add(redirectSeo);
                    }
                }

                CleanRedirectsCahche();
                CacheManager.Insert(RedirectsCahcheNameRedirects, redirects, 60);
                CacheManager.Insert(RedirectsCahcheNameRedirectsWithStar, redirectsWithStar, 60);
            }

            var redirectWithStar = redirectsWithStar.Find(x => ImitateSqlLike(relativeUrl, x.RedirectFrom) || ImitateSqlLike(absoluteUrl, x.RedirectFrom));
            if (redirectWithStar != null)
            {
                return new RedirectSeo
                {
                    ID = redirectWithStar.ID,
                    ProductArtNo = redirectWithStar.ProductArtNo,
                    RedirectFrom = redirectWithStar.RedirectFrom.Replace("*", ""),
                    RedirectTo = redirectWithStar.RedirectTo
                };
            }

            RedirectSeo redirect = null;
            if (redirects.TryGetValue(relativeUrl, out redirect) || redirects.TryGetValue(absoluteUrl, out redirect))
            {
                return new RedirectSeo
                {
                    ID = redirect.ID,
                    ProductArtNo = redirect.ProductArtNo,
                    RedirectFrom = redirect.RedirectFrom,
                    RedirectTo = redirect.RedirectTo
                };
            }

            return null;
        }
        /*
        public static RedirectSeo GetByInputUrl(string relativeUrl, string absoluteUrl)
        {
            List<RedirectSeo> redirects;
            if (CacheManager.Contains(RedirectsCahcheName))
            {
                redirects = CacheManager.Get<List<RedirectSeo>>(RedirectsCahcheName);
            }
            else
            {
                redirects = GetRedirectsSeo().ToList();
                CacheManager.Insert(RedirectsCahcheName, redirects, 60);
            }
            var result = redirects.FirstOrDefault(r => ImitateSqlLike(relativeUrl, r.RedirectFrom.ToLower()) || ImitateSqlLike(absoluteUrl, r.RedirectFrom.ToLower()));
            return result != null
                ? new RedirectSeo
                {
                    ID = result.ID,
                    ProductArtNo = result.ProductArtNo,
                    RedirectFrom = result.RedirectFrom.Replace("*", ""),
                    RedirectTo = result.RedirectTo
                } : null;
        }
        */

        public static RedirectSeo GetRedirectsSeoByRedirectFrom(string redirectFrom)
        {
            return SQLDataAccess.ExecuteReadOne<RedirectSeo>(
                "SELECT * FROM [Settings].[Redirect] WHERE RedirectFrom = @RedirectFrom",
                CommandType.Text,
                GetRedirectSeoFromReader,
                new SqlParameter("@RedirectFrom", redirectFrom));
        }


        private static bool ImitateSqlLike(string input, string source)
        {
            //if (source.Contains('*'))
            //{
            var cleanSource = source.Replace("*", "");
            var index = source.IndexOf('*');

            if (index == 0)
                return input.EndsWith(cleanSource);

            if (index == source.Length - 1)
                return input.StartsWith(cleanSource);

            return input.Contains(cleanSource);
            //}
            //return input == source;
        }

        private static RedirectSeo GetRedirectSeoFromReader(SqlDataReader reader)
        {
            return new RedirectSeo
            {
                ID = SQLDataHelper.GetInt(reader, "ID"),
                RedirectFrom = SQLDataHelper.GetString(reader, "RedirectFrom").ToLower(),
                RedirectTo = SQLDataHelper.GetString(reader, "RedirectTo"),
                ProductArtNo = SQLDataHelper.GetString(reader, "ProductArtNo")
            };
        }

        public static void AddRedirectSeo(RedirectSeo redirectSeo)
        {
            CleanRedirectsCahche();
            redirectSeo.ID =
                SQLDataAccess.ExecuteScalar<int>(
                    "INSERT INTO [Settings].[Redirect] ([RedirectFrom], [RedirectTo], [ProductArtNo]) VALUES (@RedirectFrom, @RedirectTo, @ProductArtNo); SELECT SCOPE_IDENTITY();",
                    CommandType.Text,
                    new SqlParameter("@RedirectFrom", redirectSeo.RedirectFrom),
                    new SqlParameter("@RedirectTo", redirectSeo.RedirectTo),
                    new SqlParameter("@ProductArtNo", redirectSeo.ProductArtNo)
                    );
        }

        public static void UpdateRedirectSeo(RedirectSeo redirectSeo)
        {
            CleanRedirectsCahche();
            SQLDataAccess.ExecuteNonQuery("UPDATE [Settings].[Redirect] SET RedirectFrom = @RedirectFrom, RedirectTo = @RedirectTo, ProductArtNo = @ProductArtNo WHERE ID = @ID", CommandType.Text,
                                                new SqlParameter("@ID", redirectSeo.ID),
                                                new SqlParameter("@RedirectFrom", redirectSeo.RedirectFrom),
                                                new SqlParameter("@RedirectTo", redirectSeo.RedirectTo),
                                                new SqlParameter("@ProductArtNo", redirectSeo.ProductArtNo)
                                         );
        }

        public static void DeleteRedirectSeo(int id)
        {
            CleanRedirectsCahche();
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Settings].[Redirect] WHERE ID = @ID", CommandType.Text,
                                          new SqlParameter { ParameterName = "@ID", Value = id });
        }
    }
}