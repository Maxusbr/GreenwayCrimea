using System.Collections.Generic;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Brands;
using System.Linq;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Brands
{
    public class GetBrandsChooser
    {
        private AdminBrandFilterModel _filterModel;
        private SqlPaging _paging;

        public GetBrandsChooser(AdminBrandFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<AdminBrandChooserModel> Execute()
        {
            var model = new FilterResult<AdminBrandChooserModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<AdminBrandChooserModel>();
            
            return model;
        }

        public List<T> GetItemsIds<T>(string fieldName)
        {
            GetPaging();

            return _paging.ItemsIds<T>(fieldName);
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
                "BrandName",
                "(Select Count(ProductID) from Catalog.Product Where Product.BrandID=Brand.BrandID)".AsSqlField("ProductsCount")
                );

            _paging.From("[Catalog].[Brand]");

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
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy("BrandName");
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