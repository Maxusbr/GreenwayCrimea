using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.SQL;

namespace AdvantShop.Customers
{
    public class ManagerRoleService
    {
        #region ManagerRole

        public static ManagerRole GetManagerRole(int id)
        {
            return SQLDataAccess.Query<ManagerRole>("SELECT * FROM Customers.ManagerRole WHERE Id = @Id", new { id }).FirstOrDefault();
        }

        public static List<ManagerRole> GetManagerRoles()
        {
            return SQLDataAccess.Query<ManagerRole>("SELECT * FROM Customers.ManagerRole ORDER BY SortOrder, Name").ToList();
        }

        public static List<ManagerRole> GetManagerRoles(Guid customerId)
        {
            return SQLDataAccess.Query<ManagerRole>(
                "SELECT ManagerRole.* FROM Customers.ManagerRole INNER JOIN Customers.ManagerRolesMap ON ManagerRolesMap.ManagerRoleId = ManagerRole.Id " +
                "WHERE CustomerId = @customerId ORDER BY SortOrder, Name", new { customerId }).ToList();
        }

        public static int AddManagerRole(ManagerRole role)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "INSERT INTO Customers.ManagerRole (Name, SortOrder) VALUES (@Name, @SortOrder); SELECT SCOPE_IDENTITY();",
                CommandType.Text,
                new SqlParameter("@Name", role.Name),
                new SqlParameter("@SortOrder", role.SortOrder)
                );
        }

        public static void UpdateManagerRole(ManagerRole role)
        {
            SQLDataAccess.ExecuteNonQuery(
                " UPDATE Customers.ManagerRole SET Name = @Name, SortOrder = @SortOrder WHERE Id = @Id", 
                CommandType.Text,
                new SqlParameter("@Id", role.Id),
                new SqlParameter("@Name", role.Name),
                new SqlParameter("@SortOrder", role.SortOrder)
                );
        }

        public static void DeleteManagerRole(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM Customers.ManagerRole WHERE Id = @Id", 
                CommandType.Text, 
                new SqlParameter("@Id", id));
        }

        #endregion

        #region ManagerRolesMap

        public static void DeleteMap(Guid customerId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM Customers.ManagerRolesMap WHERE CustomerId = @CustomerId",
                CommandType.Text, new SqlParameter("@CustomerId", customerId));
        }

        public static void AddMap(Guid customerId, int managerRoleId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "INSERT INTO Customers.ManagerRolesMap (CustomerId, ManagerRoleId) VALUES (@CustomerId, @ManagerRoleId)",
                CommandType.Text, new SqlParameter("@CustomerId", customerId), new SqlParameter("@ManagerRoleId", managerRoleId));
        }

        #endregion
    }
}
