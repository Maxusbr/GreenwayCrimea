using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Areas.Mobile.Models.Catalog;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Models;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.FullSearch;
using AdvantShop.Core.SQL2;
using AdvantShop.Helpers;
using AdvantShop.FullSearch;

namespace AdvantShop.Areas.Mobile.Handlers.Catalog
{
    public class SearchPagingMobileHandler
    {
        #region Fields

        private readonly int _currentPageIndex;
        private SearchMobileModel _model;

        #endregion

        #region Constructor

        public SearchPagingMobileHandler(SearchMobileModel model)
        {
            _currentPageIndex = model.Page ?? 1;
            _model = model;
        }
        
        #endregion
        
        #region Help methods

        private SqlPaging BuildPaging()
        {
            var paging = new SqlPaging();
            paging.Select(
                "Product.ProductID",
                //"CountPhoto",
                "Photo.PhotoId",
                "PhotoName",
                "Photo.Description".AsSqlField("PhotoDescription"),
                "Product.ArtNo",
                "Product.Name",
                //"Recomended",
                //"Product.Bestseller",
                //"Product.New",
                //"Product.OnSale".AsSqlField("Sale"),
                "Product.Discount",
                "Product.DiscountAmount",
                //"Product.BriefDescription",
                "Product.MinAmount",
                "Product.MaxAmount",
                "Product.Enabled",
                "Product.AllowPreOrder",
                "Product.Ratio",
                "Product.UrlPath",
                "Product.DateAdded",
                "Offer.OfferID",
                "Offer.ColorID",
                "MaxAvailable".AsSqlField("Amount"),
                //"ShoppingCartItemId",
                //"Comments",
                "CurrencyValue"
                //"Brand.BrandName".AsSqlField("BrandName"),
                //"Brand.UrlPath".AsSqlField("BrandUrlPath")
                );

            paging.From("[Catalog].[Product]");
            paging.Left_Join("[Catalog].[ProductExt] ON [Product].[ProductID] = [ProductExt].[ProductID]");
            paging.Left_Join("[Catalog].[Photo] ON [Photo].[PhotoId] = [ProductExt].[PhotoId]");
            paging.Left_Join("[Catalog].[Offer] ON [ProductExt].[OfferID] = [Offer].[OfferID]");
            paging.Inner_Join("[Catalog].[Currency] ON [Currency].[CurrencyID] = [Product].[CurrencyID]");
            //paging.Left_Join("[Catalog].[Brand] ON [Product].[BrandID] = [Brand].[BrandID]");

            //paging.Left_Join(
            //    "[Catalog].[ShoppingCart] ON [ShoppingCart].[OfferID] = [Offer].[OfferID] AND [ShoppingCart].[ShoppingCartType] = 3 AND [ShoppingCart].[CustomerID] = {0}",
            //    CustomerContext.CustomerId);


            if (SettingsCatalog.ComplexFilter)
            {
                paging.Select(
                      "Colors",
                      "NotSamePrices".AsSqlField("MultiPrices"),
                      "MinPrice".AsSqlField("BasePrice")
                 );
            }
            else
            {
                paging.Select(
                        "null".AsSqlField("Colors"),
                        "0".AsSqlField("MultiPrices"),
                        "Price".AsSqlField("BasePrice")
                   );
            }

            paging = BuildSorting(paging);
            paging = BuildFilter(paging);

            paging.ItemsPerPage = _currentPageIndex != 0 ? SettingsCatalog.ProductsPerPage : int.MaxValue;
            paging.CurrentPageIndex = _currentPageIndex != 0 ? _currentPageIndex : 1;
            return paging;
        }

