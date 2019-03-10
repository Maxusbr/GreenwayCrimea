using System.Collections.Generic;
using System.Web.Mvc;

using AdvantShop.CMS;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Module.BonusSystemModule.Models;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Extensions;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Module.BonusSystemModule.Controllers
{
    [Module(Type = "BonusSystemModule")]
    public partial class BonusSystemController : ModuleController
    {
        // GET: getbonuscard/
        public ActionResult GetBonusCard()
        {
            var breadCrumbs = new List<BreadCrumbs>()
            {
                new BreadCrumbs(T("MainPage"), Url.AbsoluteRouteUrl("Home")),
                new BreadCrumbs(T("Module.BonusSystem.GetBonusCardTitle"), Url.AbsoluteRouteUrl("GetBonusCard"))
            };

            var model = new GetBonusCardViewModel
            {
                BreadCrumbs = breadCrumbs,
                BonusTextBlock = ModuleSettingsProvider.GetSettingValue<string>("BonusTextBlock", BonusSystemModule.ModuleID),
                BonusRightTextBlock = ModuleSettingsProvider.GetSettingValue<string>("BonusRightTextBlock", BonusSystemModule.ModuleID),
                Grades =
                    BonusSystem.IsActive &&
                    ModuleSettingsProvider.GetSettingValue<bool>("BonusShowGrades", BonusSystemModule.ModuleID)
                        ? BonusSystemService.GetGrades()
                        : null
            };

            SetMetaInformation(T("Module.BonusSystem.GetBonusCardTitle"));

            return View("~/Modules/BonusSystemModule/Views/BonusSystem/GetBonusCard.cshtml", model);
        }
    }
}