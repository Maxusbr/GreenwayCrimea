using System.Collections.Generic;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Brands;
using System.Linq;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Repository;

namespace AdvantShop.Web.Admin.Handlers.Brands
{
    public class GetBrands
    {
        private AdminBrandFilterModel _filterModel;
        private SqlPaging _paging;

        public GetBrands(AdminBrandFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public AdminBrandsFilterResult Execute()
        {
            var model = new AdminBrandsFilterResult();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<AdminBrandModel>();

            model.Countries = CountryService.GetAllCountries();

            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("BrandId");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select(
                "BrandId",
                "Brand.SortOrder".AsSqlField("SortOrder"),
                "BrandName",
                "BrandDescription",
                "BrandBriefDescription",
                "Enabled",
                "UrlPath",
                "BrandSiteUrl",
                "Country.CountryId".AsSqlField("CountryId"),
                "Country.CountryName".AsSqlField("CountryName"),
                "PhotoId",
                "PhotoName");

            _paging.From("[Catalog].[Brand]");
            _paging.Left_Join("[Customers].[Country] On [Brand].CountryId = [Country].CountryId");
            _paging.Left_Join("[Catalog].[Photo] On [Photo].ObjId = [Brand].BrandId and [Type] = 'Brand'");

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrWhiteSpace(_filterModel.Search))
            {
                _filterModel.BrandName = _filterModel.Search;
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.BrandName))
            {
                _paging.Where("Brand.BrandName LIKE '%'+{0}+'%'", _filterModel.BrandName);
            }

            if (_filterModel.CountryId != 0)
            {
                _paging.Where("Brand.CountryId = {0}", _filterModel.CountryId);
            }

            if (_filterModel.Enabled != null)
            {
                _paging.Where("Enabled = {0}", _filterModel.Enabled.Value ? "1" : "0");
            }

            if (_filterModel.SortingFrom != 0)
            {
                _paging.Where("Brand.SortOrder >= {0}", _filterModel.SortingFrom);
            }

            if (_filterModel.SortingTo != 0)
            {
                _paging.Where("Brand.SortOrder <= {0}", _filterModel.SortingTo);
            }

            if (_filterModel.HasPhoto != null)
            {
                _paging.Where(string.Format("PhotoName is {0} null", _filterModel.HasPhoto.Value ? "not" : ""));
            }
        }

        private void Sorting()
        {


            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy(
                    new SqlCritera("SortOrder", "", SqlSort.Asc),
                    new SqlCritera("BrandName", "", SqlSort.Asc)
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