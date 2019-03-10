using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.SQL2;
using AdvantShop.Repository;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Settings.System
{
    class GetRegion
    {
        private AdminRegionFilterModel _filterModel;
        private SqlPaging _paging;

        public GetRegion(AdminRegionFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<Region> Execute()
        {
            var model = new FilterResult<Region>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = string.Format("Найдено регионов: {0}", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<Region>();
            
            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("RegionID");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select(
                "RegionID",
                "CountryId",
                "RegionSort".AsSqlField("SortOrder"),
                "RegionName".AsSqlField("Name"),
                "RegionCode");

            _paging.From("[Customers].[Region]");

            Sorting();
            Filter();
        }

        private void Filter()
        {
            _paging.Where("CountryId = {0}", _filterModel.CountryId.ToString());
            if (!string.IsNullOrWhiteSpace(_filterModel.Search))
            {
                _filterModel.Name = _filterModel.Search;
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.Name))
            {
                _paging.Where("RegionName LIKE '%'+{0}+'%'", _filterModel.Name);
            }

            if (!string.IsNullOrEmpty(_filterModel.RegionCode))
            {
                _paging.Where("RegionCode LIKE '%'+{0}+'%'", _filterModel.RegionCode);
            }
            if (_filterModel.SortingFrom != null)
            {
                _paging.Where("RegionSort >= {0}", _filterModel.SortingFrom.ToString());
            }
            if (_filterModel.SortingTo != null)
            {
                _paging.Where("RegionSort <= {0}", _filterModel.SortingTo.ToString());
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy(
                    new SqlCritera("SortOrder", "", SqlSort.Desc),
                    new SqlCritera("RegionName", "", SqlSort.Asc)
                    );
                return;
            }

            var sorting = _filterModel.Sorting.ToLower();

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
