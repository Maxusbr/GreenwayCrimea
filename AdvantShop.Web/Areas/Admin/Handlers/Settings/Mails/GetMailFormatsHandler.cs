using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Settings.Mails
{
    public class GetMailFormatsHandler
    {
        private readonly MailFormatsFilterModel _filterModel;
        private SqlPaging _paging;

        public GetMailFormatsHandler(MailFormatsFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<MailFormatsFilterModel> Execute()
        {
            var model = new FilterResult<MailFormatsFilterModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<MailFormatsFilterModel>();
            
            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("MailFormatID");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select(
                "MailFormatID",
                "FormatName",
                "Enable",
                "MailFormat.MailFormatTypeId".AsSqlField("MailFormatTypeId"),
                "MailFormat.SortOrder".AsSqlField("SortOrder"),
                "TypeName"
                );

            _paging.From("[Settings].[MailFormat]");
            _paging.Left_Join("[Settings].[MailFormatType] ON [MailFormatType].[MailFormatTypeId] = [MailFormat].[MailFormatTypeId]");


            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.FormatName))
                _filterModel.Search = _filterModel.FormatName;

            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                _paging.Where("FormatName LIKE '%'+{0}+'%'", _filterModel.Search);
            }

            if (_filterModel.Enable != null)
            {
                _paging.Where("Enable = {0}", Convert.ToInt32(_filterModel.Enable.Value));
            }

            if (_filterModel.MailFormatTypeId != null)
            {
                _paging.Where("MailFormat.MailFormatTypeId = {0}", _filterModel.MailFormatTypeId.Value);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy("MailFormat.SortOrder");
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