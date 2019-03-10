using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.Settings.Checkout
{
    public class GetTaxesHandler
    {
        private readonly TaxesFilterModel _filterModel;
        private SqlPaging _paging;

        public GetTaxesHandler(TaxesFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<TaxModel> Execute()
        {
            var model = new FilterResult<TaxModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<TaxModel>();

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
                "TaxId",
                "Name",
                "Enabled",
                "Rate",
                "TaxType",
                "(SELECT COUNT(DISTINCT ProductId) FROM Catalog.[Product] WHERE [TaxId] = Tax.TaxId)".AsSqlField("ProductsCount")
                //"ShowInPrice"
                );

            _paging.From("[Catalog].[Tax]");

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
            if (_filterModel.ShowInPrice.HasValue)
            {
                _paging.Where("ShowInPrice = {0}", _filterModel.ShowInPrice.Value);
            }
            if (_filterModel.Enabled.HasValue)
            {
                _paging.Where("Enabled = {0}", _filterModel.Enabled.Value);
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