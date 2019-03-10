using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL;

namespace AdvantShop.Customers
{
    public class CustomerFieldService
    {
        #region CustomerField

        public static CustomerField GetCustomerField(int id)
        {
            return SQLDataAccess.Query<CustomerField>("SELECT * FROM Customers.CustomerField WHERE Id = @Id", new { id }).FirstOrDefault();
        }

        public static List<CustomerField> GetCustomerFields(bool enabled = true)
        {
            return SQLDataAccess.Query<CustomerField>("SELECT * FROM Customers.CustomerField " + (enabled ? "Where Enabled = 1 " : "") +"ORDER BY SortOrder, Name").ToList();
        }

        public static List<CustomerFieldWithValue> GetCustomerFieldsWithValue(Guid customerId)
        {
            if (customerId == Guid.Empty)
                return SQLDataAccess.Query<CustomerFieldWithValue>(
                    "SELECT cf.* " +
                    "FROM Customers.CustomerField as cf " +
                    "WHERE cf.Enabled = 1 " +
                    "ORDER BY cf.SortOrder",
                    new {customerId}).ToList();

            return SQLDataAccess.Query<CustomerFieldWithValue>(
                "SELECT cf.*, map.Value " +
                "FROM Customers.CustomerField as cf " +
                "LEFT JOIN Customers.CustomerFieldValuesMap as map ON map.CustomerId = @customerId and map.CustomerFieldId = cf.Id " +
                "WHERE cf.Enabled = 1 " +
                "ORDER BY cf.SortOrder",
                new { customerId }).ToList();
        }

        public static int AddCustomerField(CustomerField field)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "INSERT INTO Customers.CustomerField (Name, FieldType, SortOrder, Required, Enabled, ShowInClient) " +
                "VALUES (@Name, @FieldType, @SortOrder, @Required, @Enabled, @ShowInClient); SELECT SCOPE_IDENTITY();",
                CommandType.Text,
                new SqlParameter("@Name", field.Name),
                new SqlParameter("@FieldType", field.FieldType),
                new SqlParameter("@SortOrder", field.SortOrder),
                new SqlParameter("@Required", field.Required),
                new SqlParameter("@Enabled", field.Enabled),
                new SqlParameter("@ShowInClient", field.ShowInClient)
                );
        }

        public static void UpdateCustomerField(CustomerField field)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE Customers.CustomerField SET Name = @Name, FieldType = @FieldType, SortOrder = @SortOrder, Required = @Required, Enabled = @Enabled, ShowInClient = @ShowInClient WHERE Id = @Id", 
                CommandType.Text,
                new SqlParameter("@Id", field.Id),
                new SqlParameter("@Name", field.Name),
                new SqlParameter("@FieldType", field.FieldType),
                new SqlParameter("@SortOrder", field.SortOrder),
                new SqlParameter("@Required", field.Required),
                new SqlParameter("@Enabled", field.Enabled),
                new SqlParameter("@ShowInClient", field.ShowInClient)
                );
        }

        public static void DeleteCustomerField(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM Customers.CustomerField WHERE Id = @Id", 
                CommandType.Text, 
                new SqlParameter("@Id", id));
        }

        #endregion

        #region CustomerFieldValue

        public static CustomerFieldValue GetCustomerFieldValue(int id)
        {
            return SQLDataAccess.Query<CustomerFieldValue>("SELECT * FROM Customers.CustomerFieldValue WHERE Id = @Id", new { id }).FirstOrDefault();
        }

        public static List<CustomerFieldValue> GetCustomerFieldValues(int customerFieldId)
        {
            return SQLDataAccess.Query<CustomerFieldValue>("SELECT * FROM Customers.CustomerFieldValue WHERE CustomerFieldId = @customerFieldId ORDER BY SortOrder, Value", new { customerFieldId }).ToList();
        }

        public static int AddCustomerFieldValue(CustomerFieldValue value)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "INSERT INTO Customers.CustomerFieldValue (Value, CustomerFieldId, SortOrder) VALUES (@Value, @CustomerFieldId, @SortOrder); SELECT SCOPE_IDENTITY();",
                CommandType.Text,
                new SqlParameter("@Value", value.Value),
                new SqlParameter("@CustomerFieldId", value.CustomerFieldId),
                new SqlParameter("@SortOrder", value.SortOrder)
                );
        }

        public static void UpdateCustomerFieldValue(CustomerFieldValue value)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE Customers.CustomerFieldValue SET Value = @Value, CustomerFieldId = @CustomerFieldId, SortOrder = @SortOrder WHERE Id = @Id",
                CommandType.Text,
                new SqlParameter("@Id", value.Id),
                new SqlParameter("@Value", value.Value),
                new SqlParameter("@CustomerFieldId", value.CustomerFieldId),
                new SqlParameter("@SortOrder", value.SortOrder)
                );
        }

        public static void DeleteCustomerFieldValue(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM Customers.CustomerFieldValue WHERE Id = @Id",
                CommandType.Text,
                new SqlParameter("@Id", id));
        }

        public static void DeleteCustomerFieldValues(int customerFieldId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM Customers.CustomerFieldValue WHERE CustomerFieldId = @CustomerFieldId",
                CommandType.Text,
                new SqlParameter("@CustomerFieldId", customerFieldId));
        }

        #endregion

        #region CustomerFieldValuesMap

        public static void DeleteFieldMap(Guid customerId, int fieldId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM Customers.CustomerFieldValuesMap WHERE CustomerId = @CustomerId AND CustomerFieldId = @CustomerFieldId",
                CommandType.Text, 
                new SqlParameter("@CustomerId", customerId),
                new SqlParameter("@CustomerFieldId", fieldId)
                );
        }

        public static void UpdateFieldsMapValues(int fieldId, string value)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE Customers.CustomerFieldValuesMap SET Value = @Value WHERE CustomerFieldId = @CustomerFieldId",
                CommandType.Text,
                new SqlParameter("@CustomerFieldId", fieldId),
                new SqlParameter("@Value", value)
                );
        }

        public static void AddUpdateMap(Guid customerId, int fieldId, string value)
        {
            SQLDataAccess.ExecuteNonQuery(
                "IF ((Select Count(*) FROM Customers.CustomerFieldValuesMap WHERE CustomerId = @CustomerId AND CustomerFieldId = @CustomerFieldId) > 0) " +
                "UPDATE Customers.CustomerFieldValuesMap SET Value = @Value WHERE CustomerId = @CustomerId AND CustomerFieldId = @CustomerFieldId " +
                "ELSE " +
                "INSERT INTO Customers.CustomerFieldValuesMap (CustomerId, CustomerFieldId, Value) VALUES (@CustomerId, @CustomerFieldId, @Value) ",
                CommandType.Text, 
                new SqlParameter("@CustomerId", customerId),
                new SqlParameter("@CustomerFieldId", fieldId),
                new SqlParameter("@Value", value ?? (object)DBNull.Value)
                );
        }

        #endregion

        public static List<object> GetCustomerFieldTypesSelectOptions()
        {
            return Enum.GetValues(typeof(CustomerFieldType)).Cast<CustomerFieldType>().Select(x => (object)new
            {
                label = x.Localize(),
                value = (int)x,
            }).ToList();
        }
    }
}
