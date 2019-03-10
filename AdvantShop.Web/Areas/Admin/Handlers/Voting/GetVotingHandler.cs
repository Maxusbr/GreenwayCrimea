using AdvantShop.Web.Admin.Models.Voting;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Infrastructure.Admin;
using System;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Web.Admin.Handlers.Voting
{
    public class GetVotingHandler
    {
        private VotingThemeFilterModel _filterModel;
        private SqlPaging _paging;

        public GetVotingHandler(VotingThemeFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<VotingThemeFilterResultModel> Execute()
        {
            var model = new FilterResult<VotingThemeFilterResultModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Voting.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<VotingThemeFilterResultModel>();

            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("VoiceThemeID");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select(
                "PsyID",
                "VoiceThemeID".AsSqlField("ID"),
                "Name",
                "IsDefault",
                "IsHaveNullVoice",
                "IsClose",
                "DateAdded",
                "DateModify",
                "(Select Count(AnswerId) FROM [Voice].[Answer] WHERE [FKIDTheme] = VoiceThemeID)".AsSqlField("CountAnswers")
                );

            _paging.From("[Voice].[VoiceTheme]");

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

            if (_filterModel.IsDefault != null)
            {
                _paging.Where("IsDefault = {0}", (bool)_filterModel.IsDefault ? "1" : "0");
            }

            if (_filterModel.IsClose != null)
            {
                _paging.Where("IsClose = {0}", (bool)_filterModel.IsClose ? "1" : "0");
            }

            if (_filterModel.IsHaveNullVoice != null)
            {
                _paging.Where("IsHaveNullVoice = {0}", (bool)_filterModel.IsHaveNullVoice ? "1" : "0");
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

            if (!string.IsNullOrEmpty(_filterModel.CountAnswersFrom))
            {
                _paging.Where("(Select Count(AnswerId) FROM [Voice].[Answer] WHERE [FKIDTheme] = VoiceThemeID) >= {0}", _filterModel.CountAnswersFrom.TryParseInt(-1));
            }

            if (!string.IsNullOrEmpty(_filterModel.CountAnswersTo))
            {
                _paging.Where("(Select Count(AnswerId) FROM [Voice].[Answer] WHERE [FKIDTheme] = VoiceThemeID) <= {0}", _filterModel.CountAnswersTo.TryParseInt(-1));
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy(new SqlCritera("DateAdded", "", SqlSort.Desc));
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
