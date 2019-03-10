using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Admin.Models.CustomerSegments;

namespace AdvantShop.Web.Admin.Handlers.CustomerSegments
{
    public class GetCustomerSegments
    {
        private readonly CustomerSegmentsFilterModel _filterModel;
        private SqlPaging _paging;

        public GetCustomerSegments(CustomerSegmentsFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<CustomerSegmentViewModel> Execute()
        {
            var model = new FilterResult<CustomerSegmentViewModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<CustomerSegmentViewModel>();
            
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
                "Id", 
                "Name",
                "(Select Count(*) From [Customers].[CustomerSegment_Customer] as c Where c.SegmentId = [CustomerSegment].Id)".AsSqlField("CustomersCount"),
                "CreatedDate"
                );

            _paging.From("[Customers].[CustomerSegment]");

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrEmpty(_filterModel.Search))
            {
                _paging.Where("(Name LIKE '%'+{0}+'%')", _filterModel.Search);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.Name))
            {
                _paging.Where("(Name LIKE '%'+{0}+'%')", _filterModel.Name);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderByDesc("CreatedDate");
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