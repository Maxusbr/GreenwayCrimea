using System;
using System.Web.Mvc;
using AdvantShop.Areas.AdminMobile.Models.Orders;
using AdvantShop.Areas.AdminMobile.Models.Tasks;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL2;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Saas;
using AdvantShop.Web.Admin.Filters;

namespace AdvantShop.Areas.AdminMobile.Controllers
{
    [SaasFeature(ESaasProperty.HaveCrm)]
    [Auth(RoleAction.Crm)]
    public class TasksController : BaseAdminMobileController
    {
        // GET: AdminMobile/Tasks/
        public ActionResult Index()
        {
            var model = new TasksViewModel();
            model.Statuses.Add(new SelectListItem {Text = T("AdminMobile.Tasks.AllTasks"), Value = "-1"});

            foreach (ManagerTaskStatus status in Enum.GetValues(typeof(ManagerTaskStatus)))
            {
                model.Statuses.Add(new SelectListItem
                {
                    Text = status.Localize(),
                    Value = ((int) status).ToString()
                });
            }
            
            SetMetaInformation(T("AdminMobile.Tasks.Tasks"));
            return View(model);
        }


        // GET: AdminMobile/Task/{taskId}
        public ActionResult Task(int taskId)
        {
            var task = ManagerTaskService.GetManagerTask(taskId);
            if (task == null)
                return new EmptyResult();

            var model = new TaskViewModel
            {
                Task = task
            };

            Order order;

            if (task.OrderId.HasValue && (order = OrderService.GetOrder(task.OrderId.Value)) != null)
            {
                model.Order = new OrderModel
                {
                    OrderId = order.OrderID,
                    Number = order.Number,
                };
            }

            foreach (ManagerTaskStatus status in Enum.GetValues(typeof(ManagerTaskStatus)))
            {
                model.Statuses.Add(new SelectListItem
                {
                    Text = status.Localize(),
                    Value = ((int)status).ToString(),
                    Selected = status == task.Status
                });
            }

            SetMetaInformation(task.Name);

            return View(model);
        }

        public JsonResult GetTasks(int page, int status)
        {
            Manager currentManager;
            if (!CustomerContext.CurrentCustomer.IsManager || (currentManager = ManagerService.GetManager(CustomerContext.CurrentCustomer.Id)) == null)
                return null;
            
            if (page == 0)
                page = 1;

            var paging = new SqlPaging(page, 10);
            paging.Select(
                "TaskId",
                "Name",
                "Status",
                "DueDate",
                "DateCreated"
                );

            paging.From("[Customers].[ManagerTask]");

            var flag = false;
            if (!CustomerContext.CurrentCustomer.IsAdmin)
            {
                paging.Where("AssignedManagerId = {0}", currentManager.ManagerId);
                flag = true;
            }

            if (status != -1)
            {
                paging.Where((flag ? " AND " : "" ) + "Status = {0}", status);
            }

            paging.OrderByDesc("DateCreated");

            var items = paging.PageItemsList<TaskModel>();

            return Json(items);
        }

        public JsonResult ChangeStatus(int taskId, int status)
        {
            if (!CustomerContext.CurrentCustomer.IsManager)
                return null;

            var task = ManagerTaskService.GetManagerTask(taskId);
            if (task == null)
                return null;

            if (task.Status != (ManagerTaskStatus)status)
            {
                ManagerTaskService.ChangeTaskStatus(taskId, (ManagerTaskStatus)status);
                ManagerTaskService.OnChangeManagerTaskStatus(task);
                return Json(new {ResultCode = 0});
            }

            return null;
        }
    }
}