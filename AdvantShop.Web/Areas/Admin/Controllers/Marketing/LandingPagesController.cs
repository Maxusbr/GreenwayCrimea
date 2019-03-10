using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Models.LandingPages;
using AdvantShop.Web.Infrastructure.Controllers;

namespace AdvantShop.Web.Admin.Controllers.Marketing
{
    [Auth(RoleAction.Marketing)]
    [SaasFeature(Saas.ESaasProperty.LandingPage)]
    public partial class LandingPagesController : BaseAdminController
    {
        public ActionResult Index()
        {
            var model = new LandingPageAdminModel
            {
                ActiveLandingPage = SettingsLandingPage.ActiveLandingPage,
                LandingPageCommonStatic = 
                    SettingsLandingPage.LandingPageCommonStatic ??
                    "<div class='plp-reasons'><div class='plp-subTitle'>5 причин купить у нас:</div><div class='r-container'>" +
                    "<div class='row center-xs'><div class='col-xs-3 cs-bg-3 reas-flex'>" +
                    "<div class='r-item'><div class='r-img'><img src='modules/productlandingpage/templates/lp/pictures/r1.png' alt='' /></div>" +
                    "<div class='r-info'><div class='r-title'>100% гарантия возврата</div>" +
                    "<div class='r-txt'>Мы вернем деньги, если продукция вам не подойдет или не понравится</div></div></div></div><div class='col-xs-3 cs-bg-3 reas-flex'>" +
                    "<div class='r-item'><div class='r-img'><img src='modules/productlandingpage/templates/lp/pictures/r2.png' alt='' /></div>" +
                    "<div class='r-info'><div class='r-title'>Доставим без предоплаты</div>" +
                    "<div class='r-txt'>Мы доставим товар в любой регион России с оплатой при получении</div></div></div></div>" +
                    "<div class='col-xs-3 cs-bg-3 reas-flex'><div class='r-item'><div class='r-img'><img src='modules/productlandingpage/templates/lp/pictures/r3.png' alt='' /></div>" +
                    "<div class='r-info'><div class='r-title'>Подарок при покупке</div>" +
                    "<div class='r-txt'>При любой покупке вы получаете шикарную косметичку в подарок</div></div></div></div>" +
                    "<div class='col-xs-3 cs-bg-3 reas-flex'><div class='r-item'><div class='r-img'><img src='modules/productlandingpage/templates/lp/pictures/r4.png' alt='' /></div>" +
                    "<div class='r-info'><div class='r-title'>При покупке ноутбука</div><div class='r-txt'>Стильная сумка <br /> в подарок!</div></div></div></div>" +
                    "<div class='col-xs-3 cs-bg-3 reas-flex'><div class='r-item'><div class='r-img'><img src='modules/productlandingpage/templates/lp/pictures/r5.png' alt='' /></div>" +
                    "<div class='r-info'><div class='r-title'>Возврат без проблем</div>" +
                    "<div class='r-txt'>Возвращаете товар курьеру если чтото не нравится. + Возврат в течении 14 дней</div></div></div></div></div></div></div>"
            };

            SetMetaInformation("Landing Page");
            SetNgController(NgControllers.NgControllersTypes.LandingPagesCtrl);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(LandingPageAdminModel model)
        {
            // fix for moving landing from module to engine
            SettingsLandingPage.LandingPageCommonStatic = model.LandingPageCommonStatic.DefaultOrEmpty().Replace("modules/productlandingpage/", "landings/");
            SettingsLandingPage.ActiveLandingPage = model.ActiveLandingPage;

            return RedirectToAction("Index");
        }
    }
}
