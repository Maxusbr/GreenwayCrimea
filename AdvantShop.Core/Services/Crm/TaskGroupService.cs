using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Configuration;
using AdvantShop.Core.SQL;
using AdvantShop.Helpers;

namespace AdvantShop.Core.Services.Crm
{
    public class TaskGroupService
    {
        private static TaskGroup GetTaskGroupFromReader(SqlDataReader reader)
        {
            return new TaskGroup
            {
                Id = SQLDataHelper.GetInt(reader, "Id"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                SortOrder = SQLDataHelper.GetInt(reader, "SortOrder"),
                DateCreated = SQLDataHelper.GetDateTime(reader, "DateCreated"),
            };
        }

        public static TaskGroup GetTaskGroup(int TaskGroupId)
        {
            return SQLDataAccess.ExecuteReadOne(
                "SELECT * FROM Customers.TaskGroup WHERE Id = @Id", CommandType.Text,
                GetTaskGroupFromReader, new SqlParameter("@Id", TaskGroupId));
        }

        public static List<TaskGroup> GetAllTaskGroups()
        {
            return SQLDataAccess.ExecuteReadList<TaskGroup>(
                "SELECT * FROM Customers.TaskGroup ORDER BY SortOrder", CommandType.Text, GetTaskGroupFromReader);
        }

        public static List<TaskGroup> GetActiveTaskGroups(int count)
        {
            return SQLDataAccess.ExecuteReadList<TaskGroup>(
                "SELECT TOP(@count) TaskGroup.*, " +
                "(SELECT MAX(DateAppointed) FROM Customers.Task WHERE TaskGroupId = TaskGroup.Id AND IsDeferred = 0 AND ([Status] = 0 OR [Status] = 1)) AS LastDate " +
                "FROM Customers.TaskGroup ORDER BY LastDate DESC ", 
                CommandType.Text, GetTaskGroupFromReader, 
                new SqlParameter("@count", count));
        }

        public static int AddTaskGroup(TaskGroup group)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "INSERT INTO Customers.TaskGroup (Name, SortOrder, DateCreated, DateModified) " +
                "VALUES (@Name, @SortOrder, GETDATE(), GETDATE()); SELECT SCOPE_IDENTITY();", 
                CommandType.Text, 
                new SqlParameter("@Name", group.Name),
                new SqlParameter("@SortOrder", group.SortOrder));
        }

        public static void UpdateTaskGroup(TaskGroup group)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE Customers.TaskGroup SET Name = @Name, SortOrder = @SortOrder, DateModified = GETDATE() WHERE Id = @Id", CommandType.Text,
                new SqlParameter("@Id", group.Id),
                new SqlParameter("@Name", group.Name),
                new SqlParameter("@SortOrder", group.SortOrder));
        }

        public static void DeleteTaskGroup(int id)
        {
            if (SettingsTasks.DefaultTaskGroup == id)
                SettingsTasks.DefaultTaskGroup = 0;
            SQLDataAccess.ExecuteNonQuery(
                "IF (SELECT COUNT(Id) FROM Customers.Task WHERE TaskGroupId = @Id) = 0 DELETE FROM Customers.TaskGroup WHERE Id = @Id", CommandType.Text,
                new SqlParameter("@Id", id));
        }
    }
}