using AdvantShop.Areas.Mobile.Models.Catalog;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Models;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.Core.SQL2;
using AdvantShop.Helpers;
using System.Web.WebPages;

namespace AdvantShop.Areas.Mobile.Handlers.Catalog
{
    public class ProductListMobileHandler
    {
        #region Fields

        private SqlPaging _paging;
        private readonly EProductOnMain _type;
        private readonly int _currentPageIndex = 1;
        private readonly ESortOrder _sort;
        private readonly int _productListId;

        private ProductListPagingMobileModel _model;

        #endregion

        #region Constructor

        public ProductListMobileHandler(EProductOnMain type, int? page, ESortOrder sort, int? productListId)
        {
            _type = type;
            _currentPageIndex = page ?? 1;
            _sort = sort;
            _productListId = productListId ?? 0;
        }

        #endregion

        public ProductListPagingMobileModel Get()
        {
            _model = new ProductListPagingMobileModel(true);

            BuildPaging();
            BuildSorting();
            BuildFilter();

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

            _model.Products = new ProductViewMobileModel(products);

            GetViewMode();

            return _model;
        }

        private void BuildPaging()
        {
            _paging = new SqlPaging();
            _paging.Select(
                "Product.ProductID",
                //"CountPhoto",
                "Photo.PhotoId",
                "PhotoName",
                "Photo.Description".AsSqlField("PhotoDescription"),
                "Product.ArtNo",
                "Product.Name",
                "Recomended",
                "Product.Bestseller",
                "Product.New",
                "Product.OnSale as Sale",
                "Product.Discount",
                "Product.DiscountAmount",
                //"Product.BriefDescription",
                "Product.MinAmount",
                "Product.MaxAmount",
                "Product.Enabled",
                //"Product.AllowPreOrder",
                //"Product.Ratio",
                "Product.UrlPath",
                "Product.DateAdded",
                "Offer.OfferID",
                "Offer.ColorID",
                "Gifts",
                "MaxAvailable".As("Amount"),
                //"ShoppingCartItemId",
                //"Comments",
                "CurrencyValue"//,
                //"MinPrice".AsSqlField("BasePrice")
                //"Brand.BrandName as BrandName",
                //"Brand.UrlPath as BrandUrlPath"
                );

            _paging.From("[Catalog].[Product]");
            _paging.Left_Join("[Catalog].[ProductExt] ON [Product].[ProductID] = [ProductExt].[ProductID]");
            _paging.Left_Join("[Catalog].[Photo] ON [Photo].[PhotoId] = [ProductExt].[PhotoId]");
            _paging.Left_Join("[Catalog].[Offer] ON [ProductExt].[OfferID] = [Offer].[OfferID]");
            _paging.Inner_Join("[Catalog].[Currency] ON [Currency].[CurrencyID] = [Product].[CurrencyID]");
            //_paging.Left_Join("[Catalog].[Brand] ON [Product].[BrandID] = [Brand].[BrandID]");

            //_paging.Left_Join("[Catalog].[ShoppingCart] ON [ShoppingCart].[OfferID] = [Offer].[OfferID] AND [ShoppingCart].[ShoppingCartType] = 3 AND [ShoppingCart].[CustomerID] = {0}", CustomerContext.CustomerId);

            //_paging.Inner_Join("[Catalog].[ProductCategories] ON [ProductCategories].[CategoryID] = {0} and  ProductCategories.ProductId = [Product].[ProductID]", _category.CategoryId);
            if (SettingsCatalog.ComplexFilter)
            {
                _paging.Select(
                      "Colors",
                      "NotSamePrices".AsSqlField("MultiPrices"),
                      "MinPrice".AsSqlField("BasePrice")
                 );
            }
            else
            {
                _paging.Select(
                        "null".AsSqlField("Colors"),
                        "0".AsSqlField("MultiPrices"),
                        "Price".AsSqlField("BasePrice")
                   );
            }

            _paging.ItemsPerPage = _currentPageIndex != 0 ? SettingsCatalog.ProductsPerPage : int.MaxValue;
            _paging.CurrentPageIndex = _currentPageIndex != 0 ? _currentPageIndex : 1;
        }

        private void BuildSorting()
        {
            _model.Filter.Sorting = _sort;

            if (SettingsCatalog.MoveNotAvaliableToEnd)
            {
                _paging.OrderByDesc("(CASE WHEN Price=0 THEN 0 ELSE 1 END)".AsSqlField("TempSort"));
                _paging.OrderByDesc("AmountSort".AsSqlField("TempAmountSort"));
            }

            switch (_sort)
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

            if (SettingsCatalog.ShowOnlyAvalible)
            {
                _paging.Where("AND MaxAvailable>{0}", 0);
            }
        }

        private void GetViewMode()
        {
            var cookieMode = CommonHelper.GetCookieString("mobile_viewmode");
            var mode = cookieMode.Parse<ProductViewMode>(ProductViewMode.List);
            if (mode != ProductViewMode.List && mode != ProductViewMode.Tile)
            {
                mode = ProductViewMode.List;
                CommonHelper.SetCookie("mobile_viewmode", mode.ToString().ToLower());
            }
            _model.Filter.ViewMode = mode.ToString().ToLower();
        }
    }
}