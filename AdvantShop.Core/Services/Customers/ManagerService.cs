//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;

namespace AdvantShop.Customers
{
    public class ManagerService
    {
        public static Manager GetManager(int managerId)
        {
            return SQLDataAccess.ExecuteReadOne<Manager>(
                "SELECT * FROM [Customers].[Managers] WHERE ManagerId = @ManagerId", CommandType.Text,
                GetManagerFromReader, new SqlParameter("@ManagerId", managerId));
        }

        public static Manager GetManager(Guid customerId)
        {
            return SQLDataAccess.ExecuteReadOne<Manager>(
                "SELECT * FROM [Customers].[Managers] WHERE CustomerId = @CustomerId", CommandType.Text,
                GetManagerFromReader, new SqlParameter("@CustomerId", customerId));
        }

        public static List<Manager> GetManagersList(bool onlyActive = true)
        {
            return
                SQLDataAccess.ExecuteReadList<Manager>(
                    "SELECT Managers.* FROM [Customers].[Managers] INNER JOIN Customers.Customer ON Customer.CustomerId = Managers.CustomerId " +
                    (onlyActive ? "WHERE [Active] = 1 AND [Enabled] = 1 " : string.Empty),
                    CommandType.Text, GetManagerFromReader);
        }

        public static List<Guid> GetManagerIdsList()
        {
            return
                SQLDataAccess.ExecuteReadList<Guid>(
                    "SELECT CustomerId FROM [Customers].[Managers]",
                    CommandType.Text, reader => SQLDataHelper.GetGuid(reader, "CustomerId"));
        }

        public static List<Customer> GetCustomerManagersList()
        {
            return SQLDataAccess.ExecuteReadList<Customer>(
                "SELECT [Customer].* FROM [Customers].[Customer] " +
                "INNER JOIN [Customers].[Managers] ON [Customer].CustomerID = [Managers].[CustomerId] " +
                "Where [Managers].Active = 1 and Customer.Enabled = 1",
                CommandType.Text, CustomerService.GetFromSqlDataReader);
        }

        public static Customer GetMostFreeCustomer(int? managerRoleId, string city = null)
        {
            return SQLDataAccess.ExecuteReadOne<Customer>(
                "SELECT TOP(1) c.*, (SELECT COUNT(Id) FROM Customers.Task WHERE AssignedManagerId = m.ManagerId AND Status <> 2) AS OpenTasksCount " +
                "FROM Customers.Customer AS c " +
                "INNER JOIN Customers.Managers AS m ON m.CustomerID = c.CustomerId " +
                "WHERE c.[Enabled] = 1 " +
                (managerRoleId.HasValue ? "AND EXISTS(SELECT CustomerId FROM Customers.ManagerRolesMap WHERE ManagerRoleId = @ManagerRoleId AND CustomerId = m.CustomerId) " : string.Empty) +
                (city.IsNotEmpty() ? "AND City = @City " : string.Empty) +
                "ORDER BY OpenTasksCount",
                CommandType.Text, CustomerService.GetFromSqlDataReader, 
                managerRoleId.HasValue ? new SqlParameter("@ManagerRoleId", managerRoleId.Value) : null,
                city.IsNotEmpty() ? new SqlParameter("@City", city) : null);
        }

        //public static List<Guid> GetManagersIDs()
        //{
        //    return SQLDataAccess.ExecuteReadList<Guid>("SELECT [CustomerId] FROM [Customers].[Managers]",
        //        CommandType.Text, reader => SQLDataHelper.GetGuid(reader, "CustomerId"));
        //}

