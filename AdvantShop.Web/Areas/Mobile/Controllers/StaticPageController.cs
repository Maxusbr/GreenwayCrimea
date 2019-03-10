using System.Web.Mvc;
using AdvantShop.CMS;
using AdvantShop.ViewModel.StaticPage;

namespace AdvantShop.Areas.Mobile.Controllers
{
    public class StaticPageController : BaseMobileController
    {
        public ActionResult Index(string url)
        {
            var staticPage = StaticPageService.GetStaticPage(url);
            if (staticPage == null || !staticPage.Enabled)
                return Error404();
            var model = new StaticPageViewModel()
            {
                Title = staticPage.PageName,
                Text = staticPage.PageText,
            };

			SetMetaInformation(staticPage.Meta, staticPage.PageName);

            SetTitle(staticPage.PageName);

            return View(model);
        }
    }
}