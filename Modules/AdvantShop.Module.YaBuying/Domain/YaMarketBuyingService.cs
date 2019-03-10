//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------


using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.Modules;
using AdvantShop.Helpers;
using AdvantShop.Repository;

namespace AdvantShop.Module.YaBuying.Domain
{
    public class YaMarketByuingService
    {
        public static bool InstallModule()
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Module.YaMarketShippings') AND type in (N'U'))" +
                "Begin " +
                @"CREATE TABLE Module.YaMarketShippings
	                (
	                Id int NOT NULL IDENTITY (1, 1),
	                ShippingMethodId int NOT NULL,
	                Type nvarchar(25) NOT NULL,
	                MinDate int NOT NULL,
	                MaxDate int NOT NULL
	                )  ON [PRIMARY]

                ALTER TABLE Module.YaMarketShippings ADD CONSTRAINT
	                PK_YaMarketShippings PRIMARY KEY CLUSTERED 
	                (Id) WITH(STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

                ALTER TABLE Module.YaMarketShippings ADD CONSTRAINT
	                FK_YaMarketShippings_ShippingMethod FOREIGN KEY (ShippingMethodId) REFERENCES [Order].ShippingMethod
	                (ShippingMethodID) ON UPDATE  NO ACTION 
	                 ON DELETE  CASCADE  " +
                "End",
                CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(
                "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Module.YaMarketOrder') AND type in (N'U'))" +
                "Begin " +
                @"CREATE TABLE Module.YaMarketOrder
                    (
                    MarketOrderId int NOT NULL,
                    OrderId int NOT NULL,
                    Status [nvarchar](max) NOT NULL
                    )  ON [PRIMARY] " +
                "End",
                CommandType.Text);

            YaMarketBuyingSettings.AuthToken = "";
            YaMarketBuyingSettings.Payments = "";
            YaMarketBuyingSettings.Outlets = "";

            YaMarketBuyingSettings.AuthClientId = "";
            YaMarketBuyingSettings.AuthTokenToMarket = "";
            YaMarketBuyingSettings.Login = "";
            YaMarketBuyingSettings.CampaignId = "";

            YaMarketBuyingSettings.UpaidStatusId = 0;
            //YaMarketBuyingSettings.ProcessingStatusId = 0;
            //YaMarketBuyingSettings.DeliveryStatusId = 0;
            

            YaMarketBuyingSettings.DeliveredStatusId = 0;

            return true;
        }

        public static bool UpdateModule()
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Module.YaMarketPaymentCountry') AND type in (N'U')) " +
                "Begin " +
                @"CREATE TABLE Module.YaMarketPaymentCountry
		            (CountryId int NOT NULL, MethodId nvarchar(255) NOT NULL)  ON [PRIMARY]

	            ALTER TABLE Module.YaMarketPaymentCountry ADD CONSTRAINT
		            FK_YaMarketPaymentCountryy_Country FOREIGN KEY
		            (CountryId) REFERENCES Customers.Country
		            (CountryID) ON UPDATE  NO ACTION 
		             ON DELETE  CASCADE " +
                "End",
                CommandType.Text);

            ModulesRepository.ModuleExecuteNonQuery(
                "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Module.YaMarketPaymentCity') AND type in (N'U')) " +
                "Begin " +
                @"CREATE TABLE Module.YaMarketPaymentCity
		            (CityId int NOT NULL, MethodId nvarchar(255) NOT NULL)  ON [PRIMARY]

	            ALTER TABLE Module.YaMarketPaymentCity ADD CONSTRAINT
		            FK_YaMarketPaymentCity_City FOREIGN KEY
		            (CityId) REFERENCES Customers.City
		            (CityID) ON UPDATE  NO ACTION 
		             ON DELETE  CASCADE  " +
                "End",
                CommandType.Text);

            // Переносим статусы из DeliveryStatusId в DeliveryStatusesIds и удаляем DeliveryStatusId
            ModulesRepository.ModuleExecuteNonQuery(
                "If Exists(Select 1 From [Settings].[ModuleSettings] Where [Name] = 'DeliveryStatusId' and ModuleName = 'YaMarketBuying') " +
                "begin " +
                    "If Not Exists(Select 1 From[Settings].[ModuleSettings] Where[Name] = 'DeliveryStatusesIds' and ModuleName = 'YaMarketBuying') " +
                        "Insert Into[Settings].[ModuleSettings]([Name],[Value],[ModuleName]) Values('DeliveryStatusesIds', (Select top(1) Value From[Settings].[ModuleSettings] Where[Name] = 'DeliveryStatusId' and ModuleName = 'YaMarketBuying'), 'YaMarketBuying') " +

                    "Delete from[Settings].[ModuleSettings] Where[Name] = 'DeliveryStatusId' and ModuleName = 'YaMarketBuying' " +
                "end",
                CommandType.Text);

