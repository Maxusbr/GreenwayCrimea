using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL2;
using AdvantShop.Repository.Currencies;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Settings.Catalog
{
    public class GetCurrenciesHandler
    {
        private readonly CurrencyFilterModel _filterModel;
        private SqlPaging _paging;

        public GetCurrenciesHandler(CurrencyFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<Currency> Execute()
        {
            var model = new FilterResult<Currency>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<Currency>();

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
                "CurrencyID",
                "Name",
                "Code".AsSqlField("Symbol"),
                "CurrencyValue".AsSqlField("Rate"),
                "CurrencyISO3".AsSqlField("Iso3"),
                "CurrencyNumIso3".AsSqlField("NumIso3"),
                "IsCodeBefore",
                "EnablePriceRounding",
                "RoundNumbers");

            _paging.From("[Catalog].[Currency]");

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (_filterModel.Search.IsNotEmpty())
            {
                _paging.Where("Name LIKE '%'+{0}+'%'", _filterModel.Search);
            }
            if (_filterModel.Name.IsNotEmpty())
            {
                _paging.Where("Name LIKE '%'+{0}+'%'", _filterModel.Name);
            }
            if (_filterModel.Iso3.IsNotEmpty())
            {
                _paging.Where("CurrencyISO3 LIKE '%'+{0}+'%'", _filterModel.Iso3);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy("Name");
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