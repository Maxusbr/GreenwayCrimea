using System.Collections.Generic;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.SQL2;
using AdvantShop.Web.Admin.Models.Catalog;
using AdvantShop.Web.Admin.Models.MainPageProducts;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Handlers.MainPageProducts
{
    public class GetMainPageProducts
    {
        private readonly CatalogFilterModel _filterModel;
        private SqlPaging _paging;
        private readonly EProductOnMain _type;

        public GetMainPageProducts(CatalogFilterModel filterModel, EProductOnMain type)
        {
            _filterModel = filterModel;
            _type = type;
        }

        public FilterResult<MainPageProductModel> Execute()
        {
            var model = new FilterResult<MainPageProductModel>();

            GetPaging();

            model.TotalItemsCount = _paging.TotalRowsCount;
            model.TotalPageCount = _paging.PageCount();
            model.TotalString = LocalizationService.GetResourceFormat("Admin.Grid.FildTotal", model.TotalItemsCount);

            if (model.TotalPageCount < _filterModel.Page && _filterModel.Page > 1)
            {
                return model;
            }

            model.DataItems = _paging.PageItemsList<MainPageProductModel>();
            
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
                "Product.ProductID",
                "Product.Name",
                "Product.ArtNo".AsSqlField("ProductArtNo")
                );

            _paging.From("[Catalog].[Product]");
            
            switch (_type)
            {
                case EProductOnMain.Best:
                    _paging.Select("Bestseller");
                    _paging.Select("SortBestseller".AsSqlField("SortOrder"));
                    _paging.Where("Bestseller = 1");
                    break;

                case EProductOnMain.New:
                    _paging.Select("New");
                    _paging.Select("SortNew".AsSqlField("SortOrder"));
                    _paging.Where("New = 1");
                    break;

                case EProductOnMain.Sale:
                    _paging.Select("Discount");
                    _paging.Select("DiscountAmount");
                    _paging.Select("SortDiscount".AsSqlField("SortOrder"));
                    _paging.Where("(Discount <> 0 or DiscountAmount <> 0)");
                    break;
            }

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
                switch (_type)
                {
                    case EProductOnMain.Best:
                        _paging.OrderBy("SortBestseller".AsSqlField("SortOrder"));
                        _paging.OrderByDesc("DateAdded".AsSqlField("DateAddedSort"));
                        break;

                    case EProductOnMain.New:
                        _paging.OrderBy("SortNew".AsSqlField("SortOrder"));
                        _paging.OrderByDesc("DateAdded".AsSqlField("DateAddedSort"));
                        break;

                    case EProductOnMain.Sale:
                        _paging.OrderBy("SortDiscount".AsSqlField("SortOrder"));
                        _paging.OrderByDesc("Discount".AsSqlField("DiscountSort"));
                        _paging.OrderByDesc("DiscountAmount".AsSqlField("DiscountAmountSort"));
                        break;
                }
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

                switch (_type)
                {
                    case EProductOnMain.Best:
                    case EProductOnMain.New:
                        _paging.OrderByDesc("DateAdded".AsSqlField("DateAddedSort"));
                        break;

                    case EProductOnMain.Sale:
                        _paging.OrderByDesc("Discount".AsSqlField("DiscountSort"));
                        _paging.OrderByDesc("DiscountAmount".AsSqlField("DiscountAmountSort"));
                        break;
                }
            }
        }
    }
}