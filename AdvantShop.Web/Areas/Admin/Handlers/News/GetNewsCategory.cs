using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.News;
using AdvantShop.News;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.News
{
    public class GetNewsCategory
    {
        private NewsCategoryFilterModel _filterModel;
        private SqlPaging _paging;

        public GetNewsCategory(NewsCategoryFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<NewsCategory> Execute()
        {
            var model = new FilterResult<NewsCategory>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = string.Format("Найдено категорий: {0}", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<NewsCategory>();
            
            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("NewsCategoryID");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };
            
            _paging.Select(
                "Name",
                "NewsCategoryID",
                "SortOrder",
                "UrlPath");

            _paging.From("[Settings].[NewsCategory]");

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
                _paging.Where("Name LIKE '%'+{0}+'%'", _filterModel.Name);
            }
            
            if (!string.IsNullOrEmpty(_filterModel.UrlPath))
            {
                _paging.Where("UrlPath LIKE '%'+{0}+'%'", _filterModel.UrlPath);
            }

            if(_filterModel.SortOrderFrom != null)
            {
                _paging.Where("SortOrder >= {0}",_filterModel.SortOrderFrom);
            }
            if (_filterModel.SortOrderTo != null)
            {
                _paging.Where("SortOrder <= {0}", _filterModel.SortOrderTo);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy(
                    new SqlCritera("SortOrder", "", SqlSort.Asc),
                    new SqlCritera("Name", "", SqlSort.Asc)
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
