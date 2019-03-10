using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Models.Users;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Users
{
    public class GetUsersHandler
    {
        private readonly UsersFilterModel _filterModel;
        private SqlPaging _paging;
        private Customer _currentCustomer;

        public GetUsersHandler(UsersFilterModel filterModel)
        {
            _filterModel = filterModel;
            _currentCustomer = CustomerContext.CurrentCustomer;
        }

        public AdminUsersFilterResult Execute()
        {
            var model = new AdminUsersFilterResult();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Users.Grid.TotalString", model.TotalItemsCount);
            model.ManagersCount = ManagerService.GetManagersCount();

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<AdminUserModel>();

            return model;
        }

        public List<Guid> GetItemsIds(string fieldName)
        {
            GetPaging();

            return _paging.ItemsIds<Guid>(fieldName);
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select(
                "Customer.CustomerID",
                "Customer.Email",
                "Customer.FirstName + ' ' + Customer.LastName".AsSqlField("FullName"),
                "Customer.RegistrationDateTime",
                "Customer.CustomerRole",
                "Customer.Avatar",
                "Departments.Name".AsSqlField("DepartmentName"),
                "Customer.Enabled",
                "Managers.ManagerId".AsSqlField("AssociatedManagerId")
                );

            _paging.From("Customers.Customer");
            _paging.Left_Join("Customers.Managers ON Customer.CustomerID = Managers.CustomerId");
            _paging.Left_Join("Customers.Departments ON Departments.DepartmentId = Managers.DepartmentId");

            _paging.Where("Customer.CustomerRole in ({0},{1})", (int)Role.Administrator, (int)Role.Moderator);

            if (_currentCustomer.IsModerator && !_currentCustomer.HasRoleAction(RoleAction.Customers))
            {
                _paging.Where("Customer.CustomerId = {0}", CustomerContext.CurrentCustomer.Id);
            }

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (_filterModel.Search.IsNotEmpty())
            {
                _paging.Where(
                    "(Customer.Email LIKE '%'+{0}+'%' OR Customer.Firstname + ' ' + Customer.Lastname LIKE '%'+{0}+'%' OR Customer.Phone LIKE '%'+{0}+'%')",
                    _filterModel.Search);
            }
            if (_filterModel.FullName.IsNotEmpty())
            {
                _paging.Where("Customer.Firstname + ' ' + Customer.Lastname LIKE '%'+{0}+'%'", _filterModel.FullName);
            }
            if (_filterModel.Email.IsNotEmpty())
            {
                _paging.Where("Customer.Email LIKE '%'+{0}+'%'", _filterModel.Email);
            }
            if (_filterModel.DepartmentId.HasValue)
            {
                if (_filterModel.DepartmentId.Value == -1)
                    _paging.Where("Departments.DepartmentId is null");
                else
                    _paging.Where("Departments.DepartmentId = {0}", _filterModel.DepartmentId.Value);
            }
            if (_filterModel.Enabled.HasValue)
            {
                _paging.Where("Customer.Enabled = {0}", _filterModel.Enabled.Value);
            }
            if (_filterModel.HasPhoto.HasValue)
            {
                _paging.Where(_filterModel.HasPhoto.Value ? "(Avatar is not null and Avatar <> '')" : "(Avatar is null or Avatar = '')");
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderByDesc("RegistrationDateTime");
                return;
            }

            var sorting = _filterModel.Sorting.ToLower().Replace("formatted", "");

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