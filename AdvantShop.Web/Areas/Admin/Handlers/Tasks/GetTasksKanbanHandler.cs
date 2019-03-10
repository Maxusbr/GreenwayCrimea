using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Models.Tasks;

namespace AdvantShop.Web.Admin.Handlers.Tasks
{
    public class GetTasksKanbanHandler
    {
        private readonly TasksKanbanFilterModel _filter;
        private int _currentManagerId;
        private Guid _currentCustomerId;

        public GetTasksKanbanHandler(TasksKanbanFilterModel filterModel)
        {
            _filter = filterModel;
            _currentCustomerId = CustomerContext.CustomerId;
            var currentManager = ManagerService.GetManager(_currentCustomerId);
            if (currentManager != null)
                _currentManagerId = currentManager.ManagerId;
        }

        public TasksKanbanModel Execute()
        {
            var model = new TasksKanbanModel
            {
                Name = "Kanban"
            };
            if (!_filter.Columns.Any())
            {
                _filter.Columns = new List<TasksKanbanColumnFilterModel>
                {
                    new TasksKanbanColumnFilterModel(ETasksKanbanColumn.New.ToString()),
                    new TasksKanbanColumnFilterModel(ETasksKanbanColumn.InProgress.ToString()),
                    new TasksKanbanColumnFilterModel(ETasksKanbanColumn.Done.ToString()),
                    //new TasksKanbanColumnFilterModel(ETasksKanbanColumn.Accepted.ToString())
                };
            }
            foreach (var filterColumn in _filter.Columns)
            {
                var paging = GetPaging(filterColumn);
                if (paging == null)
                    continue;
                var column  = new TasksKanbanColumnModel
                {
                    Id = filterColumn.Id,
                    Name = filterColumn.Type.Localize(),
                    Class = filterColumn.Id.ToLower(),
                    Page = filterColumn.Page,
                    CardsPerColumn = filterColumn.CardsPerColumn,
                    TotalCardsCount = paging.TotalRowsCount,
                    TotalPagesCount = paging.PageCount(paging.TotalRowsCount, filterColumn.CardsPerColumn),
                };

                if (column.TotalPagesCount >= filterColumn.Page || filterColumn.Page == 1)
                    column.Cards = paging.PageItemsList<TaskKanbanModel>();

                model.Columns.Add(column);
            }

            return model;
        }

        public List<TaskKanbanModel> GetCards()
        {
            var result = new List<TaskKanbanModel>();
            if (_filter.ColumnId.IsNullOrEmpty() || !_filter.Columns.Any(x => x.Id == _filter.ColumnId))
                return result;

            var paging = GetPaging(_filter.Columns.FirstOrDefault(x => x.Id == _filter.ColumnId), false);

            return paging != null ? paging.PageItemsList<TaskKanbanModel>() : new List<TaskKanbanModel>();
        }

        private SqlPaging GetPaging(TasksKanbanColumnFilterModel columnFilter, bool allCards = true)
        {
            var type = columnFilter.Id.TryParseEnum<ETasksKanbanColumn>();
            if (type == ETasksKanbanColumn.None)
                return null;

            var paging = new SqlPaging()
            {
                ItemsPerPage = allCards ? columnFilter.CardsPerColumn * columnFilter.Page : columnFilter.CardsPerColumn,
                CurrentPageIndex = allCards ? 1 : columnFilter.Page
            };

            paging.Select(
                "Task.Id",
                "Task.TaskGroupId",
                "Task.Name",
                //"Task.Description",
                "Task.Status",
                "Task.Accepted",
                "Task.Priority",
                "Task.DueDate",
                "Task.DateAppointed",
                "AppointedCustomer.CustomerID".AsSqlField("AppointedCustomerId"),
                "AppointedCustomer.FirstName + ' ' + AppointedCustomer.LastName".AsSqlField("AppointedName"),
                "AssignedCustomer.CustomerID".AsSqlField("AssignedCustomerId"),
                "AssignedCustomer.FirstName + ' ' + AssignedCustomer.LastName".AsSqlField("AssignedName"),
                "TaskGroup.Name".AsSqlField("TaskGroupName"),
                "ViewedTask.ViewDate",
                ("(case when ViewedTask.ViewDate is not null OR [AssignedManagerId] <> " + _currentManagerId.ToString() + " then 1 else 0 end)").AsSqlField("Viewed"),
                ("(case when ViewedTask.ViewDate is null " +
                 "then (select count(AdminComment.Id) FROM CMS.AdminComment WHERE ObjId = Task.Id AND Type = 'Task' AND Deleted = 0 AND AdminComment.CustomerId <> '" + _currentCustomerId.ToString() + "') " +
                 "else (select count(AdminComment.Id) FROM CMS.AdminComment WHERE ObjId = Task.Id AND Type = 'Task' AND Deleted = 0 AND AdminComment.CustomerId <> '" + _currentCustomerId.ToString() + "' AND AdminComment.DateCreated > ViewedTask.ViewDate) " +
                 "end)").AsSqlField("NewCommentsCount"),
                "AppointedCustomer.Avatar".AsSqlField("AppointedCustomerAvatar"),
                "AssignedCustomer.Avatar".AsSqlField("AssignedCustomerAvatar")
                );

            paging.From("[Customers].[Task]");
            paging.Left_Join("Customers.Managers as AppointedManager ON Task.AppointedManagerId = AppointedManager.ManagerId");
            paging.Left_Join("Customers.Customer as AppointedCustomer ON AppointedCustomer.CustomerID = AppointedManager.CustomerId");
            paging.Left_Join("Customers.Managers as AssignedManager ON Task.AssignedManagerId = AssignedManager.ManagerId");
            paging.Left_Join("Customers.Customer as AssignedCustomer ON AssignedCustomer.CustomerID = AssignedManager.CustomerId");
            paging.Inner_Join("Customers.TaskGroup ON Task.TaskGroupId = TaskGroup.Id");
            //paging.Left_Join("Customers.Customer as ClientCustomer ON Task.CustomerId = ClientCustomer.CustomerId");
            //paging.Left_Join("[Order].[Order] as TaskOrder ON Task.OrderId = TaskOrder.OrderId");
            paging.Left_Join("Customers.ViewedTask ON Task.Id = ViewedTask.TaskId AND ViewedTask.ManagerId = " + _currentManagerId.ToString());

            //paging.OrderByDesc("TaskGroupId");
            paging.Where("Task.IsDeferred = 0");
            Sorting(paging);
            Filter(paging, columnFilter);

            return paging;
        }

