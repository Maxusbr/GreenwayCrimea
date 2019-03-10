//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.CMS;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Core.Caching
{
    /// <summary>
    /// Retun the special formated cache object names
    /// </summary>
    /// <remarks></remarks>
    public class CacheNames
    {
        public const string TemplateSetPref = "TemplateSettings_";
        public const string Category = "Category_";
        public const string StaticBlock = "StaticBlock_";

        public const string News = "News_";
        public const string Voiting = "Voiting_";
        public const string MenuPrefix = "MenuCache_";
        public const string PaymentOptions = "PaymentOptions_";
        public const string ShippingOptions = "ShippingOptions_";

        public const string MenuCatalog = "MenuCatalog_";
        public const string SQLPagingCount = "SQLPagingCount_";
        public const string SQLPagingItems = "SQLPagingItems_";

        public const string ProductList = "ProductList_";

        public const string BrandsInCategory = "BrandsInCategory_";
        public const string PropertiesInCategory = "PropertiesInCategory_";
		
		public const string ShippingForCityAndCountry = "ShippingForCityAndCountry_";

        public const string AdminMenu = "AdminMenu";

        public const string BonusCard = "BonusCard_";

        public const string Customer = "Customer_";

        public const string IsDebug = "IsDebug";

        public static string GetModuleSettingsCacheObjectName()
        {
            return "ModuleSettings";
        }

        public static string GetTemplateSettings(string template)
        {
            return TemplateSetPref + template;
        }

        public static string GetTemplateSettingsCacheObjectName(string template, string strName)
        {
            return TemplateSetPref + template + "_" + strName;
        }

        public static string GetCategoryCacheObjectPrefix()
        {
            return Category;
        }

        public static string GetCategoryCacheObjectName(int id)
        {
            return Category + id;
        }

        public static string GetStaticBlockCacheObjectName(string strName)
        {
            return StaticBlock + strName;
        }

        public static string GetBestSellersCacheObjectName()
        {
            return "GetBestSellers";
        }

        public static string GetMenuCacheObjectName(EMenuType menuType, EMenuItemShowMode showMode = EMenuItemShowMode.All, int? selectedId = null)
        {
            return MenuPrefix + menuType +
                (showMode == EMenuItemShowMode.Authorized ? "_Auth" : string.Empty) +
                (selectedId.HasValue ? "_" + selectedId.Value : string.Empty);
        }

        public static string GetMainMenuCacheObjectName(int id)
        {
            return GetMenuCacheObjectName(EMenuType.Top, selectedId: id);
        }

        public static string GetMainMenuCacheObjectName()
        {
            return GetMenuCacheObjectName(EMenuType.Top);
        }

        public static string GetMainMenuAuthCacheObjectName()
        {
            return GetMenuCacheObjectName(EMenuType.Top, EMenuItemShowMode.Authorized);
        }

        public static string GetBottomMenuCacheObjectName()
        {
            return GetMenuCacheObjectName(EMenuType.Bottom);
        }

        public static string GetBottomMenuAuthCacheObjectName()
        {
            return GetMenuCacheObjectName(EMenuType.Bottom, EMenuItemShowMode.Authorized);
        }

        public static string GetOrderPriceDiscountCacheObjectName()
        {
            return "OrderPriceDiscount";
        }

        public static string GetXmlSettingsCacheObjectName()
        {
            return "XMLSettings";
        }

        public static string GetRoutesCacheObjectName()
        {
            return "Routes";
        }

        public static string GetNewsForMainPage()
        {
            return News + "ForMainPage";
        }

        public static string GetUrlCacheObjectName()
        {
            return "UrlSynonyms";
        }

        public static string GetAltSessionCacheObjectName(string sessionId)
        {
            return "AltSession_" + sessionId;
        }

        internal static string GetCurrenciesCacheObjectName()
        {
            return "Currencies";
        }

        public static string GetRoleActionsCacheObjectName(string customerId)
        {
            return "RoleActions_" + customerId;
        }

        public static string GetDesignCacheObjectName(string designType)
        {
            return "Designs_" + designType;
        }

        public static string GetPaymentOptionsCacheName(int preorderHash)
        {
            return PaymentOptions + preorderHash;
        }

        public static string GetShippingOptionsCacheName(int preorderHash)
        {
            return ShippingOptions + preorderHash;
        }

        public static string SQlPagingCountCacheName(string cashKey, string queryHash, string paramsHashCode)
        {
            return SQLPagingCount + cashKey + (queryHash + "_" + paramsHashCode).GetHashCode();
        }

        public static string SQlPagingItemsCacheName(string cashKey, string queryHash, string paramsHashCode)
        {
            return SQLPagingItems + cashKey + (queryHash + "_" + paramsHashCode).GetHashCode();
        }

        public static string MainPageProductsCacheName(string type, int count)
        {
            var currency = CurrencyService.CurrentCurrency;
            return SQLPagingItems + "_MainPageProducts_" + type + count.ToString() + currency.Iso3 + currency.Rate.ToString();
        }

        public static string MainPageProductsCountCacheName(string type, bool enabled)
        {
            return SQLPagingItems + "_MainPageProducts_Count_" + type + enabled.ToString();
        }

        public static string ProductListCacheName(int listId, int count)
        {
            var currency = CurrencyService.CurrentCurrency;
            return SQLPagingItems + ProductList + listId.ToString() + count.ToString() + currency.Iso3 + currency.Rate.ToString();
        }

        public static string BrandsInCategoryCacheName(int categoryid, bool indepth, int flag, int _listId = 0)
        {
            return BrandsInCategory + categoryid + "_" + indepth + "_" + flag + "_" + _listId;
        }

        public static string PropertiesInCategoryCacheName(int categoryid, bool indepth)
        {
            return PropertiesInCategory + categoryid + "_" + indepth;
        }
		
		public static string GetShippingForCityAndCountry(int methodId, string countryName, string cityName)
        {
			return ShippingForCityAndCountry + methodId + "_" + countryName + "_" + cityName;
        }
    }
}