        private SqlPaging BuildSorting(SqlPaging paging)
        {
            var sort = _model.Sort;

            paging.OrderByDesc("(CASE WHEN Price=0 THEN 0 ELSE 1 END)".AsSqlField("TempSort"),
                                 "AmountSort".AsSqlField("TempAmountSort"));

            switch (sort)
            {
                case ESortOrder.AscByName:
                    paging.OrderBy("Product.Name".AsSqlField("NameSort"));
                    break;
                case ESortOrder.DescByName:
                    paging.OrderByDesc("Product.Name".AsSqlField("NameSort"));
                    break;

                case ESortOrder.AscByPrice:
                    paging.OrderBy("PriceTemp");
                    break;

                case ESortOrder.DescByPrice:
                    paging.OrderByDesc("PriceTemp");
                    break;

                case ESortOrder.AscByRatio:
                    paging.OrderBy("Ratio".AsSqlField("RatioSort"));
                    break;

                case ESortOrder.DescByRatio:
                    paging.OrderByDesc("Ratio".AsSqlField("RatioSort"));
                    break;

                case ESortOrder.AscByAddingDate:
                    paging.OrderBy("DateAdded".AsSqlField("DateAddedSort"));
                    break;

                case ESortOrder.DescByAddingDate:
                    paging.OrderByDesc("DateAdded".AsSqlField("DateAddedSort"));
                    break;
            }
            return paging;
        }

        private SqlPaging BuildFilter(SqlPaging paging)
        {
            paging.Where("Product.Enabled={0}", true);
            paging.Where("AND CategoryEnabled={0}", true);
            paging.Where("AND (Offer.Main={0} OR Offer.Main IS NULL)", true);

            if (SettingsCatalog.ShowOnlyAvalible)
            {
                paging.Where("AND MaxAvailable>{0}", 0);
            }

            if (!string.IsNullOrEmpty(_model.Q))
            {
                var productIds = ProductSeacher.Search(_model.Q).SearchResultItems.Select(x => x.Id).ToList();

                var tanslitQ = StringHelper.TranslitToRusKeyboard(_model.Q);
                var translitProductIds = ProductSeacher.Search(tanslitQ).SearchResultItems.Select(x => x.Id).ToList();

                var resultIds = productIds.Union(translitProductIds).Distinct();

                paging.Inner_Join(
                    "(select item, sort from [Settings].[ParsingBySeperator]({0},'/') ) as dtt on Product.ProductId=convert(int, dtt.item)",
                    String.Join("/", resultIds));

                if (_model.Sort == ESortOrder.NoSorting)
                {
                    paging.OrderBy("dtt.sort");
                }
            }
            return paging;
        }

        private string GetViewMode()
        {
            var cookieMode = CommonHelper.GetCookieString("mobile_viewmode");
            var mode = cookieMode.Parse<ProductViewMode>(SettingsCatalog.DefaultSearchView);
            return mode.ToString().ToLower();
        }

        #endregion

        public SearchMobileModel Get()
        {
            _model = GetProduct();
            _model = GetCategory();

            return _model;
        }

        private SearchMobileModel GetProduct()
        {
            var paging = BuildPaging();

            var totalCount = paging.TotalRowsCount;
            var totalPages = paging.PageCount(totalCount);

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

            var products = paging.PageItemsList<ProductModel>();

            _model.Products = new ProductViewMobileModel(products);
            _model.ViewMode = GetViewMode();
            return _model;
        }

        private SearchMobileModel GetCategory()
        {
            if (string.IsNullOrEmpty(_model.Q))
            {
                _model.Categories = new CategoryListMobileViewModel()
                {
                    Categories = new List<Category>(),
                };
                return _model;
            }

            var categoryIds = CategorySeacher.Search(_model.Q).SearchResultItems.Select(x => x.Id).ToList();

            var tanslitQ = StringHelper.TranslitToRusKeyboard(_model.Q);
            var translitCategoryIds = CategorySeacher.Search(tanslitQ).SearchResultItems.Select(x => x.Id).ToList();

            var categories =
                categoryIds.Union(translitCategoryIds)
                    .Distinct()
                    .Select(CategoryService.GetCategory)
                    .Where(cat => cat != null && cat.Enabled && cat.ParentsEnabled)
                    .Take(10)
                    .ToList();

            _model.Categories = new CategoryListMobileViewModel()
            {
                Categories = categories,
                PhotoHeight = SettingsPictureSize.SmallCategoryImageHeight,
                CountCategoriesInLine = SettingsDesign.CountCategoriesInLine
            };

            return _model;
        }
    }
}