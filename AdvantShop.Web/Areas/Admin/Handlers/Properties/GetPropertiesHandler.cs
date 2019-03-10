using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Properties;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Properties
{
    public class GetPropertiesHandler
    {
        private readonly PropertiesFilterModel _filterModel;
        private SqlPaging _paging;

        public GetPropertiesHandler(PropertiesFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<PropertyModel> Execute()
        {
            var model = new FilterResult<PropertyModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<PropertyModel>();
            
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
                "Property.PropertyId",
                "Name",
                "UseInFilter",
                "UseInDetails",
                "UseInBrief",
                "SortOrder",
                "GroupName",
                "ISNULL(GroupId, 0)".AsSqlField("GroupId"),
                "(Select Count(ProductID) from Catalog.ProductPropertyValue Where [PropertyValueID] in (SELECT [PropertyValueID]  FROM [Catalog].[PropertyValue] WHERE [PropertyID] = [Property].PropertyID))".AsSqlField("ProductsCount")
            );

            _paging.From("[Catalog].[Property]");
            _paging.Left_Join("[Catalog].[PropertyGroup] ON [Property].[GroupId] = [PropertyGroup].[PropertyGroupId]");
            
            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (_filterModel.GroupId != null)
            {
                if (_filterModel.GroupId == -1)
                {
                    _paging.Where("Property.GroupId is null");
                }
                else
                {
                    _paging.Where("Property.GroupId = {0}", _filterModel.GroupId);
                }
            }

            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                _paging.Where("Name LIKE '%'+{0}+'%'", _filterModel.Search);
            }

            if (!string.IsNullOrEmpty(_filterModel.Name))
            {
                _paging.Where("Name LIKE '%'+{0}+'%'", _filterModel.Name);
            }

            if (_filterModel.UseInFilter != null)
            {
                _paging.Where("UseInFilter = {0}", Convert.ToInt32(_filterModel.UseInFilter));
            }

            if (_filterModel.UseInDetails != null)
            {
                _paging.Where("UseInDetails = {0}", Convert.ToInt32(_filterModel.UseInDetails));
            }

            if (_filterModel.UseInBrief != null)
            {
                _paging.Where("UseInBrief = {0}", Convert.ToInt32(_filterModel.UseInBrief));
            }

            if (_filterModel.SortingFrom != null)
            {
                _paging.Where("SortOrder >= {0}", _filterModel.SortingFrom.Value);
            }

            if (_filterModel.SortingTo != null)
            {
                _paging.Where("SortOrder <= {0}", _filterModel.SortingTo.Value);
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