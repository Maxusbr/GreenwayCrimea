//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AdvantShop.Core.SQL;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;
using AdvantShop.Mails;

namespace AdvantShop.Customers
{
    public class ManagerTaskService
    {
        private static ManagerTask GetManagerTaskFromReader(SqlDataReader reader)
        {
            return new ManagerTask
            {
                TaskId = SQLDataHelper.GetInt(reader, "TaskId"),
                AssignedManagerId = SQLDataHelper.GetInt(reader, "AssignedManagerId"),
                AppointedManagerId = SQLDataHelper.GetInt(reader, "AppointedManagerId"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                Description = SQLDataHelper.GetString(reader, "Description"),
                Status = (ManagerTaskStatus)SQLDataHelper.GetInt(reader, "Status"),
                DueDate = SQLDataHelper.GetDateTime(reader, "DueDate"),
                DateCreated = SQLDataHelper.GetDateTime(reader, "DateCreated"),
                OrderId = SQLDataHelper.GetNullableInt(reader, "OrderId"),
                CustomerId = SQLDataHelper.GetNullableGuid(reader, "CustomerId"),
                LeadId = SQLDataHelper.GetNullableInt(reader, "LeadId"),
                ResultShort = SQLDataHelper.GetString(reader, "ResultShort"),
                ResultFull = SQLDataHelper.GetString(reader, "ResultFull"),
            };
        }

        public static ManagerTask GetManagerTask(int managerTaskId)
        {
            return SQLDataAccess.ExecuteReadOne(
                    "SELECT * FROM Customers.ManagerTask WHERE TaskId = @TaskId", CommandType.Text,
                    GetManagerTaskFromReader, new SqlParameter("@TaskId", managerTaskId));
        }

        public static List<ManagerTask> GetManagerTasks(int assignedManagerId, int count)
        {
            return SQLDataAccess.ExecuteReadList<ManagerTask>(
                    "SELECT TOP(@Count) * FROM Customers.ManagerTask WHERE AssignedManagerId = @AssignedManagerId ORDER BY DateCreated DESC", CommandType.Text,
                    GetManagerTaskFromReader, new SqlParameter("@AssignedManagerId", assignedManagerId), new SqlParameter("@Count", count));
        }

        public static List<ManagerTask> GeAllTasks()
        {
            return SQLDataAccess.ExecuteReadList<ManagerTask>(
                    "SELECT * FROM Customers.ManagerTask", CommandType.Text, GetManagerTaskFromReader);
        }


        public static List<ManagerTask> GetManagerTasks(int assignedManagerId, ManagerTaskStatus status, int count)
        {
            return SQLDataAccess.ExecuteReadList<ManagerTask>(
                    "SELECT TOP(@Count) * FROM Customers.ManagerTask WHERE AssignedManagerId = @AssignedManagerId and Status = @status ORDER BY DateCreated DESC", CommandType.Text,
                    GetManagerTaskFromReader, 
                    new SqlParameter("@Count", count),
                    new SqlParameter("@Status", (int)status),
                    new SqlParameter("@AssignedManagerId", assignedManagerId));
        }

        public static List<ManagerTask> GeTasksByLead(int leadId)
        {
            return SQLDataAccess.ExecuteReadList(
                    "SELECT * FROM Customers.ManagerTask WHERE LeadId = @LeadId", CommandType.Text,
                    GetManagerTaskFromReader, new SqlParameter("@LeadId", leadId));
        }

        public static int AddManagerTask(ManagerTask task)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "INSERT INTO Customers.ManagerTask (AssignedManagerId, AppointedManagerId, Name, Description, Status, DueDate, DateCreated, DateModified, OrderId, CustomerId,LeadId,ResultShort,ResultFull) " +
                "VALUES (@AssignedManagerId, @AppointedManagerId, @Name, @Description, @Status, @DueDate, GETDATE(), GETDATE(), @OrderId, @CustomerId,@LeadId,@ResultShort,@ResultFull); SELECT SCOPE_IDENTITY();", 
                CommandType.Text, 
                new SqlParameter("@AssignedManagerId", task.AssignedManagerId),
                new SqlParameter("@AppointedManagerId", task.AppointedManagerId),
                new SqlParameter("@Name", task.Name),
                new SqlParameter("@Description", task.Description),
                new SqlParameter("@Status", task.Status),
                new SqlParameter("@DueDate", task.DueDate),
                new SqlParameter("@OrderId", task.OrderId ?? (object)DBNull.Value),
                new SqlParameter("@CustomerId", task.CustomerId ?? (object)DBNull.Value),
                new SqlParameter("@LeadId", task.LeadId ?? (object)DBNull.Value),
                new SqlParameter("@ResultShort", task.ResultShort ?? string.Empty),
                new SqlParameter("@ResultFull", task.ResultFull ?? string.Empty));
        }

