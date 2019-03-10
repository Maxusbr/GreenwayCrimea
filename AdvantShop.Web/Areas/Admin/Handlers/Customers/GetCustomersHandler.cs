using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Models.Customers;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Helpers;

namespace AdvantShop.Web.Admin.Handlers.Customers
{
    public class GetCustomersHandler
    {
        private readonly CustomersFilterModel _filterModel;
        private SqlPaging _paging;

        public GetCustomersHandler(CustomersFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<AdminCustomerModel> Execute()
        {
            var model = new FilterResult<AdminCustomerModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<AdminCustomerModel>();
            
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
                "[Customer].CustomerID",
                "[Customer].Email".AsSqlField("Email"),
                "[Customer].Phone".AsSqlField("Phone"),
                "[Customer].ManagerId",
                "[Customer].Rating",
                //"[Customer].CustomerGroupId",
                "(Select [Customer].Firstname + ' ' + [Customer].Lastname)".AsSqlField("Name"),

                ("(Select Top(1) [Order].OrderId From [Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].[OrderId] = [OrderCustomer].[OrderId] " +
                 "WHERE [CustomerId] = [Customer].[CustomerId] Order by [OrderDate] Desc)").AsSqlField("LastOrderId"),

                ("(Select Top(1) [Order].Number From [Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].[OrderId] = [OrderCustomer].[OrderId] " +
                 "WHERE [CustomerId] = [Customer].[CustomerId] Order by [OrderDate] Desc)").AsSqlField("LastOrderNumber"),

                ("(Select ISNULL(SUM([Sum]),0) From  [Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                 "WHERE [OrderCustomer].[CustomerId] = [Customer].[CustomerId] and [Order].[PaymentDate] is not null)")
                    .AsSqlField("OrdersSum"),

                ("(Select COUNT([Order].[OrderId]) From  [Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                 "WHERE [OrderCustomer].[CustomerId] = [Customer].[CustomerId] and [Order].[PaymentDate] is not null)")
                    .AsSqlField("OrdersCount"),

                "(Select Top(1) [City] From [Customers].[Contact] Where [Contact].[CustomerID] = [Customer].[CustomerID])"
                    .AsSqlField("Location"),

                "(Select [FirstName] + ' ' + [LastName] From  [Customers].[Customer]  WHERE [CustomerId] = [Managers].[CustomerId])"
                    .AsSqlField("ManagerName"),

                "[Customer].RegistrationDateTime".AsSqlField("RegistrationDateTime")
                );

            _paging.From("[Customers].[Customer]");
            _paging.Left_Join("[Customers].[Managers] ON [Customer].[ManagerId] = [Managers].[ManagerId]");

            if (_filterModel.CustomerFields != null)
            {
                _paging.Left_Join("Customers.CustomerFieldValuesMap ON CustomerFieldValuesMap.CustomerId = [Customer].[CustomerId]");
            }

            _paging.Where("[Customer].[CustomerRole] = {0}",
                _filterModel.Role.HasValue ? (int) _filterModel.Role.Value : (int) Role.User);
            
            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                _paging.Where(
                    "([Customer].Email LIKE '%'+{0}+'%' OR [Customer].Firstname + ' ' + [Customer].Lastname LIKE '%'+{0}+'%' OR [Customer].Lastname + ' ' + [Customer].Firstname LIKE '%'+{0}+'%' OR [Customer].Phone LIKE '%'+{0}+'%')",
                    _filterModel.Search);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.Name))
            {
                _paging.Where("([Customer].Lastname + ' ' + [Customer].Firstname LIKE '%'+{0}+'%' OR [Customer].Firstname + ' ' + [Customer].Lastname LIKE '%'+{0}+'%')", _filterModel.Name);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.Email))
            {
                _paging.Where("[Customer].Email LIKE '%'+{0}+'%'", _filterModel.Email);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.Phone))
            {
                long? standartPhone = StringHelper.ConvertToStandardPhone(_filterModel.Phone, true, true);
                _paging.Where("convert(nvarchar, StandardPhone) LIKE '%'+{0}+'%'", standartPhone != null ? standartPhone.ToString() : "null");
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.ManagerName))
            {
                _paging.Where("(Select [FirstName] + ' ' + [LastName] From  [Customers].[Customer]  WHERE [CustomerId] = [Managers].[CustomerId]) LIKE '%'+{0}+'%'", _filterModel.ManagerName);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.LastOrderNumber))
            {
                _paging.Where(
                    "(Select Top(1) [Order].Number From [Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].[OrderId] = [OrderCustomer].[OrderId] " +
                    "WHERE [CustomerId] = [Customer].[CustomerId] Order by [OrderDate] Desc) LIKE '%'+{0}+'%'",
                    _filterModel.LastOrderNumber);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.Location))
            {
                _paging.Where(
                    "(Select Top(1) [City] From [Customers].[Contact] Where [Contact].[CustomerID] = [Customer].[CustomerID]) LIKE '%'+{0}+'%'",
                    _filterModel.Location);
            }

            if (_filterModel.OrdersCountFrom != 0)
            {
                _paging.Where(
                    "(Select COUNT([Order].[OrderId]) From  [Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                    "WHERE [OrderCustomer].[CustomerId] = [Customer].[CustomerId] and [Order].[PaymentDate] is not null) >= {0}", _filterModel.OrdersCountFrom);
            }

            if (_filterModel.OrdersCountTo != 0)
            {
                _paging.Where(
                    "(Select COUNT([Order].[OrderId]) From  [Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                    "WHERE [OrderCustomer].[CustomerId] = [Customer].[CustomerId] and [Order].[PaymentDate] is not null) <= {0}", _filterModel.OrdersCountTo);
            }

            if (_filterModel.OrderSumFrom != 0)
            {
                _paging.Where(
                    "(Select ISNULL(SUM([Sum]),0) From  [Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                    "WHERE [OrderCustomer].[CustomerId] = [Customer].[CustomerId] and [Order].[PaymentDate] is not null) >= {0}",
                    _filterModel.OrderSumFrom);
            }

            if (_filterModel.OrderSumTo != 0)
            {
                _paging.Where(
                    "(Select ISNULL(SUM([Sum]),0) From  [Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                    "WHERE [OrderCustomer].[CustomerId] = [Customer].[CustomerId] and [Order].[PaymentDate] is not null) <= {0}",
                    _filterModel.OrderSumTo);
            }

            if (_filterModel.LastOrderFrom != 0)
            {
                _paging.Where("LastOrder >= {0}", _filterModel.LastOrderFrom);
            }

            if (_filterModel.LastOrderTo != 0)
            {
                _paging.Where("LastOrder <= {0}", _filterModel.LastOrderTo);
            }

            DateTime from, to;

            if (!string.IsNullOrWhiteSpace(_filterModel.RegistrationDateTimeFrom) && DateTime.TryParse(_filterModel.RegistrationDateTimeFrom, out from))
            {
                _paging.Where("RegistrationDateTime >= {0}", from);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.RegistrationDateTimeTo) && DateTime.TryParse(_filterModel.RegistrationDateTimeTo, out to))
            {
                _paging.Where("RegistrationDateTime <= {0}", to);
            }

            if (_filterModel.Group != 0)
            {
                _paging.Where("[Customer].CustomerGroupId = {0}", _filterModel.Group);
            }

            if (_filterModel.CustomerFields != null)
            {
                foreach (var fieldFilter in _filterModel.CustomerFields.Where(x => x.Value != null))
                {
                    var fieldsFilterModel = fieldFilter.Value;
                    if (fieldsFilterModel.DateFrom.HasValue)
                    {
                        _paging.Where("(CustomerFieldValuesMap.CustomerFieldId = {0} and CustomerFieldValuesMap.Value >= {1})", fieldFilter.Key, fieldsFilterModel.DateFrom.Value.ToString("yyyy-MM-dd"));
                    }
                    if (fieldsFilterModel.DateTo.HasValue)
                    {
                        _paging.Where("(CustomerFieldValuesMap.CustomerFieldId = {0} and CustomerFieldValuesMap.Value is not null and CustomerFieldValuesMap.Value <> '' and CustomerFieldValuesMap.Value <= {1})", fieldFilter.Key, fieldsFilterModel.DateTo.Value.ToString("yyyy-MM-dd"));
                    }
                    if (fieldsFilterModel.From.IsNotEmpty())
                    {
                        var value = fieldsFilterModel.From.TryParseInt(true);
                        _paging.Where("(CustomerFieldValuesMap.CustomerFieldId = {0} and CustomerFieldValuesMap.Value >= {1})", fieldFilter.Key, value ?? Int32.MaxValue);
                    }
                    if (fieldsFilterModel.To.IsNotEmpty())
                    {
                        var value = fieldsFilterModel.To.TryParseInt(true);
                        _paging.Where("(CustomerFieldValuesMap.CustomerFieldId = {0} and CustomerFieldValuesMap.Value is not null and CustomerFieldValuesMap.Value <> '' and CustomerFieldValuesMap.Value <= {1})", fieldFilter.Key, value ?? Int32.MaxValue);
                    }
                    if (fieldsFilterModel.ValueExact.IsNotEmpty())
                    {
                        _paging.Where("(CustomerFieldValuesMap.CustomerFieldId = {0} and CustomerFieldValuesMap.Value = {1})", fieldFilter.Key, fieldsFilterModel.ValueExact);
                    }
                    if (fieldsFilterModel.Value.IsNotEmpty())
                    {
                        _paging.Where("(CustomerFieldValuesMap.CustomerFieldId = {0} and CustomerFieldValuesMap.Value like '%' + {1} + '%')", fieldFilter.Key, fieldsFilterModel.Value);
                    }
                }
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