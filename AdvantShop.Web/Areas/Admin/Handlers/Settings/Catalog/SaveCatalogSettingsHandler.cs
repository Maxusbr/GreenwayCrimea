using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Repository.Currencies;
using AdvantShop.Web.Admin.Models.Settings.CatalogSettings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Catalog
{
    public class SaveCatalogSettingsHandler
    {
        private readonly CatalogSettingsModel _model;
        public SaveCatalogSettingsHandler(CatalogSettingsModel model)
        {
            _model = model;
        }

        public void Execute()
        {
            var tempIso3 = SettingsCatalog.DefaultCurrencyIso3;
            SettingsCatalog.DefaultCurrencyIso3 = _model.DefaultCurrencyIso3.DefaultOrEmpty();
            if (SettingsCatalog.DefaultCurrencyIso3 != tempIso3)
            {
                CurrencyService.CurrentCurrency = CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3);
                CategoryService.ClearCategoryCache();
            }

            SettingsCatalog.AllowToChangeCurrency = _model.AllowToChangeCurrency;
            SettingsMain.EnableAutoUpdateCurrencies = _model.AutoUpdateCurrencies;

            SettingsCatalog.ProductsPerPage = _model.ProductsPerPage;
            SettingsCatalog.EnableProductRating = _model.EnableProductRating;
            SettingsCatalog.EnablePhotoPreviews = _model.EnablePhotoPreviews;
            SettingsCatalog.ShowCountPhoto = _model.ShowCountPhoto;
            SettingsCatalog.EnableCompareProducts = _model.EnableCompareProducts;
            SettingsCatalog.EnabledCatalogViewChange = _model.EnableCatalogViewChange;
            SettingsCatalog.EnabledSearchViewChange = _model.EnableSearchViewChange;
            SettingsCatalog.DefaultCatalogView = _model.DefaultCatalogView;
            SettingsCatalog.DefaultSearchView = _model.DefaultSearchView;

            if (SettingsCatalog.DisplayCategoriesInBottomMenu != _model.DisplayCategoriesInBottomMenu)
            {
                CacheManager.RemoveByPattern(CacheNames.MenuPrefix);
            }

            SettingsCatalog.DisplayCategoriesInBottomMenu = _model.DisplayCategoriesInBottomMenu;

            var tempShowOnlyAvalible = SettingsCatalog.ShowOnlyAvalible;
            SettingsCatalog.ShowOnlyAvalible = _model.ShowOnlyAvalible;

            if (tempShowOnlyAvalible != SettingsCatalog.ShowOnlyAvalible)
            {
                CategoryService.RecalculateProductsCountManual();
            }

            SettingsCatalog.MoveNotAvaliableToEnd = _model.MoveNotAvaliableToEnd;

            SettingsCatalog.ExcludingFilters = _model.ExcludingFilters;
            SettingsCatalog.ShowQuickView = _model.ShowQuickView;

            SettingsDesign.FilterVisibility = _model.FilterVisibility;
            SettingsCatalog.ShowPriceFilter = _model.ShowPriceFilter;
            SettingsCatalog.ShowProducerFilter = _model.ShowProducerFilter;
            SettingsCatalog.ShowSizeFilter = _model.ShowSizeFilter;
            SettingsCatalog.ShowColorFilter = _model.ShowColorFilter;
            SettingsCatalog.ComplexFilter = _model.ComplexFilter;
            SettingsCatalog.SizesHeader = _model.SizesHeader.DefaultOrEmpty();
            SettingsCatalog.ColorsHeader = _model.ColorsHeader.DefaultOrEmpty();
            SettingsCatalog.ShowProductArtNo = _model.ShowProductArtNo;

            SettingsPictureSize.ColorIconWidthCatalog = _model.ColorIconWidthCatalog;
            SettingsPictureSize.ColorIconHeightCatalog = _model.ColorIconHeightCatalog;
            SettingsPictureSize.ColorIconWidthDetails = _model.ColorIconWidthDetails;
            SettingsPictureSize.ColorIconHeightDetails = _model.ColorIconHeightDetails;

            SettingsCatalog.BuyButtonText = _model.BuyButtonText.DefaultOrEmpty();
            SettingsCatalog.PreOrderButtonText = _model.PreOrderButtonText.DefaultOrEmpty();

            SettingsCatalog.DisplayBuyButton = _model.DisplayBuyButton;
            SettingsCatalog.DisplayPreOrderButton = _model.DisplayPreOrderButton;

            SettingsCatalog.ShowProductsCount = _model.ShowProductsCount;

            SettingsCatalog.ShowCategoryTreeInBrand = _model.ShowCategoryTreeInBrand;
            SettingsCatalog.ShowProductsInBrand = _model.ShowProductsInBrand;

            SettingsCatalog.SearchExample = _model.SearchExample.DefaultOrEmpty();

            SettingsCatalog.BrandsPerPage = _model.BrandsPerPage;

            SettingsCatalog.SearchDeep = _model.SearchDeep;

            SettingsCatalog.SearchMaxItems = _model.SearchMaxItems;


            SettingsCatalog.DisplayWeight = _model.DisplayWeight;
            SettingsCatalog.DisplayDimensions = _model.DisplayDimensions;
            SettingsCatalog.ShowStockAvailability = _model.ShowStockAvailability;
            SettingsCatalog.CompressBigImage = _model.CompressBigImage;
            SettingsCatalog.ModerateReviews = _model.ModerateReviews;
            SettingsCatalog.AllowReviews = _model.AllowReviews;
            SettingsCatalog.DisplayReviewsImage = _model.DisplayReviewsImage;
            SettingsCatalog.AllowReviewsImageUploading = _model.AllowReviewsImageUploading;
            SettingsPictureSize.ReviewImageWidth = _model.ReviewImageWidth;
            SettingsPictureSize.ReviewImageHeight = _model.ReviewImageHeight;
            SettingsDesign.EnableZoom = _model.EnableZoom;
            SettingsDesign.ShowShippingsMethodsInDetails = _model.ShowShippingsMethodsInDetails;
            SettingsDesign.ShippingsMethodsInDetailsCount = _model.ShippingsMethodsInDetailsCount;
            SettingsCatalog.RelatedProductName = _model.RelatedProductName.DefaultOrEmpty();
            SettingsCatalog.AlternativeProductName = _model.AlternativeProductName.DefaultOrEmpty();
            SettingsDesign.RelatedProductSourceType = _model.RelatedProductSourceType;
            SettingsCatalog.RelatedProductsMaxCount = _model.RelatedProductsMaxCount;
        }
    }
}
