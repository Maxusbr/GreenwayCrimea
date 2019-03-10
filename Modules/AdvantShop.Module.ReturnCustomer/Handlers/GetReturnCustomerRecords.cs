using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Module.ReturnCustomer.Models;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Module.ReturnCustomer.Handlers
{
    public class GetReturnCustomerRecords
    {
        private readonly ReturnCustomerRecordFilterModel _filterModel;
        private SqlPaging _paging;

        public GetReturnCustomerRecords(ReturnCustomerRecordFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<ReturnCustomerRecordModel> Execute()
        {
            var model = new FilterResult<ReturnCustomerRecordModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<ReturnCustomerRecordModel>();

            return model;
        }

        public List<Guid> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<Guid>("CustomerID");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select(
                "[ReturnCustomerRecords].[CustomerID]",
                "[ReturnCustomerRecords].[LastActionDate]".AsSqlField("LastActionDate"),
                "[ReturnCustomerRecords].[LastSendingDate]".AsSqlField("LastSendingDate"),
                "(SELECT [Customer].[Firstname] + ' ' + [Customer].[Lastname])".AsSqlField("CustomerName"),
                "[Customer].[Email]".AsSqlField("Email"),
                "[Customer].[Phone]".AsSqlField("Phone")
                );

            _paging.From("[Module].[ReturnCustomerRecords]");
            _paging.Left_Join("[Customers].[Customer] ON [ReturnCustomerRecords].[CustomerID] = [Customer].[CustomerID]");
            
            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                _paging.Where(
                    "([Customer].[Email] LIKE '%'+{0}+'%' OR [Customer].[Firstname] + ' ' + [Customer].[Lastname] LIKE '%'+{0}+'%' OR [Customer].[Lastname] + ' ' + [Customer].[Firstname] LIKE '%'+{0}+'%' OR [Customer].[Phone] LIKE '%'+{0}+'%')",
                    _filterModel.Search);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.CustomerName))
            {
                _paging.Where("([Customer].[Lastname] + ' ' + [Customer].[Firstname] LIKE '%'+{0}+'%' OR [Customer].[Firstname] + ' ' + [Customer].[Lastname] LIKE '%'+{0}+'%')", _filterModel.CustomerName);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.Email))
            {
                _paging.Where("[Customer].[Email] LIKE '%'+{0}+'%'", _filterModel.Email);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.Phone))
            {
                long? standartPhone = AdvantShop.Helpers.StringHelper.ConvertToStandardPhone(_filterModel.Phone, true, true);
                _paging.Where("convert(nvarchar, [Customer].[StandardPhone]) LIKE '%'+{0}+'%'", standartPhone != null ? standartPhone.ToString() : "null");
            }

            DateTime from, to;

            if (!string.IsNullOrWhiteSpace(_filterModel.LastActionDateFrom) && DateTime.TryParse(_filterModel.LastActionDateFrom, out from))
            {
                _paging.Where("LastActionDate >= {0}", from);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.LastActionDateTo) && DateTime.TryParse(_filterModel.LastActionDateTo, out to))
            {
                _paging.Where("LastActionDate <= {0}", to);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.LastSendingDateFrom) && DateTime.TryParse(_filterModel.LastSendingDateFrom, out from))
            {
                _paging.Where("LastSendingDate >= {0}", from);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.LastSendingDateTo) && DateTime.TryParse(_filterModel.LastSendingDateTo, out to))
            {
                _paging.Where("LastSendingDate <= {0}", to);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderByDesc("LastActionDate");
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