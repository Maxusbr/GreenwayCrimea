using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.SQL;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.SEO;
using AdvantShop.Web.Infrastructure.Controllers;
using Newtonsoft.Json;

namespace AdvantShop.Web.Infrastructure.Extensions
{
    public static class LayoutExtensions
    {
        private const string BundlesContextKey = "Page_Bundle_";

        #region Page title, keywords, description

        public static string Title
        {
            get { return HttpContext.Current.Items["Page_Title"] as string; }
            set { HttpContext.Current.Items["Page_Title"] = value; }
        }

        public static string H1
        {
            get { return HttpContext.Current.Items["Page_H1"] as string; }
            set { HttpContext.Current.Items["Page_H1"] = value; }
        }

        public static NgControllers.NgControllersTypes NgController
        {
            get
            {
                return HttpContext.Current.Items.Contains("Page_NgController")
                    ? (NgControllers.NgControllersTypes)HttpContext.Current.Items["Page_NgController"]
                    : NgControllers.NgControllersTypes.AppCtrl;
            }
            set { HttpContext.Current.Items["Page_NgController"] = value; }
        }

        public static string Description
        {
            get { return HttpContext.Current.Items["Page_Description"] as string; }
            set { HttpContext.Current.Items["Page_Description"] = value; }
        }

        public static string Keywords
        {
            get { return HttpContext.Current.Items["Page_Keywords"] as string; }
            set { HttpContext.Current.Items["Page_Keywords"] = value; }
        }

        /// <summary>
        /// Current page in paging. Start from 1..n
        /// </summary>
        public static int CurrentPage
        {
            get { return Convert.ToInt32(HttpContext.Current.Items["Page_CurrentPage"]); }
            set { HttpContext.Current.Items["Page_CurrentPage"] = value; }
        }

        /// <summary>
        /// Total pages in paging
        /// </summary>
        public static int TotalPages
        {
            get { return Convert.ToInt32(HttpContext.Current.Items["Page_TotalPages"]); }
            set { HttpContext.Current.Items["Page_TotalPages"] = value; }
        }

        public static MetaType MetaType
        {
            get
            {
                MetaType metaType = MetaType.Default;
                var type = HttpContext.Current.Items["Page_MetaType"] as string;
                if (Enum.TryParse(type, false, out metaType))
                    return metaType;

                return MetaType.Default;
            }
            set { HttpContext.Current.Items["Page_MetaType"] = value; }
        }

        public static string NotifyMessages
        {
            get { return HttpContext.Current.Items["Page_NotifyMessages"] as string; }
            set { HttpContext.Current.Items["Page_NotifyMessages"] = value; }
        }

        public static IHtmlString GetPageTitle(this HtmlHelper helper)
        {
            return new HtmlString(Title.HtmlEncode());
        }

        public static IHtmlString GetPageH1(this HtmlHelper helper)
        {
            return new HtmlString(H1.HtmlEncode());
        }

        public static IHtmlString GetNgController(this HtmlHelper helper)
        {
            return new HtmlString(NgControllers.GetNgControllerInitString(NgController));
        }

        public static IHtmlString GetPageDescription(this HtmlHelper helper)
        {
            return new HtmlString(Description.HtmlEncode());
        }

        public static IHtmlString GetPageKeywords(this HtmlHelper helper)
        {
            return new HtmlString(Keywords.HtmlEncode());
        }

        public static IHtmlString GetCanonicalTag(this HtmlHelper helper)
        {
            if (CurrentPage != 0 && TotalPages != 0 && HttpContext.Current != null)
            {
                var query = HttpContext.Current.Request.QueryString;
                if (query.Count == 0 || (query.Count == 1 && query["page"] != null))
                {
                    var tag = string.Format("<link rel=\"canonical\" href=\"{0}\" />", UrlService.GetCanonicalUrl());

                    if (CurrentPage >= 2)
                        tag += string.Format("\n<link rel=\"prev\" href=\"{0}{1}\" />", UrlService.GetCanonicalUrl(),
                            CurrentPage != 2 ? "?page=" + (CurrentPage - 1) : "");

                    if (CurrentPage < TotalPages)
                        tag += string.Format("\n<link rel=\"next\" href=\"{0}?page={1}\" />",
                            UrlService.GetCanonicalUrl(), CurrentPage + 1);

                    return new HtmlString(tag);
                }
            }

            return new HtmlString("<link rel=\"canonical\" href=\"" + UrlService.GetCanonicalUrl() + "\" />");
        }

        public static MvcHtmlString GetNotifyMessages(this HtmlHelper helper)
        {
            List<Notify> list = null;

            var str = helper.ViewContext.TempData[BaseController.NotifyMessages] as string;
            if (!string.IsNullOrEmpty(str))
            {
                helper.ViewContext.TempData[BaseController.NotifyMessages] = string.Empty;
                list = JsonConvert.DeserializeObject<List<Notify>>(str);
            }

            return helper.Partial("~/Views/Shared/_Notify.cshtml", list ?? new List<Notify>());
        }

        public static MvcHtmlString GetNotifications(this HtmlHelper helper)
        {
            List<Notify> list = null;

            var str = helper.ViewContext.TempData[BaseController.Notifications] as string;
            if (!string.IsNullOrEmpty(str))
            {
                helper.ViewContext.TempData[BaseController.Notifications] = string.Empty;
                list = JsonConvert.DeserializeObject<List<Notify>>(str);
            }

            return helper.Partial("~/Areas/Admin/Views/Shared/_Notifications.cshtml", list ?? new List<Notify>());
        }

