using AdvantShop.Core.Modules;
using AdvantShop.Module.LastOrder.Models;
using AdvantShop.Orders;
using AdvantShop.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvantShop.Module.LastOrder.Service
{
    public class FNPService
    {
        static string ModuleID = LastOrder.ModuleStringId;

        public static void InsertOrUpdateNotification(FNPModel fnp)
        {
            var pars = new List<SqlParameter> {
                new SqlParameter("@productId",fnp.ProductId),
                new SqlParameter("@fakeDateTime",fnp.FakeDateTime),
                new SqlParameter("@name",fnp.Name),
                new SqlParameter("@city",fnp.City),
            };
            var query = @"DECLARE @exist int
                            SET @exist = (SELECT Count(*) FROM Module.LastOrder WHERE ProductId = @productId)
                            IF (@exist = 0)
                                INSERT INTO Module." + ModuleID + @" (ProductID, FakeDateTime, Name, City) VALUES (@productId, @fakeDateTime, @name, @city)
                            ELSE
                                UPDATE Module." + ModuleID + @" SET FakeDateTime = @fakeDateTime, Name = @name, City = @city WHERE ProductId = @productId";
            ModuleService.SqlNonQuery(query,pars.ToArray());
        }
        
        public static void RemoveNotification(int productId)
        {
            var query = @"DELETE FROM Module." + ModuleID + @" WHERE productId = " + productId;
            ModuleService.SqlNonQuery(query);
        }

        public static FNPModel GetFakeNotification(int productId)
        {
            var query = @"SELECT * FROM Module." +ModuleID+ @" WHERE ProductId = " + productId;

            return ModulesRepository.Query<FNPModel>(query, CommandType.Text).FirstOrDefault();
        }

        public static List<FNPModel> GetFakeNotifications()
        {
            var query = @"SELECT * FROM Module." + ModuleID;
            return ModulesRepository.Query<FNPModel>(query,CommandType.Text).ToList();
        }

        public static FNPModel GenerateNotification(int productId, bool real = false, bool showUserCity = false)
        {
            var model = new FNPModel();

            model.ProductId = productId;

            if (!real)
            {
                model.FakeDateTime = GetTime();
                var rnd = new Random();
                model.Name = GetName(rnd);
                model.City = showUserCity ? IpZoneContext.CurrentZone.City : GetCity(rnd);
            }
            else
            {
                model.FakeDateTime = DateTime.Now;
                model.Name = "";
                model.City = "";
            }

            InsertOrUpdateNotification(model);

            return model;
        }

        private static DateTime GetTime()
        {
            var range = ModuleSettings.rTime;
            var rndTime = new Random().Next(range.from, range.to);
            switch (range.rType)
            {
                case RangeTimeType.Minutes:
                    return DateTime.Now.AddMinutes(-rndTime);
                case RangeTimeType.Hours:
                    return DateTime.Now.AddHours(-rndTime);
                default:
                    return DateTime.Now.AddDays(-rndTime);
            }
            
        }

        private static string GetName(Random rnd)
        {
            var resultName = "";
            if (ModuleSettings.UseCustomNameAndCity)
            {
                if (ModuleSettings.Names.Count > 0)
                {
                    resultName = ModuleSettings.Names[rnd.Next(0, ModuleSettings.Names.Count)];
                }
            }
            else
            {
                var names = GetNameFromDb();
                if (names.Count > 0)
                {
                    resultName = names[rnd.Next(0, names.Count)];
                }
            }
            return resultName;
        }

        private static string GetCity(Random rnd)
        {
            var resultCity = "";
            if (ModuleSettings.UseCustomNameAndCity)
            {
                if (ModuleSettings.Citys.Count > 0)
                {
                    resultCity = ModuleSettings.Citys[rnd.Next(0, ModuleSettings.Citys.Count)];
                }
            }
            else
            {
                var citys = GetCityFromDb();
                if (citys.Count > 0)
                {
                    resultCity = citys[rnd.Next(0, citys.Count)];
                }
            }
            return resultCity;
        }

        private static List<string> GetCityFromDb()
        {
            var query = @"SELECT CityName
                          FROM [Customers].[City]
                          INNER JOIN [Customers].[Region] ON [Customers].[City].[RegionID] = [Customers].[Region].[RegionID]
                          INNER JOIN [Customers].[Country] ON [Customers].[Country].[CountryID] = [Customers].[Region].[CountryID]
                          WHERE [Customers].[Country].[CountryName] = 'Россия'";

            return ModulesRepository.Query<string>(query, CommandType.Text).ToList();
        }

        private static List<string> GetNameFromDb()
        {
            var customers = Customers.CustomerService.GetCustomersbyRole(Customers.Role.User);
            return customers.Select(x => x.FirstName+ " " + x.LastName).ToList();
        }

        public static void InsertOrUpdateNotification(Order order)
        {
            foreach (var item in order.OrderItems)
            {
                if (item.ProductID != null)
                {
                    GenerateNotification((int)item.ProductID, true);
                }
            }
        }

        public static void ClearNotifications()
        {
            var mp = ModuleSettings.SaveMaxPeriod;

            var checkDate = DateTime.Now;

            switch (mp.PeriodType)
            {
                case SaveMaxPeriodType.Hours:
                    checkDate = checkDate.AddHours(-(mp.Period));
                    break;
                case SaveMaxPeriodType.Days:
                    checkDate = checkDate.AddDays(-(mp.Period));
                    break;
            }

            var notifications = GetFakeNotifications();

            foreach (var notify in notifications)
            {
                if (notify.FakeDateTime < checkDate)
                {
                    RemoveNotification(notify.ProductId);
                }
            }
        }
    }
}
