using AdvantShop.Configuration;
using AdvantShop.Core.Models;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.SQL2;
using AdvantShop.Customers;
using AdvantShop.Models.Brand;
using AdvantShop.ViewModel.Catalog;


namespace AdvantShop.Handlers.Brands
{
    public class BrandProductPagingHandler
    {
        private BrandModel _brandModel;
        private SqlPaging _paging;
        private readonly int _currentPageIndex;
        
        private ProductListViewModel _model;

        public BrandProductPagingHandler(BrandModel brandModel)
        {
            _brandModel = brandModel;
            _currentPageIndex = brandModel.Page ?? 1;
        }

        public ProductListViewModel GetForBrandItem()
        {
            _model = new ProductListViewModel();
            
            BuildPaging();
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

            _model.Products = new ProductViewModel(products);
            
            return _model;
        }


        #region Help Methods

        private void BuildPaging()
        {
            _paging = new SqlPaging();
            _paging.Select(
                "Product.ProductID",
                "CountPhoto",
                "Photo.PhotoId",
                "PhotoName",
                "Photo.Description as PhotoDescription",
                "Product.ArtNo",
                "Product.Name",
                "Recomended",
                "Product.Bestseller",
                "Product.New",
                "Product.OnSale as Sale",
                "Product.Discount",
                "Product.DiscountAmount",
                "Product.BriefDescription",
                "Product.MinAmount",
                "Product.MaxAmount",
                "Product.Enabled",
                "Product.AllowPreOrder",
                "Product.Ratio",
                "Product.UrlPath",
                "Product.DateAdded",
                "Offer.OfferID",
                "Offer.ColorID",
                "MaxAvailable AS Amount",
                "ShoppingCartItemId",
                "Comments",
                "CurrencyValue",
				"ProductExt.Gifts",
                "Brand.BrandName as BrandName",
                "Brand.UrlPath as BrandUrlPath"
                );

            _paging.From("[Catalog].[Product]");
            _paging.Left_Join("[Catalog].[ProductExt] ON [Product].[ProductID] = [ProductExt].[ProductID]");
            _paging.Left_Join("[Catalog].[Photo] ON [Photo].[PhotoId] = [ProductExt].[PhotoId]");
            _paging.Left_Join("[Catalog].[Offer] ON [ProductExt].[OfferID] = [Offer].[OfferID]");
            _paging.Inner_Join("[Catalog].[Currency] ON [Currency].[CurrencyID] = [Product].[CurrencyID]");
            _paging.Inner_Join("[Catalog].[Brand] ON [Product].[BrandID] = [Brand].[BrandID] AND [Brand].BrandId = {0}", _brandModel.BrandId);


            _paging.Left_Join("[Catalog].[ShoppingCart] ON [ShoppingCart].[OfferID] = [Offer].[OfferID] AND [ShoppingCart].[ShoppingCartType] = 3 AND [ShoppingCart].[CustomerID] = {0}", CustomerContext.CustomerId);
            
            if (SettingsCatalog.ComplexFilter)
            {
                _paging.Select(
                      "Colors",
                      "NotSamePrices as MultiPrices",
                      "MinPrice as BasePrice"
                 );
            }
            else
            {
                _paging.Select(
                        "null as Colors",
                        "0 as MultiPrices",
                        "Price as BasePrice"
                   );
            }

            _paging.ItemsPerPage = _currentPageIndex != 0 ? SettingsCatalog.ProductsPerPage : int.MaxValue;
            _paging.CurrentPageIndex = _currentPageIndex != 0 ? _currentPageIndex : 1;
        }

        private void BuildFilter()
        {
            _paging.Where("Product.Enabled={0}", true);
            _paging.Where("AND CategoryEnabled={0}", true);
            _paging.Where("AND Offer.Main={0} AND Offer.Main IS NOT NULL", true);
			
			if (SettingsCatalog.ShowOnlyAvalible)
            {
                _paging.Where("AND MaxAvailable>{0}", 0);
            }

            _paging.OrderByDesc("(CASE WHEN Price=0 THEN 0 ELSE 1 END)".AsSqlField("TempSort"),
                               "AmountSort".AsSqlField("TempAmountSort"));
        }

        #endregion
    }
}