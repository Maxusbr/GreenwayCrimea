using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Attachments;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Trial;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.AdminNotifications;
using AdvantShop.Web.Admin.Handlers.Attachments;
using AdvantShop.Web.Admin.Handlers.Tasks;
using AdvantShop.Web.Admin.Models;
using AdvantShop.Web.Admin.Models.Attachments;
using AdvantShop.Web.Admin.Models.Tasks;
using AdvantShop.Web.Admin.ViewModels.Tasks;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Tasks)]    
    public partial class TasksController : BaseAdminController
    {
        private TasksListViewModel GetTasksListViewModel(TasksFilterModel filter)
        {
            var model = new GetIndexModel(filter).Execute();

            SetMetaInformation(T("Admin.Tasks.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.TasksCtrl);

            // временно отключаем
            //if (!BizProcessRuleService.ExistBizProcessRules())
            //    ShowNotification(NotifyType.Notice, T("Admin.Tasks.BizProcessesNotSet"));

            TrialService.TrackEvent(ETrackEvent.Trial_VisitTasks);

            return model;
        }

        public ActionResult Index(TasksFilterModel filter)
        {
            return View(GetTasksListViewModel(filter));
        }

        public ActionResult Project(TasksFilterModel filter)
        {
            return View("Index", GetTasksListViewModel(filter));
        }

        public ActionResult View(int id)
        {
            return Redirect(Url.RouteUrl(new { controller = "Tasks", action = "Index"}) + "#?modal=" + id);
        }

        public JsonResult GetTasks(TasksFilterModel model)
        {
            var handler = new GetTasksHandler(model);
            var result = handler.Execute();

            return Json(result);
        }

        #region Kanban

        public JsonResult GetKanban(TasksKanbanFilterModel model)
        {
            var handler = new GetTasksKanbanHandler(model);
            var result = handler.Execute();

            return Json(result);
        }

        public JsonResult GetKanbanColumn(TasksKanbanFilterModel model)
        {
            var handler = new GetTasksKanbanHandler(model);
            var result = handler.GetCards();

            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeSorting(int id, int? prevId, int? nextId)
        {
            var handler = new ChangeTaskSorting(id, prevId, nextId);
            var result = handler.Execute();

            return Json(new { result = result });
        }

        #endregion

        public JsonResult GetTask(int id)
        {
            var manager = ManagerService.GetManager(CustomerContext.CustomerId);

            var task = TaskService.GetTask(id, manager == null ? (int?)null : manager.ManagerId);
            if (task == null || task.IsDeferred)
                return Json(new { result = false });

            var handler = new GetTaskModel(task);
            var result = handler.Execute();

            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddTask(TaskModel model)
        {
            var manager = ManagerService.GetManager(CustomerContext.CustomerId);
            if (manager == null)
                return Json(new { result = false });

            model.AppointedManagerId = manager.ManagerId;
            model.DateAppointed = DateTime.Now;
            model.IsAutomatic = false;
            model.IsDeferred = false;
            var task = new AddTaskHandler(model).Execute();
            if (task == null)
                return Json(new { result = false });
            var handler = new UploadAttachmentsHandler(task.Id);
            var result = handler.Execute<TaskAttachment>();

            new TaskAddedNotificationsHandler(task).Execute();

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult EditTask(TaskModel model)
        {
            var manager = ManagerService.GetManager(CustomerContext.CustomerId);
            if (manager == null)
                return Json(new { result = false });

            var result = new EditTaskHandler(model).Execute();

            return Json(new { result = result });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeTaskStatus(int id, TaskStatus status)
        {
            //var manager = ManagerService.GetManager(CustomerContext.CustomerId);
            //if (manager == null)
            //    return Json(new { result = false });

            var task = TaskService.GetTask(id);
            var model = (TaskModel)task;
            if (task.Accepted && status != TaskStatus.Completed)
            {
                model.Accepted = false;
                model.ResultFull = string.Empty;
                model.ResultShort = string.Empty;
            }
            model.Status = status;
            var result = new EditTaskHandler(model).Execute();

            return Json(new { result = result, status = model.StatusString });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult CompleteTask(int id, string taskResult, int? orderStatusId, int? dealStatusId)
        {
            //var manager = ManagerService.GetManager(CustomerContext.CustomerId);
            //if (manager == null)
            //    return Json(new { result = false });

            var task = TaskService.GetTask(id);
            var model = (TaskModel)task;
            model.Status = TaskStatus.Completed;
            model.ResultFull = taskResult;
            var result = new EditTaskHandler(model).Execute();

            if (task.OrderId.HasValue && orderStatusId.HasValue)
            {
                var order = OrderService.GetOrder(task.OrderId.Value);
                if (order != null && order.OrderStatusId != orderStatusId.Value)
                {
                    OrderStatusService.ChangeOrderStatus(task.OrderId.Value, orderStatusId.Value,
                        LocalizationService.GetResourceFormat("Admin.Tasks.CompleteTask.OrderStatusBasis", task.Id, taskResult.Default("-")));
                    TrialService.TrackEvent(TrialEvents.ChangeOrderStatus, "");
                }
            }
            if (task.LeadId.HasValue && dealStatusId.HasValue)
            {
                var lead = LeadService.GetLead(task.LeadId.Value);
                if (lead != null && lead.DealStatusId != dealStatusId.Value)
                {
                    lead.DealStatusId = dealStatusId.Value;
                    LeadService.UpdateLead(lead);
                }
            }

            return Json(new { result = result });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AcceptTask(int id)
        {
            //var manager = ManagerService.GetManager(CustomerContext.CustomerId);
            //if (manager == null)
            //    return Json(new { result = false });

            var task = TaskService.GetTask(id);
            var model = (TaskModel)task;
            model.Status = TaskStatus.Completed;
            model.Accepted = true;
            var result = new EditTaskHandler(model).Execute();

            return Json(new { result = result});
        }

        #region Inplace

        //[HttpPost, ValidateJsonAntiForgeryToken]
        //public JsonResult InplaceTask(TaskModel model)
        //{
        //    if (model.Id != 0)
        //    {
        //        var task = TaskService.GetTask(model.Id);
        //        if (task == null)
        //            return Json(new { result = false });
        //        task.Name = model.Name;
        //        TaskService.UpdateTask(task);
        //    }

        //    return Json(new { result = true });
        //}

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteTask(int id)
        {
            //var currentManager = ManagerService.GetManager(CustomerContext.CustomerId);
            //if (currentManager == null)
            //    return Json(new { result = false });

            // only appointed manager can delete task
            //if (!TaskService.CanDeleteTask(id, currentManager.ManagerId))
            //    return Json(new { result = false });

            var modifier = CustomerContext.CurrentCustomer;
            var task = TaskService.GetTask(id);

            var customersToNotify = new List<Customer>();
            if (task.AssignedManager != null)
                customersToNotify.Add(task.AssignedManager.Customer);
            if (task.AppointedManager != null)
                customersToNotify.Add(task.AppointedManager.Customer);

            new AdminNotificationsHandler().NotifyCustomers(new OnTaskDeletedNotification(task, modifier),
                customersToNotify.Where(x => x != null && x.Id != modifier.Id && x.HasRoleAction(RoleAction.Tasks)).Select(x => x.Id).ToArray());

            TaskService.DeleteTask(id);

            return Json(new { result = true });
        }

        #endregion

        #region Commands

        private void Command(TasksCommand command, Func<int, TasksCommand, bool> func)
        {
            var currentManager = ManagerService.GetManager(CustomerContext.CustomerId);
            if (currentManager == null)
                return;

            command.ManagerId = currentManager.ManagerId;
            command.Customer = CustomerContext.CurrentCustomer;

            if (command.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in command.Ids)
                {
                    func(id, command);
                }
            }
            else
            {
                var handler = new GetTasksHandler(command);
                var ids = handler.GetItemsIds("Task.Id");

                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteTasks(TasksCommand command)
        {
            Command(command, (id, c) =>
            {
                //if (!TaskService.CanDeleteTask(id, c.ManagerId))
                //    return false;
                TaskService.DeleteTask(id);
                return true;
            });
            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeTaskStatuses(TasksCommand command, TaskStatus status)
        {
            Command(command, (id, c) =>
            {
                var task = TaskService.GetTask(id);
                if (task == null)
                    return false;
                if (task.Status != status)
                {
                    TaskService.ChangeTaskStatus(id, status);
                    var prev = (Task)task.Clone();
                    task.Status = status;
                    TaskService.OnTaskChanged(c.Customer, prev, task, prev.Attachments);
                }
                return true;
            });
            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AcceptTasks(TasksCommand command)
        {
            Command(command, (id, c) =>
            {
                TaskService.SetTaskAccepted(id);
                return true;
            });
            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult MarkViewed(TasksCommand command)
        {
            Command(command, (id, c) =>
            {
                TaskService.SetTaskViewed(id, c.ManagerId);
                return true;
            });
            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult MarkNotViewed(TasksCommand command)
        {
            Command(command, (id, c) =>
            {
                TaskService.SetTaskViewed(id, c.ManagerId, false);
                return true;
            });
            return Json(true);
        }

        #endregion

        #region Attachments

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ValidateAttachments()
        {
            var handler = new UploadAttachmentsHandler(null);
            var result = handler.Validate<TaskAttachment>();

            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadAttachments(int taskId)
        {
            var task = TaskService.GetTask(taskId);
            var prevState = (Task)task.Clone();
            var oldAttachments = prevState.Attachments;

            var handler = new UploadAttachmentsHandler(taskId);
            var result = handler.Execute<TaskAttachment>();

            TaskService.OnTaskChanged(CustomerContext.CurrentCustomer, prevState, task, oldAttachments);
            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteAttachment(int id, int taskId)
        {
            var task = TaskService.GetTask(taskId);
            var prevState = (Task)task.Clone();
            var oldAttachments = prevState.Attachments;

            var result = AttachmentService.DeleteAttachment<TaskAttachment>(id);

            TaskService.OnTaskChanged(CustomerContext.CurrentCustomer, prevState, task, oldAttachments);
            return Json(new { result = result });
        }

        public JsonResult GetTaskAttachments(int id)
        {
            return Json(AttachmentService.GetAttachments<TaskAttachment>(id)
                .Select(x => new AttachmentModel
                {
                    Id = x.Id,
                    ObjId = x.ObjId,
                    FileName = x.FileName,
                    FilePath = x.Path,
                    FilePathAdmin = x.PathAdmin,
                    FileSize = x.FileSizeFormatted
                })
            );
        }

        #endregion

        public JsonResult GetTaskPrioritiesSelectOptions()
        {
            return Json(Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>()
                .Select(x => new SelectItemModel(x.Localize(), x.ConvertIntString())));
        }

        public JsonResult GetTaskStatusesSelectOptions()
        {
            return Json(Enum.GetValues(typeof(TaskStatus)).Cast<TaskStatus>()
                .Select(x => new SelectItemModel(x.Localize(), x.ConvertIntString())));
        }

        public JsonResult GetNotCompletedTaskStatusesSelectOptions()
        {
            return Json(Enum.GetValues(typeof(TaskStatus)).Cast<TaskStatus>()
                .Where(x => x != TaskStatus.Completed)
                .Select(x => new SelectItemModel(x.Localize(), x.ConvertIntString())));
        }

        public JsonResult GetTaskFormData()
        {
            return Json(new
            {
                managers = ManagerService.GetManagersList().OrderBy(x => x.FullName).Select(x => new SelectItemModel(x.FullName, x.ManagerId.ToString())),
                taskGroups = TaskGroupService.GetAllTaskGroups().Select(x => new SelectItemModel(x.Name, x.Id.ToString())),
                priorities = Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>().Select(x => new SelectItemModel(x.Localize(), x.ConvertIntString())),
                defaultTaskGroupId = SettingsTasks.DefaultTaskGroup == 0 ? (int?)null : SettingsTasks.DefaultTaskGroup,
                filesHelpText = FileHelpers.GetFilesHelpText(EAdvantShopFileTypes.TaskAttachment)
            });
        }

        [ChildActionOnly]
        public ActionResult NavMenu()
        {
            var groups = TaskGroupService.GetActiveTaskGroups(4);

            return PartialView(groups);
        }
    }
}
