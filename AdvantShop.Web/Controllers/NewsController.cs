using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.InplaceEditor;
using AdvantShop.Core.Services.SEO;
using AdvantShop.Customers;
using AdvantShop.Handlers.News;
using AdvantShop.News;
using AdvantShop.SEO;
using AdvantShop.ViewModel.News;
using AdvantShop.Web.Infrastructure.ActionResults;
using AdvantShop.Web.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml.Linq;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.SEO.MetaData;
using AdvantShop.FilePath;

namespace AdvantShop.Controllers
{
    public partial class NewsController : BaseClientController
    {
        // GET: news, newscategory/{url}
        public ActionResult NewsCategory(string url, int? page)
        {
            var newsCategory = !string.IsNullOrEmpty(url) ? NewsService.GetNewsCategory(url) : null;

            if (!string.IsNullOrEmpty(url) && newsCategory == null)
                return Error404();

            var newsCategoryId = newsCategory != null ? newsCategory.NewsCategoryId : 0;
            var currentPage = page ?? 1;

            var paging = new NewsPagingHandler(newsCategoryId, currentPage).Get();
            if ((paging.Pager.TotalPages < paging.Pager.CurrentPage && paging.Pager.CurrentPage > 1) ||
                    paging.Pager.CurrentPage < 0)
            {
                return Error404();
            }

            var breadCrumbs = newsCategory != null
                ? new List<BreadCrumbs>()
                {
                    new BreadCrumbs(T("MainPage"), Url.AbsoluteRouteUrl("Home")),
                    new BreadCrumbs(T("News.NewsItem.News"), Url.AbsoluteRouteUrl("NewsHome")),
                    new BreadCrumbs(newsCategory.Name, Url.AbsoluteRouteUrl("NewsCategory", newsCategory.UrlPath))
                }
                : new List<BreadCrumbs>()
                {
                    new BreadCrumbs(T("MainPage"), Url.AbsoluteRouteUrl("Home")),
                    new BreadCrumbs(T("News.NewsItem.News"), Url.AbsoluteRouteUrl("NewsHome"))
                };

            var meta = newsCategory == null
              ? new MetaInfo
              {
                  Type = MetaType.News,
                  Title = SettingsNews.NewsMetaTitle,
                  MetaKeywords = SettingsNews.NewsMetaKeywords,
                  MetaDescription = SettingsNews.NewsMetaDescription,
                  H1 = SettingsNews.NewsMetaH1
              }
              : newsCategory.Meta;

            var newsMeta = SetMetaInformation(meta, 
                name: newsCategory != null ? newsCategory.Name : string.Empty,
                page: currentPage, 
                totalPages: paging.Pager != null ? paging.Pager.TotalPages : 0);

            var model = new NewsCategoryViewModel
            {
                NewsCategory = newsCategory,
                SubCategories = new NewsCategoryListViewModel
                {
                    NewsCategories = NewsService.GetNewsCategories(true).Where(item => item.CountNews > 0).ToList(),
                    Selected = newsCategory != null ? newsCategory.NewsCategoryId : 0
                },
                Pager = paging.Pager,
                News = paging.News,
                BreadCrumbs = breadCrumbs,
                H1 = newsMeta.H1
            };
            
            var tagManager = GoogleTagManagerContext.Current;
            if (tagManager.Enabled)
                tagManager.PageType = ePageType.info;

            var og = new OpenGraphModel() {Type = OpenGraphType.Article};
            foreach (var news in model.News)
                if (news.Picture != null && !string.IsNullOrEmpty(news.Picture.PhotoName))
                    og.Images.Add(news.Picture.ImageSrc());

            MetaDataContext.CurrentObject = og;

            return View(model);
        }

