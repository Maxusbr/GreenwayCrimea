using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using AdvantShop.CMS;
using AdvantShop.Core.Modules;
using AdvantShop.Module.Blog.Domain;
using AdvantShop.Module.Blog.Handlers.Blog;
using AdvantShop.Module.Blog.Models;
using AdvantShop.SEO;
using AdvantShop.Web.Infrastructure.ActionResults;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Extensions;
using AdvantShop.Web.Infrastructure.Filters;
using System.Xml.Linq;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Core.Services.SEO.MetaData;

namespace AdvantShop.Module.Blog.Controllers
{
    [Module(Type = "Blog")]
    public partial class HomeController : ModuleController
    {
        // GET: blog/
        // GET: blog/{category}/
        public ActionResult BlogCategory(string category, int? page)
        {
            var blogCategory = !string.IsNullOrEmpty(category) ? BlogService.GetBlogCategory(category) : null;

            if (!string.IsNullOrEmpty(category) && blogCategory == null)
                return Error404();

            var blogCategoryId = blogCategory != null ? blogCategory.ItemCategoryId : 0;
            var currentPage = page ?? 1;

            var paging = new BlogPagingHandler(blogCategoryId, currentPage).Get();
            if ((paging.Pager.TotalPages < paging.Pager.CurrentPage && paging.Pager.CurrentPage > 1) ||
                    paging.Pager.CurrentPage < 0)
            {
                return Error404();
            }

            var blogTitle = ModuleSettingsProvider.GetSettingValue<string>("PageTitle", Blog.ModuleID);
            var categoryTitle = ModuleSettingsProvider.GetSettingValue<string>("CategoriesListTitle", Blog.ModuleID);

            var breadCrumbs = blogCategory != null
                ? new List<BreadCrumbs>()
                {
                    new BreadCrumbs(T("MainPage"), Url.AbsoluteRouteUrl("Home")),
                    new BreadCrumbs(blogTitle, Url.AbsoluteRouteUrl("BlogHome")),
                    new BreadCrumbs(blogCategory.Name, Url.AbsoluteRouteUrl("BlogCategory", blogCategory.UrlPath))
                }
                : new List<BreadCrumbs>()
                {
                    new BreadCrumbs(T("MainPage"), Url.AbsoluteRouteUrl("Home")),
                    new BreadCrumbs(blogTitle, Url.AbsoluteRouteUrl("BlogHome"))
                };

            var model = new BlogCategoryViewModel
            {
                BlogCategory = blogCategory,
                SubCategories = new BlogCategoryListViewModel
                {
                    BlogCategories = BlogService.GetListBlogCategory().Where(item => item.CountItems > 0).ToList(),
                    Selected = blogCategory != null ? blogCategory.ItemCategoryId : 0,
                    Title = categoryTitle
                },
                Pager = paging.Pager,
                BlogItems = paging.BlogItems,
                BreadCrumbs = breadCrumbs,
                BlogTitle = blogTitle,
                SbRightBlock = ModuleSettingsProvider.GetSettingValue<string>("SbBlogRight", Blog.ModuleID),
                ShowAddDate = ModuleSettingsProvider.GetSettingValue<bool>("ShowAddDate", Blog.ModuleID)
            };

            var meta = blogCategory == null
                ? new MetaInfo()
                {
                    Title = HttpUtility.HtmlEncode(ModuleSettingsProvider.GetSettingValue<string>("MetaTitle", Blog.ModuleID)),
                    MetaKeywords = HttpUtility.HtmlEncode(ModuleSettingsProvider.GetSettingValue<string>("MetaKeywords", Blog.ModuleID)),
                    MetaDescription = HttpUtility.HtmlEncode(ModuleSettingsProvider.GetSettingValue<string>("MetaDescription", Blog.ModuleID)),
                    H1 = HttpUtility.HtmlEncode(ModuleSettingsProvider.GetSettingValue<string>("MetaTitle", Blog.ModuleID)),
                }
                : new MetaInfo()
                {
                    Title = blogCategory.MetaTitle,
                    MetaKeywords = blogCategory.MetaKeywords,
                    MetaDescription = blogCategory.MetaDescription,
                    H1 = blogCategory.MetaTitle
                };

            SetMetaInformation(meta, meta.Title, page: currentPage);
            
            var metaBlog = new OpenGraphModel();
            foreach (var blog in model.BlogItems)
            {
                metaBlog.Images.Add(blog.Picture);
            }
            metaBlog.Type = OpenGraphType.Article;

            MetaDataContext.CurrentObject = metaBlog;

            return View("~/Modules/Blog/Views/Home/BlogCategory.cshtml", model);
        }