            // Переносим статусы из ProcessingStatusId в ProcessingStatusesIds и удаляем ProcessingStatusId
            ModulesRepository.ModuleExecuteNonQuery(
                "If Exists(Select 1 From [Settings].[ModuleSettings] Where [Name] = 'ProcessingStatusId' and ModuleName = 'YaMarketBuying') " +
                "begin " +
                    "If Not Exists(Select 1 From[Settings].[ModuleSettings] Where[Name] = 'ProcessingStatusesIds' and ModuleName = 'YaMarketBuying') " +
                        "Insert Into[Settings].[ModuleSettings]([Name],[Value],[ModuleName]) Values('ProcessingStatusesIds', (Select top(1) Value From[Settings].[ModuleSettings] Where[Name] = 'ProcessingStatusId' and ModuleName = 'YaMarketBuying'), 'YaMarketBuying') " +

                    "Delete from[Settings].[ModuleSettings] Where[Name] = 'ProcessingStatusId' and ModuleName = 'YaMarketBuying' " +
                "end",
                CommandType.Text);
            
            return true;
        }


        public static List<YaMarketShipping> GetShippings()
        {
            return ModulesRepository.ModuleExecuteReadList("Select * From Module.YaMarketShippings", CommandType.Text,
                reader => new YaMarketShipping()
                {
                    Id = SQLDataHelper.GetInt(reader, "Id"),
                    ShippingMethodId = SQLDataHelper.GetInt(reader, "ShippingMethodId"),
                    Type = SQLDataHelper.GetString(reader, "Type"),
                    MinDate = SQLDataHelper.GetInt(reader, "MinDate"),
                    MaxDate = SQLDataHelper.GetInt(reader, "MaxDate")
                });
        }

        public static string GetShippingType(string shippingName, string yandexCode)
        {
            if (yandexCode != "AUTO")
            {
                return yandexCode;
            }

            if (shippingName == null)
            {
                return "DELIVERY";
            }

            var name = shippingName.ToLower();

            if (name.Contains("почта"))
            {
                return "POST";
            }
            else if (name.Contains("самовывоз"))
            {
                return "PICKUP";
            }
            else if (name.Contains("курьер"))
            {
                return "DELIVERY";
            }

            return string.Empty;
        }

        public static void AddShipping(YaMarketShipping shipping)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "INSERT INTO [Module].[YaMarketShippings] ([ShippingMethodId],[Type],[MinDate],[MaxDate]) VALUES(@ShippingMethodId,@Type,@MinDate,@MaxDate)", 
                CommandType.Text,
                new SqlParameter("@ShippingMethodId", shipping.ShippingMethodId),
                new SqlParameter("@Type", shipping.Type),
                new SqlParameter("@MinDate", shipping.MinDate),
                new SqlParameter("@MaxDate", shipping.MaxDate));
        }

        public static void DeleteShippings()
        {
            ModulesRepository.ModuleExecuteNonQuery("Delete From [Module].[YaMarketShippings]", CommandType.Text);
        }

        public static YaOrder GetOrder(int marketOrderId)
        {
            return ModulesRepository.ModuleExecuteReadOne(
                "Select * From Module.YaMarketOrder Where MarketOrderId = @MarketOrderId", CommandType.Text,
                reader => new YaOrder()
                {
                    MarketOrderId = SQLDataHelper.GetInt(reader, "MarketOrderId"),
                    OrderId = SQLDataHelper.GetInt(reader, "OrderId"),
                    Status = SQLDataHelper.GetString(reader, "Status"),
                },
                new SqlParameter("@MarketOrderId", marketOrderId));
        }

        public static int GetMarketOrderId(int orderId)
        {
            return ModulesRepository.ModuleExecuteScalar<int>(
                "Select MarketOrderId From Module.YaMarketOrder Where OrderId = @OrderId", CommandType.Text,
                new SqlParameter("@OrderId", orderId));
        }

        public static void AddOrder(YaOrder order)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "Insert Into Module.YaMarketOrder (MarketOrderId,OrderId,Status) VALUES (@MarketOrderId, @OrderId, @Status)", CommandType.Text,
                new SqlParameter("@MarketOrderId", order.MarketOrderId),
                new SqlParameter("@OrderId", order.OrderId),
                new SqlParameter("@Status", order.Status ?? string.Empty));
        }

        public static void UpdateOrder(YaOrder order)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "Update Module.YaMarketOrder Set Status=@Status Where MarketOrderId=@MarketOrderId and OrderId=@OrderId", CommandType.Text,
                new SqlParameter("@MarketOrderId", order.MarketOrderId),
                new SqlParameter("@OrderId", order.OrderId),
                new SqlParameter("@Status", order.Status ?? string.Empty));
        }

        public static DataTable GetHistory()
        {
            return ModulesRepository.ModuleExecuteTable(
                "Select [Order].OrderId, MarketOrderId, Status, OrderDate, Sum "+
                "From [Module].[YaMarketOrder] Left Join [Order].[Order] On [YaMarketOrder].[OrderId] = [Order].[OrderId]", 
                CommandType.Text);
        }
        

        #region Payment country

        public static void AddPaymentCountry(string methodId, int countryId)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "Insert into [Module].[YaMarketPaymentCountry] (MethodId, CountryId) Values (@MethodId, @CountryId)", CommandType.Text,
                new SqlParameter("@MethodId", methodId),
                new SqlParameter("@CountryId", countryId));
        }

        public static void DeletePaymentCountry(string methodId, int countryId)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "Delete from [Module].[YaMarketPaymentCountry] Where MethodId = @MethodId and CountryId = @CountryId", CommandType.Text,
                new SqlParameter("@MethodId", methodId),
                new SqlParameter("@CountryId", countryId));
        }

        public static bool IsExistPaymentCountry(string methodId, int countryId)
        {
            return ModulesRepository.ModuleExecuteScalar<int>(
                "Select Count(*) from [Module].YaMarketPaymentCountry where MethodId=@MethodId and CountryId=@CountryId",
                CommandType.Text,
                new SqlParameter("@MethodId", methodId),
                new SqlParameter("@CountryId", countryId)) > 0;
        }

        public static List<Country> GetPaymentCountries(string methodId)
        {
            return ModulesRepository.ModuleExecuteReadList(
                "Select Country.CountryId, Country.CountryName From [Customers].[Country] " +
                "Left Join [Module].[YaMarketPaymentCountry] On Country.CountryId = YaMarketPaymentCountry.CountryId " +
                "Where MethodId = @MethodId", CommandType.Text,
                reader => new Country()
                {
                    CountryId = SQLDataHelper.GetInt(reader, "CountryId"),
                    Name = SQLDataHelper.GetString(reader, "CountryName"),
                },
                new SqlParameter("@MethodId", methodId));
        }

        #endregion

        #region Payment city

        public static void AddPaymentCity(string methodId, int cityId)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "Insert into [Module].[YaMarketPaymentCity] (MethodId, CityId) Values (@MethodId, @CityId)", CommandType.Text,
                new SqlParameter("@MethodId", methodId),
                new SqlParameter("@CityId", cityId));
        }

        public static void DeletePaymentCity(string methodId, int cityId)
        {
            ModulesRepository.ModuleExecuteNonQuery(
                "Delete from [Module].[YaMarketPaymentCity] Where MethodId = @MethodId and CityId = @CityId", CommandType.Text,
                new SqlParameter("@MethodId", methodId),
                new SqlParameter("@CityId", cityId));
        }

        public static bool IsExistPaymentCity(string methodId, int cityId)
        {
            return ModulesRepository.ModuleExecuteScalar<int>(
                "Select Count(*) from [Module].[YaMarketPaymentCity] where MethodId=@MethodId and CityId=@CityId",
                CommandType.Text,
                new SqlParameter("@MethodId", methodId),
                new SqlParameter("@CityId", cityId)) > 0;
        }

        public static List<City> GetPaymentCities(string methodId)
        {
            return ModulesRepository.ModuleExecuteReadList(
                "Select City.CityId, City.CityName From [Customers].[City] " +
                "Left Join [Module].[YaMarketPaymentCity] On City.CityId = YaMarketPaymentCity.CityId " +
                "Where MethodId = @MethodId", CommandType.Text,
                reader => new City()
                {
                    CityId = SQLDataHelper.GetInt(reader, "CityId"),
                    Name = SQLDataHelper.GetString(reader, "CityName"),
                },
                new SqlParameter("@MethodId", methodId));
        }

        #endregion

    }
}