        public static bool CustomerIsManager(Guid customerId)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM [Customers].[Managers] WHERE CustomerId = @CustomerId And Active = 1", CommandType.Text,
                new SqlParameter("@CustomerId", customerId)) > 0;
        }

        private static Manager GetManagerFromReader(SqlDataReader reader)
        {
            return new Manager
            {
                ManagerId = SQLDataHelper.GetInt(reader, "ManagerId"),
                CustomerId = SQLDataHelper.GetGuid(reader, "CustomerId"),
                DepartmentId = SQLDataHelper.GetNullableInt(reader, "DepartmentId"),
                Position = SQLDataHelper.GetString(reader, "Position"),
                Active = SQLDataHelper.GetBoolean(reader, "Active")
            };
        }

        public static int GetManagersCount()
        {
            return SQLDataAccess.ExecuteScalar<int>("SELECT Count(ManagerId) FROM [Customers].[Managers]", CommandType.Text);
        }

        public static void AddOrUpdateManager(Manager manager)
        {
            var isNew = GetManager(manager.CustomerId) != null ? false : true;

            if (isNew && (Saas.SaasDataService.IsSaasEnabled &&
                  GetManagersCount() >= Saas.SaasDataService.CurrentSaasData.EmployeesCount))
            {
                return;
            }
            
            manager.ManagerId = SQLDataAccess.ExecuteScalar<int>(
                "IF ((SELECT COUNT(*) FROM [Customers].[Managers] WHERE CustomerId = @CustomerId) = 0) BEGIN" +
                " INSERT INTO [Customers].[Managers] ([CustomerId],[DepartmentId],[Position],[Active]) VALUES (@CustomerId,@DepartmentId,@Position,@Active); SELECT SCOPE_IDENTITY();  " +
                "END ELSE BEGIN " +
                " UPDATE [Customers].[Managers] SET [DepartmentId] = @DepartmentId, [Position] = @Position, [Active] = @Active " +
                " WHERE CustomerId = @CustomerId; " +
                " SELECT ManagerId FROM [Customers].[Managers] WHERE CustomerId = @CustomerId " +
                "END",
                CommandType.Text,
                new SqlParameter("@CustomerId", manager.CustomerId),
                new SqlParameter("@Position", manager.Position ?? string.Empty),
                new SqlParameter("@DepartmentId", manager.DepartmentId ?? (object)DBNull.Value),
                new SqlParameter("@Active", manager.Active)
                );

            if (isNew)
                TaskService.SetAllTasksViewed(manager.ManagerId);

            CacheManager.RemoveByPattern(CacheNames.Customer);
        }

        public static void DeleteManagerPhoto(int managerId)
        {
            PhotoService.DeletePhotos(managerId, PhotoType.Manager);

            CacheManager.RemoveByPattern(CacheNames.Customer);
        }

        public static void DeleteManager(Guid customerId)
        {
            DeleteManager(GetManager(customerId));
        }

        public static void DeleteManager(int managerId)
        {
            DeleteManager(GetManager(managerId));
        }

        private static void DeleteManager(Manager manager)
        {
            if (manager != null)
            {
                DeleteManagerPhoto(manager.ManagerId);

                SQLDataAccess.ExecuteNonQuery("DELETE FROM [Customers].[Managers] WHERE ManagerId = @ManagerId",
                    CommandType.Text, new SqlParameter("@ManagerId", manager.ManagerId));

                CacheManager.RemoveByPattern(CacheNames.Customer);
            }
        }

        public static void ForcedDeleteManager(Manager manager)
        {
            try
            {
                var managerId = manager.ManagerId;

                SQLDataAccess.ExecuteNonQuery("Update [Order].[Order] Set ManagerId = null Where ManagerId=@ManagerId",
                    CommandType.Text, new SqlParameter("@ManagerId", managerId));

                SQLDataAccess.ExecuteNonQuery("Update [Order].[Lead] Set ManagerId = null Where ManagerId=@ManagerId",
                    CommandType.Text, new SqlParameter("@ManagerId", managerId));

                SQLDataAccess.ExecuteNonQuery("Update Customers.Customer Set ManagerId = null Where ManagerId=@ManagerId",
                    CommandType.Text, new SqlParameter("@ManagerId", managerId));

                SQLDataAccess.ExecuteNonQuery(
                    "Update Customers.ManagerTask Set AssignedManagerId = null Where AssignedManagerId=@ManagerId",
                    CommandType.Text, new SqlParameter("@ManagerId", managerId));

                SQLDataAccess.ExecuteNonQuery(
                    "Update Customers.Task Set AssignedManagerId = null Where AssignedManagerId=@ManagerId",
                CommandType.Text, new SqlParameter("@ManagerId", managerId));


                SQLDataAccess.ExecuteNonQuery(
                    "Update Customers.ManagerTask Set AppointedManagerId = null Where AppointedManagerId=@ManagerId",
                    CommandType.Text, new SqlParameter("@ManagerId", managerId));

                SQLDataAccess.ExecuteNonQuery(
                    "Update Customers.Task Set AppointedManagerId = null Where AppointedManagerId=@ManagerId",
                CommandType.Text, new SqlParameter("@ManagerId", managerId));


                DeleteManager(manager);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        public static bool HasAssignedTasks(int managerId)
        {
            var oldTasksCount = SQLDataAccess.ExecuteScalar<int>("SELECT count(*) from Customers.ManagerTask Where AssignedManagerId=@ManagerId", CommandType.Text, new SqlParameter("@ManagerId", managerId));
            var tasksCount = SQLDataAccess.ExecuteScalar<int>("SELECT count(*) from Customers.Task Where AssignedManagerId=@ManagerId", CommandType.Text, new SqlParameter("@ManagerId", managerId));
            return oldTasksCount + tasksCount > 0;
        }

        public static bool HasAppointedTasks(int managerId)
        {
            var oldTasksCount = SQLDataAccess.ExecuteScalar<int>("SELECT count(*) from Customers.ManagerTask Where AppointedManagerId=@ManagerId", CommandType.Text, new SqlParameter("@ManagerId", managerId));
            var tasksCount = SQLDataAccess.ExecuteScalar<int>("SELECT count(*) from Customers.Task Where AppointedManagerId=@ManagerId", CommandType.Text, new SqlParameter("@ManagerId", managerId));
            return oldTasksCount + tasksCount > 0;
        }

        public static bool HasAssignedOrders(int managerId)
        {
            return SQLDataAccess.ExecuteScalar<int>("SELECT count(*) from [order].[order] Where ManagerId=@ManagerId", CommandType.Text, new SqlParameter("@ManagerId", managerId)) > 0;
        }

        public static bool HasAssignedLeads(int managerId)
        {
            return SQLDataAccess.ExecuteScalar<int>("SELECT count(*) from [order].[lead] Where ManagerId=@ManagerId", CommandType.Text, new SqlParameter("@ManagerId", managerId)) > 0;
        }

        public static bool HasAssignedCustomers(int managerId)
        {
            return SQLDataAccess.ExecuteScalar<int>("SELECT count(*) from Customers.Customer Where ManagerId=@ManagerId", CommandType.Text, new SqlParameter("@ManagerId", managerId)) > 0;
        }

        public static bool CanDelete(int managerId)
        {
            string message;
            return CanDelete(managerId, out message);
        }

        public static bool CanDelete(int managerId, out string message)
        {
            var reasons = new List<string>();
            if (HasAssignedTasks(managerId))
                reasons.Add(LocalizationService.GetResource("Core.Customers.ErrorDeleteManager.Tasks"));
            if (HasAssignedOrders(managerId))
                reasons.Add(LocalizationService.GetResource("Core.Customers.ErrorDeleteManager.Orders"));
            if (HasAssignedLeads(managerId))
                reasons.Add(LocalizationService.GetResource("Core.Customers.ErrorDeleteManager.Leads"));
            if (HasAssignedCustomers(managerId))
                reasons.Add(LocalizationService.GetResource("Core.Customers.ErrorDeleteManager.Customers"));
            if (HasAppointedTasks(managerId))
                reasons.Add(LocalizationService.GetResource("Core.Customers.ErrorDeleteManager.AppointedTasks"));
            message = reasons.Any() 
                ? string.Format(LocalizationService.GetResource("Core.Customers.ErrorDeleteManager"), reasons.AggregateString(", ")) 
                : string.Empty;
            return !reasons.Any();
        }
    }
}
