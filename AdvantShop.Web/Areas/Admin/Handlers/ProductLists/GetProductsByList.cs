using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Catalog;
using AdvantShop.Web.Admin.Models.ProductLists;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.ProductLists
{
    public class GetProductsByList
    {
        private CatalogFilterModel _filterModel;
        private SqlPaging _paging;

        public GetProductsByList(CatalogFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public FilterResult<ProductListItemModel> Execute()
        {
            var model = new FilterResult<ProductListItemModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<ProductListItemModel>();
            
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
                "Product.ProductId",
                "Product.Name",
                "Product.ArtNo".AsSqlField("ProductArtNo"),
                "Product_ProductList.ListId",
                "Product_ProductList.SortOrder"
                );
            _paging.From("[Catalog].[Product]");
            _paging.Left_Join("Catalog.Product_ProductList On Product_ProductList.ProductId = Product.ProductId");

            if (_filterModel.ListId != null)
                _paging.Where("Product_ProductList.ListId = {0}", _filterModel.ListId.Value);
            

            Sorting();
            Filter();
        }

        private void Filter()
        {
            if (!string.IsNullOrWhiteSpace(_filterModel.Search))
            {
                _paging.Where("(Product.ArtNo LIKE '%'+{0}+'%' OR Product.Name LIKE '%'+{0}+'%')", _filterModel.Search);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.ArtNo))
            {
                _paging.Where("Product.ArtNo LIKE '%'+{0}+'%'", _filterModel.ArtNo);
            }

            if (!string.IsNullOrWhiteSpace(_filterModel.Name))
            {
                _paging.Where("Product.Name LIKE '%'+{0}+'%'", _filterModel.Name);
            }
        }

        private void Sorting()
        {
            if (string.IsNullOrEmpty(_filterModel.Sorting) || _filterModel.SortingType == FilterSortingType.None)
            {
                _paging.OrderBy("Product_ProductList.SortOrder");
                _paging.OrderBy("Product.ProductId");
                return;
            }

            var sorting = _filterModel.Sorting.ToLower();

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