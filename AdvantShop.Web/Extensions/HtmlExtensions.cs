using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Repository;
using AdvantShop.ViewModel.StaticBlock;
using AdvantShop.Web.Infrastructure.Extensions;
using AdvantShop.Core.Services.InplaceEditor;
using AdvantShop.Configuration;
using Resources;

namespace AdvantShop.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString StaticBlock(this HtmlHelper helper, string key, string cssClass = null, bool onlyContent = false)
        {
            var sb = StaticBlockService.GetPagePartByKeyWithCache(key);

            if (sb == null || !sb.Enabled)
                return MvcHtmlString.Create("");

            if (onlyContent)
                return MvcHtmlString.Create(sb.Content);

            var content = sb.Content;
            var canUseInplace = InplaceEditorService.CanUseInplace(RoleAction.Cms);

            if (content.IsNotEmpty() && canUseInplace)
            {
                content = InplaceEditorService.PrepareContent(content);
            }

            var sbModel = new StaticBlockViewModel()
            {
                CssClass = cssClass,
                InplaceAttributes = canUseInplace ? helper.InplaceStaticBlock(sb.StaticBlockId) : new HtmlString(""),
                OnlyContent = onlyContent,
                Content = content,
                Key = sb.Key,
                CanUseInplace = canUseInplace
            };

            return helper.Partial("_StaticBlock", sbModel);
        }

        public static MvcHtmlString Captcha(this HtmlHelper helper, string ngModel)
        {
            return helper.Action("Captcha", "Common", new { ngModel });
        }

        public static MvcHtmlString Rating(this HtmlHelper helper, double rating, int objId = 0, string url = null,
            bool readOnly = true, string binding = null)
        {
            return helper.Action("Rating", "Common",
                new RouteValueDictionary()
                {
                    {"objId", objId},
                    {"rating", rating},
                    {"url", url},
                    {"readOnly", readOnly},
                    {"binding", binding}
                });
        }

        public static HtmlString RenderCustomOptions(this HtmlHelper helper, List<EvaluatedCustomOptions> evlist)
        {
            if (evlist == null || evlist.Count == 0)
                return new MvcHtmlString("");

            var html = new StringBuilder("<ul class=\"cart-full-properties\">");
            foreach (var ev in evlist)
            {
                html.AppendFormat(
                    "<li class=\"cart-full-properties-item\"><div class=\"cart-full-properties-name cs-light\">{0}:</div> <div class=\"cart-full-properties-value\">{1}</div></li>",
                    ev.CustomOptionTitle, ev.OptionTitle);
            }
            html.Append("</ul>");

            return new HtmlString(html.ToString());
        }

        public static HtmlString GetCityPhone(this HtmlHelper helper, bool encode = false)
        {
            return new HtmlString(encode ? helper.AttributeEncode(CityService.GetPhone()) : CityService.GetPhone());
        }

        public static IHtmlString RenderLabels(this HtmlHelper helper, bool recommended, bool sales, bool best,
            bool news, Discount discount, int labelCount = 5, List<string> customLabels = null, bool warranty = false)
        {
            var labels = new StringBuilder();

            if (warranty && labelCount-- > 0)
                labels.AppendFormat(
                    "<div class=\"products-view-label\"><span class=\"products-view-label-inner products-view-label-warranty\">{0}</span></div>",
                    LocalizationService.GetResource("Catalog.Label.Warranty"));

            if (recommended && labelCount-- > 0)
                labels.AppendFormat(
                    "<div class=\"products-view-label\"><span class=\"products-view-label-inner products-view-label-recommend\">{0}</span></div>",
                    LocalizationService.GetResource("Catalog.Label.Recomended"));

            if (sales && labelCount-- > 0)
                labels.AppendFormat(
                    "<div class=\"products-view-label\"><span class=\"products-view-label-inner products-view-label-sales\">{0}</span></div>",
                    LocalizationService.GetResource("Catalog.Label.Sales"));

            if (best && labelCount-- > 0)
                labels.AppendFormat(
                    "<div class=\"products-view-label\"><span class=\"products-view-label-inner products-view-label-best\">{0}</span></div>",
                    LocalizationService.GetResource("Catalog.Label.Best"));

            if (discount != null && discount.HasValue && labelCount-- > 0)
            {
                labels.AppendFormat(
                    "<div class=\"products-view-label\"><span class=\"products-view-label-inner products-view-label-discount\">{0} {1}</span></div>",
                    LocalizationService.GetResource("Catalog.Label.Discount"),
                    discount.Type == DiscountType.Percent 
                        ? discount.Percent.FormatPriceInvariant() + "%" 
                        : discount.Amount.FormatPrice()
                );
            }

            if (news && labelCount > 0)
                labels.AppendFormat(
                    "<div class=\"products-view-label\"><span class=\"products-view-label-inner products-view-label-new\">{0}</span></div>",
                    LocalizationService.GetResource("Catalog.Label.New"));

            if (customLabels != null)
                foreach (var customLabel in customLabels)
                    labels.Append(
                        "<div class=\"products-view-label\"><span class='products-view-label-inner products-view-label-best'>" +
                        customLabel + "</span></div>");

            return new HtmlString(labels.ToString());
        }

        public static MvcHtmlString SingleBreadCrumb(this HtmlHelper helper, string name)
        {
            var breadCrumbs = new List<BreadCrumbs>();
            var url = new UrlHelper(helper.ViewContext.RequestContext);
            breadCrumbs.Add(new BreadCrumbs(LocalizationService.GetResource("MainPage"), url.AbsoluteRouteUrl("Home")));
            breadCrumbs.Add(new BreadCrumbs(name, string.Empty));
            return helper.Action("BreadCrumbs", "Common", new RouteValueDictionary() {{"breadCrumbs", breadCrumbs}});
        }

        public static HtmlString Numerals(this HtmlHelper helper, float count, string zeroText, string oneText, string twoText, string fiveText)
        {
            return new HtmlString(count + " " + Strings.Numerals(count, zeroText, oneText, twoText, fiveText));
        }

        public static HtmlString Numerals(this HtmlHelper helper, float count, IHtmlString zeroText, IHtmlString oneText, IHtmlString twoText, IHtmlString fiveText)
        {
            return new HtmlString(count + " " + Strings.Numerals(count, zeroText, oneText, twoText, fiveText));
        }


        public static HtmlString GetCustomerManager(this HtmlHelper helper)
        {
            return new HtmlString(CustomerService.GetCurrentCustomerManager());
        }

        public static string GetShippingLocalize(string shippingType)
        {
            return Resource.ResourceManager.GetString(string.Format("Enums_ShippingType_{0}", shippingType), Thread.CurrentThread.CurrentCulture);
        }

        public static string GetPaymentLocalize(string type)
        {
            return Resource.ResourceManager.GetString(string.Format("Enums_PaymentType_{0}", type), Thread.CurrentThread.CurrentCulture);
        }

        public static string GetToolbarClass(this HtmlHelper helper)
        {
            return SettingsDesign.DisplayToolBarBottom || !AdvantShop.Helpers.MobileHelper.IsMobileBrowser() ? "toolbar-bottom-enabled" :"toolbar-bottom-disabled";
        }
    }
}