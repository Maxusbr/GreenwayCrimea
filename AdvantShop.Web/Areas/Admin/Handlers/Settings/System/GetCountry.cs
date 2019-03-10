using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.SQL2;
using AdvantShop.Repository;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Settings.System
{
    public class GetCountry
    {
        private AdminCountryFilterModel _filterModel;
        private SqlPaging _paging;

        public GetCountry(AdminCountryFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<Country> Execute()
        {
            var model = new FilterResult<Country>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = string.Format("Найдено стран: {0}", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<Country>();
            
            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("CountryId");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };
            
            _paging.Select(
                "CountryId",
                "CountryName".AsSqlField("Name"),
                "SortOrder".AsSqlField("SortOrder"),
                "CountryISO2".AsSqlField("Iso2"),
                "CountryISO3".AsSqlField("Iso3"),
                "DisplayInPopup",
                "DialCode");

            _paging.From("[Customers].[Country]");

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrWhiteSpace(_filterModel.Search))
            {
                _filterModel.Name = _filterModel.Search;
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.Name))
            {
                _paging.Where("CountryName LIKE '%'+{0}+'%'", _filterModel.Name);
            }
            
            if (!string.IsNullOrEmpty(_filterModel.ISO2))
            {
                _paging.Where("CountryISO2 LIKE '%'+{0}+'%'", _filterModel.ISO2);
            }

            if (!string.IsNullOrEmpty(_filterModel.ISO3))
            {
                _paging.Where("CountryISO3 LIKE '%'+{0}+'%'", _filterModel.ISO3);
            }

            if (_filterModel.DialCode.HasValue)
            {
                _paging.Where("DialCode = {0}", _filterModel.DialCode.Value);
            }
            if(_filterModel.DisplayInPopup.HasValue)
            {
                _paging.Where("DisplayInPopup = {0}", _filterModel.DisplayInPopup.Value ? 1 : 0);
            }
            if(_filterModel.SortingFrom.HasValue)
            {
                _paging.Where("SortOrder >= {0}",_filterModel.SortingFrom.Value);
            }
            if (_filterModel.SortingTo.HasValue)
            {
                _paging.Where("SortOrder <= {0}", _filterModel.SortingTo.Value);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy(
                    new SqlCritera("SortOrder", "", SqlSort.Desc),
                    new SqlCritera("CountryName", "", SqlSort.Asc)
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
