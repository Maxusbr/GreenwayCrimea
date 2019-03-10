using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.SQL;

namespace AdvantShop.Admin.Handlers.Statistics
{
    public class ManagersHandler
    {
        #region Properties

        private readonly DateTime _dateFrom;
        private readonly DateTime _dateTo;

        private class ManagerStatisticData
        {
            public Guid CustomerId { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public int OrdersCountAssign { get; set; }
            public int OrdersCount { get; set; }
            public float OrdersSum { get; set; }
        }

        #endregion

        public ManagersHandler(DateTime dateFrom, DateTime dateTo)
        {
            _dateFrom = dateFrom;
            _dateTo = dateTo;
        }


        public object GetManagers()
        {
            return GetManagerData();
        }

        private List<ManagerStatisticData> GetManagerData()
        {
            return SQLDataAccess.Query<ManagerStatisticData>(
                "Select Email, (Lastname + ' ' + Firstname) as Name, Customer.CustomerId, " +
                
                "(Select count(orderId) from [order].[order] inner join [order].[orderstatus] on [orderstatus].[OrderStatusID]=[order].[OrderStatusID]  " +
                "where ManagerId=[Managers].ManagerId and OrderDate > @DateFrom and OrderDate < @DateTo) as OrdersCountAssign, " +
                
                "(Select count(orderId) from [order].[order] inner join [order].[orderstatus] on [orderstatus].[OrderStatusID]=[order].[OrderStatusID]  " +
                "where ManagerId=[Managers].ManagerId and [IsCompleted]=1 and PaymentDate is not null and OrderDate > @DateFrom and OrderDate < @DateTo) as OrdersCount, " +
                
                "(Select Sum(sum) from [order].[order] inner join [order].[orderstatus] on [orderstatus].[OrderStatusID]=[order].[OrderStatusID]  " +
                "where ManagerId=[Managers].ManagerId and [IsCompleted]=1 and PaymentDate is not null and OrderDate > @DateFrom and OrderDate < @DateTo) as OrdersSum " +
                
                "From [Customers].[Customer] " +
                "INNER JOIN [Customers].[Managers] ON [Customer].CustomerID = [Managers].[CustomerId] " +
                "Order By Name",
                new {DateFrom = _dateFrom, DateTo = _dateTo}).ToList();
        }
    }
}