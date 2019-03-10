using AdvantShop.Web.Admin.Models.TaskGroups;
using AdvantShop.Web.Admin.ViewModels.TaskGroups;

namespace AdvantShop.Web.Admin.Handlers.TaskGroups
{
    public class GetIndexModel
    {
        private readonly TaskGroupsFilterModel _filter;

        public GetIndexModel(TaskGroupsFilterModel filter)
        {
            _filter = filter;
        }

        public TaskGroupsListViewModel Execute()
        {
            var model = new TaskGroupsListViewModel();
            return model;
        }
    }
}
