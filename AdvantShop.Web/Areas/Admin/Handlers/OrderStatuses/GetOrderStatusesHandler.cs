using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.OrderStatuses;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.OrderStatuses
{
    public class GetOrderStatusesHandler
    {
        private readonly OrderStatusesFilterModel _filterModel;
        private SqlPaging _paging;

        public GetOrderStatusesHandler(OrderStatusesFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<OrderStatusModel> Execute()
        {
            var model = new FilterResult<OrderStatusModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<OrderStatusModel>();
            
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

            _paging.Select(
                "OrderStatusId",
                "StatusName",
                "IsDefault",
                "IsCanceled",
                "IsCompleted",
                "CommandID",
                "Color",
                "Hidden",
                "SortOrder"
                );

            _paging.From("[Order].[OrderStatus]");


            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.Name))
                _filterModel.Search = _filterModel.Name;

            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                _paging.Where("StatusName LIKE '%'+{0}+'%'", _filterModel.Search);
            }

            if (_filterModel.IsDefault != null)
            {
                _paging.Where("IsDefault = {0}", Convert.ToInt32(_filterModel.IsDefault.Value));
            }

            if (_filterModel.IsCanceled != null)
            {
                _paging.Where("IsCanceled = {0}", Convert.ToInt32(_filterModel.IsCanceled.Value));
            }

            if (_filterModel.IsCompleted != null)
            {
                _paging.Where("IsCompleted = {0}", Convert.ToInt32(_filterModel.IsCompleted.Value));
            }

            if (_filterModel.CommandId != null)
            {
                _paging.Where("CommandId = {0}", _filterModel.CommandId.Value);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy("SortOrder");
                return;
            }

            var sorting = _filterModel.Sorting.ToLower().Replace("formatted", "").Replace("command", "commandid");

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