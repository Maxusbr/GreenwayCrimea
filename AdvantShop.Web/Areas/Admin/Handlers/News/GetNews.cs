using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.News;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.News
{
    public class GetNews
    {
        private readonly NewsFilterModel _filterModel;
        private SqlPaging _paging;

        public GetNews(NewsFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<NewsModel> Execute()
        {
            var model = new FilterResult<NewsModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<NewsModel>();
            
            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("NewsId");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select(
                "NewsId",
                "Title",
                "ShowOnMainPage",
                "Enabled",
                "[News].[NewsCategoryId]".AsSqlField("NewsCategoryId"),
                "[NewsCategory].[Name]".AsSqlField("NewsCategory"),
                "AddingDate"
                );

            _paging.From("[Settings].[News]");
            _paging.Left_Join("[Settings].[NewsCategory] ON [NewsCategory].[NewsCategoryID] = [News].[NewsCategoryId]");


            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.Title))
                _filterModel.Search = _filterModel.Title;

            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                _paging.Where("Title LIKE '%'+{0}+'%'", _filterModel.Search);
            }

            if (_filterModel.ShowOnMainPage.HasValue)
            {
                _paging.Where("ShowOnMainPage = {0}", _filterModel.ShowOnMainPage.Value);
            }

            if (_filterModel.Enabled.HasValue)
            {
                _paging.Where("Enabled = {0}", _filterModel.Enabled.Value);
            }

            if (_filterModel.NewsCategoryId != null)
            {
                _paging.Where("[News].[NewsCategoryId] = {0}", _filterModel.NewsCategoryId.Value);
            }

            DateTime from, to;

            if (!string.IsNullOrWhiteSpace(_filterModel.AddingDateFrom) && DateTime.TryParse(_filterModel.AddingDateFrom, out from))
            {
                _paging.Where("AddingDate >= {0}", from);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.AddingDateTo) && DateTime.TryParse(_filterModel.AddingDateTo, out to))
            {
                _paging.Where("AddingDate <= {0}", to);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderByDesc("AddingDate");
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