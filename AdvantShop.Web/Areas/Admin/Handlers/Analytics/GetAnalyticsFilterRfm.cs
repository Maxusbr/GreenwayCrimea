using System;
using System.Linq;
using AdvantShop.Web.Admin.Handlers.Analytics.Reports;
using AdvantShop.Web.Admin.Models.Analytics;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Analytics
{
    public class GetAnalyticsFilterRfm
    {
        private readonly BaseFilterModel _filterModel;
        private readonly string _group;
        private readonly DateTime _from;
        private readonly DateTime _to;

        public GetAnalyticsFilterRfm(BaseFilterModel filterModel, string group, DateTime from, DateTime to)
        {
            _filterModel = filterModel;
            _group = group;
            _from = from;
            _to = to;
        }

        public FilterResult<AnalyticsFilterRfmModel> Execute()
        {
            var model = new FilterResult<AnalyticsFilterRfmModel>();

            if (string.IsNullOrEmpty(_group))
                return model;

            var arr = _group.ToUpper().Split('_');
            if (arr.Length != 4)
                return model;
            
            var handler = new RfmAnalysisHandler(_from, _to);
            var data = handler.GetDataItems();

            if (data == null)
                return model;

            var items = arr[1] == "M"
                ? data.Where(x => x.R.ToString() == arr[2] && x.M.ToString() == arr[3]).ToList()
                : data.Where(x => x.R.ToString() == arr[2] && x.F.ToString() == arr[3]).ToList();

            if (items.Count == 0)
                return model;

            var customerIds = items.Select(x => x.CustomerId).ToList(); //.Select(x => "'" + x.CustomerId + "'");

            model.TotalItemsCount = customerIds.Count;
            model.TotalPageCount = (int)Math.Ceiling((double)model.TotalItemsCount / _filterModel.ItemsPerPage);

            model.DataItems =
                customerIds.Skip((_filterModel.Page - 1) * _filterModel.ItemsPerPage)
                    .Take(_filterModel.ItemsPerPage)
                    .Select(x => new AnalyticsFilterRfmModel() { CustomerId = x })
                    .ToList();

            //var paging = new SqlPaging()
            //{
            //    ItemsPerPage = _filterModel.ItemsPerPage,
            //    CurrentPageIndex = _filterModel.Page
            //};

            //paging.Select(
            //    "CustomerId",
            //    "(Firstname + ' ' + Lastname) as Name",
            //    "Email",
            //    "Phone");

            //paging.From("[Order].[OrderCustomer]");
            //paging.Where("CustomerId In (" + String.Join(",", customerIds) + ")");


            //model.TotalItemsCount = paging.TotalRowsCount;
            //model.TotalPageCount = paging.PageCount();

            //model.DataItems = paging.PageItemsList<AnalyticsFilterRfmModel>();

            return model;
        }

    }
}
