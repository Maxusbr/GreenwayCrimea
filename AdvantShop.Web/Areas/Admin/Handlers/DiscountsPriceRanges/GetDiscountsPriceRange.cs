using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.DiscountsPriceRanges;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.DiscountsPriceRanges
{
    public class GetDiscountsPriceRange
    {
        private DiscountsPriceRangeFilterModel _filterModel;
        private SqlPaging _paging;

        public GetDiscountsPriceRange(DiscountsPriceRangeFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<DiscountsPriceRangeModel> Execute()
        {
            var model = new FilterResult<DiscountsPriceRangeModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<DiscountsPriceRangeModel>();
            
            return model;
        }

        public List<T> GetItemsIds<T>()
        {
            GetPaging();

            return _paging.ItemsIds<T>("OrderPriceDiscountID");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select(
                "OrderPriceDiscountID",
                "PriceRange",
                "PercentDiscount");

            _paging.From("[Order].[OrderPriceDiscount]");

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrWhiteSpace(_filterModel.Search))
            {
                _paging.Where("PriceRange LIKE '%'+{0}+'%'", _filterModel.Search);
            }

            if (_filterModel.PriceRange != null)
            {
                _paging.Where("PriceRange LIKE {0}+'%'", _filterModel.PriceRange.Value.ToString());
            }

            if (_filterModel.PercentDiscount != null)
            {
                _paging.Where("PercentDiscount LIKE {0}+'%'", _filterModel.PercentDiscount.Value.ToString());
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy("PriceRange");
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