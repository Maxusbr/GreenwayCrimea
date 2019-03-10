using System;
//using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Orders;

namespace AdvantShop.Modules.Convead
{
    public class ConveadService
    {
        public static string ModuleName
        {
            get { return "Convead"; }
        }

        public static bool GetOrderHistory(out string error, DateTime? from = null)
        {
            DateTime startDate = DateTime.Now.AddMinutes(-10);
            DateTime endDate = DateTime.Now;
            bool res = true;
            error = "";

            try
            {
                string query = "Select [OrderID] FROM [Order].[StatusHistory] where [Date] >= CONVERT( DATETIME, @StartDate, 21 ) and [Date] <= CONVERT( DATETIME, @EndDate, 21 )";
                var ordersIds = SQLDataAccess.ExecuteReadList<int>(query,
                                                    CommandType.Text,
                                                    reader => SQLDataHelper.GetInt(reader, "OrderID"),
                                                    new SqlParameter("@StartDate", startDate.ToString("yyyy-MM-dd HH:mm:ss.fff") ),
                                                    new SqlParameter("@EndDate", endDate.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                foreach (int orderId in ordersIds)
                {
                    IOrder order = OrderService.GetOrder(orderId);
                    var conv = new Module.Convead.Convead();
                    conv.DoOrderChangeStatus(order);
                }
            }
            catch (Exception ex) {
                Debug.LogError(ex);
            }
            return res;
        }
        

        
    }
}