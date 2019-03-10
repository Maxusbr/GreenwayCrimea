using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.PropertyValues;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.PropertyValues
{
    public class GetPropertyValuesHandler
    {
        private readonly PropertyValuesFilterModel _filterModel;
        private SqlPaging _paging;

        public GetPropertyValuesHandler(PropertyValuesFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<PropertyValueModel> Execute()
        {
            var model = new FilterResult<PropertyValueModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<PropertyValueModel>();
            
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
                "PropertyValueId",
                "PropertyId",
                "Value",
                "SortOrder",
                "(Select Count(ProductID) from Catalog.ProductPropertyValue Where propertyValueid=[PropertyValue].propertyValueid)".AsSqlField("ProductsCount")
            );

            _paging.From("[Catalog].[PropertyValue]");

            _paging.Where("PropertyId = {0}", _filterModel.PropertyId);
            
            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                _paging.Where("Value LIKE '%'+{0}+'%'", _filterModel.Search);
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