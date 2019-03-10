using System;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Trial;
using AdvantShop.Web.Admin.Models.Tasks;

namespace AdvantShop.Web.Admin.Handlers.Tasks
{
    public class AddTaskHandler
    {
        private readonly TaskModel _model;

        public AddTaskHandler(TaskModel model)
        {
            _model = model;
        }

        public Task Execute()
        {
            var task = new Task()
            {
                Name = _model.Name.DefaultOrEmpty(),
                AssignedManagerId = _model.AssignedManagerId,
                AppointedManagerId = _model.AppointedManagerId,
                DueDate = _model.DueDate,
                Description = _model.Description.DefaultOrEmpty().Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>"),
                TaskGroupId = _model.TaskGroupId == 0 ? SettingsTasks.DefaultTaskGroup : _model.TaskGroupId,
                Priority = _model.Priority,
                Status = TaskStatus.Open,
                Accepted = false,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                OrderId = _model.OrderId,
                LeadId = _model.LeadId,
                ReviewId = _model.ReviewId,
                CustomerId = _model.ClientCustomerId,
                DateAppointed = _model.DateAppointed,
                IsAutomatic = _model.IsAutomatic,
                IsDeferred = _model.IsDeferred,
            };

            task.Id = TaskService.AddTask(task);

            TrialService.TrackEvent(ETrackEvent.Trial_AddTask);

            return task;
        }
    }
}
