using System.Collections.Generic;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Infrastructure.Handlers
{
    public abstract class AbstractHandler<TKey, TIds, TResult> : IHandler<TResult, TIds> where TKey : BaseFilterModel
    {
        private SqlPaging _paging;
        protected readonly TKey FilterModel;

        protected AbstractHandler(TKey filterModel)
        {
            FilterModel = filterModel;
        }

        public FilterResult<TResult> Execute()
        {
            var model = new FilterResult<TResult>();
            _paging = _paging != null ? _paging : GetPaging();

            model.PageIndex = FilterModel.Page;
            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = T("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < FilterModel.Page && FilterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<TResult>();

            return model;
        }

        public List<TIds> GetItemsIds(string field)
        {
            _paging = _paging != null ? _paging : GetPaging();

            return _paging.ItemsIds<TIds>(field);
        }

        protected string T(string t, object param1)
        {
            return LocalizationService.GetResource(t);
        }

        private SqlPaging GetPaging( )
        {
            _paging = new SqlPaging
            {
                ItemsPerPage = FilterModel.ItemsPerPage,
                CurrentPageIndex = FilterModel.Page
            };

            _paging = Select(_paging);
            _paging = Filter(_paging);
            _paging = Sorting(_paging);
            return _paging;
        }

        protected virtual SqlPaging Select(SqlPaging paging)
        {
            return paging;
        }

        protected virtual SqlPaging Filter(SqlPaging paging)
        {
            return paging;
        }

        protected virtual SqlPaging Sorting(SqlPaging paging)
        {
            return paging;
        }
    }
}