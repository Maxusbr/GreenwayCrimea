using AdvantShop.Configuration;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.ViewModel.Home;
using AdvantShop.Web.Infrastructure.Controllers;
using System.Web.Mvc;

namespace AdvantShop.Module.MobileVersion.Controllers
{
    public class MobileVersionController : ModuleController
    {
        public ActionResult MobileOverlap()
        {
            var forcedCookie = CommonHelper.GetCookie("ForcedDesktop");
            if (forcedCookie != null && forcedCookie.Value == "true")
                return new EmptyResult();
            
            var model = new MobileOverlapViewModel()
            {
                logoPath = FoldersHelper.GetPath(FolderType.Pictures, SettingsMain.LogoImageName, false),
                logoAlt = SettingsMain.LogoImageAlt
            };
            return PartialView("~/Modules/MobileVersion/Views/MobileOverlap.cshtml", model);
        }

        public ActionResult ToMobileIcon()
        {
            if (MobileHelper.IsMobileBrowser())
            {
                var model = new MobileOverlapViewModel()
                {
                    ToolbarEnabled = SettingsDesign.DisplayToolBarBottom
                };

                return PartialView("~/Modules/MobileVersion/Views/ToMobileIcon.cshtml", model);
            }
            
            return new EmptyResult();
        }
    }
}
