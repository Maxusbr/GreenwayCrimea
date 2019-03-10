using System;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Tasks
{
    public class TasksKanbanFilterModel : KanbanFilterModel<TasksKanbanColumnFilterModel>
    {
        public string Name { get; set; }

        public TaskPriority? Priority { get; set; }
        public TaskStatus? Status { get; set; }

        public int? AppointedManagerId { get; set; }
        public int? AssignedManagerId { get; set; }
        public int? TaskGroupId { get; set; }

        public bool? Accepted { get; set; }
        public bool? Viewed { get; set; }

        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }

        public DateTime? DateCreatedFrom { get; set; }
        public DateTime? DateCreatedTo { get; set; }

        public bool OnlyMy { get; set; }
    }

    public class TasksKanbanColumnFilterModel : KanbanColumnFilterModel
    {
        public TasksKanbanColumnFilterModel() : base() { }
        public TasksKanbanColumnFilterModel(string id) : base(id) { }

        public ETasksKanbanColumn Type
        {
            get { return Id.TryParseEnum<ETasksKanbanColumn>(); }
        }
    }

}
