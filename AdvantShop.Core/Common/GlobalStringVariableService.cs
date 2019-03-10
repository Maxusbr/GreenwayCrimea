//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Localization;
using AdvantShop.SEO;

namespace AdvantShop.Core
{
    public class SeoToken
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public SeoToken(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }

    public class GlobalStringVariableService
    {
        public static List<SeoToken> GetGlobalVariables()
        {
            return new List<SeoToken>()
            {
                new SeoToken("#STORE_NAME#", SettingsMain.ShopName)
            };
        }

        public static string TranslateExpression(string str)
        {
            var dict = GetGlobalVariables();
            return TranslateExpression(str, dict);
        }

        public static string TranslateExpression(string str, MetaType type, string name, string categoryName = null, string brandName = null, string price = null, string tags = null, string productArtNo = null, int page = 0)
        {
            var dict = GetGlobalVariables();
            dict.AddRange(GetSpecificVariables(type, name, categoryName, brandName, price, tags, productArtNo, page));
            return TranslateExpression(str, dict);
        }

        public static IEnumerable<SeoToken> GetSpecificVariables(MetaType type, string name, string categoryName = null, string brandName = null, string price = null, string tags = null, string productArtNo = null, int page = 0)
        {
            if (string.IsNullOrEmpty(name))
                return Enumerable.Empty<SeoToken>();

            switch (type)
            {
                case MetaType.Category:
                    return new List<SeoToken>()
                    {
                        new SeoToken("#CATEGORY_NAME#", name),
                        new SeoToken("#PAGE#", page > 1 ? LocalizationService.GetResource("Infrastructure.MetaInformation.CatalogPageIs") + page : string.Empty),
                        new SeoToken("#TAGS#", tags)
                    };
                case MetaType.Product:
                    return new List<SeoToken>()
                    {
                        new SeoToken("#PRODUCT_NAME#", name),
                        new SeoToken("#CATEGORY_NAME#", categoryName),
                        new SeoToken("#BRAND_NAME#", brandName),
                        new SeoToken("#PRICE#", price),
                        new SeoToken("#TAGS#", tags),
                        new SeoToken("#PRODUCT_ARTNO#", productArtNo)
                    };
                case MetaType.News:
                    return new List<SeoToken>() {new SeoToken("#NEWS_NAME#", name)};
                case MetaType.NewsCategory:
                    return new List<SeoToken>() {new SeoToken("#NEWSCATEGORY_NAME#", name)};
                case MetaType.Brand:
                    return new List<SeoToken>() {new SeoToken("#BRAND_NAME#", name)};
                case MetaType.StaticPage:
                    return new List<SeoToken>() {new SeoToken("#PAGE_NAME#", name)};
                case MetaType.Tag:
                    return new List<SeoToken>() {new SeoToken("#TAG_NAME#", name)};
                default:
                    return Enumerable.Empty<SeoToken>();
            }
        }


        public static string TranslateExpression(string strExpr, List<SeoToken> tokens)
        {
            if (strExpr == null) return string.Empty;

            var strRes = strExpr;
            
            for (var i = 0; i < tokens.Count; i++)
            {
                if (strRes.Contains(tokens[i].Key))
                {
                    strRes = strRes.Replace(tokens[i].Key, tokens[i].Value);
                }
            }
            return strRes;
        }
    }

}