        // GET: blog/{url}
        public ActionResult BlogItem(string url) // string category, 
        {
            var blogItem = BlogService.GetBlogItem(url);
            if (blogItem == null)
                return Error404();

            var blogCategory = blogItem.ItemCategoryId != null
                ? BlogService.GetBlogCategory((int) blogItem.ItemCategoryId)
                : null;

            var blogTitle = ModuleSettingsProvider.GetSettingValue<string>("PageTitle", Blog.ModuleID);
            var categoryTitle = ModuleSettingsProvider.GetSettingValue<string>("CategoriesListTitle", Blog.ModuleID);

            var breadcrumbs = new List<BreadCrumbs>
            {
                new BreadCrumbs(T("MainPage"), Url.AbsoluteRouteUrl("Home")),
                new BreadCrumbs(blogTitle,  Url.AbsoluteRouteUrl("BlogHome")),
            };

            if (blogCategory != null)
            {
                breadcrumbs.Add(new BreadCrumbs(blogCategory.Name, Url.AbsoluteRouteUrl("BlogCategory", new { category = blogCategory.UrlPath })));
            }

            breadcrumbs.Add(new BreadCrumbs(blogItem.Title, Url.AbsoluteRouteUrl("BlogItem", new { url = blogItem.UrlPath })));

            SetMetaInformation(
                new MetaInfo()
                {
                    Title = blogItem.MetaTitle,
                    MetaKeywords = blogItem.MetaKeywords,
                    MetaDescription = blogItem.MetaDescription,
                    H1 = blogItem.MetaTitle,
                },
                blogItem.Title);

            var model = new BlogItemViewModel()
            {
                BlogItem = blogItem,
                BreadCrumbs = breadcrumbs,
                BlogCategoriesList = new BlogCategoryListViewModel
                {
                    BlogCategories = BlogService.GetListBlogCategory().Where(item => item.CountItems > 0).ToList(),
                    Selected = blogItem.ItemCategoryId ?? 0,
                    Title = categoryTitle
                },
                SbAfterText = ModuleSettingsProvider.GetSettingValue<string>("StaticBlock", Blog.ModuleID),
                SbRightBlock = ModuleSettingsProvider.GetSettingValue<string>("SbBlogRight", Blog.ModuleID),
                ShowAddDate = ModuleSettingsProvider.GetSettingValue<bool>("ShowAddDate", Blog.ModuleID)
            };

            model.BlogProducts = new BlogProductsViewModel
            {
                Products = new ProductViewModel(BlogService.GetBlogProductModels(blogItem.ItemId))
                {
                    CountProductsInLine = 1,
                    DisplayPhotoPreviews = false
                }
            };

            var metaBlog = new OpenGraphModel();
            if (model.BlogItem.Picture != null && !string.IsNullOrEmpty(model.BlogItem.Picture))
            {
                metaBlog.Images.Clear();
                metaBlog.Images.Add(UrlService.GetUrl() + "pictures/modules/blog/" + model.BlogItem.Picture);
            }
            metaBlog.Type = OpenGraphType.Article;

            MetaDataContext.CurrentObject = metaBlog;

            return View("~/Modules/Blog/Views/Home/BlogItem.cshtml", model);
        }

        //[ChildActionOnly]
        //public ActionResult BlogProduct(string url)
        //{
        //    return PartialView("~/Modules/Blog/Views/Home/BlogProduct.cshtml", null);
        //}

        public ActionResult Rss()
        {
            if(!ModuleSettingsProvider.GetSettingValue<bool>("ShowRssBlog", Blog.ModuleID))
            {
                return Error404();
            }
            var blogTitle = ModuleSettingsProvider.GetSettingValue<string>("PageTitle", Blog.ModuleID);
            var paging = new BlogPagingHandler(0, 1, 100).Get();

            var result = new List<SyndicationItem>();
            foreach (var item in paging.BlogItems)
            {
                var temp = new SyndicationItem(item.Title, item.TextAnnotation, new Uri(Url.AbsoluteRouteUrl("BlogItem", new { url = item.UrlPath })));
                temp.PublishDate = item.AddingDate;
                var photoSrc = !string.IsNullOrEmpty(item.Picture) ? (UrlService.GetUrl() + "pictures/modules/blog/" + item.Picture) : null;
                if (!string.IsNullOrWhiteSpace(photoSrc))
                    temp.ElementExtensions.Add(new XElement("enclosure", new XAttribute("type", "image/jpeg"), new XAttribute("url", photoSrc)));

                result.Add(temp);
            }

            //var items = paging.BlogItems.Select(x => new SyndicationItem(x.Title, x.TextAnnotation, new Uri(Url.AbsoluteRouteUrl("BlogItem", new { url = x.UrlPath })))
            //{
            //    PublishDate = x.AddingDate
            //}).ToList();
            return new RssResult(blogTitle, result);
        }
    }
}