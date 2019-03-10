using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Filters;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Module.BonusSystemModule.Models;
using AdvantShop.SEO;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Extensions;

namespace AdvantShop.Module.BonusSystemModule.Controllers
{
    [Module(Type = "BonusSystemModule")]
    public partial class HomeController : ModuleController
    {
        // GET: getbonuscard/
        public ActionResult GetBonusCard()
        {
            var breadCrumbs = new List<BreadCrumbs>()
            {
                new BreadCrumbs(T("AdvantShop.MainPage"), Url.AbsoluteRouteUrl("Home")),
                new BreadCrumbs(T("AdvantShop.Module.BonusSystem.GetBonusCardTitle"), Url.AbsoluteRouteUrl("GetBonusCard"))
            };

            var model = new GetBonusCardViewModel
            {
                BreadCrumbs = breadCrumbs,
                BonusTextBlock =
                    ModuleSettingsProvider.GetSettingValue<string>("BonusTextBlock", BonusSystemModule.ModuleID),
                BonusRightTextBlock =
                    ModuleSettingsProvider.GetSettingValue<string>("BonusRightTextBlock", BonusSystemModule.ModuleID),
                Grades =
                    BonusSystem.IsActive &&
                    ModuleSettingsProvider.GetSettingValue<bool>("BonusShowGrades", BonusSystemModule.ModuleID)
                        ? BonusSystemService.GetGrades()
                        : null
            };

            SetMetaInformation(
                new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, T("AdvantShop.Module.BonusSystem.GetBonusCardTitle"))),
                string.Empty);

            return View("~/Modules/BonusSystemModule/Views/Home/GetBonusCard.cshtml", model);
        }
    }
}