using System;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Localization;

namespace AdvantShop.Web.Admin.Models.Tasks
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int TaskGroupId { get; set; }
        public string TaskGroupName { get; set; }

        /// <summary>
        /// Постановщик
        /// </summary>
        public int? AppointedManagerId { get; set; }
        public Guid? AppointedCustomerId { get; set; }
        public string AppointedName { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public int? AssignedManagerId { get; set; }
        public Guid? AssignedCustomerId { get; set; }
        public string AssignedName { get; set; }

        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public bool Accepted { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime DateCreated { get; set; }

        public int? OrderId { get; set; }
        public string OrderNumber { get; set; }

        public Guid? ClientCustomerId { get; set; }
        public string ClientName { get; set; }

        public int? LeadId { get; set; }

        public int? ReviewId { get; set; }

        public string ResultShort { get; set; }
        public string ResultFull { get; set; }

        public DateTime DateAppointed { get; set; }
        public bool IsAutomatic { get; set; }
        public bool IsDeferred { get; set; }

        public string AppointedCustomerAvatar { get; set; }
        public string AppointedCustomerAvatarSrc
        {
            get { return AppointedCustomerAvatar.IsNotEmpty() ? FoldersHelper.GetPath(FolderType.Avatar, AppointedCustomerAvatar, false) : null; }
        }

        public string AssignedCustomerAvatar { get; set; }
        public string AssignedCustomerAvatarSrc
        {
            get { return AssignedCustomerAvatar.IsNotEmpty() ? FoldersHelper.GetPath(FolderType.Avatar, AssignedCustomerAvatar, false) : null; }
        }

        public string StatusString { get { return Status.ToString().ToLower(); } }
        public string StatusFormatted { get { return Status == TaskStatus.Completed && Accepted ? LocalizationService.GetResource("Admin.Tasks.TaskModel.Accepted") : Status.Localize(); } }
        public string PriorityFormatted { get { return Priority.Localize(); } }
        public string DueDateFormatted { get { return DueDate.HasValue ? Culture.ConvertDateWithoutSeconds(DueDate.Value) : string.Empty; } }
        public string DateCreatedFormatted { get { return Culture.ConvertShortDate(DateCreated); } }
        public string DateCreatedFormattedFull { get { return DateCreated.ToString("dd MMMM yyyy HH:mm"); } }

        //public bool CanDelete { get; set; }
        public DateTime? ViewDate { get; set; }
        public bool Viewed { get; set; }
        public int NewCommentsCount { get; set; }

        public bool Completed { get { return Status == TaskStatus.Completed; } }
        public bool Overdue { get { return !Completed && DueDate.HasValue && DueDate.Value < DateTime.Now; } }
        public bool InProgress { get { return Status == TaskStatus.InProgress; } }

        public static explicit operator TaskModel(Task task)
        {
            return new TaskModel
            {
                Id = task.Id,
                TaskGroupId = task.TaskGroupId,
                AssignedManagerId = task.AssignedManagerId,
                AppointedManagerId = task.AppointedManagerId,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status,
                Accepted = task.Accepted,
                Priority = task.Priority,
                DueDate = task.DueDate,
                LeadId = task.LeadId,
                OrderId = task.OrderId,
                OrderNumber = task.Order != null ? task.Order.Number : null,
                ReviewId = task.ReviewId,
                ClientCustomerId = task.CustomerId,
                ClientName = task.ClientCustomer != null ? task.ClientCustomer.GetFullName() : null,
                ResultShort = task.ResultShort,
                ResultFull = task.ResultFull,
                DateCreated = task.DateCreated,
            };
        }
    }
}
