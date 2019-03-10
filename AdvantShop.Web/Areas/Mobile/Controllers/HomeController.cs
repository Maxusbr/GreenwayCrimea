using System.Web.Mvc;
using AdvantShop.Areas.Mobile.Handlers.Home;
using AdvantShop.Areas.Mobile.Models.Home;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.CMS;
using AdvantShop.Core.Services.Configuration.Settings;
using AdvantShop.FilePath;
using AdvantShop.Repository;

namespace AdvantShop.Areas.Mobile.Controllers
{
    public class HomeController : BaseMobileController
    {
        // GET: Mobile/Home
        public ActionResult Index()
        {
            var model = new HomeMobileHandler().Get();

            SetTitle(T("MainPage") );
            SetMetaInformation(null, string.Empty);

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult Logo()
        {
            if (string.IsNullOrEmpty(SettingsMain.LogoImageName))
                return new EmptyResult();

            var model = new LogoMobileModel
            {
                ImgSource = FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.LogoImageName, false),
                LogoAlt = SettingsMain.LogoImageAlt
            };

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult CityChanger()
        {
            if (!SettingsMobile.DisplayCity)
                return new EmptyResult();

            var model = new CityChangerMobileViewModel()
            {
                CityName = IpZoneContext.CurrentZone.City
            };

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult Carousel()
        {
            if (!SettingsMobile.DisplaySlider)
                return new EmptyResult();

            var sliders = CarouselService.GetAllCarouselsMainPage(ECarouselPageMode.Mobile);
            if (sliders.Count == 0)
                return new EmptyResult();

            return PartialView("Carousel", new CarouselMobileViewModel() {
                Sliders = sliders,
                Speed = SettingsDesign.CarouselAnimationSpeed,
                Pause = SettingsDesign.CarouselAnimationDelay
            });
        }
        
        public ActionResult ToFullVersion()
        {
            // Todo: add cookie

            return RedirectToRoute("Home");
        }
    }
}