using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.SQL2;
using AdvantShop.Repository;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Settings.SearchSettings
{
    public class GetSettingsSearch
    {
        private SettingsSearchModel _filterModel;
        private SqlPaging _paging;

        public GetSettingsSearch(SettingsSearchModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<SettingsSearch> Execute()
        {
            var model = new FilterResult<SettingsSearch>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = string.Format("Найдено настроек: {0}", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<SettingsSearch>();
            
            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("ID");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };
            
            _paging.Select(
                "Id",
                "Title",
                "Link",
                "KeyWords", 
                "SortOrder");

            _paging.From("[Settings].[SettingsSearch]");

            Sorting();
            Filter();
        }

        private void Filter()
        {
            
            if (!string.IsNullOrWhiteSpace(_filterModel.Search))
            {
                _filterModel.Title = _filterModel.Search;
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.Title))
            {
                _paging.Where("Title LIKE '%'+{0}+'%'", _filterModel.Title);
            }
            if (_filterModel.KeyWords != null)
            {
                _paging.Where("KeyWords LIKE '%'+{0}+'%'", _filterModel.KeyWords);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy(
                    new SqlCritera("Title", "", SqlSort.Asc)
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
