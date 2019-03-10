using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.SQL;

namespace AdvantShop.Core.Services.CustomerSegments
{
    public static class CustomerSegmentService
    {
        public static CustomerSegment Get(int id)
        {
            return SQLDataAccess.Query<CustomerSegment>("Select * From [Customers].[CustomerSegment] Where Id=@id", new {id}).FirstOrDefault();
        }

        public static List<CustomerSegment> GetList()
        {
            return SQLDataAccess.Query<CustomerSegment>("Select * From [Customers].[CustomerSegment]").ToList();
        }

        public static List<CustomerSegment> GetListByCustomerId(Guid customerId)
        {
            return SQLDataAccess.Query<CustomerSegment>(
                "Select s.* From [Customers].[CustomerSegment] as s " +
                "Left Join [Customers].[CustomerSegment_Customer] as c ON s.Id = c.SegmentId " +
                "Where c.CustomerId = @customerId", new {customerId}).ToList();
        }

        public static void Add(CustomerSegment segment)
        {
            segment.Id = SQLDataAccess.ExecuteScalar<int>(
                "Insert Into [Customers].[CustomerSegment] (Name, Filter, CreatedDate) Values (@Name, @Filter, getDate()); " +
                "Select scope_identity();",
                CommandType.Text,
                new SqlParameter("@Name", segment.Name),
                new SqlParameter("@Filter", segment.Filter ?? ""));
        }

        public static void Update(CustomerSegment segment)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Update [Customers].[CustomerSegment] Set Name=@Name, Filter=@Filter Where Id=@id",
                CommandType.Text,
                new SqlParameter("@id", segment.Id),
                new SqlParameter("@Name", segment.Name),
                new SqlParameter("@Filter", segment.Filter ?? ""));
        }

        public static void Delete(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Delete From [Customers].[CustomerSegment] Where Id=@id", CommandType.Text,
                new SqlParameter("@id", id));
        }


        #region CustomerSegment_Customer

        public static void AddCustomer(Guid customerId, int segmentId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Insert Into [Customers].[CustomerSegment_Customer] (CustomerId, SegmentId) Values (@customerId, @segmentId)",
                CommandType.Text,
                new SqlParameter("@customerId", customerId),
                new SqlParameter("@segmentId", segmentId));
        }

        public static void DeleteCustomersRelation(int segmentId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Delete From [Customers].[CustomerSegment_Customer] Where SegmentId=@SegmentId", CommandType.Text,
                new SqlParameter("@SegmentId", segmentId));
        }

        #endregion
    }
}
