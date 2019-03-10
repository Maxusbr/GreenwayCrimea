using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.CustomerFieldValues;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.CustomerFieldValues
{
    public class GetCustomerFieldValuesHandler
    {
        private readonly CustomerFieldValuesFilterModel _filterModel;
        private SqlPaging _paging;

        public GetCustomerFieldValuesHandler(CustomerFieldValuesFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<AdminCustomerFieldValueModel> Execute()
        {
            var model = new FilterResult<AdminCustomerFieldValueModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.CustomerFieldValues.Grid.TotalString", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<AdminCustomerFieldValueModel>();

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
                "Id",
                "CustomerFieldId",
                "Value",
                "SortOrder"
                );

            _paging.From("Customers.CustomerFieldValue");

            Sorting();
            Filter();
        }

        private void Filter()
        {
            _paging.Where("CustomerFieldId = {0}", _filterModel.FieldId);

            if (_filterModel.Search.IsNotEmpty())
            {
                _paging.Where("Value LIKE '%'+{0}+'%'", _filterModel.Search);
            }
            if (_filterModel.Value.IsNotEmpty())
            {
                _paging.Where("Value LIKE '%'+{0}+'%'", _filterModel.Value);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy("SortOrder");
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