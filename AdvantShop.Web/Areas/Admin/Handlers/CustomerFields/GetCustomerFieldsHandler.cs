using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.CustomerFields;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.CustomerFields
{
    public class GetCustomerFieldsHandler
    {
        private readonly CustomerFieldsFilterModel _filterModel;
        private SqlPaging _paging;

        public GetCustomerFieldsHandler(CustomerFieldsFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<AdminCustomerFieldModel> Execute()
        {
            var model = new FilterResult<AdminCustomerFieldModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.CustomerFields.Grid.TotalString", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<AdminCustomerFieldModel>();

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
                "Name",
                "FieldType",
                "SortOrder",
                "Required",
                "ShowInClient",
                "Enabled"
                );

            _paging.From("Customers.CustomerField");

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (_filterModel.Search.IsNotEmpty())
            {
                _paging.Where("Name LIKE '%'+{0}+'%'", _filterModel.Search);
            }
            if (_filterModel.Name.IsNotEmpty())
            {
                _paging.Where("Name LIKE '%'+{0}+'%'", _filterModel.Name);
            }
            if (_filterModel.FieldType.HasValue)
            {
                _paging.Where("FieldType = {0}", _filterModel.FieldType.Value);
            }
            if (_filterModel.Required.HasValue)
            {
                _paging.Where("Required = {0}", _filterModel.Required.Value);
            }
            if (_filterModel.ShowInClient.HasValue)
            {
                _paging.Where("ShowInClient = {0}", _filterModel.ShowInClient.Value);
            }
            if (_filterModel.Enabled.HasValue)
            {
                _paging.Where("Enabled = {0}", _filterModel.Enabled.Value);
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