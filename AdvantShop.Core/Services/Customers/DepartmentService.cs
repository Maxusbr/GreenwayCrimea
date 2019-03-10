//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.Customers
{
    public class DepartmentService
    {
        public static Department GetDepartment(int departmentId)
        {
            return SQLDataAccess.ExecuteReadOne<Department>(
                "SELECT * FROM [Customers].[Departments] WHERE DepartmentId = @DepartmentId", CommandType.Text,
                GetDepartmentFromReader, new SqlParameter("@DepartmentId", departmentId));
        }

        public static List<Department> GetDepartmentsList()
        {
            return
                SQLDataAccess.ExecuteReadList<Department>(
                    "SELECT * FROM [Customers].[Departments] ORDER BY [Sort], [Name]",
                    CommandType.Text, GetDepartmentFromReader);
        }

        public static List<Department> GetDepartmentsList(bool enabled)
        {
            return
                SQLDataAccess.ExecuteReadList<Department>(
                    "SELECT * FROM [Customers].[Departments] WHERE [Enabled] = @enabled ORDER BY [Sort], [Name]",
                    CommandType.Text, GetDepartmentFromReader,new SqlParameter("@enabled", enabled));
        }

        public static List<int> GetDepartmentsIDs()
        {
            return SQLDataAccess.ExecuteReadColumn<int>("SELECT [DepartmentId] FROM [Customers].[Departments]", CommandType.Text, "DepartmentId");
        }

        private static Department GetDepartmentFromReader(SqlDataReader reader)
        {
            return new Department
            {
                DepartmentId = SQLDataHelper.GetInt(reader, "DepartmentId"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                Sort = SQLDataHelper.GetInt(reader, "Sort"),
                Enabled = SQLDataHelper.GetBoolean(reader, "Enabled")
            };
        }

        public static int AddDepartments(Department department)
        {
            return SQLDataAccess.ExecuteScalar<int>(" INSERT INTO [Customers].[Departments] " +
                                                    " ([Name], [Sort], [Enabled]) " +
                                                    " VALUES (@Name, @Sort, @Enabled); SELECT SCOPE_IDENTITY();",
                CommandType.Text,
                new SqlParameter("@Name", department.Name),
                new SqlParameter("@Sort", department.Sort),
                new SqlParameter("@Enabled", department.Enabled)
                );
        }

        public static void UpdateDepartments(Department department)
        {
            SQLDataAccess.ExecuteNonQuery(
                " UPDATE [Customers].[Departments] SET [Name] = @Name, [Sort] = @Sort, [Enabled] = @Enabled " +
                " WHERE DepartmentId = @DepartmentId", CommandType.Text,
                new SqlParameter("@DepartmentId", department.DepartmentId),
                new SqlParameter("@Name", department.Name),
                new SqlParameter("@Sort", department.Sort),
                new SqlParameter("@Enabled", department.Enabled)
                );
        }

        public static void DeleteDepartments(int departmentId)
        {
            SQLDataAccess.ExecuteNonQuery("DELETE FROM [Customers].[Departments] WHERE DepartmentId = @DepartmentId", CommandType.Text, new SqlParameter("@DepartmentId", departmentId));
        }
    }
}
