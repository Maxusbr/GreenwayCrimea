using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using AdvantShop.Core;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Web.Infrastructure.Localization;
using AdvantShop.Web.Infrastructure.Admin.Buttons;
using AdvantShop.Catalog;
using AdvantShop.Web.Admin.ViewModels.Common;

namespace AdvantShop.Web.Admin.Extensions
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString Label(this HtmlHelper html, string expression, LocalizedString labelText)
        {
            return html.Label(expression, labelText.ToString());
        }

        public static MvcHtmlString Label(this HtmlHelper html, string expression, LocalizedString labelText, object htmlAttributes)
        {
            return html.Label(expression, labelText.ToString(), htmlAttributes);
        }

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, LocalizedString labelText)
        {
            return html.LabelFor(expression, labelText.ToString());
        }

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, LocalizedString labelText, object htmlAttributes)
        {
            return html.LabelFor(expression, labelText.ToString(), htmlAttributes);
        }

        public static MvcHtmlString Button(this HtmlHelper helper, LocalizedString text, eButtonType type = eButtonType.Simple, eButtonSize size = eButtonSize.Small,
                                           eColorType colorType = eColorType.Success, string link = null, string cssClass = null, string name = null, bool validation = false, string[] attributes = null)
        {
            var model = new ButtonModel()
            {
                Text = text,
                Name = name,
                Size = size,
                Type = type,
                ColorType = colorType,
                Link = link,
                Attributes = attributes,
                CssClass = cssClass,
                Validation = validation
            };

            return helper.Partial("_Button", model);
        }

        public static IHtmlString RenderModuleCssBundles(this HtmlHelper helper, string bundleName)
        {
            var bundles = new List<string>();

            foreach (var module in AttachedModules.GetModules<IAdminBundles>())
            {
                var instance = (IAdminBundles)Activator.CreateInstance(module);
                var items = instance.AdminCssBottom();
                if (items != null && items.Count > 0)
                    bundles.AddRange(items);
            }

            if (bundles.Count == 0)
                return new HtmlString("");

            return new HtmlString(JsCssTool.MiniCss(bundles, bundleName));
        }

        public static IHtmlString RenderModuleJsBundles(this HtmlHelper helper, string bundleName)
        {
            var bundles = new List<string>();

            foreach (var module in AttachedModules.GetModules<IAdminBundles>())
            {
                var instance = (IAdminBundles)Activator.CreateInstance(module);
                var items = instance.AdminJsBottom();
                if (items != null && items.Count > 0)
                    bundles.AddRange(items);
            }

            if (bundles.Count == 0)
                return new HtmlString("");

            return new HtmlString(JsCssTool.MiniJs(bundles, bundleName));
        }

        public static MvcHtmlString BootstrapPager(this HtmlHelper helper, int currentPageIndex, Func<int, string> action, int totalItems, int pageSize = 10, int numberOfLinks = 5)
        {
            if (totalItems <= 0)
            {
                return MvcHtmlString.Empty;
            }

            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var lastPageNumber = (int)Math.Ceiling((double)currentPageIndex / numberOfLinks) * numberOfLinks;
            var firstPageNumber = lastPageNumber - (numberOfLinks - 1);
            var hasPreviousPage = currentPageIndex > 1;
            var hasNextPage = currentPageIndex < totalPages;
            if (lastPageNumber > totalPages)
            {
                lastPageNumber = totalPages;
            }

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");
            ul.InnerHtml += AddLink(1, action, currentPageIndex == 1, "disabled", "<<", "First Page");
            ul.InnerHtml += AddLink(currentPageIndex - 1, action, !hasPreviousPage, "disabled", "<", "Previous Page");
            for (int i = firstPageNumber; i <= lastPageNumber; i++)
            {
                ul.InnerHtml += AddLink(i, action, i == currentPageIndex, "active", i.ToString(), i.ToString());
            }

            ul.InnerHtml += AddLink(currentPageIndex + 1, action, !hasNextPage, "disabled", ">", "Next Page");
            ul.InnerHtml += AddLink(totalPages, action, currentPageIndex == totalPages, "disabled", ">>", "Last Page");
            return MvcHtmlString.Create(ul.ToString());
        }

        private static TagBuilder AddLink(int index, Func<int, string> action, bool condition, string classToAdd, string linkText, string tooltip)
        {
            var li = new TagBuilder("li");
            li.MergeAttribute("title", tooltip);
            if (condition)
            {
                li.AddCssClass(classToAdd);
            }
            var a = new TagBuilder("a");
            a.MergeAttribute("href", !condition ? action(index) : "javascript:");
            a.SetInnerText(linkText);
            li.InnerHtml = a.ToString();
            return li;
        }

        public static MvcHtmlString PictureUploader(this HtmlHelper helper, PhotoType photoType, int objId, string startSrc, string ngOnUpdateCallback = null, object htmlAttributes = null, int? pictureId = null)
        {
            var model = new PictureUploader
            {
                StartSrc = startSrc,
                ObjId = objId,
                PhotoType = photoType,
                NgOnUpdateCallback = ngOnUpdateCallback,
                PictureId = pictureId
            };

            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            foreach (var item in attrs)
            {
                model.HtmlAttributes += String.Format(" {0}={1}", item.Key, item.Value);
            }

            switch (photoType)
            {
                case PhotoType.CategoryIcon:
                case PhotoType.CategorySmall:
                case PhotoType.CategoryBig:
                    model.UploadUrl = "/category/uploadPicture";
                    model.UploadByLinkUrl = "/category/uploadPictureByLink";
                    model.DeleteUrl = "/category/deletePicture";
                    break;
                case PhotoType.Brand:
                    model.UploadUrl = "/brands/uploadpicture";
                    model.UploadByLinkUrl = "/brands/uploadPictureByLink";
                    model.DeleteUrl = "/brands/deletePicture";
                    break;
                case PhotoType.News:
                    model.UploadUrl = "/news/uploadpicture";
                    model.UploadByLinkUrl = "/news/uploadPictureByLink";
                    model.DeleteUrl = "/news/deletePicture";
                    break;
                case PhotoType.Logo:
                    model.UploadUrl = "/settings/uploadLogo";
                    model.UploadByLinkUrl = "/settings/uploadLogoByLink";
                    model.DeleteUrl = "/settings/deleteLogo";
                    break;
                case PhotoType.Favicon:
                    model.UploadUrl = "/settings/uploadFavicon";
                    model.UploadByLinkUrl = "/settings/uploadFaviconByLink";
                    model.DeleteUrl = "/settings/deleteFavicon";
                    break;
                default:
                    throw new Exception(String.Format("Not support photoType {0} for pictureUploader", photoType.ToString()));
            }

           return helper.Action("PictureUploader", "Common", model);
        }
    }
}