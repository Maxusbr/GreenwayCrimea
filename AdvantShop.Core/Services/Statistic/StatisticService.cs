//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using Newtonsoft.Json;
using RestSharp;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Core.Services.Statistic
{
    public enum EGroupDateBy
    {
        Day,
        Week,
        Month
    }

    public class StatisticService
    {

        #region Search statistic

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="description"></param>
        /// <param name="resultCount"></param>
        /// <param name="searchTerm"></param>
        /// <param name="customerId"></param>
        public static void AddSearchStatistic(string request, string searchTerm, string description, int resultCount, Guid customerId)
        {
            SQLDataAccess.ExecuteNonQuery(@"IF (SELECT COUNT(ID) FROM [Statistic].[SearchStatistic] WHERE SearchTerm = @SearchTerm AND Description = @Description AND CustomerID = @CustomerID) = 0 Begin
                INSERT INTO [Statistic].[SearchStatistic] ([Request],[ResultCount],[Date],[SearchTerm],[Description],[CustomerID]) VALUES (@Request, @Resultcount, GETDATE(), @SearchTerm, @Description,@CustomerID) end ",
                CommandType.Text,
                new SqlParameter("@Request", request),
                new SqlParameter("@Resultcount", resultCount),
                new SqlParameter("@SearchTerm", searchTerm),
                new SqlParameter("@Description", description),
                new SqlParameter("@CustomerID", customerId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetHistorySearchStatistic(int numRows)
        {
            return SQLDataAccess.ExecuteTable(
                "SELECT TOP(@NumRows) * FROM [Statistic].[SearchStatistic] ORDER BY Date DESC",
                CommandType.Text,
                new SqlParameter("@NumRows", numRows));
        }

        /// <summary>
        /// Statistic by frequency search
        /// </summary>
        /// <returns></returns>
        public static DataTable GetFrequencySearchStatistic(DateTime date)
        {
            return SQLDataAccess.ExecuteTable(
                    "SELECT [Request], COUNT([Request]) AS numOfRequest, [ResultCount],[SearchTerm],[Description] FROM [Statistic].[SearchStatistic] WHERE [Date] >= Convert(date, @Date) GROUP BY [Request],[ResultCount],[SearchTerm],[Description] ORDER BY numOfRequest DESC",
                    CommandType.Text,
                    new SqlParameter("@Date", date));
        }
        #endregion

        #region Common statistic

        public static int GetOrdersCountByDateRange(DateTime fromDate, DateTime toDate)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT COUNT(OrderID) " +
                "FROM [Order].[Order] " +
                "WHERE IsDraft = 0 AND [OrderDate] IS NOT NULL AND Convert(date, @FromDate) <= Convert(date, [OrderDate]) AND Convert(date, [OrderDate]) <=  Convert(date, @ToDate)",
                CommandType.Text,
                new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate));
        }

        public static float GetOrdersSumByDateRange(DateTime fromDate, DateTime toDate)
        {
            return SQLDataAccess.ExecuteScalar<float>(
                "SELECT isnull(Sum([Order].[Sum] * [OrderCurrency].[CurrencyValue]), 0) " +
                "FROM [Order].[Order] " +
                "Left Join [Order].[OrderCurrency] On [OrderCurrency].[OrderId] = [Order].[OrderId] " +
                "WHERE IsDraft = 0 AND [OrderDate] IS NOT NULL AND Convert(date, @FromDate) <= Convert(date, [OrderDate]) AND Convert(date, [OrderDate]) <=  Convert(date, @ToDate)",
                CommandType.Text,
                new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate));
        }
        
        public static int GetOrdersCountByDate(DateTime date)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT COUNT(OrderID) FROM [Order].[Order]  WHERE IsDraft = 0 and Convert(date, [OrderDate]) = Convert(date, @Date)",
                CommandType.Text,
                new SqlParameter("@Date", date));
        }


        public static float GetOrdersSumByDate(DateTime date)
        {
            var currentcurrency = CurrencyService.CurrentCurrency;
            var cmd = string.Format("SELECT isnull(Sum([order].[sum] * {0} / [OrderCurrency].[CurrencyValue]), 0) " +
                "FROM [Order].[Order] " +
                "Left Join [Order].[OrderCurrency] On [OrderCurrency].[OrderId] = [Order].[OrderId] " +
                "WHERE IsDraft = 0 AND [OrderDate] IS NOT NULL AND Convert(date, [OrderDate]) = Convert(date, @Date)", currentcurrency.Rate.ToString().Replace(",","."));
            return SQLDataAccess.ExecuteScalar<float>(cmd, CommandType.Text, new SqlParameter("@Date", date));
        }


        public static int GetOrdersCount()
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT COUNT(OrderID) FROM [Order].[Order] Where IsDraft = 0",
                CommandType.Text);
        }


        public static int GetProductsCount()
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT COUNT(ProductID) FROM [Catalog].[Product]",
                CommandType.Text);
        }

        /// <summary>
        /// get orders with order status @default order status@
        /// </summary>
        /// <returns>orders count</returns>
        public static int GetLastOrdersCount(int? managerId = null)
        {
            if (managerId == null)
                return SQLDataAccess.ExecuteScalar<int>(
                    "SELECT COUNT(OrderID) FROM [Order].[Order] WHERE IsDraft = 0 and [OrderStatusID] = (SELECT [OrderStatusID] FROM [Order].[OrderStatus] WHERE [IsDefault] = 1)",
                    CommandType.Text);

            var sql = "SELECT COUNT(OrderID) FROM [Order].[Order] WHERE IsDraft = 0 and [OrderStatusID] = (SELECT [OrderStatusID] FROM [Order].[OrderStatus] WHERE [IsDefault] = 1)";

            switch (SettingsManager.ManagersOrderConstraint)
            {
                case ManagersOrderConstraint.Assigned:
                    sql += " and (ManagerId = @ManagerId)";
                    break;

                case ManagersOrderConstraint.AssignedAndFree:
                    sql += " and (ManagerId = @ManagerId or ManagerId is null)";
                    break;
            }
            return SQLDataAccess.ExecuteScalar<int>(sql, CommandType.Text, new SqlParameter("@ManagerId", managerId));
        }


        /// <summary>
        /// get reviews count
        /// </summary>
        /// <returns></returns>
        public static int GetReviewsCount()
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT COUNT(ReviewID) FROM [CMS].[Review]",
                CommandType.Text);
        }

        /// <summary>
        /// get last reviews count
        /// </summary>
        /// <returns></returns>
        public static int GetLastReviewsCount()
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT COUNT(ReviewID) FROM [CMS].[Review] WHERE [Checked] = 0",
                CommandType.Text);
        }

        #endregion

        #region Leads

        public static int GetLeadsCountByDateRange(DateTime fromDate, DateTime toDate)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT COUNT(Id) FROM [Order].[Lead] WHERE Convert(date, @FromDate) <= Convert(date, [CreatedDate]) AND Convert(date, [CreatedDate]) <=  Convert(date, @ToDate)",
                CommandType.Text,
                new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate));
        }

        public static float GetLeadsSumByDateRange(DateTime fromDate, DateTime toDate)
        {
            return SQLDataAccess.ExecuteScalar<float>(
                "SELECT isnull(Sum(Price * Amount * [LeadCurrency].[CurrencyValue]), 0) " +
                "FROM [Order].[Lead] " +
                "Left Join [Order].[LeadItem] On [LeadItem].[LeadId] = [Lead].[Id] " +
                "Left Join [Order].[LeadCurrency] On [LeadCurrency].[LeadId] = [Lead].[Id] " +
                "WHERE Convert(date, @FromDate) <= Convert(date, [CreatedDate]) AND Convert(date, [CreatedDate]) <=  Convert(date, @ToDate)",
                CommandType.Text,
                new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate));
        }

        public static int GetLeadsCountByDate(DateTime date)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT COUNT(Id) FROM [Order].[Lead] WHERE Convert(date, [CreatedDate]) = Convert(date, @Date)",
                CommandType.Text,
                new SqlParameter("@Date", date));
        }


        public static float GetLeadsSumByDate(DateTime date)
        {
            return SQLDataAccess.ExecuteScalar<float>(
                "SELECT isnull(Sum(Price * Amount * [LeadCurrency].[CurrencyValue]), 0) " +
                "FROM [Order].[Lead] " +
                "Left Join [Order].[LeadItem] On [LeadItem].[LeadId] = [Lead].[Id] " +
                "Left Join [Order].[LeadCurrency] On [LeadCurrency].[LeadId] = [Lead].[Id] " +
                "WHERE Convert(date, [CreatedDate]) = Convert(date, @Date)",
                CommandType.Text,
                new SqlParameter("@Date", date));
        }

        #endregion


        #region Reviews

        public static int GetReviewsCountByDate(DateTime date)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT COUNT(ReviewId) FROM [CMS].[Review] WHERE Convert(date, [AddDate]) = Convert(date, @Date)",
                CommandType.Text,
                new SqlParameter("@Date", date));
        }

        public static int GetReviewsCountByDateRange(DateTime fromDate, DateTime toDate)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT COUNT(ReviewId) FROM [CMS].[Review] WHERE Convert(date, @FromDate) <= Convert(date, [AddDate]) AND Convert(date, [AddDate]) <=  Convert(date, @ToDate)",
                CommandType.Text,
                new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate));
        }

        #endregion

        #region Calls

        public static int GetCallsCountByDate(DateTime date)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT COUNT(Id) FROM [Customers].[Call] WHERE Convert(date, [CallDate]) = Convert(date, @Date)",
                CommandType.Text,
                new SqlParameter("@Date", date));
        }

        public static int GetCallsCountByDateRange(DateTime fromDate, DateTime toDate)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT COUNT(Id) FROM [Customers].[Call] WHERE Convert(date, @FromDate) <= Convert(date, [CallDate]) AND Convert(date, [CallDate]) <=  Convert(date, @ToDate)",
                CommandType.Text,
                new SqlParameter("@FromDate", fromDate),
                new SqlParameter("@ToDate", toDate));
        }

        #endregion

        public static int GetRemainingLessons()
        {
            var callApiPath = SettingsLic.BasePlatformUrl;
            var client = new RestClient(callApiPath);
            var request = new RestRequest("/shop/api/RemainingLessons.ashx?id="+ CustomerContext.CustomerId, Method.GET)
            {
                RequestFormat = DataFormat.Json,
                Timeout = 9000
            };
            request.AddHeader("Authentication", SettingsLic.LicKey);
            
            IRestResponse res = client.Execute(request);
            if (res.StatusCode != HttpStatusCode.OK) return 0;
            string a = res.Content;
            return a.TryParseInt();
        }
    }
}