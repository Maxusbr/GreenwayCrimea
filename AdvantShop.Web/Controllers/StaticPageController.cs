using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.CMS;
using AdvantShop.Core.Services.SEO;
using AdvantShop.ViewModel.StaticPage;
using AdvantShop.Core.Services.InplaceEditor;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;

namespace AdvantShop.Controllers
{
    public partial class StaticPageController : BaseClientController
    {
        public ActionResult Index(string url)
        {

            try {
                throw new System.Exception(string.Format("{0} - {1} - {2}", Request.UserHostAddress, Request.Url.ToString(), Request.UrlReferrer));
            }
            catch (System.Exception ex)
            {
                Debug.Log.Error(ex);
            }

            if (string.IsNullOrEmpty(url))
                return Error404();

            var staticPage = StaticPageService.GetStaticPage(url);
            if (staticPage == null || !staticPage.Enabled)
                return Error404();
	        if (url.Equals("business")) return RedirectToAction("Index", "Business", url);

			var metaInformation = SetMetaInformation(staticPage.Meta, staticPage.PageName);

            var model = new StaticPageViewModel()
            {
                Id = staticPage.StaticPageId,
                Title = staticPage.PageName,
                Text = staticPage.PageText,
                UrlPath = staticPage.UrlPath,
                H1 = metaInformation.H1,
                BreadCrumbs = new List<BreadCrumbs>()
                {
                    new BreadCrumbs(T("MainPage"), Url.RouteUrl("Home")),
                }
            };

            if (!string.IsNullOrEmpty(model.Text) && InplaceEditorService.CanUseInplace(RoleAction.Cms))
            {
                model.Text = InplaceEditorService.PrepareContent(model.Text);
            }

            model.BreadCrumbs.AddRange(
                StaticPageService.GetParentStaticPages(staticPage.StaticPageId)
                    .Select(StaticPageService.GetStaticPage)
                    .Select(stPage => new BreadCrumbs
                    {
                        Name = stPage.PageName,
                        Url = Url.RouteUrl("StaticPage", new { url = stPage.UrlPath })
                    }).Reverse());

            var subPages = StaticPageService.GetChildStaticPages(staticPage.StaticPageId, true);
            foreach (var subPage in subPages)
            {
                model.SubPages.Add(new StaticPageViewModel()
                {
                    Id = subPage.StaticPageId,
                    Title = subPage.PageName,
                    Text = subPage.PageText,
                    UrlPath = subPage.UrlPath
                });
            }

            var tagManager = GoogleTagManagerContext.Current;
            if (tagManager.Enabled)
            {
                tagManager.PageType = ePageType.info;
            }

            return View(model);
        }
    }
}