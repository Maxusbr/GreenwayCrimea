using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Grades;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Grades
{
    public class GetGradeHandler
    {
        private readonly GradeFilterModel _filterModel;
        private SqlPaging _paging;

        public GetGradeHandler(GradeFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<GradesModel> Execute()
        {
            var model = new FilterResult<GradesModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<GradesModel>();
            
            return model;
        }

        public List<int> GetItemsIds(string fieldName)
        {
            GetPaging();

            return _paging.ItemsIds<int>(fieldName);
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select("Id", "Name", "BonusPercent", "PurchaseBarrier", "SortOrder");
            _paging.From("[Bonus].[Grade]");
            
            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                _paging.Where("Name LIKE '%'+{0}+'%'", _filterModel.Search);
            }

            if (!string.IsNullOrEmpty(_filterModel.Name))
            {
                _paging.Where("Name LIKE '%'+{0}+'%'", _filterModel.Name);
            }

            if (_filterModel.BonusPercent.HasValue)
            {
                _paging.Where("BonusPercent={0}", _filterModel.BonusPercent.Value);
            }

            if (_filterModel.PurchaseBarrier.HasValue)
            {
                _paging.Where("PurchaseBarrier={0}", _filterModel.PurchaseBarrier.Value);
            }

            //if (!string.IsNullOrEmpty(_filterModel.Url))
            //{
            //    _paging.Where("UrlPath LIKE '%'+{0}+'%'", _filterModel.Url);
            //}

            //if (_filterModel.Enabled != null)
            //{
            //    _paging.Where("Enabled = {0}", Convert.ToInt32(_filterModel.Enabled.Value));
            //}
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