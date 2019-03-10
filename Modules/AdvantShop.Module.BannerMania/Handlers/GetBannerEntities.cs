using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Module.BannerMania.Models;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Module.BannerMania.Handlers
{
    public class GetBannerEntities
    {
        private readonly BannerEntityFilterModel _filterModel;
        private SqlPaging _paging;

        public GetBannerEntities(BannerEntityFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<BannerEntityModel> Execute()
        {
            var model = new FilterResult<BannerEntityModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<BannerEntityModel>();
            
            foreach(var item in model.DataItems)
            {
                item.Placement = Service.BMService.GetPlacementTypeName(item.Placement);
                item.EntityType = Service.BMService.GetEntityTypeName(item.EntityType);
            }

            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("BannerId");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select(
                "[BannerId]",
                "[EntityId]",
                "[EntityType]".AsSqlField("EntityType"),
                "[EntityName]".AsSqlField("EntityName"),
                "[ImagePath]",
                "[Placement]".AsSqlField("Placement"),
                "[URL]".AsSqlField("URL"),
                "[NewWindow]".AsSqlField("NewWindow"),
                "[Enabled]".AsSqlField("Enabled")
                );

            _paging.From("[Module].[BannerMania]");
            
            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.EntityName))
                _filterModel.Search = _filterModel.EntityName;

            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                _paging.Where("[EntityName] LIKE '%'+{0}+'%'", _filterModel.Search);
            }

            if (!string.IsNullOrEmpty(_filterModel.EntityType))
            {
                _paging.Where("[EntityType] = {0}", _filterModel.EntityType);
            }

            if (!string.IsNullOrEmpty(_filterModel.Placement))
            {
                _paging.Where("[Placement] = {0}", _filterModel.Placement);
            }

            if (!string.IsNullOrEmpty(_filterModel.URL))
            {
                _paging.Where("[URL] LIKE '%'+{0}+'%'", _filterModel.URL);
            }

            if (_filterModel.NewWindow.HasValue)
            {
                _paging.Where("[NewWindow] = {0}", _filterModel.NewWindow.Value ? "1" : "0");
            }

            if (_filterModel.Enabled.HasValue)
            {
                _paging.Where("[Enabled] = {0}", _filterModel.Enabled.Value ? "1" : "0");
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy("EntityName");
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