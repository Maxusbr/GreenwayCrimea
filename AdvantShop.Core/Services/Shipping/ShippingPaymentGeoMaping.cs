//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core;
using System.Collections.Generic;
using AdvantShop.Core.SQL;
using AdvantShop.Repository;
using AdvantShop.Core.Caching;

namespace AdvantShop
{
    public class ShippingPaymentGeoMaping
    {
        //******* IsExist
        public static bool IsExistPaymentCountry(int methodId, int countryId)
        {
            return SQLDataAccess.ExecuteScalar<int>("Select Count(*) from [order].PaymentCountry where MethodId=@MethodId and CountryId=@CountryId",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CountryId", Value = countryId }
                                            ) > 0;
        }

        public static bool IsExistShippingCountry(int methodId, int countryId)
        {
            return SQLDataAccess.ExecuteScalar<int>("Select Count(*) from [order].ShippingCountry where MethodId=@MethodId and CountryId=@CountryId",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CountryId", Value = countryId }
                                            ) > 0;
        }

        public static bool IsExistPaymentCity(int methodId, int countryId)
        {
            return SQLDataAccess.ExecuteScalar<int>("Select Count(*) from [order].PaymentCity where MethodId=@MethodId and CityId=@CityId",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CityId", Value = countryId }
                                            ) > 0;
        }

        public static bool IsExistShippingCity(int methodId, int countryId)
        {
            return SQLDataAccess.ExecuteScalar<int>("Select Count(*) from [order].ShippingCity where MethodId=@MethodId and CityId=@CityId",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CityId", Value = countryId }
                                            ) > 0;
        }

        public static bool IsExistShippingCityExcluded(int methodId, int cityId)
        {
            return SQLDataAccess.ExecuteScalar<int>("Select Count(*) from [order].ShippingCityExcluded where MethodId=@MethodId and CityId=@CityId",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CityId", Value = cityId }
                                            ) > 0;
        }

        public static bool IsExistShippingCountryExcluded(int methodId, int countryId)
        {
            return SQLDataAccess.ExecuteScalar<int>("Select Count(*) from [order].ShippingCountryExcluded where MethodId=@MethodId and CountryId=@CountryId",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CountryId", Value = countryId }
                                            ) > 0;
        }

        //********** Add
        public static void AddPaymentCountry(int methodId, int countryId)
        {
            SQLDataAccess.ExecuteNonQuery("insert into [order].PaymentCountry (MethodId,CountryId) values (@MethodId,@CountryId)",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CountryId", Value = countryId }
                                            );
        }

        public static void AddShippingCountry(int methodId, int countryId)
        {
            SQLDataAccess.ExecuteNonQuery("insert into [order].ShippingCountry (MethodId,CountryId) values (@MethodId,@CountryId)",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CountryId", Value = countryId }
                                            );
        }

        public static void AddPaymentCity(int methodId, int countryId)
        {
            SQLDataAccess.ExecuteNonQuery("insert into [order].PaymentCity (MethodId,CityId) values (@MethodId,@CityId)",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CityId", Value = countryId }
                                            );
        }

        public static void AddShippingCity(int methodId, int countryId)
        {
            SQLDataAccess.ExecuteNonQuery("insert into [order].ShippingCity (MethodId,CityId) values (@MethodId,@CityId)",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CityId", Value = countryId }
                                            );
        }

        public static void AddShippingCityExcluded(int methodId, int cityId)
        {
            SQLDataAccess.ExecuteNonQuery("insert into [order].ShippingCityExcluded (MethodId,CityId) values (@MethodId,@CityId)",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CityId", Value = cityId }
                                            );
        }

        public static void AddShippingCountryExcluded(int methodId, int countryId)
        {
            SQLDataAccess.ExecuteNonQuery("insert into [order].ShippingCountryExcluded (MethodId,CountryId) values (@MethodId,@CountryId)",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CountryId", Value = countryId }
                                            );
        }

