using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AdvantShop.Core.SQL;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Core.Services.Attachments;
using AdvantShop.CMS;
using AdvantShop.Customers;
using System.Text;
using AdvantShop.Localization;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Core.Services.Crm
{
    public class TaskService
    {
        private static Task GetTaskFromReader(SqlDataReader reader)
        {
            return new Task
            {
                Id = SQLDataHelper.GetInt(reader, "Id"),
                TaskGroupId = SQLDataHelper.GetInt(reader, "TaskGroupId"),
                AssignedManagerId = SQLDataHelper.GetNullableInt(reader, "AssignedManagerId"),
                AppointedManagerId = SQLDataHelper.GetNullableInt(reader, "AppointedManagerId"),
                Name = SQLDataHelper.GetString(reader, "Name"),
                Description = SQLDataHelper.GetString(reader, "Description"),
                Status = (TaskStatus)SQLDataHelper.GetInt(reader, "Status"),
                Accepted = SQLDataHelper.GetBoolean(reader, "Accepted"),
                Priority = (TaskPriority)SQLDataHelper.GetInt(reader, "Priority"),
                DueDate = SQLDataHelper.GetNullableDateTime(reader, "DueDate"),
                LeadId = SQLDataHelper.GetNullableInt(reader, "LeadId"),
                OrderId = SQLDataHelper.GetNullableInt(reader, "OrderId"),
                ReviewId = SQLDataHelper.GetNullableInt(reader, "ReviewId"),
                CustomerId = SQLDataHelper.GetNullableGuid(reader, "CustomerId"),
                ResultShort = SQLDataHelper.GetString(reader, "ResultShort"),
                ResultFull = SQLDataHelper.GetString(reader, "ResultFull"),
                DateCreated = SQLDataHelper.GetDateTime(reader, "DateCreated"),
                DateModified = SQLDataHelper.GetDateTime(reader, "DateModified"),
                DateAppointed = SQLDataHelper.GetDateTime(reader, "DateAppointed"),
                IsAutomatic = SQLDataHelper.GetBoolean(reader, "IsAutomatic"),
                IsDeferred = SQLDataHelper.GetBoolean(reader, "IsDeferred"),
                SortOrder = SQLDataHelper.GetInt(reader, "SortOrder")
            };
        }

        public static Task GetTask(int taskId, int? managerId = null)
        {
            var task = SQLDataAccess.ExecuteReadOne(
                    "SELECT * FROM Customers.Task WHERE Id = @Id", CommandType.Text,
                    GetTaskFromReader, new SqlParameter("@Id", taskId));
            if (task != null && managerId.HasValue)
                SetTaskViewed(taskId, managerId.Value);
            return task;
        }

        public static IEnumerable<Task> GetAllTasks()
        {
            return SQLDataAccess.ExecuteReadIEnumerable<Task>(
                "SELECT * FROM Customers.Task", CommandType.Text, GetTaskFromReader);
        }

        public static List<Task> GetDeferredTasks()
        {
            return SQLDataAccess.Query<Task>(
                "SELECT * FROM Customers.Task WHERE IsDeferred = 1 AND DateAppointed <= @dateTo", new { dateTo = DateTime.Now }).ToList();
        }

        public static List<Task> GetTasksForAutocomplete(string query)
        {

            if (query.IsDecimal())
            {
                return SQLDataAccess.Query<Task>(
                   "SELECT * FROM [Customers].[Task] " +
                   "WHERE convert(nvarchar,[id]) =  @q", new { q = query }).ToList();
            }
            else
            {
                var translitKeyboard = StringHelper.TranslitToRusKeyboard(query);

                return SQLDataAccess.Query<Task>(
                    "SELECT * FROM [Customers].[Task] " +
                    "WHERE (convert(nvarchar,[id]) = @q " +
                    "OR [Name] LIKE '%' + @q + '%' OR [Name] like '%' + @qtr + '%') AND IsDeferred = 0", new { q = query, qtr = translitKeyboard }).ToList();
            }
        }

        public static int AddTask(Task task)
        {
            task.Id = SQLDataAccess.ExecuteScalar<int>(
                "INSERT INTO Customers.Task " +
                "(TaskGroupId, AssignedManagerId, AppointedManagerId, Name, Description, Status, Accepted, Priority, DueDate, LeadId, OrderId, CustomerId, ResultShort, ResultFull, " +
                "DateCreated, DateModified, DateAppointed, IsAutomatic, IsDeferred, ReviewId, SortOrder) " +
                "VALUES (@TaskGroupId, @AssignedManagerId, @AppointedManagerId, @Name, @Description, @Status, @Accepted, @Priority, @DueDate, @LeadId, @OrderId, @CustomerId, @ResultShort, @ResultFull, " +
                "GETDATE(), GETDATE(), @DateAppointed, @IsAutomatic, @IsDeferred, @ReviewId, " +
                "(SELECT ISNULL(MAX(SortOrder), 0) + 5 FROM Customers.Task) " +
                "); SELECT SCOPE_IDENTITY();",
                CommandType.Text,
                new SqlParameter("@TaskGroupId", task.TaskGroupId),
                new SqlParameter("@AssignedManagerId", task.AssignedManagerId ?? (object)DBNull.Value),
                new SqlParameter("@AppointedManagerId", task.AppointedManagerId ?? (object)DBNull.Value),
                new SqlParameter("@Name", task.Name),
                new SqlParameter("@Description", task.Description ?? string.Empty),
                new SqlParameter("@Status", task.Status),
                new SqlParameter("@Accepted", task.Accepted),
                new SqlParameter("@Priority", task.Priority),
                new SqlParameter("@DueDate", task.DueDate ?? (object)DBNull.Value),
                new SqlParameter("@LeadId", task.LeadId ?? (object)DBNull.Value),
                new SqlParameter("@OrderId", task.OrderId ?? (object)DBNull.Value),
                new SqlParameter("@CustomerId", task.CustomerId ?? (object)DBNull.Value),
                new SqlParameter("@ResultShort", task.ResultShort ?? string.Empty),
                new SqlParameter("@ResultFull", task.ResultFull ?? string.Empty),
                new SqlParameter("@DateAppointed", task.DateAppointed),
                new SqlParameter("@IsAutomatic", task.IsAutomatic),
                new SqlParameter("@IsDeferred", task.IsDeferred),
                new SqlParameter("@ReviewId", task.ReviewId ?? (object)DBNull.Value)
                );

            if (task.AppointedManagerId.HasValue)
                SetTaskViewed(task.Id, task.AppointedManagerId.Value);

            return task.Id;
        }

        public static void UpdateTask(Task task)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE Customers.Task SET TaskGroupId=@TaskGroupId, AssignedManagerId=@AssignedManagerId, AppointedManagerId=@AppointedManagerId, Name=@Name, Description=@Description, " +
                "Status=@Status, Accepted=@Accepted, Priority=@Priority, DueDate=@DueDate, LeadId=@LeadId, OrderId=@OrderId, CustomerId=@CustomerId, ResultShort=@ResultShort, ResultFull=@ResultFull, " +
                "DateModified=GETDATE(), DateAppointed=@DateAppointed, IsAutomatic=@IsAutomatic, IsDeferred=@IsDeferred, ReviewId=@ReviewId, SortOrder=@SortOrder " +
                "WHERE Id = @Id", CommandType.Text,
                new SqlParameter("@Id", task.Id),
                new SqlParameter("@TaskGroupId", task.TaskGroupId),
                new SqlParameter("@AssignedManagerId", task.AssignedManagerId ?? (object)DBNull.Value),
                new SqlParameter("@AppointedManagerId", task.AppointedManagerId ?? (object)DBNull.Value),
                new SqlParameter("@Name", task.Name),
                new SqlParameter("@Description", task.Description ?? string.Empty),
                new SqlParameter("@Status", task.Status),
                new SqlParameter("@Accepted", task.Accepted),
                new SqlParameter("@Priority", task.Priority),
                new SqlParameter("@DueDate", task.DueDate ?? (object)DBNull.Value),
                new SqlParameter("@LeadId", task.LeadId ?? (object)DBNull.Value),
                new SqlParameter("@OrderId", task.OrderId ?? (object)DBNull.Value),
                new SqlParameter("@CustomerId", task.CustomerId ?? (object)DBNull.Value),
                new SqlParameter("@ResultShort", task.ResultShort ?? string.Empty),
                new SqlParameter("@ResultFull", task.ResultFull ?? string.Empty),
                new SqlParameter("@DateAppointed", task.DateAppointed),
                new SqlParameter("@IsAutomatic", task.IsAutomatic),
                new SqlParameter("@IsDeferred", task.IsDeferred),
                new SqlParameter("@ReviewId", task.ReviewId ?? (object)DBNull.Value),
                new SqlParameter("@SortOrder", task.SortOrder)
                );
        }

        public static bool CanDeleteTask(int taskId, int managerId)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                "SELECT AppointedManagerId FROM Customers.Task WHERE Id = @Id", CommandType.Text,
                new SqlParameter("@Id", taskId)) == managerId;
        }

        public static void DeleteTask(int id)
        {
            var task = GetTask(id);
            if (task == null)
                return;
            OnTaskDeleted(CustomerContext.CurrentCustomer, task);
            AttachmentService.DeleteAttachments<TaskAttachment>(id);
            AdminCommentService.DeleteAdminComments(id, AdminCommentType.Task);

            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM Customers.Task WHERE Id = @Id", CommandType.Text,
                new SqlParameter("@Id", id));
        }

        public static void ChangeTaskStatus(int id, TaskStatus status)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE Customers.Task SET Status = @Status WHERE Id = @Id", CommandType.Text,
                new SqlParameter("@Status", (int)status), new SqlParameter("@Id", id));
        }

        public static int GetTasksCountByManagerId(TaskStatus status, int assignedManagerId)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM [Customers].[Task] WHERE Status = @Status AND AssignedManagerId = @AssignedManagerId AND IsDeferred = 0",
                    CommandType.Text,
                    new SqlParameter("@Status", (int)status),
                    new SqlParameter("@AssignedManagerId", assignedManagerId)
            );
        }

        public static int GetNotViewedTasksCount(int assignedManagerId)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                    "SELECT COUNT(Id) FROM Customers.Task LEFT JOIN Customers.ViewedTask ON Task.Id = ViewedTask.TaskId AND ViewedTask.ManagerId = @AssignedManagerId " +
                    "WHERE ViewDate IS NULL AND (Status = @StatusOpen OR Status = @StatusInProgress) AND IsDeferred = 0",
                    CommandType.Text,
                    new SqlParameter("@AssignedManagerId", assignedManagerId),
                    new SqlParameter("@StatusOpen", TaskStatus.Open),
                    new SqlParameter("@StatusInProgress", TaskStatus.InProgress)
            );
        }

        public static int GetOpenTasksCount(int assignedManagerId)
        {
            return SQLDataAccess.ExecuteScalar<int>(
                    "SELECT COUNT(Id) FROM Customers.Task " +
                    "WHERE AssignedManagerId = @AssignedManagerId AND (Status = @StatusOpen OR Status = @StatusInProgress) AND IsDeferred = 0",
                    CommandType.Text,
                    new SqlParameter("@AssignedManagerId", assignedManagerId),
                    new SqlParameter("@StatusOpen", TaskStatus.Open),
                    new SqlParameter("@StatusInProgress", TaskStatus.InProgress)
            );
        }

        public static void SetTaskViewed(int taskId, int managerId, bool viewed = true)
        {
            if (!viewed)
            {
                SQLDataAccess.ExecuteNonQuery(
                    "DELETE FROM Customers.ViewedTask WHERE TaskId = @TaskId AND ManagerId = @ManagerId",
                    CommandType.Text, new SqlParameter("@TaskId", taskId), new SqlParameter("@ManagerId", managerId));
            }
            else
            {
                SQLDataAccess.ExecuteNonQuery(
                    @"IF(SELECT COUNT(TaskId) FROM Customers.ViewedTask WHERE TaskId = @TaskId AND ManagerId = @ManagerId) > 0
	                 UPDATE Customers.ViewedTask SET ViewDate = GETDATE() WHERE TaskId = @TaskId AND ManagerId = ManagerId
                 ELSE
	                 INSERT INTO Customers.ViewedTask (TaskId, ManagerId, ViewDate) VALUES (@TaskId, @ManagerId, GETDATE())",
                    CommandType.Text, new SqlParameter("@TaskId", taskId), new SqlParameter("@ManagerId", managerId));
            }
        }

        public static void SetAllTasksViewed(int managerId, bool viewed = true)
        {
            if (!viewed)
            {
                SQLDataAccess.ExecuteNonQuery(
                    "DELETE FROM Customers.ViewedTask WHERE ManagerId = @ManagerId",
                    CommandType.Text, new SqlParameter("@ManagerId", managerId));
            }
            else
            {
                SQLDataAccess.ExecuteNonQuery(
                    @"INSERT INTO Customers.ViewedTask (TaskId, ManagerId, ViewDate) " +
                    "SELECT Id, @ManagerId, GETDATE() FROM Customers.Task WHERE Id NOT IN (SELECT Id FROM Customers.ViewedTask WHERE ManagerId = @ManagerId) AND IsDeferred = 0",
                    CommandType.Text, new SqlParameter("@ManagerId", managerId));
            }
        }

        public static void SetTaskAccepted(int id)
        {
            SQLDataAccess.ExecuteNonQuery(
                "UPDATE Customers.Task SET Status = @Status, Accepted = 1 WHERE Id = @Id",
                CommandType.Text, new SqlParameter("@Id", id), new SqlParameter("@Status", TaskStatus.Completed));
        }

        public static List<Task> GetTaskByAssignedManagerId(int id)
        {
            return SQLDataAccess.ExecuteReadList<Task>(
                  "SELECT * FROM Customers.Task where AssignedManagerId=@AssignedManagerId", CommandType.Text, GetTaskFromReader, new SqlParameter("@AssignedManagerId", id));
        }

        public static List<Task> GetTaskByAppointedManagerId(int id)
        {
            return SQLDataAccess.ExecuteReadList<Task>(
                  "SELECT * FROM Customers.Task where AppointedManagerId=@AppointedManagerId", CommandType.Text, GetTaskFromReader, new SqlParameter("@AppointedManagerId", id));
        }

        public static List<Task> GetTaskByCustomerId(Guid customerId)
        {
            return SQLDataAccess.ExecuteReadList<Task>(
                  "SELECT * FROM Customers.Task where CustomerId=@CustomerId", CommandType.Text, GetTaskFromReader, new SqlParameter("@CustomerId", customerId));
        }

        #region Task Events

        public static void OnTaskCreated(Task task)
        {
            if (task.IsDeferred)
                return;
            var mailTemplate = new TaskMailTemplate(
                task.Id.ToString(), task.TaskGroup != null ? task.TaskGroup.Name : string.Empty,
                task.Name, task.Description, task.Status.Localize(), task.Priority.Localize(),
                task.DueDate.HasValue ? task.DueDate.Value.ToString("dd.MM.yyyy") : "-",
                task.DateAppointed.ToString("dd.MM.yyyy HH:mm"),
                task.Attachments.Select(x => GetAttachmentLinkHTML(x.Path, x.FileName)).DefaultIfEmpty("-").AggregateString(", "),
                task.AssignedManager != null ? task.AssignedManager.FullName : string.Empty,
                task.AssignedManager != null ? task.AssignedManager.CustomerId.ToString() : string.Empty,
                task.AppointedManager != null ? task.AppointedManager.FullName : string.Empty,
                task.AppointedManager != null ? task.AppointedManager.CustomerId.ToString() : string.Empty);

            mailTemplate.BuildMail();

            SendMail.SendMailNow(task.AssignedManager.CustomerId, task.AssignedManager.Email, mailTemplate.Subject, mailTemplate.Body, true);
        }

        public static void OnTaskDeleted(Customer modifier, Task task)
        {
            if (task.IsDeferred)
                return;

            var mailTemplate = new TaskDeletedMailTemplate(
                modifier.FirstName + " " + modifier.LastName, modifier.Id.ToString(),
                task.Id.ToString(), task.TaskGroup != null ? task.TaskGroup.Name : string.Empty,
                task.Name, task.Description, task.Status.Localize(), task.Priority.Localize(),
                task.DueDate.HasValue ? task.DueDate.Value.ToString("dd.MM.yyyy") : "-",
                task.DateAppointed.ToString("dd.MM.yyyy HH:mm"),
                task.Attachments.Select(x => GetAttachmentLinkHTML(x.Path, x.FileName)).DefaultIfEmpty("-").AggregateString(", "),
                task.AssignedManager != null ? task.AssignedManager.FullName : string.Empty,
                task.AssignedManager != null ? task.AssignedManager.CustomerId.ToString() : string.Empty,
                task.AppointedManager != null ? task.AppointedManager.FullName : string.Empty,
                task.AppointedManager != null ? task.AppointedManager.CustomerId.ToString() : string.Empty);

            mailTemplate.BuildMail();

            var customersToNotify = new Dictionary<Guid, Customer>();
            if (task.AssignedManager != null)
                customersToNotify.TryAddValue(task.AssignedManager.CustomerId, task.AssignedManager.Customer);
            if (task.AppointedManager != null)
                customersToNotify.TryAddValue(task.AppointedManager.CustomerId, task.AppointedManager.Customer);

            SendMails(mailTemplate, customersToNotify);
        }

        public static void OnTaskCommentAdded(AdminComment comment, Task task)
        {
            if (task.IsDeferred)
                return;
            var mailTemplate = new TaskCommentAddedMailTemplate(
                comment.Name, comment.CustomerId.ToString(), comment.Text,
                task.Id.ToString(), task.TaskGroup != null ? task.TaskGroup.Name : string.Empty,
                task.Name, task.Description, task.Status.Localize(), task.Priority.Localize(),
                task.DueDate.HasValue ? task.DueDate.Value.ToString("dd.MM.yyyy") : "-",
                task.DateAppointed.ToString("dd.MM.yyyy HH:mm"),
                task.Attachments.Select(x => GetAttachmentLinkHTML(x.Path, x.FileName)).DefaultIfEmpty("-").AggregateString(", "),
                task.AssignedManager != null ? task.AssignedManager.FullName : string.Empty,
                task.AssignedManager != null ? task.AssignedManager.CustomerId.ToString() : string.Empty,
                task.AppointedManager != null ? task.AppointedManager.FullName : string.Empty,
                task.AppointedManager != null ? task.AppointedManager.CustomerId.ToString() : string.Empty);

            mailTemplate.BuildMail();

            var customersToNotify = new Dictionary<Guid, Customer>();
            if (task.AssignedManager != null)
                customersToNotify.TryAddValue(task.AssignedManager.CustomerId, task.AssignedManager.Customer);
            if (task.AppointedManager != null)
                customersToNotify.TryAddValue(task.AppointedManager.CustomerId, task.AppointedManager.Customer);
            AdminComment parentComment;
            if (comment.ParentId.HasValue && (parentComment = AdminCommentService.GetAdminComment(comment.ParentId.Value)) != null && parentComment.Customer != null)
                customersToNotify.TryAddValue(parentComment.Customer.Id, parentComment.Customer);

            SendMails(mailTemplate, customersToNotify);
        }

        public static void OnTaskChanged(Customer modifier, Task old, Task modified, List<TaskAttachment> oldAttachments)
        {
            if (modified.IsDeferred)
                return;
            var changesTable = GenerateTaskChangesTable(modifier, old, modified, oldAttachments);
            if (changesTable.IsNullOrEmpty())
                return;
            var mailTemplate = new TaskChangedMailTemplate(changesTable, modifier.FirstName + " " + modifier.LastName, modifier.Id.ToString(),
                modified.Id.ToString(), modified.TaskGroup != null ? modified.TaskGroup.Name : string.Empty,
                old.Name, modified.Description, modified.Status.Localize(), modified.Priority.Localize(),
                modified.DueDate.HasValue ? modified.DueDate.Value.ToString("dd.MM.yyyy") : null,
                modified.DateAppointed.ToString("dd.MM.yyyy"),
                modified.Attachments.Select(x => GetAttachmentLinkHTML(x.Path, x.FileName)).AggregateString(", "),
                modified.AssignedManager != null ? modified.AssignedManager.FullName : string.Empty,
                modified.AssignedManager != null ? modified.AssignedManager.CustomerId.ToString() : string.Empty,
                modified.AppointedManager != null ? modified.AppointedManager.FullName : string.Empty,
                modified.AppointedManager != null ? modified.AppointedManager.CustomerId.ToString() : string.Empty);
            mailTemplate.BuildMail();

            var customersToNotify = new Dictionary<Guid, Customer>();
            if (modified.AssignedManager != null)
                customersToNotify.TryAddValue(modified.AssignedManager.CustomerId, modified.AssignedManager.Customer);
            if (modified.AppointedManager != null)
                customersToNotify.TryAddValue(modified.AppointedManager.CustomerId, modified.AppointedManager.Customer);
            // при смене постановщика или исполнителя оповестить предыдущего
            if (old.AssignedManager != null && old.AssignedManagerId != modified.AssignedManagerId)
                customersToNotify.TryAddValue(old.AssignedManager.CustomerId, old.AssignedManager.Customer);
            if (old.AppointedManager != null && old.AppointedManagerId != modified.AppointedManagerId)
                customersToNotify.TryAddValue(old.AppointedManager.CustomerId, old.AppointedManager.Customer);

            SendMails(mailTemplate, customersToNotify);
        }

        private static void SendMails(MailTemplate mailTemplate, Dictionary<Guid, Customer> customersToNotify)
        {
            foreach (var item in customersToNotify.Where(x => x.Key != CustomerContext.CustomerId && x.Value != null && x.Value.HasRoleAction(RoleAction.Tasks)))
            {
                SendMail.SendMailNow(item.Key, item.Value.EMail, mailTemplate.Subject, mailTemplate.Body, true);
            }
        }

        #region TaskChangesTable

        private static string GenerateTaskChangesTable(Customer modifier, Task old, Task modified, List<TaskAttachment> oldAttachments)
        {
            var sbRows = new StringBuilder();
            if (old.Name.DefaultOrEmpty() != modified.Name.DefaultOrEmpty())
                sbRows.Append(GetHtmlRow(LocalizationService.GetResource("Core.Services.Crm.Task.ChangesTable.Name"),
                    StringHelper.GenerateDiffHtml(old.Name, modified.Name)));
            if (old.AssignedManagerId != modified.AssignedManagerId)
                sbRows.Append(GetHtmlChangesRow(LocalizationService.GetResource("Core.Services.Crm.Task.ChangesTable.AssignedManager"),
                    old.AssignedManager != null ? old.AssignedManager.FullName : null,
                    modified.AssignedManager != null ? modified.AssignedManager.FullName : null));
            if (old.AppointedManagerId != modified.AppointedManagerId)
                sbRows.Append(GetHtmlChangesRow(LocalizationService.GetResource("Core.Services.Crm.Task.ChangesTable.AppointedManager"),
                    old.AppointedManager != null ? GetManagerLinkHTML(old.AppointedManager.FullName, old.AppointedManager.CustomerId) : string.Empty,
                    modified.AppointedManager != null ? GetManagerLinkHTML(modified.AppointedManager.FullName, modified.AppointedManager.CustomerId) : string.Empty));
            if (old.Status != modified.Status)
                sbRows.Append(GetHtmlChangesRow(LocalizationService.GetResource("Core.Services.Crm.Task.ChangesTable.Status"),
                    old.Status.Localize(), modified.Status.Localize()));
            else if (!old.Accepted && modified.Accepted)
                sbRows.Append(GetHtmlChangesRow(LocalizationService.GetResource("Core.Services.Crm.Task.ChangesTable.Status"),
                    old.Status.Localize(), LocalizationService.GetResource("Core.Services.Crm.Task.ChangesTable.Accepted")));
            if (old.ResultFull.DefaultOrEmpty() != modified.ResultFull.DefaultOrEmpty())
                sbRows.Append(GetHtmlChangesRow(LocalizationService.GetResource("Core.Services.Crm.Task.ChangesTable.Result"),
                    old.ResultFull, modified.ResultFull));
            if (old.DueDate != modified.DueDate)
                sbRows.Append(GetHtmlChangesRow(LocalizationService.GetResource("Core.Services.Crm.Task.ChangesTable.DueDate"),
                    old.DueDate.HasValue ? Culture.ConvertShortDate(old.DueDate.Value) : null, modified.DueDate.HasValue ? Culture.ConvertShortDate(modified.DueDate.Value) : null));
            if (old.Priority != modified.Priority)
                sbRows.Append(GetHtmlChangesRow(LocalizationService.GetResource("Core.Services.Crm.Task.ChangesTable.Priority"),
                    old.Priority.Localize(), modified.Priority.Localize()));

            var deletedAttachments = oldAttachments.Select(x => x.FileName).Where(x => !modified.Attachments.Select(y => y.FileName).Contains(x));
            var newAttachments = modified.Attachments.Where(x => !oldAttachments.Select(y => y.FileName).Contains(x.FileName)).Select(x => GetAttachmentLinkHTML(x.Path, x.FileName));
            if (deletedAttachments.Any() || newAttachments.Any())
                sbRows.Append(GetHtmlChangesRow(LocalizationService.GetResource("Core.Services.Crm.Task.ChangesTable.Attachments"),
                    deletedAttachments.AggregateString(", "), newAttachments.AggregateString(", ")));

            if (old.Description.DefaultOrEmpty() != modified.Description.DefaultOrEmpty())
                sbRows.AppendFormat("<tr><td colspan='2' style='padding: 10px 0;'>{0}</td></tr>", StringHelper.GenerateDiffHtml(old.Description, modified.Description));

            return GenerateTaskChangesTable(modifier, sbRows.ToString());
        }

        private static string GenerateTaskChangesTable(Customer modifier, string rowsHtml)
        {
            if (rowsHtml.IsNullOrEmpty())
                return string.Empty;

            return string.Format("<table><tr>{0}</tr>{1}</table>", GetHtmlRow(LocalizationService.GetResource("Core.Services.Crm.Task.ChangesTable.ModifiedBy"),
                GetManagerLinkHTML(modifier.FirstName + " " + modifier.LastName, modifier.Id)), rowsHtml);
        }

        private static string GetManagerLinkHTML(string name, Guid id)
        {
            if (id == Guid.Empty)
                return name;
            return string.Format("<a href='{0}{1}'>{2}</a>", UrlService.GetAdminUrl("customers/edit/"), id, name);
        }

        private static string GetHtmlRow(string fieldName, string value)
        {
            return string.Format("<tr><td style='color: #acacac; padding: 5px 15px 5px 0;'>{0}:</td><td>{1}</td></tr>",
                fieldName, value);
        }

        private static string GetHtmlChangesRow(string fieldName, string oldValue, string newValue)
        {
            var oldValueFormat = "<span style='background-color:#ffe7e7;text-decoration:line-through;'>{0}</span>";
            var newValueFormat = "<span style='background-color:#ddfade;'>{0}</span>";

            return GetHtmlRow(fieldName, string.Format("{0} {1}",
                oldValue.IsNotEmpty() ? string.Format(oldValueFormat, oldValue) : string.Empty,
                newValue.IsNotEmpty() ? string.Format(newValueFormat, newValue) : string.Empty));
        }

        private static string GetAttachmentLinkHTML(string url, string name)
        {
            return string.Format("<a href=\"{0}\">{1}</a>", url, name);
        }

        #endregion

        #endregion
    }
}