        // GET: news/{url}
        public ActionResult NewsItem(string url)
        {
            var newsItem = NewsService.GetNews(url);
            if (newsItem == null || !newsItem.Enabled)
                return Error404();

            var category = NewsService.GetNewsCategoryById(newsItem.NewsCategoryId);
            var breadcrumbs = new List<BreadCrumbs>
            {
                new BreadCrumbs(T("MainPage"), Url.AbsoluteRouteUrl("Home")),
                new BreadCrumbs(T("News.NewsItem.News"),  Url.AbsoluteRouteUrl("NewsHome")),
                new BreadCrumbs(category.Name, Url.AbsoluteRouteUrl("NewsCategory", new { url = category.UrlPath})),
                new BreadCrumbs(newsItem.Title, newsItem.UrlPath),
            };

            SetMetaInformation(newsItem.Meta, newsItem.Title);

            var model = new NewsItemViewModel()
            {
                NewsItem = newsItem,
                NewsCategoriesList = new NewsCategoryListViewModel
                {
                    NewsCategories = NewsService.GetNewsCategories(true).Where(item => item.CountNews > 0).ToList(),
                    Selected = newsItem.NewsCategoryId
                },
                BreadCrumbs = breadcrumbs
            };

            if (!string.IsNullOrEmpty(model.NewsItem.TextToPublication) && InplaceEditorService.CanUseInplace(RoleAction.Cms))
            {
                model.NewsItem.TextToPublication = InplaceEditorService.PrepareContent(model.NewsItem.TextToPublication);
            }

            var tagManager = GoogleTagManagerContext.Current;
            if (tagManager.Enabled)
            {
                tagManager.PageType = ePageType.info;
            }

            model.NewsProducts = new NewsProductsViewModel
            {
                Products = new ProductViewModel(NewsService.GetNewsProductModels(newsItem.NewsId))
                {
                    CountProductsInLine = 1,
                    DisplayPhotoPreviews = false
                }
            };

            var metaNews = new OpenGraphModel();
            if (model.NewsItem.Picture != null && !string.IsNullOrEmpty(model.NewsItem.Picture.ImageSrc()))
            {
                metaNews.Images.Clear();
                metaNews.Images.Add(model.NewsItem.Picture.ImageSrc());
            }
            metaNews.Type = OpenGraphType.Article;

            MetaDataContext.CurrentObject = metaNews;

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult NewsBlock()
        {
            if (!SettingsDesign.NewsVisibility)
                return new EmptyResult();

            var news = NewsService.GetNewsForMainPage();
            if (news.Count == 0)
                return new EmptyResult();

            return PartialView("NewsBlock", news);
        }

        public JsonResult Subscribe(string email, bool? agree)
        {
            if (SettingsCheckout.IsShowUserAgreementText && agree != null && !agree.Value)
                return Json(new { status = "error", agree = "none" });

            if (string.IsNullOrWhiteSpace(email) || SubscriptionService.IsSubscribe(email))
                return Json(new { status = "error" });

            SubscriptionService.Subscribe(email);

            return Json(new { status = "success" });
        }

        public ActionResult NewsSubscription()
        {
            if (!SettingsDesign.NewsSubscriptionVisibility)
                return new EmptyResult();

            return PartialView();
        }

        public ActionResult NewsSubscriptionSlim()
        {
            if (!SettingsDesign.NewsSubscriptionVisibility)
                return new EmptyResult();

            return PartialView();
        }


        public ActionResult Rss()
        {
            var result = new List<SyndicationItem>();
            if (SettingsNews.RssViewNews)
            {
                var paging = new NewsPagingHandler(0, 1, 100).Get();
                
                foreach (var item in paging.News)
                {
                    var temp = new SyndicationItem(item.Title, item.TextAnnotation, new Uri(Url.AbsoluteRouteUrl("News", new { url = item.UrlPath })));
                    temp.PublishDate = item.AddingDate;

                    var photoSrc = !string.IsNullOrEmpty(item.Picture.ImageSrc()) ? item.Picture.ImageSrc() : null;
                    var photoSize = Helpers.FileHelpers.GetFileSize(FoldersHelper.GetPathAbsolut(FolderType.News, item.Picture.PhotoName));
                    if (!string.IsNullOrWhiteSpace(photoSrc))
                        temp.ElementExtensions.Add(new XElement("enclosure",
                                                   new XAttribute("type", "image/jpeg"),
                                                   new XAttribute("url", photoSrc),
                                                   new XAttribute("length", photoSize > 0 ? photoSize.ToString() : "1")
                                                   ));

                    result.Add(temp);
                }
            }
            else
            {
                return Error404();
            }
            //var items = paging.News.Select(x => new SyndicationItem(x.Title, x.TextAnnotation, new Uri(Url.AbsoluteRouteUrl("News", new { url = x.UrlPath })))
            //{
            //    PublishDate = x.AddingDate
            //}).ToList();
            return new RssResult(T("News.NewsItem.News"), result);
        }
    }
}