        //********** Delete
        public static void DeletePaymentCountry(int methodId, int countryId)
        {
            SQLDataAccess.ExecuteNonQuery("delete from [order].PaymentCountry where MethodId=@MethodId and CountryId=@CountryId",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CountryId", Value = countryId }
                                            );
        }

        public static void DeleteShippingCountry(int methodId, int countryId)
        {
            SQLDataAccess.ExecuteNonQuery("delete from [order].ShippingCountry where MethodId=@MethodId  and CountryId=@CountryId",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CountryId", Value = countryId }
                                            );
        }

        public static void DeletePaymentCity(int methodId, int countryId)
        {
            SQLDataAccess.ExecuteNonQuery("delete from [order].PaymentCity where MethodId=@MethodId and CityId=@CityId",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CityId", Value = countryId }
                                            );
        }

        public static void DeleteShippingCity(int methodId, int cityId)
        {
            SQLDataAccess.ExecuteNonQuery("delete from [order].ShippingCity where MethodId=@MethodId and CityId=@CityId",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CityId", Value = cityId }
                                            );
        }
        
        public static void DeleteShippingCityExcluded(int methodId, int cityId)
        {
            SQLDataAccess.ExecuteNonQuery("delete from [order].ShippingCityExcluded where MethodId=@MethodId and CityId=@CityId",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CityId", Value = cityId }
                                            );
        }

        public static void DeleteShippingCountryExcluded(int methodId, int CountryId)
        {
            SQLDataAccess.ExecuteNonQuery("delete from [order].ShippingCountryExcluded where MethodId=@MethodId and CountryId=@CountryId",
                                            CommandType.Text,
                                            new SqlParameter { ParameterName = "@MethodId", Value = methodId },
                                            new SqlParameter { ParameterName = "@CountryId", Value = CountryId }
                                            );
        }

        //****** Get by Shipping
        public static List<Country> GetCountryByShippingId(int shippingId)
        {
            return SQLDataAccess.ExecuteReadList<Country>("select * from [Customers].Country where CountryID in (select CountryID from [Order].ShippingCountry where MethodId=@MethodId)",
                                                            CommandType.Text,
                                                            CountryService.GetCountryFromReader,
                                                            new SqlParameter { ParameterName = "@MethodId", Value = shippingId }
                                                            );
        }

        public static List<City> GetCityByShippingId(int shippingId)
        {
            return SQLDataAccess.ExecuteReadList<City>("select * from [Customers].City where CityID in (select CityID from [Order].ShippingCity where MethodId=@MethodId)",
                                                            CommandType.Text,
                                                            CityService.GetCityFromReader,
                                                            new SqlParameter { ParameterName = "@MethodId", Value = shippingId }
                                                            );
        }

        public static List<City> GetCityByShippingIdExcluded(int shippingId)
        {
            return SQLDataAccess.ExecuteReadList<City>("select * from [Customers].City where CityID in (select CityID from [Order].ShippingCityExcluded where MethodId=@MethodId)",
                                                            CommandType.Text,
                                                            CityService.GetCityFromReader,
                                                            new SqlParameter { ParameterName = "@MethodId", Value = shippingId }
                                                            );
        }

        public static List<Country> GetCountryByShippingIdExcluded(int shippingId)
        {
            return SQLDataAccess.ExecuteReadList<Country>("select * from [Customers].Country where CountryID in (select CountryID from [Order].ShippingCountryExcluded where MethodId=@MethodId)",
                                                            CommandType.Text,
                                                            CountryService.GetCountryFromReader,
                                                            new SqlParameter { ParameterName = "@MethodId", Value = shippingId }
                                                            );
        }

        //***** Get By Payment
        public static List<Country> GetCountryByPaymentId(int paymentId)
        {
            return SQLDataAccess.ExecuteReadList<Country>("select * from [Customers].Country where CountryID in (select CountryID from [Order].PaymentCountry where MethodId=@MethodId)",
                                                            CommandType.Text,
                                                            CountryService.GetCountryFromReader,
                                                            new SqlParameter { ParameterName = "@MethodId", Value = paymentId }
                                                            );
        }

