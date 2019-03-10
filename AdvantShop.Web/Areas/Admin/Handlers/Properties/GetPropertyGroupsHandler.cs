using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Properties;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Properties
{
    public class GetPropertyGroupsHandler
    {
        private readonly PropertyGroupsFilterModel _filterModel;
        private SqlPaging _paging;

        public GetPropertyGroupsHandler(PropertyGroupsFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<PropertyGroup> Execute()
        {
            var model = new FilterResult<PropertyGroup>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<PropertyGroup>();
            
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
                "PropertyGroupId",
                "GroupName".AsSqlField("Name"),
                "GroupSortOrder".AsSqlField("SortOrder")
                );

            _paging.From("[Catalog].[PropertyGroup]");
            
            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                _paging.Where("GroupName LIKE '%'+{0}+'%'", _filterModel.Search);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy("GroupSortOrder");
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