        private void Filter(SqlPaging paging, TasksKanbanColumnFilterModel columnFilter)
        {
            _filter.Accepted = false;

            if (_filter.OnlyMy)
                _filter.AssignedManagerId = _currentManagerId;

            switch (columnFilter.Type)
            {
                case ETasksKanbanColumn.New:
                    _filter.Status = TaskStatus.Open;
                    break;
                case ETasksKanbanColumn.InProgress:
                    _filter.Status = TaskStatus.InProgress;
                    break;
                case ETasksKanbanColumn.Done:
                    _filter.Status = TaskStatus.Completed;
                    break;
                case ETasksKanbanColumn.Accepted:
                    _filter.Accepted = true;
                    break;
                default:
                    return;
            }
            if (!string.IsNullOrWhiteSpace(_filter.Search))
            {
                if (_filter.Search.IsInt())
                    paging.Where("Task.Id = {0}", _filter.Search.TryParseInt());
                else
                    paging.Where("Task.Name LIKE '%' + {0} + '%'", _filter.Search);
            }
            if (!string.IsNullOrWhiteSpace(_filter.Name))
            {
                paging.Where("Task.Name LIKE '%' + {0} + '%'", _filter.Search);
            }
            if (_filter.Status.HasValue)
            {
                paging.Where("Task.Status = {0}", _filter.Status.Value);
            }
            if (_filter.TaskGroupId.HasValue)
            {
                paging.Where("Task.TaskGroupId = {0}", _filter.TaskGroupId.Value);
            }
            if (_filter.Accepted.HasValue)
            {
                paging.Where("Task.Accepted = {0}", _filter.Accepted.Value);
            }
            if (_filter.Viewed.HasValue)
            {
                if (_filter.Viewed.Value)
                    paging.Where("(ViewedTask.ViewDate is not null OR [AssignedManagerId] <> " + _currentManagerId.ToString() + ")");
                else
                    paging.Where("ViewedTask.ViewDate is null AND [AssignedManagerId] = " + _currentManagerId.ToString());
            }
            if (_filter.Priority.HasValue)
            {
                paging.Where("Task.Priority = {0}", _filter.Priority.Value);
            }
            if (_filter.AppointedManagerId.HasValue)
            {
                paging.Where("Task.AppointedManagerId = {0}", _filter.AppointedManagerId.Value);
            }
            if (_filter.AssignedManagerId.HasValue)
            {
                paging.Where("Task.AssignedManagerId = {0}", _filter.AssignedManagerId.Value);
            }
            if (_filter.DueDateFrom.HasValue)
            {
                paging.Where("Task.DueDate >= {0}", _filter.DueDateFrom.Value);
            }
            if (_filter.DueDateTo.HasValue)
            {
                paging.Where("Task.DueDate <= {0}", _filter.DueDateTo.Value);
            }
            if (_filter.DateCreatedFrom.HasValue)
            {
                paging.Where("Task.DateCreated >= {0}", _filter.DateCreatedFrom.Value);
            }
            if (_filter.DateCreatedTo.HasValue)
            {
                paging.Where("Task.DateCreated <= {0}", _filter.DateCreatedTo.Value);
            }
        }

        private void Sorting(SqlPaging paging)
        {
            paging.OrderBy("Task.SortOrder");
            //if (string.IsNullOrEmpty(_filter.Sorting) || _filter.SortingType == FilterSortingType.None)
            //{
            //    switch (_filter.FilterBy)
            //    {
            //        case TasksPreFilterType.AssignedToMe:
            //        case TasksPreFilterType.AppointedByMe:
            //        case TasksPreFilterType.Order:
            //        case TasksPreFilterType.Lead:
            //            // задачи в работе выше завершенных
            //            paging.OrderBy("StatusSort");
            //            break;
            //        default:
            //            break;
            //    }
            //    paging.OrderByDesc("Task.DateAppointed");
            //    return;
            //}

            //var sorting = _filter.Sorting.ToLower().Replace("formatted", string.Empty);

            //var field = paging.SelectFields().FirstOrDefault(x => x.FieldName == sorting);
            //if (field != null)
            //{
            //    if (_filter.SortingType == FilterSortingType.Asc)
            //    {
            //        paging.OrderBy(sorting);
            //    }
            //    else
            //    {
            //        paging.OrderByDesc(sorting);
            //    }
            //}
        }
    }
}