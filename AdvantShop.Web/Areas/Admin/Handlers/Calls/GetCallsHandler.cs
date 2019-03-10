using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Calls;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Calls
{
    public class GetCallsHandler
    {
        private CallsFilterModel _filterModel;
        private SqlPaging _paging;

        public GetCallsHandler(CallsFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<CallModel> Execute()
        {
            var model = new FilterResult<CallModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
                return model;
            
            model.DataItems = _paging.PageItemsList<CallModel>();
            
            model.TotalString += LocalizationService.GetResourceFormat("Admin.Calls.Grid.TotalString", model.TotalItemsCount);

            return model;
        }

        public List<int> GetItemsIds()
        {
            GetPaging();

            return _paging.ItemsIds<int>("Call.Id");
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
                "CallId",
                "Type",
                "SrcNum",
                "DstNum",
                "Extension",
                "CallDate",
                "CallAnswerDate",
                "Duration",
                "RecordLink",
                "CalledBack",
                "HangupStatus",
                "OperatorType"
                );

            _paging.From("Customers.Call");

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrWhiteSpace(_filterModel.Search))
            {
                _paging.Where("SrcNum LIKE '%'+{0}+'%' OR DstNum LIKE '%'+{0}+'%' OR Extension LIKE '%'+{0}+'%' OR DstNum LIKE '%'+{0}+'%' OR DstNum LIKE '%'+{0}+'%'", _filterModel.Search);
            }

            if (_filterModel.Type.HasValue)
            {
                _paging.Where("Type = {0}", _filterModel.Type.Value.ToString());
            }
            if (!string.IsNullOrWhiteSpace(_filterModel.SrcNum))
            {
                _paging.Where("SrcNum LIKE '%'+{0}+'%'", _filterModel.SrcNum);
            }
            if (!string.IsNullOrWhiteSpace(_filterModel.DstNum))
            {
                _paging.Where("DstNum LIKE '%'+{0}+'%'", _filterModel.DstNum);
            }
            if (!string.IsNullOrWhiteSpace(_filterModel.Extension))
            {
                _paging.Where("Extension LIKE '%'+{0}+'%'", _filterModel.Extension);
            }
            if (_filterModel.DurationFrom.HasValue)
            {
                _paging.Where("Duration >= {0}", _filterModel.DurationFrom.Value);
            }
            if (_filterModel.DurationTo.HasValue)
            {
                _paging.Where("Duration <= {0}", _filterModel.DurationTo.Value);
            }
            if (_filterModel.CallDateFrom.HasValue)
            {
                _paging.Where("CallDate >= {0}", _filterModel.CallDateFrom.Value);
            }
            if (_filterModel.CallDateTo.HasValue)
            {
                _paging.Where("CallDate <= {0}", _filterModel.CallDateTo.Value);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderByDesc("Call.CallDate");
                return;
            }

            string sorting = _filterModel.Sorting.ToLower().Replace("formatted", "");
            
            var field = _paging.SelectFields().FirstOrDefault(x => x.FieldName == sorting);
            if (field != null)
            {
                if (_filterModel.SortingType == FilterSortingType.Asc)
                {
                    _paging.OrderBy(field);
                }
                else
                {
                    _paging.OrderByDesc(field);
                }
            }
        }
    }
}
