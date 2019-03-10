using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Coupons;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Coupons
{
    public class GetCoupons
    {
        private readonly CouponsFilterModel _filterModel;
        private SqlPaging _paging;

        public GetCoupons(CouponsFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<CouponModel> Execute()
        {
            var model = new FilterResult<CouponModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<CouponModel>();
            
            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();
            return _paging.ItemsIds<int>("CouponId");
        }

        private void GetPaging()
        {
            _paging = new SqlPaging()
            {
                ItemsPerPage = _filterModel.ItemsPerPage,
                CurrentPageIndex = _filterModel.Page
            };

            _paging.Select(
                "CouponId",
                "Code",
                "Type",
                "Value",
                "AddingDate",
                "ExpirationDate",
                "PossibleUses",
                "ActualUses",
                "Enabled",
                "MinimalOrderPrice"
                );

            _paging.From("[Catalog].[Coupon]");

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                _paging.Where("Code LIKE '%'+{0}+'%'", _filterModel.Search);
            }

            if (!string.IsNullOrEmpty(_filterModel.Code))
            {
                _paging.Where("Code LIKE '%'+{0}+'%'", _filterModel.Code);
            }

            if (!string.IsNullOrEmpty(_filterModel.Value))
            {
                _paging.Where("Value LIKE {0}+'%'", _filterModel.Value);
            }

            if (_filterModel.Type != null)
            {
                _paging.Where("Type = {0}", (int)_filterModel.Type.Value);
            }

            if (!string.IsNullOrEmpty(_filterModel.MinimalOrderPrice))
            {
                _paging.Where("MinimalOrderPrice LIKE {0}+'%'", _filterModel.MinimalOrderPrice);
            }

            if (_filterModel.Enabled != null)
            {
                _paging.Where("Enabled = {0}", Convert.ToBoolean(_filterModel.Enabled.Value));
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

            DateTime fromExp, toExp;

            if (!string.IsNullOrWhiteSpace(_filterModel.ExpirationDateFrom) && DateTime.TryParse(_filterModel.ExpirationDateFrom, out fromExp))
            {
                _paging.Where("ExpirationDate >= {0}", fromExp);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.ExpirationDateTo) && DateTime.TryParse(_filterModel.ExpirationDateTo, out toExp))
            {
                _paging.Where("ExpirationDate <= {0}", toExp);
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