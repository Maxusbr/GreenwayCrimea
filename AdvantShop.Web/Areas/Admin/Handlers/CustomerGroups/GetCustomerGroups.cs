using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.CustomerGroups;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.CustomerGroups
{
    public class GetCustomerGroups
    {
        private readonly CustomerGroupsFilterModel _filterModel;
        private SqlPaging _paging;

        public GetCustomerGroups(CustomerGroupsFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<AdminCustomerGroupModel> Execute()
        {
            var model = new FilterResult<AdminCustomerGroupModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<AdminCustomerGroupModel>();
            
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
                "CustomerGroupId",
                "GroupName",
                "GroupDiscount",
                "MinimumOrderPrice"
                );

            _paging.From("[Customers].[CustomerGroup]");

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                _paging.Where("GroupName LIKE '%'+{0}+'%'", _filterModel.Search);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy("GroupDiscount");
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