using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.StaticBlock;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.StaticBlock
{
    public class GetStaticBlock
    {
        private StaticBlockFilterModel _filterModel;
        private SqlPaging _paging;

        public GetStaticBlock(StaticBlockFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<StaticBlockFilterResultModel> Execute()
        {
            var model = new FilterResult<StaticBlockFilterResultModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<StaticBlockFilterResultModel>();

            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("StaticBlockID");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select(
                "InnerName",
                "StaticBlockID",
                "[Key]",
                "Added",
                "Modified",
                "Enabled");

            _paging.From("[CMS].[StaticBlock]");

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrWhiteSpace(_filterModel.Search))
            {
                _paging.Where("[Key] LIKE '%'+{0}+'%' OR InnerName LIKE '%'+{0}+'%'", _filterModel.Search);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.Key))
            {
                _paging.Where("[Key] LIKE '%'+{0}+'%'", _filterModel.Key);
            }
            if (!string.IsNullOrWhiteSpace(_filterModel.InnerName))
            {
                _paging.Where("InnerName LIKE '%'+{0}+'%'", _filterModel.InnerName);
            }
            if (_filterModel.Enabled != null)
            {
                _paging.Where("Enabled = {0}", (bool)_filterModel.Enabled ? "1" : "0");
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy(new SqlCritera("[Key]", "", SqlSort.Asc));
                return;
            }

            var sorting = _filterModel.Sorting.ToLower().Replace("formatted", "");
            if(sorting == "key")
            {
                sorting = "[key]";
            }

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
