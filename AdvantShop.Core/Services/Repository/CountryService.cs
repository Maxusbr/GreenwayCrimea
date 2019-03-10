//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace AdvantShop.Repository
{
    public class CountryService
    {
        private const string CountryCacheKey = "Country_";

        public static List<Country> GetAllCountries()
        {
            const string cacheKey = CountryCacheKey + "All";

            return CacheManager.Get(cacheKey,
                () =>
                    SQLDataAccess.ExecuteReadList("SELECT * FROM [Customers].[Country] ORDER BY [CountryName] ASC",
                        CommandType.Text, GetCountryFromReader));
        }

        public static List<Country> GetAllCountryIdAndName()
        {
            return SQLDataAccess.ExecuteReadList("SELECT CountryID,CountryName FROM [Customers].[Country]",
                CommandType.Text,
                reader => new Country
                {
                    CountryId = SQLDataHelper.GetInt(reader, "CountryID"),
                    Name = SQLDataHelper.GetString(reader, "CountryName")
                });
        }

        public static List<Country> GetCountriesByDisplayInPopup()
        {
            return CacheManager.Get(CountryCacheKey + "DisplayInPopup",
                () =>
                    SQLDataAccess.ExecuteReadList(
                        "Select top 12 * From Customers.Country Where DisplayInPopup=1 Order By SortOrder desc, CountryName asc",
                        CommandType.Text, GetCountryFromReader));
        }

        #region Update / Add / Delete Country

        public static void Delete(int countryId)
        {
            if (countryId != SettingsMain.SellerCountryId)
            {
                SQLDataAccess.ExecuteNonQuery("DELETE FROM [Customers].[Country] where CountryID = @CountryId",
                    CommandType.Text, new SqlParameter("@CountryId", countryId));

                CacheManager.RemoveByPattern(CountryCacheKey);
            }
        }

        public static void Add(Country country)
        {
            country.CountryId =
                SQLDataAccess.ExecuteScalar<int>(
                    "INSERT INTO [Customers].[Country] (CountryName, CountryISO2, CountryISO3, DisplayInPopup,SortOrder,DialCode) VALUES (@Name, @ISO2, @ISO3, @DisplayInPopup,@SortOrder,@DialCode); SELECT scope_identity();",
                    CommandType.Text,
                    new SqlParameter("@Name", country.Name),
                    new SqlParameter("@ISO2", country.Iso2),
                    new SqlParameter("@ISO3", country.Iso3),
                    new SqlParameter("@DisplayInPopup", country.DisplayInPopup),
                    new SqlParameter("@SortOrder", country.SortOrder),
                    new SqlParameter("@DialCode", country.DialCode ?? (object)DBNull.Value)
                    );
            CacheManager.RemoveByPattern(CountryCacheKey);
        }

        public static void Update(Country country)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [Customers].[Country] set CountryName=@name, CountryISO2=@ISO2, CountryISO3=@ISO3, DisplayInPopup=@DisplayInPopup, SortOrder=@SortOrder, DialCode=@DialCode Where CountryID = @id",
                CommandType.Text,
                new SqlParameter("@id", country.CountryId),
                new SqlParameter("@name", country.Name),
                new SqlParameter("@ISO2", country.Iso2),
                new SqlParameter("@ISO3", country.Iso3),
                new SqlParameter("@DisplayInPopup", country.DisplayInPopup),
                new SqlParameter("@SortOrder", country.SortOrder),
                new SqlParameter("@DialCode", country.DialCode ?? (object)DBNull.Value));

            CacheManager.RemoveByPattern(CountryCacheKey);
        }

        #endregion


        public static string GetIso2(string name)
        {
            return
                SQLDataAccess.ExecuteScalar<string>(
                    "SELECT [CountryISO2] FROM [Customers].[Country] Where CountryName = @CountryName",
                    CommandType.Text, new SqlParameter("@CountryName", name));
        }

        public static Country GetCountry(int id)
        {
            var cacheKey = CountryCacheKey + id;

            var country =
                CacheManager.Get<Country>(cacheKey,
                    () => SQLDataAccess.ExecuteReadOne("SELECT * FROM [Customers].[Country] Where CountryID = @id",
                        CommandType.Text, GetCountryFromReader, new SqlParameter("@id", id)));

            return country;
        }

        public static Country GetCountryByName(string countryName)
        {
            return SQLDataAccess.ExecuteReadOne("SELECT * FROM [Customers].[Country] Where CountryName = @CountryName",
                CommandType.Text, GetCountryFromReader, new SqlParameter("@CountryName", countryName));
        }

        public static Country GetCountryByIso2(string iso2)
        {
            return SQLDataAccess.ExecuteReadOne("SELECT * FROM [Customers].[Country] Where CountryISO2 = @iso2",
                CommandType.Text, GetCountryFromReader, new SqlParameter("@iso2", iso2));
        }


        public static Country GetCountryFromReader(SqlDataReader reader)
        {
            return new Country
            {
                CountryId = SQLDataHelper.GetInt(reader, "CountryID"),
                Iso2 = SQLDataHelper.GetString(reader, "CountryISO2"),
                Iso3 = SQLDataHelper.GetString(reader, "CountryISO3"),
                Name = SQLDataHelper.GetString(reader, "CountryName"),
                DisplayInPopup = SQLDataHelper.GetBoolean(reader, "DisplayInPopup"),
                SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                DialCode = SQLDataHelper.GetNullableInt(reader, "DialCode")
            };
        }


        //public static string GetCountryNameById(int countryId)
        //{
        //    return SQLDataAccess.ExecuteScalar<string>(
        //        "SELECT CountryName FROM Customers.Country Where CountryID = @id",
        //        CommandType.Text, new SqlParameter("@id", countryId));
        //}

        //public static string GetCountryIso2ById(int countryId)
        //{
        //    return SQLDataAccess.ExecuteScalar<string>(
        //        "SELECT CountryISO2 FROM Customers.Country Where CountryID = @id",
        //        CommandType.Text, new SqlParameter("@id", countryId));
        //}

        //public static List<int> GetCountryIdByIp(string Ip)
        //{
        //    long ipDec;
        //    try
        //    {
        //        if (Ip == "::1")
        //            ipDec = 127 * 16777216 + 1;
        //        else
        //        {
        //            string[] ip = Ip.Split('.');
        //            ipDec = (Int32.Parse(ip[0])) * 16777216 + (Int32.Parse(ip[1])) * 65536 + (Int32.Parse(ip[2])) * 256 + Int32.Parse(ip[3]);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        ipDec = 127 * 16777216 + 1;
        //    }
        //    List<int> ids = SQLDataAccess.ExecuteReadList<int>("SELECT CountryID FROM Customers.Country Where CountryISO2 = (SELECT country_code FROM Customers.GeoIP Where begin_num <= @IP AND end_num >= @IP)",
        //                                                 CommandType.Text,
        //                                                 reader => SQLDataHelper.GetInt(reader, "CountryID"), new SqlParameter("@IP", ipDec));
        //    return ids;
        //}

        //public static List<string> GetCountryNameByIp(string Ip)
        //{
        //    long ipDec;
        //    try
        //    {
        //        if (Ip == "::1")
        //            ipDec = 127 * 16777216 + 1;
        //        else
        //        {
        //            string[] ip = Ip.Split('.');
        //            ipDec = (Int32.Parse(ip[0])) * 16777216 + (Int32.Parse(ip[1])) * 65536 + (Int32.Parse(ip[2])) * 256 + Int32.Parse(ip[3]);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        ipDec = 127 * 16777216 + 1;
        //    }

        //    List<string> listNames = SQLDataAccess.ExecuteReadList<string>("SELECT * FROM Customers.Country WHERE CountryISO2 = (SELECT country_code FROM Customers.GeoIP WHERE begin_num <= @IP AND end_num >= @IP)",
        //                                                                   CommandType.Text, reader => SQLDataHelper.GetString(reader, "CountryName"),
        //                                                                   new SqlParameter("@IP", ipDec)) ?? new List<string> { { "local" } };

        //    if (listNames.Count == 0)
        //        listNames.Add("local");

        //    return listNames;
        //}

        public static int GetCountryIdByName(string countryName)
        {
            return SQLDataAccess.ExecuteScalar<int>("SELECT CountryID FROM Customers.Country Where CountryName = @name",
                CommandType.Text, new SqlParameter("@name", countryName));
        }

        public static List<string> GetCountriesByName(string name)
        {
            var translit = StringHelper.TranslitToRusKeyboard(name);

            return
                SQLDataAccess.ExecuteReadList(
                    "Select CountryName From Customers.Country Where CountryName like @name + '%' or CountryName like @trname + '%'",
                    CommandType.Text,
                    reader => SQLDataHelper.GetString(reader, "CountryName"), 
                    new SqlParameter("@name", name),
                    new SqlParameter("@trname", translit));
        }        
    }
}