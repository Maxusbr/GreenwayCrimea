using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.StaticPages;
using AdvantShop.Web.Infrastructure.Admin;
using System;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Web.Admin.Handlers.StaticPages
{
    public class GetStaticPages
    {
        private readonly StaticPagesFilterModel _filterModel;
        private SqlPaging _paging;

        public GetStaticPages(StaticPagesFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<StaticPagesModel> Execute()
        {
            var model = new FilterResult<StaticPagesModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<StaticPagesModel>();
            
            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("StaticPageId");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select(
                "StaticPageId",
                "PageName",
                "Enabled",
                "SortOrder",
                "ModifyDate"
                );

            _paging.From("[CMS].[StaticPage]");

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                _filterModel.PageName = _filterModel.Search;
            }

            if (!string.IsNullOrEmpty(_filterModel.PageName))
            {
                _paging.Where("PageName LIKE '%'+{0}+'%'", _filterModel.PageName);
            }

            if (_filterModel.Enabled != null)
            {
                _paging.Where("Enabled = {0}",(bool)_filterModel.Enabled ? "1" : "0");
            }

            if (_filterModel.SortOrder != null)
            {
                _paging.Where("SortOrder = {0}", _filterModel.SortOrder.Value);
            }

            if (_filterModel.Selected != null)
            {
                _paging.Where("StaticPageId <> {0}", _filterModel.Selected.ToString());
            }

            DateTime from, to;

            if (!string.IsNullOrWhiteSpace(_filterModel.ModifyDateFrom) && DateTime.TryParse(_filterModel.ModifyDateFrom, out from))
            {
                _paging.Where("ModifyDate >= {0}", from);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.ModifyDateTo) && DateTime.TryParse(_filterModel.ModifyDateTo, out to))
            {
                _paging.Where("ModifyDate <= {0}", to);
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