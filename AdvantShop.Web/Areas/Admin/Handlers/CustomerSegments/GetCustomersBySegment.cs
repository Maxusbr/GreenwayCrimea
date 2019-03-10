using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.CustomerSegments;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Customers;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Admin.Models.CustomerSegments;

namespace AdvantShop.Web.Admin.Handlers.CustomerSegments
{
    public class GetCustomersBySegment
    {
        private readonly bool? _onlyCustomerId;
        private readonly CustomersBySegmentFilterModel _filterModel;
        private readonly bool _exportToCsv;

        private SqlPaging _paging;

        public GetCustomersBySegment(CustomersBySegmentFilterModel filterModel)
        {
            _filterModel = filterModel;
            _exportToCsv = filterModel.OutputDataType == FilterOutputDataType.Csv;
        }

        public GetCustomersBySegment(CustomersBySegmentFilterModel filterModel, bool onlyCustomerId) :this(filterModel)
        {
            _onlyCustomerId = onlyCustomerId;
        }

        public FilterResult<CustomerBySegmentViewModel> Execute()
        {
            var model = new FilterResult<CustomerBySegmentViewModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<CustomerBySegmentViewModel>();
            
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

            if (_onlyCustomerId.HasValue)
            {
                _paging.Select("[Customer].CustomerID");
            }
            else
            {
                _paging.Select(
                    "[Customer].CustomerID",
                    "[Customer].Email".AsSqlField("Email"),
                    "[Customer].Phone".AsSqlField("Phone"),
                    "[Customer].Firstname",
                    "[Customer].Lastname",

                    ("(Select COUNT([Order].[OrderId]) From  [Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                     "WHERE [OrderCustomer].[CustomerId] = [Customer].[CustomerId] and [Order].[PaymentDate] is not null)")
                        .AsSqlField("OrdersCount"),

                    "[Customer].RegistrationDateTime".AsSqlField("RegistrationDateTime")
                    );

                if (_exportToCsv)
                {
                    _paging.Select(
                        ("(Select Top(1) [Order].OrderId From [Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].[OrderId] = [OrderCustomer].[OrderId] " +
                         "WHERE [CustomerId] = [Customer].[CustomerId] Order by [OrderDate] Desc)").AsSqlField("LastOrderId"),

                        ("(Select Top(1) [Order].Number From [Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].[OrderId] = [OrderCustomer].[OrderId] " +
                         "WHERE [CustomerId] = [Customer].[CustomerId] Order by [OrderDate] Desc)").AsSqlField(
                             "LastOrderNumber"),

                        ("(Select ISNULL(SUM([Sum]),0) From  [Order].[Order] LEFT JOIN [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                         "WHERE [OrderCustomer].[CustomerId] = [Customer].[CustomerId] and [Order].[PaymentDate] is not null)")
                            .AsSqlField("OrdersSum"),

                        "(Select Top(1) [City] From [Customers].[Contact] Where [Contact].[CustomerID] = [Customer].[CustomerID])"
                            .AsSqlField("Location"),

                        "(Select [FirstName] + ' ' + [LastName] From  [Customers].[Customer]  WHERE [CustomerId] = [Managers].[CustomerId])"
                            .AsSqlField("ManagerName")
                        );

                    _paging.Left_Join("[Customers].[Managers] ON [Customer].[ManagerId] = [Managers].[ManagerId]");
                }
            }

            _paging.From("[Customers].[Customer]");
            _paging.Where("[Customer].[CustomerRole] = {0}", (int)Role.User);
            

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

            var segment = CustomerSegmentService.Get(_filterModel.Id);
            if (segment == null || segment.SegmentFilter == null)
                return;

            var filter = segment.SegmentFilter;

            if (filter.OrdersSumFrom != null)
            {
                _paging.Where(
                    "(Select ISNULL(SUM([Sum]),0) " +
                    "From [Order].[Order] " +
                    "Left Join [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                    "Where [OrderCustomer].[CustomerId] = [Customer].[CustomerId]) >= {0}", 
                    filter.OrdersSumFrom.Value);
            }
            if (filter.OrdersSumTo != null)
            {
                _paging.Where(
                    "(Select ISNULL(SUM([Sum]),0) " +
                    "From [Order].[Order] " +
                    "Left Join [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                    "Where [OrderCustomer].[CustomerId] = [Customer].[CustomerId]) <= {0}",
                    filter.OrdersSumTo.Value);
            }

