using AdvantShop.Web.Admin.Models.Voting;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Infrastructure.Admin;
using System;

namespace AdvantShop.Web.Admin.Handlers.Voting
{
    public class GetAnswersHandler
    {

        private VotingAnswerFilterModel _filterModel;
        private SqlPaging _paging;

        public GetAnswersHandler(VotingAnswerFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<VotingAnswerFilterResultModel> Execute()
        {
            var model = new FilterResult<VotingAnswerFilterResultModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Answer.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<VotingAnswerFilterResultModel>();

            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("AnswerID");
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
                "AnswerID".AsSqlField("ID"),
                "CountVoice",
                "Sort".AsSqlField("SortOrder"),
                "IsVisible",
                "DateAdded",
                "DateModify",
                "FKIDTheme".AsSqlField("ThemeId"));

            _paging.From("[Voice].[Answer]");

            Sorting();
            Filter();
        }

        private void Filter()
        {
            _paging.Where("FKIDTheme = {0}", _filterModel.ThemeId.ToString());
            if (!string.IsNullOrWhiteSpace(_filterModel.Search))
            {
                _filterModel.Name = _filterModel.Search;
            }
            if (!string.IsNullOrWhiteSpace(_filterModel.Name))
            {
                _paging.Where("Name LIKE '%'+{0}+'%'", _filterModel.Name);
            }
            if (_filterModel.CountVoiceFrom != null)
            {
                _paging.Where("CountVoice >= {0}", _filterModel.CountVoiceFrom.Value.ToString());
            }
            if (_filterModel.CountVoiceTo != null)
            {
                _paging.Where("CountVoice <= {0}", _filterModel.CountVoiceTo.Value.ToString());
            }
            if (_filterModel.SortOrderFrom != null)
            {
                _paging.Where("Sort >= {0}", _filterModel.SortOrderFrom);
            }
            if (_filterModel.SortOrderTo != null)
            {
                _paging.Where("Sort <= {0}", _filterModel.SortOrderTo);
            }
            if (_filterModel.IsVisible != null)
            {
                _paging.Where("IsVisible = {0}", (bool)_filterModel.IsVisible ? "1" : "0");
            }

            DateTime dateAddFrom, dateAddTo;

            if (!string.IsNullOrWhiteSpace(_filterModel.DateAddedFrom) && DateTime.TryParse(_filterModel.DateAddedFrom, out dateAddFrom))
            {
                _paging.Where("DateAdded >= {0}", dateAddFrom);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.DateAddedTo) && DateTime.TryParse(_filterModel.DateAddedTo, out dateAddTo))
            {
                _paging.Where("DateAdded <= {0}", dateAddTo);
            }

            DateTime dateModifFrom, dateModifTo;

            if (!string.IsNullOrWhiteSpace(_filterModel.DateModifyFrom) && DateTime.TryParse(_filterModel.DateModifyFrom, out dateModifFrom))
            {
                _paging.Where("DateModify >= {0}", dateModifFrom);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.DateModifyTo) && DateTime.TryParse(_filterModel.DateModifyTo, out dateModifTo))
            {
                _paging.Where("DateModify <= {0}", dateModifTo);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy(
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
