using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.Repository
{
    public class IpZoneService
    {
        public static IpZone GetZoneByCityId(int cityId)
        {
            return SQLDataAccess.ExecuteReadOne(
                "Select Top(1) City.CityID, City.CityName, Region.RegionID, Region.RegionName, Country.CountryID, Country.CountryName " +
                "From Customers.City " +
                "Inner Join Customers.Region On Region.RegionId = City.RegionId " +
                "Inner Join Customers.Country On Country.CountryId = Region.CountryId " +
                "Where CityID = @CityId " +
                "Order by Country.CountryID",
                CommandType.Text,
                reader => new IpZone()
                {
                    CountryId = SQLDataHelper.GetInt(reader, "CountryId"),
                    CountryName = SQLDataHelper.GetString(reader, "CountryName"),
                    RegionId = SQLDataHelper.GetInt(reader, "RegionID"),
                    Region = SQLDataHelper.GetString(reader, "RegionName"),
                    CityId = SQLDataHelper.GetInt(reader, "CityId"),
                    City = SQLDataHelper.GetString(reader, "CityName"),
                },
                new SqlParameter("@CityId", cityId));
        }

        public static IpZone GetZoneByCity(string city, int? countryID)
        {
            return SQLDataAccess.ExecuteReadOne(
                "Select Top(1) City.CityID, City.CityName, Region.RegionID, Region.RegionName, Country.CountryID, Country.CountryName " +
                "From Customers.City " +
                "Inner Join Customers.Region On Region.RegionId = City.RegionId " +
                "Inner Join Customers.Country On Country.CountryId = Region.CountryId " +
                "Where @City = LOWER(CityName) and (Country.CountryID=@countryID OR @countryID is null)" +
                "Order by Country.CountryID",
                CommandType.Text,
                reader => new IpZone()
                {
                    CountryId = SQLDataHelper.GetInt(reader, "CountryId"),
                    CountryName = SQLDataHelper.GetString(reader, "CountryName"),
                    RegionId = SQLDataHelper.GetInt(reader, "RegionID"),
                    Region = SQLDataHelper.GetString(reader, "RegionName"),
                    CityId = SQLDataHelper.GetInt(reader, "CityId"),
                    City = SQLDataHelper.GetString(reader, "CityName"),
                },
                new SqlParameter("@City", city), new SqlParameter("@countryID", (object)countryID ?? DBNull.Value));
        }

        public static List<IpZone> GetIpZonesByCity(string cityName)
        {
            string translitRu = StringHelper.TranslitToRus(cityName);
            string translitKeyboard = StringHelper.TranslitToRusKeyboard(cityName);

            return SQLDataAccess.ExecuteReadList<IpZone>(
                "Select Top (10) CityName, CityID, " +
                "(case when (select count(*) from [Customers].[City] where CityName = Cities.CityName) > 1 then (Region.RegionName) else '' end) as RegionName, " +
                "Region.RegionId, CountryId " +
                "From Customers.City as Cities INNER JOIN Customers.Region ON Region.RegionID = Cities.RegionId " +
                "WHERE Replace(CityName,'ё','е') like @name + '%' OR Replace(CityName,'ё','е') like @translitRu + '%' " +
                " OR Replace(CityName,'ё','е') like @translitKeyboard + '%' order by CityName",
                CommandType.Text, reader => new IpZone()
                {
                    CityId = SQLDataHelper.GetInt(reader, "CityID"),
                    City = SQLDataHelper.GetString(reader, "CityName"),
                    RegionId = SQLDataHelper.GetInt(reader, "RegionId"),
                    Region = SQLDataHelper.GetString(reader, "RegionName"),
                    CountryId = SQLDataHelper.GetInt(reader, "CountryId"),
                },
                new SqlParameter("@name", cityName),
                new SqlParameter("@translitRu", translitRu),
                new SqlParameter("@translitKeyboard", translitKeyboard));
        }
    }
}