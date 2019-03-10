using AdvantShop.Configuration;
using AdvantShop.Web.Admin.Models.Settings.CatalogSettings;

namespace AdvantShop.Web.Admin.Handlers.Settings.Catalog
{
    public class GetCatalogSettings
    {
        public CatalogSettingsModel Execute()
        {
            var model = new CatalogSettingsModel()
            {
                ShowQuickView = SettingsCatalog.ShowQuickView,
                ProductsPerPage = SettingsCatalog.ProductsPerPage,
                ShowProductsCount = SettingsCatalog.ShowProductsCount,
                ShowProductArtNo = SettingsCatalog.ShowProductArtNo,
                EnableProductRating = SettingsCatalog.EnableProductRating,
                EnableCompareProducts = SettingsCatalog.EnableCompareProducts,
                EnablePhotoPreviews = SettingsCatalog.EnablePhotoPreviews,
                ShowCountPhoto = SettingsCatalog.ShowCountPhoto,
                ShowOnlyAvalible = SettingsCatalog.ShowOnlyAvalible,
                MoveNotAvaliableToEnd = SettingsCatalog.MoveNotAvaliableToEnd,

                EnableCatalogViewChange = SettingsCatalog.EnabledCatalogViewChange,
                EnableSearchViewChange = SettingsCatalog.EnabledSearchViewChange,

                ExcludingFilters = SettingsCatalog.ExcludingFilters,

                FilterVisibility = SettingsDesign.FilterVisibility,
                ShowPriceFilter = SettingsCatalog.ShowPriceFilter,
                ShowProducerFilter = SettingsCatalog.ShowProducerFilter,
                ShowSizeFilter = SettingsCatalog.ShowSizeFilter,
                ShowColorFilter = SettingsCatalog.ShowColorFilter,
                ComplexFilter = SettingsCatalog.ComplexFilter,
                SizesHeader = SettingsCatalog.SizesHeader,
                ColorsHeader = SettingsCatalog.ColorsHeader,


                ColorIconWidthCatalog = SettingsPictureSize.ColorIconWidthCatalog,
                ColorIconHeightCatalog = SettingsPictureSize.ColorIconHeightCatalog,
                ColorIconWidthDetails = SettingsPictureSize.ColorIconWidthDetails,
                ColorIconHeightDetails = SettingsPictureSize.ColorIconHeightDetails,
                DefaultCatalogView = SettingsCatalog.DefaultCatalogView,
                DefaultSearchView = SettingsCatalog.DefaultSearchView,
                                
                BuyButtonText = SettingsCatalog.BuyButtonText,
                PreOrderButtonText = SettingsCatalog.PreOrderButtonText,


                DisplayBuyButton = SettingsCatalog.DisplayBuyButton,
                DisplayPreOrderButton = SettingsCatalog.DisplayPreOrderButton,


                DisplayCategoriesInBottomMenu = SettingsCatalog.DisplayCategoriesInBottomMenu,
                BrandsPerPage = SettingsCatalog.BrandsPerPage,


                SearchExample = SettingsCatalog.SearchExample,
                SearchDeep = SettingsCatalog.SearchDeep,
                SearchMaxItems = SettingsCatalog.SearchMaxItems,

                ShowCategoryTreeInBrand = SettingsCatalog.ShowCategoryTreeInBrand,
                ShowProductsInBrand = SettingsCatalog.ShowProductsInBrand,

                DefaultCurrencyIso3 = SettingsCatalog.DefaultCurrencyIso3,
                AllowToChangeCurrency = SettingsCatalog.AllowToChangeCurrency,
                AutoUpdateCurrencies = SettingsMain.EnableAutoUpdateCurrencies,


                DisplayWeight = SettingsCatalog.DisplayWeight,
                DisplayDimensions = SettingsCatalog.DisplayDimensions,
                ShowStockAvailability = SettingsCatalog.ShowStockAvailability,
                CompressBigImage = SettingsCatalog.CompressBigImage,
                ModerateReviews = SettingsCatalog.ModerateReviews,
                AllowReviews = SettingsCatalog.AllowReviews,
                DisplayReviewsImage = SettingsCatalog.DisplayReviewsImage,
                AllowReviewsImageUploading = SettingsCatalog.AllowReviewsImageUploading,
                ReviewImageWidth = SettingsPictureSize.ReviewImageWidth,
                ReviewImageHeight = SettingsPictureSize.ReviewImageHeight,
                EnableZoom = SettingsDesign.EnableZoom,
                ShowShippingsMethodsInDetails = SettingsDesign.ShowShippingsMethodsInDetails,
                ShippingsMethodsInDetailsCount = SettingsDesign.ShippingsMethodsInDetailsCount,
                RelatedProductName = SettingsCatalog.RelatedProductName,
                AlternativeProductName = SettingsCatalog.AlternativeProductName,
                RelatedProductSourceType = SettingsDesign.RelatedProductSourceType,
                RelatedProductsMaxCount = SettingsCatalog.RelatedProductsMaxCount
            };


            return model;
        }
    }
}