        public static void UpdateManagerTask(ManagerTask task)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE Customers.ManagerTask SET AssignedManagerId = @AssignedManagerId, AppointedManagerId = @AppointedManagerId, Name = @Name, Description = @Description, " +
                "Status = @Status, DueDate = @DueDate, DateModified = GETDATE(), OrderId = @OrderId, CustomerId = @CustomerId, LeadId=@LeadId, ResultShort=@ResultShort, ResultFull=@ResultFull " +
                "WHERE TaskId = @TaskId", CommandType.Text,
                new SqlParameter("@TaskId", task.TaskId),
                new SqlParameter("@AssignedManagerId", task.AssignedManagerId),
                new SqlParameter("@AppointedManagerId", task.AppointedManagerId),
                new SqlParameter("@Name", task.Name),
                new SqlParameter("@Description", task.Description),
                new SqlParameter("@Status", task.Status),
                new SqlParameter("@DueDate", task.DueDate),
                new SqlParameter("@OrderId", task.OrderId ?? (object)DBNull.Value),
                new SqlParameter("@CustomerId", task.CustomerId ?? (object)DBNull.Value),
                new SqlParameter("@LeadId", task.LeadId ?? (object)DBNull.Value),
                new SqlParameter("@ResultShort", task.ResultShort ?? string.Empty),
                new SqlParameter("@ResultFull", task.ResultFull ?? string.Empty));
        }

        public static void DeleteManagerTask(int taskId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM Customers.ManagerTask WHERE TaskId = @TaskId", CommandType.Text,
                new SqlParameter("@TaskId", taskId));
        }

        public static void ChangeTaskStatus(int taskId, ManagerTaskStatus status)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE Customers.ManagerTask SET Status = @Status WHERE TaskId = @TaskId", CommandType.Text,
                new SqlParameter("@Status", (int)status), new SqlParameter("@TaskId", taskId));
        }

        public static int GetManagerTasksCount(ManagerTaskStatus status)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM Customers.ManagerTask WHERE Status = @Status",
                    CommandType.Text,
                    new SqlParameter("@Status", (int)status));
        }

        public static int GetManagerTasksCountByManagerId(ManagerTaskStatus status, int assignedManagerId)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM [Customers].[ManagerTask] WHERE Status = @Status AND AssignedManagerId = @assignedManagerId",
                    CommandType.Text,
                    new SqlParameter("@Status", (int)status),
                    new SqlParameter("@assignedManagerId", (int)assignedManagerId)
            );
        }

        public static void OnSetManagerTask(ManagerTask task)
        {
            var mailTemplate = new SetManagerTaskMailTemplate(
                task.AssignedManager.FirstName + " " + task.AssignedManager.LastName,
                task.AppointedManager.FirstName + " " + task.AppointedManager.LastName,
                task.Name, task.Description, task.Status.Localize(), task.DueDate.ToString("dd.MM.yyyy"));
            mailTemplate.BuildMail();

            SendMail.SendMailNow(task.AssignedManager.CustomerId, task.AssignedManager.Email, mailTemplate.Subject, mailTemplate.Body, true);
        }

        public static void OnChangeManagerTaskStatus(ManagerTask task)
        {
            var mailTemplate = new ChangeManagerTaskStatusMailTemplate(
                task.AssignedManager.FirstName + " " + task.AssignedManager.LastName,
                task.AppointedManager.FirstName + " " + task.AppointedManager.LastName,
                task.Name, task.Description, task.Status.Localize(), task.DueDate.ToString("dd.MM.yyyy"));
            mailTemplate.BuildMail();

            SendMail.SendMailNow(task.AssignedManager.CustomerId, task.AssignedManager.Email, mailTemplate.Subject, mailTemplate.Body, true);
            SendMail.SendMailNow(task.AppointedManager.CustomerId, task.AppointedManager.Email, mailTemplate.Subject, mailTemplate.Body, true);
        }
    }
}