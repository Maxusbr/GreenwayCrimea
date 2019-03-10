using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Models;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.SQL2;
using AdvantShop.Helpers;
using AdvantShop.Models.Catalog;
using AdvantShop.Repository.Currencies;
using AdvantShop.ViewModel.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvantShop.Handlers.Catalog
{
    public class ProductListHandler
    {
        #region Fields

        private SqlPaging _paging;
        private readonly bool _indepth;
        private readonly int _currentPageIndex = 1;
        private readonly CategoryModel _categoryModel;
        private readonly EProductOnMain _type;
        private readonly int _productListId;

        private ProductListPagingModel _model;

        #endregion

        public ProductListHandler(EProductOnMain type, bool indepth, CategoryModel categoryModel, int? productListId)
        {
            _type = type;
            _indepth = indepth;
            _categoryModel = categoryModel;
            _currentPageIndex = categoryModel.Page ?? 1;
            _productListId = productListId ?? 0;
        }

        public ProductListPagingModel Get()
        {
            _model = new ProductListPagingModel(_indepth);

            BuildPaging();

            var totalCount = _paging.TotalRowsCount;
            var totalPages = _paging.PageCount(totalCount);

            _model.Pager = new Pager()
            {
                TotalItemsCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = _currentPageIndex
            };

            if ((totalPages < _currentPageIndex && _currentPageIndex > 1) || _currentPageIndex < 0)
            {
                return _model;
            }

            var products = _paging.PageItemsList<ProductModel>();

            _model.Products = new ProductViewModel(products);

            BuildExludingFilters();
            GetViewMode();

            return _model;
        }

        private void BuildPaging()
        {
            _paging = new SqlPaging("ProductList");
            _paging.Select(
                "[Product].[ProductID]",
                "CountPhoto",
                "Photo.PhotoId",
                "PhotoName",
                "Photo.Description".AsSqlField("PhotoDescription"),
                "BriefDescription",
                "Product.ArtNo",
                "Product.Name",
                "Recomended as Recomend",
                "Bestseller",
                "New",
                "OnSale as Sales",
                "Product.Discount",
                "Product.DiscountAmount",
                "Offer.OfferID",
                "MaxAvailable".AsSqlField("Amount"),
                "MinAmount",
                "MaxAmount",
                "Product.Enabled",
                "AllowPreOrder",
                "Ratio",
                "Product.UrlPath",
                "Offer.ColorID",
                "DateAdded",
                "Comments",
                "CurrencyValue",
                "Gifts",
                "MinPrice".AsSqlField("BasePrice")
                );

            _paging.From("[Catalog].[Product]");
            _paging.Left_Join("[Catalog].[ProductExt] ON [Product].[ProductID] = [ProductExt].[ProductID]");
            _paging.Left_Join("[Catalog].[Photo] ON [Photo].[PhotoId] = [ProductExt].[PhotoId]");
            _paging.Left_Join("[Catalog].[Offer] ON [ProductExt].[OfferID] = [Offer].[OfferID]");
            _paging.Inner_Join("[Catalog].[Currency] ON [Currency].[CurrencyID] = [Product].[CurrencyID]");
            
            if (SettingsCatalog.ComplexFilter)
            {
                _paging.Select(
                      "Colors",
                      "NotSamePrices".AsSqlField("MultiPrices")
                 );
            }
            else
            {
                _paging.Select(
                        "null".AsSqlField("Colors"),
                        "0".AsSqlField("MultiPrices")
                   );
            }

            BuildSorting();
            BuildFilter();

            _paging.ItemsPerPage = _currentPageIndex != 0 ? SettingsCatalog.ProductsPerPage : int.MaxValue;
            _paging.CurrentPageIndex = _currentPageIndex != 0 ? _currentPageIndex : 1;
        }

        private void BuildSorting()
        {
            var sort = _categoryModel.Sort ?? ESortOrder.NoSorting;
            _model.Filter.Sorting = sort;

            if (SettingsCatalog.MoveNotAvaliableToEnd)
            {
                _paging.OrderByDesc("(CASE WHEN Price=0 THEN 0 ELSE 1 END)".AsSqlField("TempSort"));
                _paging.OrderByDesc("AmountSort".AsSqlField("TempAmountSort"));
            }

            switch (sort)
            {
                case ESortOrder.AscByName:
                    _paging.OrderBy("Product.Name".AsSqlField("NameSort"));
                    break;
                case ESortOrder.DescByName:
                    _paging.OrderByDesc("Product.Name".AsSqlField("NameSort"));
                    break;

                case ESortOrder.AscByPrice:
                    _paging.OrderBy("PriceTemp");
                    break;

                case ESortOrder.DescByPrice:
                    _paging.OrderByDesc("PriceTemp");
                    break;

                case ESortOrder.AscByRatio:
                    _paging.OrderBy("Ratio".AsSqlField("RatioSort"));
                    break;

                case ESortOrder.DescByRatio:
                    _paging.OrderByDesc("Ratio".AsSqlField("RatioSort"));
                    break;

                case ESortOrder.DescByAddingDate:
                    _paging.OrderByDesc("DateAdded".AsSqlField("DateAddedSort"));
                    break;

                default:
                    switch (_type)
                    {
                        case EProductOnMain.Best:
                            _paging.OrderBy("SortBestseller".AsSqlField("Sorting"));
                            _paging.OrderByDesc("DateAdded".AsSqlField("DateAddedSorting"));
                            break;
                        case EProductOnMain.New:
                            _paging.OrderBy("SortNew".AsSqlField("Sorting"));
                            _paging.OrderByDesc("DateAdded".AsSqlField("DateAddedSorting"));
                            break;
                        case EProductOnMain.Sale:
                            _paging.OrderBy("SortDiscount".AsSqlField("Sorting"));
                            _paging.OrderByDesc("Discount".AsSqlField("DiscountSorting"));
                            _paging.OrderByDesc("DiscountAmount".AsSqlField("DiscountAmountSorting"));
                            break;
                        case EProductOnMain.List:
                            _paging.OrderBy("[Product_ProductList].[SortOrder]".AsSqlField("Sorting"));
                            break;
                    }
                    break;
            }
        }

        private void BuildFilter()
        {
            _paging.Where("Product.Enabled={0}", true);
            _paging.Where("AND CategoryEnabled={0}", true);
            _paging.Where("AND (Offer.Main={0} OR Offer.Main IS NULL)", true);

            if (_categoryModel.CategoryId > 0)
            {
                _paging.Where(
                  "AND Exists( select 1 from [Catalog].[ProductCategories] INNER JOIN [Settings].[GetChildCategoryByParent]({0}) AS hCat ON hCat.id = [ProductCategories].[CategoryID] and  ProductCategories.ProductId = [Product].[ProductID])",
                  _categoryModel.CategoryId);
            }

            if (_type == EProductOnMain.Best)
            {
                _paging.Where("AND Bestseller=1");
            }
            else if (_type == EProductOnMain.New)
            {
                _paging.Where("AND New=1");
            }
            else if (_type == EProductOnMain.Sale)
            {
                _paging.Where("AND (Discount>0 or DiscountAmount>0)");
            }
            else if (_type == EProductOnMain.List && _productListId != 0)
            {
                _paging.Left_Join("[Catalog].[Product_ProductList] ON [Product_ProductList].[ProductId] = [Product].[ProductId]");
                _paging.Where("AND [Product_ProductList].[ListId] = {0}", _productListId);
            }

            var currency = CurrencyService.CurrentCurrency;
            if (SettingsCatalog.DefaultCurrencyIso3 != currency.Iso3)
            {
                _paging.Where("", currency.Iso3);
            }

            if (!string.IsNullOrEmpty(_categoryModel.Brand))
            {
                var brandIds = _categoryModel.Brand.Split(',').Select(item => item.TryParseInt()).Where(id => id != 0).ToList();
                if (brandIds.Count > 0)
                {
                    _model.Filter.BrandIds = brandIds;
                    _paging.Where("AND Product.BrandID IN ({0})", brandIds.ToArray());
                }
            }

            if (_categoryModel.PriceFrom.HasValue || _categoryModel.PriceTo.HasValue)
            {
                var pricefrom = _categoryModel.PriceFrom ?? 0;
                var priceto = _categoryModel.PriceTo ?? int.MaxValue;

                _model.Filter.PriceFrom = pricefrom;
                _model.Filter.PriceTo = priceto;

                _paging.Where("and (ProductExt.PriceTemp>= {0} ", pricefrom * currency.Rate);
                _paging.Where("and ProductExt.PriceTemp <= {0})", priceto * currency.Rate);
            }

            if (SettingsCatalog.ShowOnlyAvalible || _categoryModel.Available)
            {
                _paging.Where("AND MaxAvailable>{0}", 0);
            }

            if (!string.IsNullOrEmpty(_categoryModel.Size))
            {
                var sizeIds = _categoryModel.Size.Split(',').Select(item => item.TryParseInt()).Where(id => id != 0).ToList();
                if (sizeIds.Count > 0)
                {
                    _model.Filter.SizeIds = sizeIds;
                    _paging.Where(
                        SettingsCatalog.ShowOnlyAvalible || _categoryModel.Available
                            ? "and Exists(Select 1 from [Catalog].[Offer] where Offer.[SizeID] IN ({0}) and Offer.ProductId = [Product].[ProductID] AND Offer.amount > 0)"
                            : "and Exists(Select 1 from [Catalog].[Offer] where Offer.[SizeID] IN ({0}) and Offer.ProductId = [Product].[ProductID])",
                        sizeIds.ToArray());
                }
            }

            if (!string.IsNullOrEmpty(_categoryModel.Color))
            {
                var colorIds = _categoryModel.Color.Split(',').Select(item => item.TryParseInt()).Where(id => id != 0).ToList();
                if (colorIds.Count > 0)
                {
                    _model.Filter.ColorIds = colorIds;
                    _paging.Where(
                        SettingsCatalog.ShowOnlyAvalible || _categoryModel.Available
                            ? "and Exists(Select 1 from [Catalog].[Offer] where Offer.[ColorID] IN ({0}) and Offer.ProductId = [Product].[ProductID] AND Offer.amount > 0)"
                            : "and Exists(Select 1 from [Catalog].[Offer] where Offer.[ColorID] IN ({0}) and Offer.ProductId = [Product].[ProductID])",
                        colorIds.ToArray());
                }

                if (SettingsCatalog.ComplexFilter)
                {
                    _paging.Select(
                        string.Format(
                            "(select Top 1 PhotoName from catalog.Photo inner join Catalog.Offer on Photo.objid=Offer.Productid and Type='product'" +
                            " Where Offer.ProductId=Product.ProductId and Photo.ColorID in({0}) Order by Photo.Main Desc, Photo.PhotoSortOrder)",
                            _model.Filter.ColorIds.AggregateString(",")).AsSqlField("AdditionalPhoto"));
                }
                else
                {
                    _paging.Select("null".AsSqlField("AdditionalPhoto"));
                }
            }
            else
            {
                _paging.Select("null".AsSqlField("AdditionalPhoto"));
            }

            if (!string.IsNullOrWhiteSpace(_categoryModel.TagUrl) && _categoryModel.TagId > 0)
            {
                _paging.Where("AND Exists(Select 1 from [Catalog].[TagMap] where TagMap.[ObjId] = Product.[ProductID] and TagMap.[Type] ='Product' and TagMap.TagId={0})", _categoryModel.TagId);
            }

            _model.Filter.Available = _categoryModel.Available;

        }

        private void BuildExludingFilters()
        {
            if (!SettingsCatalog.ExcludingFilters)
                return;
            var tasks = new List<Task>();

            if (SettingsCatalog.ShowProducerFilter)
            {
               var task = Task.Factory.StartNew(() =>
                {
                    _model.Filter.AvailableBrandIds =
                        _paging.GetCustomData("Product.BrandID", " AND Product.BrandID is not null",
                            reader => SQLDataHelper.GetInt(reader, "BrandID"), true);
                });
                tasks.Add(task);
            }

            if (SettingsCatalog.ShowSizeFilter)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    _model.Filter.AvailableSizeIds =
                        _paging.GetCustomData("sizeOffer.SizeID", " AND sizeOffer.SizeID is not null",
                            reader => SQLDataHelper.GetInt(reader, "SizeID"), true,
                            "Left JOIN [Catalog].[Offer] as sizeOffer ON [Product].[ProductID] = [sizeOffer].[ProductID]");
                });
                tasks.Add(task);
            }

            if (SettingsCatalog.ShowColorFilter)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    _model.Filter.AvailableColorIds =
                        _paging.GetCustomData("colorOffer.ColorID", " AND colorOffer.ColorID is not null",
                            reader => SQLDataHelper.GetInt(reader, "ColorID"), true,
                            "Left JOIN [Catalog].[Offer] as colorOffer ON [Product].[ProductID] = [colorOffer].[ProductID]");
                });
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray(), 1000);
        }

        public ProductListPagingModel GetForFilter()
        {
            _model = new ProductListPagingModel(_indepth);

            BuildPaging();
            BuildExludingFilters();

            return _model;
        }

        public ProductListPagingModel GetForFilterProductCount()
        {
            _model = new ProductListPagingModel(_indepth);

            BuildPaging();
            //BuildExludingFilters();

            var totalCount = _paging.TotalRowsCount;

            _model.Pager = new Pager
            {
                TotalItemsCount = totalCount,
                CurrentPage = _currentPageIndex
            };

            return _model;
        }

        private void GetViewMode()
        {
            var cookieMode = CommonHelper.GetCookieString("viewmode");
            var mode = SettingsCatalog.EnabledCatalogViewChange
                ? cookieMode.Parse<ProductViewMode>(SettingsCatalog.DefaultCatalogView)
                : SettingsCatalog.DefaultCatalogView;
            _model.Filter.ViewMode = mode.ToString().ToLower();
            _model.Filter.AllowChangeViewMode = SettingsCatalog.EnabledCatalogViewChange;
        }
    }
}