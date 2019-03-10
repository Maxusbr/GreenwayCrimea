using System.Collections.Generic;
using AdvantShop.Web.Admin.Models.Tasks;

namespace AdvantShop.Web.Admin.ViewModels.Tasks
{
    public class TasksListViewModel
    {
        public TasksListViewModel()
        {
            PreFilterTypes = new List<TasksPreFilterType>
            {
                TasksPreFilterType.AssignedToMe,
                TasksPreFilterType.None,
                TasksPreFilterType.AppointedByMe,
                TasksPreFilterType.Completed,
                TasksPreFilterType.Accepted
            };
        }

        public bool UseKanban { get; set; }
        public bool KanbanMyTasks { get; set; }
        public string Title { get; set; }
        public int? TaskGroupId { get; set; }
        public TasksPreFilterType PreFilter { get; set; }
        public List<TasksPreFilterType> PreFilterTypes { get; set; }
    }
}
