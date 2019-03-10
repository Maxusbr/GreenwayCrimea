using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.SQL2;
using AdvantShop.Repository;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Settings.System
{
    public class GetCity
    {
        private AdminCityFilterModel _filterModel;
        private SqlPaging _paging;

        public GetCity(AdminCityFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<City> Execute()
        {
            var model = new FilterResult<City>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = string.Format("Найдено городов: {0}", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<City>();
            
            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("CityID");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };
            
            _paging.Select(
                "CityId",
                "City.RegionID",
                "CityName".AsSqlField("Name"),
                "CitySort",
                "City.DisplayInPopup",
                "City.PhoneNumber",
                "MobilePhoneNumber");

            _paging.From("[Customers].[City]");

            if(_filterModel.cityCountrys != null)
            {
                _paging.Left_Join("[Customers].[Region] ON Region.RegionId = City.RegionId");
            }

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if(_filterModel.cityCountrys != null)
            {
                _paging.Where("Region.CountryId = {0}", _filterModel.cityCountrys.ToString());
            }
            else
            {
                _paging.Where("RegionID = {0}", _filterModel.RegionId);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.Search))
            {
                _filterModel.Name = _filterModel.Search;
            }
                        if (!string.IsNullOrWhiteSpace(_filterModel.Name))
            {
                _paging.Where("CityName LIKE '%'+{0}+'%'", _filterModel.Name);
            }
            if (_filterModel.PhoneNumber != null)
            {
                _paging.Where("PhoneNumber LIKE '%'+{0}+'%'", _filterModel.PhoneNumber);
            }
            if (_filterModel.MobilePhoneNumber != null)
            {
                _paging.Where("MobilePhoneNumber LIKE '%'+{0}+'%'", _filterModel.MobilePhoneNumber.ToString());
            }
            if (_filterModel.DisplayInPopup != null)
            {
                _paging.Where("DisplayInPopup = {0}", (bool)_filterModel.DisplayInPopup ? 1 : 0);
            }
            if(_filterModel.SortingFrom != null)
            {
                _paging.Where("CitySort >= {0}", _filterModel.SortingFrom.ToString());
            }
            if (_filterModel.SortingTo != null)
            {
                _paging.Where("CitySort <= {0}", _filterModel.SortingTo.ToString());
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy(
                    new SqlCritera("CitySort", "", SqlSort.Desc),
                    new SqlCritera("CityName", "", SqlSort.Asc)
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
