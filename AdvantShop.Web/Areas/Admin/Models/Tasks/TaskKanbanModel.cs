using System;
using System.Collections.Generic;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Scheduler;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.FilePath;
using AdvantShop.Localization;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Tasks
{
    public enum ETasksKanbanColumn
    {
        None = 0,
        [Localize("Новые")]
        New = 1,
        [Localize("В работе")]
        InProgress = 2,
        [Localize("Сделаны")]
        Done = 3,
        [Localize("Приняты")]
        Accepted = 4
    }

    public class TasksKanbanModel : KanbanModel<TaskKanbanModel> 
    {

    }

    public class TasksKanbanColumnModel : KanbanColumnModel<TaskKanbanModel>
    {

    }

    public class TaskKanbanModel : KanbanCardModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int TaskGroupId { get; set; }
        public string TaskGroupName { get; set; }

        /// <summary>
        /// Постановщик
        /// </summary>
        public Guid? AppointedCustomerId { get; set; }
        public string AppointedName { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public Guid? AssignedCustomerId { get; set; }
        public string AssignedName { get; set; }

        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public bool Accepted { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }

        public DateTime DateAppointed { get; set; }

        public string AppointedCustomerAvatar { get; set; }
        public string AppointedCustomerAvatarSrc
        {
            get
            {
                return AppointedCustomerAvatar.IsNotEmpty() 
                    ? string.Format("{0}?rnd={1}", FoldersHelper.GetPath(FolderType.Avatar, AppointedCustomerAvatar, false), new Random().Next()) 
                    : UrlService.GetAdminStaticUrl() + "images/no-avatar.jpg";
            }
        }

        public string AssignedCustomerAvatar { get; set; }
        public string AssignedCustomerAvatarSrc
        {
            get
            {
                return AssignedCustomerAvatar.IsNotEmpty() 
                    ? string.Format("{0}?rnd={1}", FoldersHelper.GetPath(FolderType.Avatar, AssignedCustomerAvatar, false), new Random().Next())
                    : UrlService.GetAdminStaticUrl() + "images/no-avatar.jpg";
            }
        }

        public string StatusString { get { return Status.ToString().ToLower(); } }
        public string StatusFormatted { get { return Status == TaskStatus.Completed && Accepted ? LocalizationService.GetResource("Admin.Tasks.TaskModel.Accepted") : Status.Localize(); } }
        public string PriorityFormatted { get { return Priority.Localize(); } }
        public string DueDateFormatted { get { return DueDate.HasValue ? Culture.ConvertDateWithoutSeconds(DueDate.Value) : string.Empty; } }
        public string DateAppointedFormattedFull
        {
            get { return DateAppointed.Year == DateTime.Now.Year ? DateAppointed.ToString("dd MMMM HH:mm") : DateAppointed.ToString("dd MMMM yyyy HH:mm"); }
        }
        public string DueDateInFormatted
        {
            get
            {
                if (!DueDate.HasValue)
                    return string.Empty;

                TimeInterval ti;
                var datesRange = (DueDate.Value - DateTime.Now).Duration();
                if (datesRange.TotalDays > 1)
                    ti = new TimeInterval() { Interval = (int)Math.Ceiling(datesRange.TotalDays), IntervalType = TimeIntervalType.Days };
                else if (datesRange.TotalHours > 1)
                    ti = new TimeInterval() { Interval = (int)Math.Ceiling(datesRange.TotalHours), IntervalType = TimeIntervalType.Hours };
                else
                    ti = new TimeInterval() { Interval = (int)Math.Ceiling(datesRange.TotalMinutes), IntervalType = TimeIntervalType.Minutes };

                return string.Format("{0} {1}", ti.Interval, ti.Numeral("минут"));
            }
        }

        //public bool CanDelete { get; set; }
        public DateTime? ViewDate { get; set; }
        public bool Viewed { get; set; }
        public int NewCommentsCount { get; set; }

        public bool Completed { get { return Status == TaskStatus.Completed; } }
        public bool Overdue { get { return !Completed && DueDate.HasValue && DueDate.Value < DateTime.Now; } }
        public bool InProgress { get { return Status == TaskStatus.InProgress; } }
    }
}