        public static List<City> GetCityByPaymentId(int paymentId)
        {
            return SQLDataAccess.ExecuteReadList<City>("select * from [Customers].City where CityID in (select CityID from [Order].PaymentCity where MethodId=@MethodId)",
                                                            CommandType.Text,
                                                            CityService.GetCityFromReader,
                                                            new SqlParameter { ParameterName = "@MethodId", Value = paymentId }
                                                            );
        }

        //***** check Shipping
        public static bool CheckShippingEnabledGeo(int methodId, string countryName, string cityName)
        {
            return CacheManager.Get(CacheNames.GetShippingForCityAndCountry(methodId, countryName, cityName), () =>
			{

				if (!string.IsNullOrEmpty(cityName) && CheckShippingExcludedCity(methodId, cityName))
				{
					return false;
				}
                //Не разрешенные страны
                if (!string.IsNullOrEmpty(countryName) && CheckShippingExcludedCountry(methodId, countryName))
                    return false;

                if (!string.IsNullOrEmpty(cityName) && CheckShippingEnabledCity(methodId, cityName))
				{
					return true;
				}

                if (!string.IsNullOrEmpty(countryName) && CheckShippingEnabledCountry(methodId, countryName))
					if (CheckShippingEnabledCityByCountry(methodId, countryName))
					return true;
				return false;
			});
        }

        private static bool CheckShippingEnabledCity(int methodId, string cityName)
        {
            return
                SQLDataAccess.ExecuteScalar<int>(
                    "select Count(*) from Customers.City where CityName=@CityName and CityId in (select CityId from [Order].ShippingCity where MethodId=@MethodId) and CityId not in (select CityId from [Order].ShippingCityExcluded where MethodId=@MethodId)",
                    CommandType.Text, new SqlParameter { ParameterName = "@CityName", Value = cityName ?? ""},
                    new SqlParameter { ParameterName = "@MethodId", Value = methodId }) > 0;
        }

        private static bool CheckShippingExcludedCity(int methodId, string cityName)
        {
            return
                SQLDataAccess.ExecuteScalar<int>(
                    "select Count(*) from Customers.City where CityName=@CityName and CityId in (select CityId from [Order].ShippingCityExcluded where MethodId=@MethodId)",
                    CommandType.Text, new SqlParameter { ParameterName = "@CityName", Value = cityName ?? ""},
                    new SqlParameter { ParameterName = "@MethodId", Value = methodId }) > 0;
        }
        
        private static bool CheckShippingEnabledCountry(int methodId, string countryName)
        {
            return
                SQLDataAccess.ExecuteScalar<int>(
                    @"if not exists (select CountryId from [Order].ShippingCountry where MethodId=@MethodId) and not exists (select CityId from [Order].ShippingCity where MethodId=@MethodId)
						Select 1
					else
					if  exists (select CountryId from Customers.Country where CountryName=@CountryName and CountryId in (select CountryId from [Order].ShippingCountry where MethodId=@MethodId))
						select 1
					else
					select 0",
                    CommandType.Text, new SqlParameter { ParameterName = "@CountryName", Value = countryName ?? "" },
                    new SqlParameter { ParameterName = "@MethodId", Value = methodId }) > 0;
        }

        private static bool CheckShippingExcludedCountry(int methodId, string countryName)
        {
            return
                SQLDataAccess.ExecuteScalar<int>(
                    @"if exists(select ShippingCountryExcluded.CountryID from [Order].ShippingCountryExcluded left join Customers.Country on Country.CountryID = ShippingCountryExcluded.CountryId
                    where Country.CountryName = @CountryName and ShippingCountryExcluded.MethodId=@MethodId)
                        select 1
                    else 
                        select 0",
                    CommandType.Text, new SqlParameter { ParameterName = "@CountryName", Value = countryName ?? "" },
                    new SqlParameter { ParameterName = "@MethodId", Value = methodId }) > 0;
        }

