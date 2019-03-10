using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Models.Tasks;
using AdvantShop.Web.Admin.ViewModels.Tasks;

namespace AdvantShop.Web.Admin.Handlers.Tasks
{
    public class GetIndexModel
    {
        private readonly TasksFilterModel _filter;

        public GetIndexModel(TasksFilterModel filter)
        {
            _filter = filter;
        }

        public TasksListViewModel Execute()
        {
            var model = new TasksListViewModel();
            if (_filter != null)
            {
                model.PreFilter = _filter.FilterBy;

                model.UseKanban = CommonHelper.GetCookieString("tasks_viewmode") == "kanban";
                model.KanbanMyTasks = CommonHelper.GetCookieString("tasks_mykanban").TryParseBool();
                TaskGroup taskGroup = null;
                if (_filter.TaskGroupId.HasValue && (taskGroup = TaskGroupService.GetTaskGroup(_filter.TaskGroupId.Value)) != null)
                {
                    model.TaskGroupId = taskGroup.Id;
                }
                model.Title = taskGroup != null ? taskGroup.Name : LocalizationService.GetResource("Admin.Tasks.Index.Title");
            }
            return model;
        }
    }
}
