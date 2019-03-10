using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Subscription;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Customers;
using System;

namespace AdvantShop.Web.Admin.Handlers.Subscription
{
    public class GetSubscription
    {

        private SubscriptionFilterModel _filterModel;
        private SqlPaging _paging;

        public GetSubscription(SubscriptionFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<SubscriptionFilterResultModel> Execute()
        {
            var model = new FilterResult<SubscriptionFilterResultModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Subscribe.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<SubscriptionFilterResultModel>();

            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("Id");
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
                "Email",
                "Subscribe".AsSqlField("Enabled"),
                "SubscribeDate",
                "UnsubscribeDate",
                "UnsubscribeReason");

            _paging.From("[Customers].[Subscription]");

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrWhiteSpace(_filterModel.Search))
            {
                _filterModel.Email = _filterModel.Search;
            }
            
            if (!string.IsNullOrWhiteSpace(_filterModel.Email))
            {
                _paging.Where("Email LIKE '%'+{0}+'%'", _filterModel.Email);
            }

            if (_filterModel.Enabled != null)
            {
                _paging.Where("Subscribe = {0}", (bool)_filterModel.Enabled ? "1" : "0");
            }

            if (!string.IsNullOrEmpty(_filterModel.UnsubscribeReason))
            {
                _paging.Where("UnsubscribeReason LIKE '%'+{0}+'%'", _filterModel.UnsubscribeReason);
            }

            DateTime UnsFrom, UnsTo;

            if (!string.IsNullOrWhiteSpace(_filterModel.UnSubscribeFrom) && DateTime.TryParse(_filterModel.UnSubscribeFrom, out UnsFrom))
            {
                _paging.Where("UnsubscribeDate >= {0}", UnsFrom);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.UnsubscribeTo) && DateTime.TryParse(_filterModel.UnsubscribeTo, out UnsTo))
            {
                _paging.Where("UnsubscribeDate <= {0}", UnsTo);
            }

            DateTime SubFrom, SubTo;

            if (!string.IsNullOrWhiteSpace(_filterModel.SubscribeFrom) && DateTime.TryParse(_filterModel.SubscribeFrom, out SubFrom))
            {
                _paging.Where("SubscribeDate >= {0}", SubFrom);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.SubscribeTo) && DateTime.TryParse(_filterModel.SubscribeTo, out SubTo))
            {
                _paging.Where("SubscribeDate <= {0}", SubTo);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy(
                    new SqlCritera("Email", "", SqlSort.Asc)
                    );
                return;
            }

            var sorting = _filterModel.Sorting.ToLower();

            if (sorting == "subscribedatestr")
                sorting = "subscribedate";

            if (sorting == "unsubscribedatestr")
                sorting = "unsubscribedate";

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
