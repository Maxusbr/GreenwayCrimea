using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.App.Landing.Controllers.Domain;
using AdvantShop.App.Landing.Handlers.LandingAdmin;
using AdvantShop.App.Landing.Models.LandingAdmin;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.App.Landing.Domain;
using AdvantShop.App.Landing.Filters;
using AdvantShop.App.Landing.Handlers.Install;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Extensions;

using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Saas;

namespace AdvantShop.App.Landing.Controllers
{
    //[AuthLp] // todo: replace on [AdminAuth] and add role DisplayLanding. Or use BaseAdminController
    //[SaasLp]
    //[Auth(RoleAction.Landing)]
    //[SaasFeature(ESaasProperty.LandingPage)]

    public partial class LandingAdminController : LandingBaseController
    {
        private readonly LpService _lpService;
        private readonly LpTemplateService _lpTemplateService;

        public LandingAdminController()
        {
            _lpService = new LpService();
            _lpTemplateService = new LpTemplateService();
        }

        public ActionResult Index()
        {
            SetMetaInformation("Landing");
            SetNgController(NgControllers.NgControllersTypes.LandingsAdminCtrl);

            //return View("~/areas/admin/views/service/UnderConstruction.cshtml");
            return View();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Index(LandingAdminIndexPostModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                return Json(new {result = false});

            model.Url = StringHelper.Translit(model.Name);
            if (model.Template == null)
            {
                var template = _lpTemplateService.GetTemplates().FirstOrDefault();
                if (template != null)
                    model.Template = template.Key;
            }

            var lp = new Lp()
            {
                Name = model.Name,
                Url = model.Url,
                Template = model.Template,
                Enabled = true//model.Enabled,
            };

            var lpConfiguration = new LpConfiguration()
            {
                Type = model.Type,
                Goal = model.Goal
            };

            if (model.Type == LpType.OneProduct)
            {
                var product = ProductService.GetProduct(model.ProductIds.TryParseInt());
                if (product != null)
                {
                    lpConfiguration.Products = new List<Product>() {product};
                }
            }
            else if (model.Type == LpType.FewProducts)
            {
                var ids = model.ProductIds.Split(new[] {','}).Select(x => x.TryParseInt()).ToList();
                lpConfiguration.Products = new List<Product>();
                foreach (var id in ids)
                {
                    var product = ProductService.GetProduct(id);
                    if (product != null)
                    {
                        lpConfiguration.Products.Add(product);
                    }
                }
            }

            var lpId = new InstallTemplateHandler(lp, lpConfiguration).Execute();
            if (lpId == 0)
                return Json(new { result = false, error = "Ошибка при создании лендиннга" });

            return Json(new { result = true, url = Url.AbsoluteRouteUrl("Landing", new { url = lp.Url, inplace = true }) });
        }

        public JsonResult GetLandings(LandingsAdminFilterModel model)
        {
            var handler = new GetLandingsHandler(model);
            var result = handler.Execute();

            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteLanding(int id)
        {
            _lpService.Delete(id);

            return Json(new {result = true});
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Inplace(LandingAdminItemModel model)
        {
            var landing = _lpService.Get(model.Id);
            if (landing != null)
            {
                landing.Enabled = model.Enabled;
                landing.Name = model.Name;

                _lpService.Update(landing);
            }

            return Json(new { result = true });
        }
    }
}
