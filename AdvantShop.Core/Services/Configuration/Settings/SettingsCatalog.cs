//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Helpers;
using AdvantShop.Repository.Currencies;
using AdvantShop.Core.Services.FullSearch.Core;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Configuration
{
    public enum ProductViewMode
    {
        [Localize("Core.Settings.SettingsCatalog.ProductViewMode.Tile")]
        Tile = 0,

        [Localize("Core.Settings.SettingsCatalog.ProductViewMode.List")]
        List = 1,

        [Localize("Core.Settings.SettingsCatalog.ProductViewMode.Table")]
        Table = 2
    }

    public enum ProductViewPage
    {
        Catalog = 0,
        Search = 1
    }

    public class SettingsCatalog
    {
        public static int ProductsPerPage
        {
            get { return int.Parse(SettingProvider.Items["ProductsPerPage"]); }
            set { SettingProvider.Items["ProductsPerPage"] = value.ToString(); }
        }

        public static string DefaultCurrencyIso3
        {
            get { return SettingProvider.Items["DefaultCurrencyISO3"]; }
            set { SettingProvider.Items["DefaultCurrencyISO3"] = value; }
        }

        public static bool AllowToChangeCurrency
        {
            get { return Convert.ToBoolean(SettingProvider.Items["AllowToChangeCurrency"]); }
            set { SettingProvider.Items["AllowToChangeCurrency"] = value.ToString(); }
        }

        public static Currency DefaultCurrency
        {
            get
            {
                return CurrencyService.Currency(DefaultCurrencyIso3) ??
                       CurrencyService.GetAllCurrencies().FirstOrDefault();
            }
        }


        public static ProductViewMode DefaultCatalogView
        {
            get { return (ProductViewMode)int.Parse(SettingProvider.Items["DefaultCatalogView"]); }
            set { SettingProvider.Items["DefaultCatalogView"] = ((int)value).ToString(); }
        }

        public static ProductViewMode DefaultSearchView
        {
            get { return (ProductViewMode)int.Parse(SettingProvider.Items["DefaultSearchView"]); }
            set { SettingProvider.Items["DefaultSearchView"] = ((int)value).ToString(); }
        }

        public static bool EnableProductRating
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableProductRating"]); }
            set { SettingProvider.Items["EnableProductRating"] = value.ToString(); }
        }

        public static bool EnablePhotoPreviews
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnablePhotoPreviews"]); }
            set { SettingProvider.Items["EnablePhotoPreviews"] = value.ToString(); }
        }

        public static bool ShowCountPhoto
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowCountPhoto"]); }
            set { SettingProvider.Items["ShowCountPhoto"] = value.ToString(); }
        }


        public static bool ShowProductsCount
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowProductsCount"]); }
            set { SettingProvider.Items["ShowProductsCount"] = value.ToString(); }
        }

        public static bool EnableCompareProducts
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableCompareProducts"]); }
            set { SettingProvider.Items["EnableCompareProducts"] = value.ToString(); }
        }

        public static bool EnabledCatalogViewChange
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableCatalogViewChange"]); }
            set { SettingProvider.Items["EnableCatalogViewChange"] = value.ToString(); }
        }

        public static bool EnabledSearchViewChange
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableSearchViewChange"]); }
            set { SettingProvider.Items["EnableSearchViewChange"] = value.ToString(); }
        }

        public static bool CompressBigImage
        {
            get { return Convert.ToBoolean(SettingProvider.Items["CompressBigImage"]); }
            set { SettingProvider.Items["CompressBigImage"] = value.ToString(); }
        }

        public static string RelatedProductName
        {
            get { return SettingProvider.Items["RelatedProductName"]; }
            set { SettingProvider.Items["RelatedProductName"] = value; }
        }

        public static string AlternativeProductName
        {
            get { return SettingProvider.Items["AlternativeProductName"]; }
            set { SettingProvider.Items["AlternativeProductName"] = value; }
        }

        public static bool AllowReviews
        {
            get { return Convert.ToBoolean(SettingProvider.Items["AllowReviews"]); }
            set { SettingProvider.Items["AllowReviews"] = value.ToString(); }
        }

        public static bool DisplayReviewsImage
        {
            get { return Convert.ToBoolean(SettingProvider.Items["DisplayReviewsImage"]); }
            set { SettingProvider.Items["DisplayReviewsImage"] = value.ToString(); }
        }

        public static bool AllowReviewsImageUploading
        {
            get { return Convert.ToBoolean(SettingProvider.Items["AllowReviewsImageUploading"]); }
            set { SettingProvider.Items["AllowReviewsImageUploading"] = value.ToString(); }
        }

        public static bool ModerateReviews
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ModerateReviewed"]); }
            set { SettingProvider.Items["ModerateReviewed"] = value.ToString(); }
        }

        public static bool ComplexFilter
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ComplexFilter"]); }
            set { SettingProvider.Items["ComplexFilter"] = value.ToString(); }
        }

        public static string SizesHeader
        {
            get { return SQLDataHelper.GetString(SettingProvider.Items["SizesHeader"]); }
            set { SettingProvider.Items["SizesHeader"] = value; }
        }


        public static string ColorsHeader
        {
            get { return SQLDataHelper.GetString(SettingProvider.Items["ColorsHeader"]); }
            set { SettingProvider.Items["ColorsHeader"] = value; }
        }

        public static bool ShowQuickView
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowQuickView"]); }
            set { SettingProvider.Items["ShowQuickView"] = value.ToString(); }
        }

        public static bool ShowProductArtNo
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowProductArtNo"]); }
            set { SettingProvider.Items["ShowProductArtNo"] = value.ToString(); }
        }
        
       
        public static bool ExcludingFilters
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ExluderingFilters"]); }
            set { SettingProvider.Items["ExluderingFilters"] = value.ToString(); }
        }

        public static string GetRelatedProductName(int relatedType)
        {
            if (relatedType == 0)
                return RelatedProductName;
            else if (relatedType == 1)
                return AlternativeProductName;
            
            return string.Empty;
        }


        public static string BuyButtonText
        {
            get { return SettingProvider.Items["BuyButtonText"]; }
            set { SettingProvider.Items["BuyButtonText"] = value; }
        }

        public static string PreOrderButtonText
        {
            get { return SettingProvider.Items["PreOrderButtonText"]; }
            set { SettingProvider.Items["PreOrderButtonText"] = value; }
        }

        public static bool DisplayBuyButton
        {
            get { return Convert.ToBoolean(SettingProvider.Items["DisplayBuyButton"]); }
            set { SettingProvider.Items["DisplayBuyButton"] = value.ToString(); }
        }

        public static bool DisplayPreOrderButton
        {
            get { return Convert.ToBoolean(SettingProvider.Items["DisplayPreOrderButton"]); }
            set { SettingProvider.Items["DisplayPreOrderButton"] = value.ToString(); }
        }

        public static bool DisplayCategoriesInBottomMenu
        {
            get { return Convert.ToBoolean(SettingProvider.Items["DisplayCategoriesInBottomMenu"]); }
            set { SettingProvider.Items["DisplayCategoriesInBottomMenu"] = value.ToString(); }
        }

        public static bool ShowStockAvailability
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowStockAvailability"]); }
            set { SettingProvider.Items["ShowStockAvailability"] = value.ToString(); }
        }

        public static bool ShowColorFilter
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowColorFilter"]); }
            set { SettingProvider.Items["ShowColorFilter"] = value.ToString(); }
        }

        public static bool ShowSizeFilter
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowSizeFilter"]); }
            set { SettingProvider.Items["ShowSizeFilter"] = value.ToString(); }
        }

        public static string SearchExample
        {
            get { return SettingProvider.Items["SearchExample"]; }
            set { SettingProvider.Items["SearchExample"] = value; }
        }

        public static int SearchMaxItems
        {
            get { return SettingProvider.Items["SearchMaxItems"].TryParseInt(); }
            set { SettingProvider.Items["SearchMaxItems"] = value.ToString(); }
        }

        public static ESearchDeep SearchDeep
        {
            get { return SettingProvider.Items["SearchDeep"].TryParseEnum<ESearchDeep>(); }
            set { SettingProvider.Items["SearchDeep"] = value.ToString(); }
        }

        public static bool DisplayWeight
        {
            get { return Convert.ToBoolean(SettingProvider.Items["DisplayWeight"]); }
            set { SettingProvider.Items["DisplayWeight"] = value.ToString(); }
        }

        public static bool DisplayDimensions
        {
            get { return Convert.ToBoolean(SettingProvider.Items["DisplayDimensions"]); }
            set { SettingProvider.Items["DisplayDimensions"] = value.ToString(); }
        }

        public static bool ShowProducerFilter
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowProducerFilter"]); }
            set { SettingProvider.Items["ShowProducerFilter"] = value.ToString(); }
        }

        public static bool ShowPriceFilter
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowPriceFilter"]); }
            set { SettingProvider.Items["ShowPriceFilter"] = value.ToString(); }
        }


        public static bool ShowProductsInBrand
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowProductsInBrand"]); }
            set { SettingProvider.Items["ShowProductsInBrand"] = value.ToString(); }
        }
        public static bool ShowCategoryTreeInBrand
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowCategoryTree"]); }
            set { SettingProvider.Items["ShowCategoryTree"] = value.ToString(); }
        }

        public static bool ShowOnlyAvalible
        {
            get { return Convert.ToBoolean(SettingProvider.Items["ShowOnlyAvalible"]); }
            set { SettingProvider.Items["ShowOnlyAvalible"] = value.ToString(); }
        }

        public static bool MoveNotAvaliableToEnd
        {
            get { return Convert.ToBoolean(SettingProvider.Items["MoveNotAvaliableToEnd"]); }
            set { SettingProvider.Items["MoveNotAvaliableToEnd"] = value.ToString(); }
        }

        

        public static int BrandsPerPage
        {
            get { return int.Parse(SettingProvider.Items["BrandsPerPage"]); }
            set { SettingProvider.Items["BrandsPerPage"] = value.ToString(); }
        }

        public static int RelatedProductsMaxCount
        {
            get { return Convert.ToInt32(SettingProvider.Items["RelatedProductsMaxCount"]); }
            set { SettingProvider.Items["RelatedProductsMaxCount"] = value.ToString(); }
        }

        public static bool AvaliableFilterEnabled
        {
            get { return SQLDataHelper.GetBoolean(SettingProvider.Items["AvaliableFilterEnabled"]); }
            set { SettingProvider.Items["AvaliableFilterEnabled"] = value.ToString(); }
        }

        public static string BestDescription
        {
            get { return SQLDataHelper.GetString(SettingProvider.Items["BestDescription"]); }
            set { SettingProvider.Items["BestDescription"] = value.ToString(); }
        }

        public static string NewDescription
        {
            get { return SQLDataHelper.GetString(SettingProvider.Items["NewDescription"]); }
            set { SettingProvider.Items["NewDescription"] = value.ToString(); }
        }

        public static string DiscountDescription
        {
            get { return SQLDataHelper.GetString(SettingProvider.Items["DiscountDescription"]); }
            set { SettingProvider.Items["DiscountDescription"] = value.ToString(); }
        }

        public static int DefaultTaxId
        {
            get { return Convert.ToInt32(SettingProvider.Items["DefaultTaxId"]); }
            set { SettingProvider.Items["DefaultTaxId"] = value.ToString(); }
        }
    }
}