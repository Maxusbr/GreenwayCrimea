using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;
using AdvantShop.Module.RussianPostPrintBlank.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AdvantShop.Module.RussianPostPrintBlank.Service
{
    public class RPPBService
    {
        private static Dictionary<string, string> dictionary = new Dictionary<string, string>
        {
            {"дом", "д."},
            {"квартира", "кв."},
            {"строение", "стр."},
            {"корпус", "корп."},
            {"проспект", "пр-т"},
            {"проезд", "пр."},
            {"город", "г."},
            {"поселок", "пос."},
            {"улица", "ул."},
            {"переулок", "пер."},
            {"набережная", "наб."},
            {"площадь", "пл."},
            {"бульвар", "б-р"},
            {"шоссе", "ш."},
            {"область", "обл."},
            {"район", "р-н"},
            {"край", "кр."},
            {"автономный", "авт."},
            {"Республика", "Респ."},
            {"республика", "респ."}
        };

        public static bool Query(string query, SqlParameter[] parameters = null)
        {
            try
            {
                if (parameters != null)
                {
                    ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text, parameters);
                }
                else
                {
                    ModulesRepository.ModuleExecuteNonQuery(query, CommandType.Text);
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return false;
            }
        }

        public static bool Install()
        {
            var install = RPPBSettings.SetDefaultSettings();

            install &= TemplatesService.CreateTemplatesTable();

            return install;
        }

        public static bool UnInstall()
        {
            var uninstall = true;
            uninstall = RPPBSettings.RemoveSettings();
            uninstall &= TemplatesService.DeleteTemplatesTable();
            return uninstall;
        }

        public static bool Update()
        {
            var update = true;
            update = TemplatesService.CreateTemplatesTable();
            return update;
        }

        public static List<Order> GetOrdersWithStatus(int currPage, int perPage, string status, string shipping, bool? payed = null, string orderNumber = "")
        {
            var stop = currPage > 1 ? (currPage * perPage) : perPage;
            var start = currPage > 1 ? ((currPage - 1) * perPage) + 1 : currPage;
            status = status == "Любой" ? "" : status;
            var s = string.IsNullOrEmpty(status) ? "" : " [Order].[OrderStatus].[StatusName] = '" + status + "'";
            var p = payed != null ? ((bool)payed ? " PaymentDate is not null" : " PaymentDate is null") : "";
            shipping = shipping == "Любой" ? "" : shipping;
            var ship = string.IsNullOrEmpty(shipping) ? "" : " [Order].[ShippingMethod].[Name] =  '" + shipping + "'";
            var ordnum = string.IsNullOrEmpty(orderNumber) ? "" : " [Order].[OrderId] = " +orderNumber;
            var where = "";

            if (!string.IsNullOrEmpty(s))
            {
                where += " WHERE " + s;
            }

            if (!string.IsNullOrEmpty(where))
            {
                if (!string.IsNullOrEmpty(p))
                {
                    where += " AND " + p;
                }
                if(!string.IsNullOrEmpty(ship))
                {
                    where += " AND " + ship;
                }
            }
            else if (!string.IsNullOrEmpty(p))
            {
                where += " WHERE " + p;
                if (!string.IsNullOrEmpty(ship))
                {
                    where += " AND " + ship;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(ship))
                {
                    where += " WHERE " + ship;
                }
            }
            if(!string.IsNullOrEmpty(orderNumber))
            {
                if (string.IsNullOrEmpty(where))
                    where += " WHERE " + ordnum;
                else
                    where += " AND " + ordnum;
            }

            var query = @"SELECT  *
                            FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY OrderDate DESC ) AS RowNum, 
                                        [Order].[OrderId],
                                        [Order].[OrderDate], 
                                        [OrderCustomer].[LastName] + ' ' + [OrderCustomer].[FirstName] as BuyerName, 
                                        [OrderStatus].[StatusName],
                                        [Order].[PaymentDate],
                                        [Order].[ShippingMethod].Name as ShippingName
                                      FROM      [Order].[Order]
									  LEFT JOIN [Order].[OrderCustomer] ON [Order].[Order].[OrderID]=[Order].[OrderCustomer].[OrderId]
                                      LEFT JOIN [Order].[OrderStatus] ON [Order].[OrderStatus].[OrderStatusID]=[Order].[Order].[OrderStatusID]
                                      LEFT JOIN [Order].[ShippingMethod] ON [Order].[Order].[ShippingMethodId]=[Order].[ShippingMethod].[ShippingMethodID]
                                      " + where + @"
                                    ) AS RowConstrainedResult
                            WHERE   RowNum >= " + start + @"
                                AND RowNum <= " + stop + @"
                            ORDER BY RowNum";

            return ModulesRepository.Query<Order>(query, CommandType.Text).ToList();
        }

        public static int GetOrdersCount()
        {
            var query = "select count(orderid) from [Order].[Order]";
            return ModulesRepository.ModuleExecuteScalar<int>(query, CommandType.Text);
        }

        public static string CorrectAddress(string address)
        {
            var result = address;

            foreach (var pair in dictionary)
            {
                if (address.Contains("Белгород") || address.Contains("Подомское"))
                {
                    continue;
                }

                result = result.Replace(pair.Key, pair.Value);
            }

            return result;
        }

        public static string DeleteCityAndZoneFromAddress(string address, string city, string zone)
        {
            var addrWords = address.Split(',', ';');
            var resultAddr = new List<string>();

            if (addrWords.Length == 1)
            {
                var result = address;

                if (!string.IsNullOrEmpty(city))
                {
                    result = address.Replace(city.ToLower(), string.Empty).Replace(city, string.Empty);
                }

                if (!string.IsNullOrEmpty(zone))
                {
                    result = result.Replace(zone.ToLower(), string.Empty).Replace(zone, string.Empty);
                }

                return result;
            }

            foreach (var addrWord in addrWords)
            {
                if (!addrWord.ToLower().Contains(city.ToLower()) && !addrWord.ToLower().Contains(zone.ToLower()) && !resultAddr.Contains(addrWord.ToLower()))
                {
                    resultAddr.Add(addrWord);
                }
            }

            return resultAddr.Count > 0 ? string.Join(", ", resultAddr) : address;
        }
    }
}