        // TODO: Check it!
        private static bool CheckShippingEnabledCityByCountry(int methodId, string countryName)
        {
            return
                SQLDataAccess.ExecuteScalar<int>(
                    "select Count(*) from [Order].ShippingCity where MethodId=@MethodId and cityId in (Select cityId From Customers.City where RegionID in (SELECT RegionID FROM [Customers].[Region] WHERE CountryID in (select CountryId from [Order].ShippingCountry where MethodId=@MethodId and CountryId = (Select CountryId from Customers.Country where CountryName=@CountryName))))",
                    CommandType.Text,
                    new SqlParameter { ParameterName = "@CountryName", Value = countryName ?? "" },
                    new SqlParameter { ParameterName = "@MethodId", Value = methodId }) == 0;
        }


        public static bool IsExistGeoShipping(int methodId)
        {
			var temp = SQLDataAccess.ExecuteScalar<int>(@"if exists (select CityId from [Order].ShippingCity where MethodId=@MethodId)	
																select 1
															else
															if  exists (select CityId from [Order].ShippingCityExcluded where MethodId=@MethodId)
																select 1
															else
															if  exists (select CountryId from [Order].ShippingCountry where MethodId=@MethodId)
																select 1
															else
                                                            if  exists (select CountryId from [Order].ShippingCountryExcluded where MethodId=@MethodId)
																select 1
															else
																select 0",
                                                        CommandType.Text, new SqlParameter { ParameterName = "@MethodId", Value = methodId });
            return temp > 0;
        }


        //***** check payment
        public static bool CheckPaymentEnabledGeo(int methodId, string countryName, string cityName)
        {
            if (CheckPaymentEnabledCity(methodId, cityName))
                return true;
            if (CheckPaymentEnabledCountry(methodId, countryName))
                if (CheckPaymentEnabledCityByCountry(methodId, countryName))
                    return true;
            return false;
        }

        private static bool CheckPaymentEnabledCity(int methodId, string cityName)
        {
            return
                SQLDataAccess.ExecuteScalar<int>(
                    "select Count(*) from Customers.City where CityName=@CityName and CityId in (select CityId from [Order].PaymentCity where MethodId=@MethodId)",
                    CommandType.Text, new SqlParameter { ParameterName = "@CityName", Value = cityName  ?? ""},
                    new SqlParameter { ParameterName = "@MethodId", Value = methodId }) > 0;
        }

        private static bool CheckPaymentEnabledCountry(int methodId, string countryName)
        {
            return
                SQLDataAccess.ExecuteScalar<int>(
                    "select Count(*) from Customers.Country where CountryName=@CountryName and CountryId in (select CountryId from [Order].PaymentCountry where MethodId=@MethodId)",
                    CommandType.Text, new SqlParameter { ParameterName = "@CountryName", Value = countryName  ?? ""},
                    new SqlParameter { ParameterName = "@MethodId", Value = methodId }) > 0;
        }


        private static bool CheckPaymentEnabledCityByCountry(int methodId, string countryName)
        {
            return
                SQLDataAccess.ExecuteScalar<int>(
                    "select Count(*) from [Order].PaymentCity where MethodId=@MethodId and CityId in (Select CityId From Customers.City where RegionID in (SELECT RegionID FROM [Customers].[Region] WHERE CountryID in (select CountryId from [Order].PaymentCountry where MethodId=@MethodId and CountryId = (Select CountryId from Customers.Country where CountryName=@CountryName))))",
                    CommandType.Text,
                    new SqlParameter { ParameterName = "@CountryName", Value = countryName ?? "" },
                    new SqlParameter { ParameterName = "@MethodId", Value = methodId }) == 0;
        }


        public static bool IsExistGeoPayment(int methodId)
        {
            var recordsCount = SQLDataAccess.ExecuteScalar<int>("select count(CityId) from [Order].PaymentCity where MethodId=@MethodId",
                                                        CommandType.Text, new SqlParameter { ParameterName = "@MethodId", Value = methodId });
            recordsCount += SQLDataAccess.ExecuteScalar<int>("select count(CountryId) from [Order].PaymentCountry where MethodId=@MethodId",
                                                        CommandType.Text, new SqlParameter { ParameterName = "@MethodId", Value = methodId });
            return recordsCount > 0;
        }
    }
}