        #endregion

        #region Bundles

        public static void AddBundles(this HtmlHelper helper, List<string> list, string bundleName)
        {
            var bundles = HttpContext.Current.Items[BundlesContextKey + bundleName] as List<string> ?? new List<string>();
            bundles.AddRange(list);

            HttpContext.Current.Items[BundlesContextKey + bundleName] = bundles;
        }

        public static void AddBundle(this HtmlHelper helper, string listItem, string bundleName)
        {
            var bundles = HttpContext.Current.Items[BundlesContextKey + bundleName] as List<string> ?? new List<string>();
            bundles.Add(listItem);

            HttpContext.Current.Items[BundlesContextKey + bundleName] = bundles;
        }

        public static void RemoveBundle(this HtmlHelper helper, string name, string bundleName)
        {
            var bundles = HttpContext.Current.Items[BundlesContextKey + bundleName] as List<string> ?? new List<string>();

            var b = bundles.Find(x => x == name);
            if (b != null)
                bundles.Remove(b);

            HttpContext.Current.Items[BundlesContextKey + bundleName] = bundles;
        }

        public static IHtmlString RenderCssBundle(this HtmlHelper helper, string bundleName)
        {
            var bundles = HttpContext.Current.Items[BundlesContextKey + bundleName] as List<string>;

            if (bundles == null)
                return new HtmlString("");

            return new HtmlString(JsCssTool.MiniCss(bundles, bundleName));
        }

        public static IHtmlString RenderJsBundle(this HtmlHelper helper, string bundleName)
        {
            var bundles = HttpContext.Current.Items[BundlesContextKey + bundleName] as List<string>;

            if (bundles == null)
                return new HtmlString("");

            return new HtmlString(JsCssTool.MiniJs(bundles, bundleName));
        }

        public static IHtmlString RenderModuleCssBundle(this HtmlHelper helper, string bundleName)
        {
            var bundles = new List<string>();

            foreach (var moduleType in AttachedModules.GetModules<IModuleBundles>())
            {
                var moduleBundles = ((IModuleBundles)Activator.CreateInstance(moduleType)).GetCssBundles();
                if (moduleBundles != null && moduleBundles.Count > 0)
                    bundles.AddRange(moduleBundles);
            }

            if (bundles.Count == 0)
                return new HtmlString("");

            return new HtmlString(JsCssTool.MiniCss(bundles, bundleName));
        }

        public static IHtmlString RenderModuleJsBundle(this HtmlHelper helper, string bundleName)
        {
            var bundles = new List<string>();

            foreach (var moduleType in AttachedModules.GetModules<IModuleBundles>())
            {
                var moduleBundles = ((IModuleBundles)Activator.CreateInstance(moduleType)).GetJsBundles();
                if (moduleBundles != null && moduleBundles.Count > 0)
                    bundles.AddRange(moduleBundles);
            }

            if (bundles.Count == 0)
                return new HtmlString("");

            return new HtmlString(JsCssTool.MiniJs(bundles, bundleName));
        }

        #endregion

        public static IHtmlString GetMiniProfiler(this HtmlHelper helper)
        {
            if (SettingProvider.GetConfigSettingValue("Profiling") != "true")
                return new HtmlString("");

            var sb = new StringBuilder();

            var profilerSql = HttpContext.Current.Items["MiniProfiler_Sql"] as List<Profiling>;
            if (profilerSql != null)
            {
                sb.AppendFormat(
                    "<div class=\"pf\"> <div class=\"pf-title\">SQL queries: <span>(count: {0} time: {1} ms)</span></div>",
                    profilerSql.Count, profilerSql.Sum(x => x.Time).ToString("F2"));

                foreach (var pf in profilerSql)
                {
                    sb.AppendFormat(
                        "<div class=\"pf-item\">" +
                        "<div class=\"pf-name\">{0}<div class=\"pf-params\">{1}</div></div>" +
                        "<div class=\"pf-time\">{2} <span>ms</span></div>" +
                        "</div>",
                        pf.Command,
                        pf.Parameters.Aggregate("", (current, x) => current + string.Format("{0}: {1};", x.Key, x.Value)),
                        pf.Time.ToString("F2"));
                }
                sb.Append("</div>");
            }

            var profilerActions = HttpContext.Current.Items["MiniProfiler_Actions"] as List<Profiling>;
            if (profilerActions != null)
            {
                sb.AppendFormat(
                    "<div class=\"pf\"> <div class=\"pf-title\">Actions <span>(count: {0} time: {1} ms)</span></div>",
                    profilerActions.Count, profilerActions.Sum(x => x.Time).ToString("F2"));

                foreach (var pf in profilerActions)
                {
                    sb.AppendFormat(
                        "<div class=\"pf-item\">" +
                        "<div class=\"pf-name\">{0}</div>" +
                        "<div class=\"pf-time\">{1} <span>ms</span></div>" +
                        "</div>",
                        pf.Command,
                        pf.Time.ToString("F3"));
                }
                sb.Append("</div>");
            }

            return new HtmlString(sb.ToString());
        }
    }
}