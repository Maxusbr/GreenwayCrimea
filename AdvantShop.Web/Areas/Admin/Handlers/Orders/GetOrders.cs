using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.SQL2;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.Models.Orders;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Orders
{
    public class GetOrdersHandler
    {
        private readonly OrdersFilterModel _filterModel;
        private SqlPaging _paging;

        public GetOrdersHandler(OrdersFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<OrderItemsModel> Execute()
        {
            var model = new FilterResult<OrderItemsModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<OrderItemsModel>();

            model.TotalString = string.Format("Найдено заказов: {0} на сумму {1}", model.TotalItemsCount,
                PriceFormatService.FormatPrice(
                    _paging.GetCustomData("Sum ([Order].[Sum]*[OrderCurrency].[CurrencyValue]) as totalPrice", "", reader => SQLDataHelper.GetFloat(reader, "totalPrice"), true).FirstOrDefault(),
                    SettingsCatalog.DefaultCurrency));
            
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
                "[Order].[OrderID]",
                "Number",
                "case IsNumeric(number) when 1 then Replicate('0', 20 - Len(number)) + number else number end".AsSqlField("SortNumber"),
                "StatusName",
                "[Order].[OrderStatusID]".AsSqlField("OrderStatusID"),
                "PaymentDate",
                "(case when PaymentDate is null then 0 else 1 end)".AsSqlField("IsPaid"),
                "PaymentMethodName",
                "ShippingMethodName",
                "[OrderCustomer].FirstName + ' ' + [OrderCustomer].LastName".AsSqlField("BuyerName"),
                "PaymentMethod.Name".AsSqlField("PaymentMethod"),
                "ShippingMethod.Name".AsSqlField("ShippingMethod"),
                "Sum",
                "OrderDate",
                "CurrencyValue",
                "CurrencyCode",
                "CurrencySymbol",
                "[OrderCustomer].CustomerID",
                "IsCodeBefore",
                "[Order].ManagerId",
                "Color",
                "ISNULL ([Customer].Rating, 0)".AsSqlField("Rating"),
                "ManagerConfirmed"
                );

            _paging.From("[Order].[Order]");

            _paging.Left_Join("[Order].[OrderCustomer] ON [Order].[OrderID]=[OrderCustomer].[OrderID]");
            _paging.Left_Join("[Order].[OrderStatus] ON [OrderStatus].[OrderStatusID]=[Order].[OrderStatusID]");
            _paging.Left_Join("[Order].[OrderCurrency] ON [Order].[OrderID] = [OrderCurrency].[OrderID]");
            //_paging.Left_Join("[Order].[OrderContact] ON [Order].[BillingContactID] = [OrderContact].[OrderContactID]");
            _paging.Left_Join("[Order].[PaymentMethod] ON [Order].[PaymentMethodID] = [Order].[PaymentMethod].[PaymentMethodID]");
            _paging.Left_Join("[Order].[ShippingMethod] ON [Order].[ShippingMethodID] = [Order].[ShippingMethod].ShippingMethodID");
            _paging.Left_Join("[Customers].[Customer] ON [OrderCustomer].[CustomerId] = [Customer].[CustomerId]");

            if (SettingsCheckout.EnableManagersModule)
            {
                _paging.Select(
                    "ManagerCustomer.CustomerId".AsSqlField("ManagerCustomerId"),
                    "[ManagerCustomer].FirstName + ' ' + [ManagerCustomer].LastName".AsSqlField("ManagerName"));

                _paging.Left_Join("[Customers].[Managers] ON [Order].[ManagerId] = [Managers].[ManagerID]");
                _paging.Left_Join("[Customers].[Customer] as ManagerCustomer ON [Managers].[CustomerId] = [ManagerCustomer].[CustomerId]");
            }

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.Filter) &&
                (string.IsNullOrWhiteSpace(_filterModel.OrderDateFrom) && string.IsNullOrWhiteSpace(_filterModel.OrderDateTo)))
            {
                switch (_filterModel.Filter)
                {
                    case "lastmonth":
                        _filterModel.OrderDateFrom = DateTime.Now.Date.AddDays(-1).ToString("dd.MM.yyyy");
                        _filterModel.OrderDateFrom = DateTime.Now.Date.AddMonths(1).ToString("dd.MM.yyyy");
                        break;

                    case "today":
                        _filterModel.OrderDateFrom = DateTime.Now.Date.ToString("dd.MM.yyyy");
                        _filterModel.OrderDateFrom = DateTime.Now.Date.ToString("dd.MM.yyyy");
                        break;

                    case "yesterday":
                        _filterModel.OrderDateFrom = DateTime.Now.Date.AddDays(-1).ToString("dd.MM.yyyy");
                        _filterModel.OrderDateFrom = DateTime.Now.Date.AddDays(-1).ToString("dd.MM.yyyy");
                        break;
                }
            }

            switch (_filterModel.FilterBy)
            {
                case OrdersPreFilterType.New:
                    _filterModel.OrderStatusId = OrderStatusService.DefaultOrderStatus;
                    break;
                case OrdersPreFilterType.NotPaid:
                    _filterModel.IsPaid = false;
                    break;
                case OrdersPreFilterType.Paid:
                    _filterModel.IsPaid = true;
                    break;
                case OrdersPreFilterType.Drafts:
                    _filterModel.IsDraft = true;
                    break;
                default:
                    break;
            }
            
            _paging.Where("IsDraft = {0}", Convert.ToInt32(_filterModel.IsDraft));

            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                _paging.Where(
                    "(Number LIKE '%'+{0}+'%' OR [OrderCustomer].FirstName LIKE '%'+{0}+'%' OR [OrderCustomer].LastName LIKE '%'+{0}+'%' OR [OrderCustomer].FirstName + ' ' + [OrderCustomer].LastName LIKE '%'+{0}+'%' OR [OrderCustomer].LastName + ' ' + [OrderCustomer].FirstName LIKE '%'+{0}+'%' OR [Customer].Phone LIKE '%'+{0}+'%')",
                    _filterModel.Search);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.Number))
            {
                _paging.Where("Number LIKE '%'+{0}+'%'", _filterModel.Number);
            }

            if (_filterModel.OrderStatusId != 0)
            {
                _paging.Where("[Order].[OrderStatusID] = {0}", _filterModel.OrderStatusId);
            }

            if (_filterModel.PriceFrom != 0)
            {
                _paging.Where("Sum >= {0}", _filterModel.PriceFrom);
            }
            if (_filterModel.PriceTo != 0)
            {
                _paging.Where("Sum <= {0}", _filterModel.PriceTo);
            }
            
            if (!string.IsNullOrEmpty(_filterModel.BuyerName))
            {
                _paging.Where("([OrderCustomer].FirstName LIKE '%'+{0}+'%' OR [OrderCustomer].LastName LIKE '%'+{0}+'%' OR [OrderCustomer].FirstName + ' ' + [OrderCustomer].LastName LIKE '%'+{0}+'%' OR [OrderCustomer].LastName + ' ' + [OrderCustomer].FirstName LIKE '%'+{0}+'%')", _filterModel.BuyerName);
            }

            if (!string.IsNullOrEmpty(_filterModel.BuyerPhone))
            {
                _paging.Where("([OrderCustomer].Phone LIKE '%'+{0}+'%' OR [OrderCustomer].StandardPhone LIKE '%'+{0}+'%')", _filterModel.BuyerPhone);
            }

            if (!string.IsNullOrEmpty(_filterModel.BuyerEmail))
            {
                _paging.Where("([OrderCustomer].Email LIKE '%'+{0}+'%')", _filterModel.BuyerEmail);
            }

            if (!string.IsNullOrEmpty(_filterModel.BuyerCity))
            {
                _paging.Where("([OrderCustomer].City LIKE '%'+{0}+'%')", _filterModel.BuyerCity);
            }

            if (!string.IsNullOrEmpty(_filterModel.PaymentMethod))
            {
                _paging.Where("PaymentMethod.Name = {0}", _filterModel.PaymentMethod);
            }

            if (!string.IsNullOrEmpty(_filterModel.ShippingMethod))
            {
                _paging.Where("ShippingMethodName LIKE '%'+{0}+'%'", _filterModel.ShippingMethod);
            }

            if (_filterModel.IsPaid != null)
            {
                _paging.Where(_filterModel.IsPaid.Value ? "PaymentDate is not null" : "PaymentDate is null");
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.ManagerCustomerId) && SettingsCheckout.EnableManagersModule)
            {
                _paging.Where("ManagerCustomer.CustomerId = {0}", _filterModel.ManagerCustomerId);
            }

            DateTime from, to;

            if (!string.IsNullOrWhiteSpace(_filterModel.OrderDateFrom) && DateTime.TryParse(_filterModel.OrderDateFrom, out from))
            {
                _paging.Where("OrderDate >= {0}", from);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.OrderDateTo) && DateTime.TryParse(_filterModel.OrderDateTo, out to))
            {
                _paging.Where("OrderDate <= {0}", to);
            }

            if (_filterModel.OrderSourceId != null)
            {
                _paging.Where("OrderSourceId = {0}", _filterModel.OrderSourceId.Value);
            }

            var customer = CustomerContext.CurrentCustomer;

            if (customer.IsModerator)
            {
                var manager = ManagerService.GetManager(customer.Id);
                if (manager != null && manager.Active)
                {
                    if (SettingsManager.ManagersOrderConstraint == ManagersOrderConstraint.Assigned)
                    {
                        _paging.Where("[Order].ManagerId = {0}", manager.ManagerId);
                    }
                    else if (SettingsManager.ManagersOrderConstraint == ManagersOrderConstraint.AssignedAndFree)
                    {
                        _paging.Where("([Order].ManagerId = {0} or [Order].ManagerId is null)", manager.ManagerId);
                    }
                }
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderByDesc("OrderDate");
                return;
            }

            var sorting = _filterModel.Sorting.ToLower().Replace("number", "sortnumber").Replace("formatted", "");

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