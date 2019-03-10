using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Models.Tasks;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Tasks
{
    public class GetTasksHandler
    {
        private readonly TasksFilterModel _filterModel;
        private SqlPaging _paging;
        private int _currentManagerId;
        private Guid _currentCustomerId;

        public GetTasksHandler(TasksFilterModel filterModel)
        {
            _filterModel = filterModel;
            _currentCustomerId = CustomerContext.CustomerId;
            var currentManager = ManagerService.GetManager(_currentCustomerId);
            if (currentManager != null)
                _currentManagerId = currentManager.ManagerId;
        }

        public FilterResult<TaskModel> Execute()
        {
            var model = new FilterResult<TaskModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<TaskModel>();

            model.TotalString = LocalizationService.GetResourceFormat("Admin.Tasks.Grid.TotalString", model.TotalItemsCount);

            return model;
        }

        public List<int> GetItemsIds(string fieldName)
        {
            GetPaging();

            return _paging.ItemsIds<int>(fieldName);
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select(
                "Task.Id",
                "Task.TaskGroupId",
                "Task.Name",
                "Task.Status",
                "Task.Accepted",
                "Task.Priority",
                "Task.DueDate",
                "Task.DateCreated",
                "Task.DateAppointed",
                //"Task.OrderId",
                //"TaskOrder.Number".AsSqlField("OrderNumber"),
                //"AppointedManagerId",
                "AppointedCustomer.CustomerID".AsSqlField("AppointedCustomerId"),
                "AppointedCustomer.FirstName + ' ' + AppointedCustomer.LastName".AsSqlField("AppointedName"),
                //"AssignedManagerId",
                "AssignedCustomer.CustomerID".AsSqlField("AssignedCustomerId"),
                "AssignedCustomer.FirstName + ' ' + AssignedCustomer.LastName".AsSqlField("AssignedName"),
                //"(case ClientCustomer.FirstName when ISNULL(ClientCustomer.FirstName, ClientCustomer.Email) then  ClientCustomer.FirstName + ' ' + ClientCustomer.LastName end)".AsSqlField("ClientName"),
                //"ClientCustomer.CustomerID".AsSqlField("ClientCustomerId"),
                //("(case when [AppointedManagerId] = " + _currentManagerId.ToString() + " then 1 else 0 end)").AsSqlField("CanDelete"),
                ("(case when Task.Status = " + (int)TaskStatus.Completed + " then 1 else 0 end)").AsSqlField("StatusSort"),
                "TaskGroup.Name".AsSqlField("TaskGroupName"),
                "ViewedTask.ViewDate",
                ("(case when ViewedTask.ViewDate is not null OR [AssignedManagerId] <> " + _currentManagerId.ToString() + " then 1 else 0 end)").AsSqlField("Viewed"),
                ("(case when ViewedTask.ViewDate is null " +
                 "then (select count(AdminComment.Id) FROM CMS.AdminComment WHERE ObjId = Task.Id AND Type = 'Task' AND Deleted = 0 AND AdminComment.CustomerId <> '" + _currentCustomerId.ToString() + "') " +
                 "else (select count(AdminComment.Id) FROM CMS.AdminComment WHERE ObjId = Task.Id AND Type = 'Task' AND Deleted = 0 AND AdminComment.CustomerId <> '" + _currentCustomerId.ToString() + "' AND AdminComment.DateCreated > ViewedTask.ViewDate) " +
                 "end)").AsSqlField("NewCommentsCount"),
                "AppointedCustomer.Avatar".AsSqlField("AppointedCustomerAvatar"),
                "AssignedCustomer.Avatar".AsSqlField("AssignedCustomerAvatar"),
                "Task.IsAutomatic",
                "Task.IsDeferred"
                );

            _paging.From("[Customers].[Task]");
            _paging.Left_Join("Customers.Managers as AppointedManager ON Task.AppointedManagerId = AppointedManager.ManagerId");
            _paging.Left_Join("Customers.Customer as AppointedCustomer ON AppointedCustomer.CustomerID = AppointedManager.CustomerId");
            _paging.Left_Join("Customers.Managers as AssignedManager ON Task.AssignedManagerId = AssignedManager.ManagerId");
            _paging.Left_Join("Customers.Customer as AssignedCustomer ON AssignedCustomer.CustomerID = AssignedManager.CustomerId");
            _paging.Inner_Join("Customers.TaskGroup ON Task.TaskGroupId = TaskGroup.Id");
            //_paging.Left_Join("Customers.Customer as ClientCustomer ON Task.CustomerId = ClientCustomer.CustomerId");
            //_paging.Left_Join("[Order].[Order] as TaskOrder ON Task.OrderId = TaskOrder.OrderId");
            _paging.Left_Join("Customers.ViewedTask ON Task.Id = ViewedTask.TaskId AND ViewedTask.ManagerId = " + _currentManagerId.ToString());

            // for grouping in grid
            _paging.OrderBy("TaskGroup.SortOrder", "TaskGroupId");
            _paging.Where("Task.IsDeferred = 0");
            Sorting();
            Filter();
        }

        private void Filter()
        {
            _filterModel.Accepted = false;

            switch (_filterModel.FilterBy)
            {
                case TasksPreFilterType.AssignedToMe:
                    _filterModel.AssignedManagerId = _currentManagerId;
                    break;
                case TasksPreFilterType.AppointedByMe:
                    _filterModel.AppointedManagerId = _currentManagerId;
                    break;
                case TasksPreFilterType.Completed:
                    _filterModel.Status = TaskStatus.Completed;
                    break;
                case TasksPreFilterType.Accepted:
                    _filterModel.Accepted = true;
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrWhiteSpace(_filterModel.Search))
            {
                if (_filterModel.Search.IsInt())
                    _paging.Where("Task.Id = {0}", _filterModel.Search.TryParseInt());
                else
                    _paging.Where("Task.Name LIKE '%' + {0} + '%'", _filterModel.Search);
            }
            if (_filterModel.FilterBy == TasksPreFilterType.None || _filterModel.FilterBy == TasksPreFilterType.AssignedToMe)
            {
                // TasksPreFilterType.None и TasksPreFilterType.AssignedToMe - все, кроме завершенных, если не выбран статус в фильтре
                if (_filterModel.Status.HasValue && _filterModel.Status.Value != TaskStatus.Completed)
                    _paging.Where("Task.Status = {0}", _filterModel.Status.Value);
                else
                    _paging.Where("Task.Status <> {0}", TaskStatus.Completed);
            }
            else if (_filterModel.Status.HasValue)
            {
                _paging.Where("Task.Status = {0}", _filterModel.Status.Value);
            }
            if (_filterModel.TaskGroupId.HasValue)
            {
                _paging.Where("Task.TaskGroupId = {0}", _filterModel.TaskGroupId.Value);
            }
            if (_filterModel.Accepted.HasValue)
            {
                _paging.Where("Task.Accepted = {0}", _filterModel.Accepted.Value);
            }
            if (_filterModel.Viewed.HasValue)
            {
                if (_filterModel.Viewed.Value)
                    _paging.Where("(ViewedTask.ViewDate is not null OR [AssignedManagerId] <> " + _currentManagerId.ToString() + ")");
                else
                    _paging.Where("ViewedTask.ViewDate is null AND [AssignedManagerId] = " + _currentManagerId.ToString());
            }
            if (_filterModel.Priority.HasValue)
            {
                _paging.Where("Task.Priority = {0}", _filterModel.Priority.Value);
            }
            if (_filterModel.AppointedManagerId.HasValue)
            {
                _paging.Where("Task.AppointedManagerId = {0}", _filterModel.AppointedManagerId.Value);
            }
            if (_filterModel.AssignedManagerId.HasValue)
            {
                _paging.Where("Task.AssignedManagerId = {0}", _filterModel.AssignedManagerId.Value);
            }
            if (_filterModel.DueDateFrom.HasValue)
            {
                _paging.Where("Task.DueDate >= {0}", _filterModel.DueDateFrom.Value);
            }
            if (_filterModel.DueDateTo.HasValue)
            {
                _paging.Where("Task.DueDate <= {0}", _filterModel.DueDateTo.Value);
            }
            if (_filterModel.DateCreatedFrom.HasValue)
            {
                _paging.Where("Task.DateCreated >= {0}", _filterModel.DateCreatedFrom.Value);
            }
            if (_filterModel.DateCreatedTo.HasValue)
            {
                _paging.Where("Task.DateCreated <= {0}", _filterModel.DateCreatedTo.Value);
            }
            if (_filterModel.ObjId.HasValue)
            {
                switch (_filterModel.FilterBy)
                {
                    case TasksPreFilterType.Order:
                        _paging.Where("Task.OrderId = {0}", _filterModel.ObjId.Value);
                        break;
                    case TasksPreFilterType.Lead:
                        _paging.Where("Task.LeadId = {0}", _filterModel.ObjId.Value);
                        break;
                }
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                switch (_filterModel.FilterBy)
                {
                    case TasksPreFilterType.AssignedToMe:
                    case TasksPreFilterType.AppointedByMe:
                    case TasksPreFilterType.Order:
                    case TasksPreFilterType.Lead:
                        // задачи в работе выше завершенных
                        _paging.OrderBy("StatusSort");
                        break;
                    default:
                        break;
                }
                _paging.OrderByDesc("Task.DateAppointed");
                return;
            }

            var sorting = _filterModel.Sorting.ToLower().Replace("formatted", string.Empty);

            var field = _paging.SelectFields().FirstOrDefault(x => x.FieldName == sorting);
            if (field != null)
            {
                if (_filterModel.SortingType == FilterSortingType.Asc)
                {
                    _paging.OrderBy(sorting);
                }
                else
                {
                    _paging.OrderByDesc(sorting);
                }
            }
        }
    }
}