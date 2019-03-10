using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.TaskGroups;
using AdvantShop.Web.Admin.Models;
using AdvantShop.Web.Admin.Models.TaskGroups;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Tasks)]
    public partial class TaskGroupsController : BaseAdminController
    {
        public ActionResult Index(TaskGroupsFilterModel filter)
        {
            var model = new GetIndexModel(filter).Execute();

            SetMetaInformation(T("Admin.TaskGroups.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.TaskGroupsCtrl);

            return View(model);
        }

        public JsonResult GetTaskGroups(TaskGroupsFilterModel model)
        {
            var handler = new GetTaskGroupsHandler(model);
            var result = handler.Execute();

            return Json(result);
        }

        public JsonResult GetTaskGroup(int id)
        {
            var dbModel = TaskGroupService.GetTaskGroup(id);
            if (dbModel == null)
                return JsonError();

            return ProcessJsonResult(new GetTaskGroupModel(dbModel));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddTaskGroup(TaskGroupModel model)
        {
            if (model.Name.IsNullOrEmpty())
                return JsonError();
            var id = TaskGroupService.AddTaskGroup(new TaskGroup
            {
                Name = model.Name.DefaultOrEmpty(),
                SortOrder = model.SortOrder
            });
            return JsonOk(id);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateTaskGroup(TaskGroupModel model)
        {
            if (model.Name.IsNullOrEmpty())
                return JsonError();

            var dbModel = TaskGroupService.GetTaskGroup(model.Id);
            if (dbModel == null)
                return JsonError();

            dbModel.Name = model.Name.DefaultOrEmpty();
            dbModel.SortOrder = model.SortOrder;

            TaskGroupService.UpdateTaskGroup(dbModel);

            return JsonOk(new { id = dbModel.Id });
        }

        #region Inplace

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult InplaceTaskGroup(TaskGroupModel model)
        {
            if (model.Id != 0)
            {
                var taskGroup = TaskGroupService.GetTaskGroup(model.Id);
                if (taskGroup == null)
                    return Json(new { result = false });
                taskGroup.SortOrder = model.SortOrder;
                TaskGroupService.UpdateTaskGroup(taskGroup);
            }

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteTaskGroup(int id)
        {
            TaskGroupService.DeleteTaskGroup(id);
            return Json(new { result = true });
        }

        #endregion

        #region Commands

        private void Command(TaskGroupsFilterModel command, Func<int, TaskGroupsFilterModel, bool> func)
        {
            if (command.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in command.Ids)
                {
                    func(id, command);
                }
            }
            else
            {
                var handler = new GetTaskGroupsHandler(command);
                var ids = handler.GetItemsIds("Id");

                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteTaskGroups(TaskGroupsFilterModel command)
        {
            Command(command, (id, c) =>
            {
                TaskGroupService.DeleteTaskGroup(id);
                return true;
            });
            return Json(true);
        }

        #endregion

        public JsonResult GetTaskGroupsSelectOptions()
        {
            var taskGroups = TaskGroupService.GetAllTaskGroups();
            return Json(taskGroups.Select(x => new SelectItemModel(x.Name, x.Id.ToString())));
        }
    }
}
