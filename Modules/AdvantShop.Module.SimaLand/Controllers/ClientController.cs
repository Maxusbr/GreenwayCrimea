using AdvantShop.Catalog;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Module.SimaLand.Models;
using AdvantShop.Module.SimaLand.ViewModel;
using AdvantShop.Web.Infrastructure.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdvantShop.Module.SimaLand.Controllers
{
    public class ClientController : ModuleController
    {
        public ActionResult Index()
        {
            return Content("<script src=\"/modules/" + SimaLand.ModuleStringId + "/Content/scripts/label-module-simaland.js\"></script>");
        }

        public ActionResult GetLabel(Product product, Offer offer)
        {
            var model = new List<LabelViewModel>();

            var ptp = product.Tags.FirstOrDefault(x => x.UrlPath == "three-pay-two");
            var mtg = product.Tags.FirstOrDefault(x => x.UrlPath == "mt-gift");
            if (ptp != null)
                model.Add(new LabelViewModel { Tag = ptp });
            if (mtg != null)
                model.Add(new LabelViewModel { Tag = mtg });
            if (model.Count > 0)
                return PartialView("~/modules/" + SimaLand.ModuleStringId + "/Views/Client/Labels.cshtml", model);
            return Content("");
        }

        public ActionResult GetButtons(Product product, Offer offer)
        {
            var model = new List<ButtonsViewModel>();

            var ptp = product.Tags.FirstOrDefault(x => x.UrlPath == "three-pay-two");
            var mtg = product.Tags.FirstOrDefault(x => x.UrlPath == "mt-gift");
            if (ptp != null)
                model.Add(new ButtonsViewModel { Tag = ptp });
            if (mtg != null)
                model.Add(new ButtonsViewModel { Tag = mtg });
            if (model.Count > 0)
                return PartialView("~/modules/" + SimaLand.ModuleStringId + "/Views/Client/Buttons.cshtml", model);
            return Content("");
        }

        public ActionResult GetLabelsScript()
        {

            return Content("<script src=\"/modules/" + SimaLand.ModuleStringId + "/Content/scripts/labels-module-simaland.js\"></script>");
        }

        public ActionResult GetLabels(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return Json(new { have = false });
            }

            var sIds = ids.Split(',').Select(x => Convert.ToInt32(x)).ToList();

            var labels = new List<ProductIdLabelsModel>();
            
            foreach (var id in sIds)
            {
                var lbls = "";
                var product = ProductService.GetProduct(id);
                var tag = product.Tags.FirstOrDefault(x => x.UrlPath == "three-pay-two");
                var tag2 = product.Tags.FirstOrDefault(x => x.UrlPath == "mt-gift");
                if (tag != null)
                {
                    var model = new LabelViewModel();
                    model.Tag = tag;

                    var partialView = PartialView("~/modules/" + SimaLand.ModuleStringId + "/Views/Client/vLabels.cshtml", model);

                    lbls = ConvertPartialViewToString(partialView);
                }
                if (tag2 != null)
                {
                    var model = new LabelViewModel();
                    model.Tag = tag2;

                    var partialView = PartialView("~/modules/" + SimaLand.ModuleStringId + "/Views/Client/vLabels.cshtml", model);

                    lbls += ConvertPartialViewToString(partialView);
                }

                if (!string.IsNullOrEmpty(lbls))
                {
                    labels.Add(new ProductIdLabelsModel { ProdctId = product.ProductId, Labels = lbls});
                }
            }
            if (labels.Count > 0)
            {
                return Json(new { have = true, labels }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { have = false});
        }

        public string LinkCss()
        {
            return "<link rel=\"stylesheet\" href=\"/modules/"+SimaLand.ModuleStringId+"/content/styles/client-simaland-module.css\">";
        }

        protected string ConvertPartialViewToString(PartialViewResult partialView)
        {
            using (var sw = new StringWriter())
            {
                partialView.View = ViewEngines.Engines
                  .FindPartialView(ControllerContext, partialView.ViewName).View;

                var vc = new ViewContext(
                  ControllerContext, partialView.View, partialView.ViewData, partialView.TempData, sw);
                partialView.View.Render(vc, sw);

                var partialViewString = sw.GetStringBuilder().ToString();

                return partialViewString;
            }
        }

    }
}
