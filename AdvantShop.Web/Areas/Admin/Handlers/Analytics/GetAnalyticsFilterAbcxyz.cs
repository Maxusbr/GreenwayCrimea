using System;
using System.Linq;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Handlers.Analytics.Reports;
using AdvantShop.Web.Admin.Models.Analytics;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Analytics
{
    public class GetAnalyticsFilterAbcxyz
    {
        private readonly BaseFilterModel _filterModel;
        private readonly DateTime _from;
        private readonly DateTime _to;
        private string _group;

        public GetAnalyticsFilterAbcxyz(BaseFilterModel filterModel, DateTime from, DateTime to, string group)
        {
            _filterModel = filterModel;
            _from = from;
            _to = to;
            _group = group;
        }

        public FilterResult<AnalyticsFilterAbcModel> Execute()
        {
            var model = new FilterResult<AnalyticsFilterAbcModel>();

            if (string.IsNullOrEmpty(_group))
                return model;

            _group = _group.ToUpper();

            var handler = new AbcxyzAnalysisHandler(_from, _to);
            var data = handler.GetDataItems();

            if (data == null)
                return model;

            var items = data.Where(x => x.Abc == _group[0].ToString() && x.Xyz == _group[1].ToString()).ToList();
            if (items.Count == 0)
                return model;

            var artNos = items.Select(x => x.ArtNo).ToList();  //.Select(x => "'" + x.ArtNo.Replace("'", string.Empty) + "'").ToList();

            model.TotalItemsCount = artNos.Count;
            model.TotalPageCount = (int)Math.Ceiling((double)model.TotalItemsCount / _filterModel.ItemsPerPage);

            model.DataItems =
                artNos.Skip((_filterModel.Page - 1)*_filterModel.ItemsPerPage)
                    .Take(_filterModel.ItemsPerPage)
                    .Select(x => new AnalyticsFilterAbcModel() {ArtNo = x})
                    .ToList();


            //var paging = new SqlPaging()
            //{
            //    ItemsPerPage = _filterModel.ItemsPerPage,
            //    CurrentPageIndex = _filterModel.Page
            //};


            //paging.Select(
            //    "ProductId",
            //    "ArtNo", 
            //    "Name",
            //    "Price",
            //    "CurrencySymbol as Code",
            //    "CurrencyCode as CurrencyIso3",
            //    "CurrencyValue",
            //    "IsCodeBefore"
            //    );

            //paging.From("[Order].[OrderItems]");
            //paging.Left_Join("[Order].[OrderCurrency] On [OrderCurrency].[OrderId] = [OrderItems].[OrderId]");
            //paging.Where("OrderItemId in (Select Min(OrderItemId) From [Order].[OrderItems] Where ArtNo in (" + String.Join(",", artNos) + ") Group By OrderItemId) ");

            //model.TotalItemsCount = paging.TotalRowsCount;
            //model.TotalPageCount = paging.PageCount();

            //model.DataItems = paging.PageItemsList<AnalyticsFilterAbcModel>();

            return model;
        }

    }
}