            if (filter.OrdersPaidSumFrom != null)
            {
                _paging.Where(
                    "(Select ISNULL(SUM([Sum]),0) " +
                    "From [Order].[Order] " +
                    "Left Join [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                    "Where [OrderCustomer].[CustomerId] = [Customer].[CustomerId] and [Order].[PaymentDate] is not null) >= {0}",
                    filter.OrdersPaidSumFrom.Value);
            }
            if (filter.OrdersPaidSumTo != null)
            {
                _paging.Where(
                    "(Select ISNULL(SUM([Sum]),0) " +
                    "From [Order].[Order] " +
                    "Left Join [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                    "Where [OrderCustomer].[CustomerId] = [Customer].[CustomerId] and [Order].[PaymentDate] is not null) <= {0}",
                    filter.OrdersPaidSumTo.Value);
            }

            if (filter.OrdersCountFrom != null)
            {
                _paging.Where(
                    "(Select Count([Order].[OrderId]) " +
                    "From [Order].[Order] " +
                    "Left Join [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                    "Where [OrderCustomer].[CustomerId] = [Customer].[CustomerId]) >= {0}",
                    filter.OrdersCountFrom.Value);
            }
            if (filter.OrdersCountTo != null)
            {
                _paging.Where(
                    "(Select Count([Order].[OrderId]) " +
                    "From [Order].[Order] " +
                    "Left Join [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                    "Where [OrderCustomer].[CustomerId] = [Customer].[CustomerId]) <= {0}",
                    filter.OrdersCountTo.Value);
            }

            if (filter.LastOrderDateFrom != null)
            {
                _paging.Where(
                    "(Select top(1) OrderDate " +
                    "From [Order].[Order] " +
                    "Left Join [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                    "Where [OrderCustomer].[CustomerId] = [Customer].[CustomerId] " +
                    "Order by [OrderDate] Desc) >= {0}",
                    filter.LastOrderDateFrom.Value);
            }
            if (filter.LastOrderDateTo != null)
            {
                _paging.Where(
                    "(Select top(1) OrderDate " +
                    "From [Order].[Order] " +
                    "Left Join [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                    "Where [OrderCustomer].[CustomerId] = [Customer].[CustomerId] " +
                    "Order by [OrderDate] Desc) <= {0}",
                    filter.LastOrderDateTo.Value);
            }


            if (filter.AverageCheckFrom != null)
            {
                _paging.Where(
                    "(Select ISNULL(SUM([Sum])/Count([Order].OrderId),0) " +
                    "From [Order].[Order] " +
                    "Left Join [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                    "Where [OrderCustomer].[CustomerId] = [Customer].[CustomerId] and [Order].[PaymentDate] is not null) >= {0}",
                    filter.AverageCheckFrom.Value);
            }
            if (filter.AverageCheckTo != null)
            {
                _paging.Where(
                    "(Select ISNULL(SUM([Sum])/Count([Order].OrderId),0) " +
                    "From [Order].[Order] " +
                    "Left Join [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                    "Where [OrderCustomer].[CustomerId] = [Customer].[CustomerId] and [Order].[PaymentDate] is not null) <= {0}",
                    filter.AverageCheckTo.Value);
            }

            if (filter.BirthDayFrom != null)
            {
                _paging.Where("Customer.BirthDay >= {0}", filter.BirthDayFrom.Value);
            }
            if (filter.BirthDayTo != null)
            {
                _paging.Where("Customer.BirthDay <= {0}", filter.BirthDayTo.Value);
            }


            if (filter.Cities != null && filter.Cities.Count > 0)
            {
                _paging.Where(
                    "(Select top(1) [City] From [Customers].[Contact] Where [Contact].[CustomerID] = [Customer].[CustomerID]) in (" +
                    String.Join(", ", Enumerable.Range(0, filter.Cities.Count).Select(x => "{" + x + "}")) + ")",
                    filter.Cities.ToArray());
            }

            if (filter.Countries != null && filter.Countries.Count > 0)
            {
                _paging.Where(
                    "(Select top(1) [Country] From [Customers].[Contact] Where [Contact].[CustomerID] = [Customer].[CustomerID]) in (" +
                    String.Join(", ", Enumerable.Range(0, filter.Countries.Count).Select(x => "{" + x + "}")) + ")",
                    filter.Countries.ToArray());
            }

            if (filter.Categories != null && filter.Categories.Count > 0)
            {
                var categoryIds = new List<int>();

                foreach (var categoryId in filter.Categories)
                {
                    categoryIds.AddRange(CategoryService.GetAllChildCategoriesIdsByCategoryId(categoryId));
                }

                categoryIds = categoryIds.Distinct().ToList();

                _paging.Where(
                    "Exists (Select [ProductCategories].CategoryId " +
                    "From [Order].[Order] " +
                    "Left Join [Order].[OrderItems] on [OrderItems].[OrderId] = [Order].[OrderId] " +
                    "Left Join [Catalog].[ProductCategories] ON [ProductCategories].[ProductId] = [OrderItems].[ProductId] " +
                    "Left Join [Order].[OrderCustomer] ON [Order].[OrderID] = [OrderCustomer].[OrderId] " +
                    "Where [OrderCustomer].[CustomerId] = [Customer].[CustomerId] and CategoryId in (" + 
                    String.Join(", ", Enumerable.Range(0, categoryIds.Count).Select(x => "{" + x + "}")) + ") )",
                    categoryIds.Select(x => (object)x).ToArray());
            }


            if (filter.CustomerFields != null && filter.CustomerFields.Count > 0)
            {
                _paging.Left_Join("Customers.CustomerFieldValuesMap ON CustomerFieldValuesMap.CustomerId = [Customer].[CustomerId]");

                foreach (var fieldFilter in filter.CustomerFields.Where(x => x.Value != null))
                {
                    _paging.Where(
                        "(CustomerFieldValuesMap.CustomerFieldId = {0} and CustomerFieldValuesMap.Value = {1})",
                        fieldFilter.Key, fieldFilter.Value);
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