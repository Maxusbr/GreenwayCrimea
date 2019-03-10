using AdvantShop.Catalog;
using AdvantShop.Helpers;
using AdvantShop.Module.AdditionalMarkers.Models;
using AdvantShop.Module.AdditionalMarkers.Service;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdvantShop.Module.AdditionalMarkers.Controllers
{
    [Module(Type = "AdditionalMarkers")]
    public class AMClientController : ModuleController
    {
        string ModuleID = AdditionalMarkers.ModuleStringId;

        public ActionResult GetMarkers(Product product, Offer offer)
        {
            var model = MarkerService.GetMarkers(product.ProductId);
            return PartialView("~/modules/" + ModuleID + "/Views/Client/_GetMarkers.cshtml", model);
        }

        public ActionResult GetProductViewMarkers()
        {
            return PartialView("~/modules/" + ModuleID + "/Views/Client/_GetProductViewMarkers.cshtml", MobileHelper.IsMobileEnabled());
        }

        public ActionResult GetPVMarkers(List<int> productIds)
        {
            var model = new List<ProductViewMarker>();

            foreach (var productId in productIds)
            {
                var markerForPV = MarkerService.GetLinks(productId);
                if (markerForPV.Markers.Count > 0)
                {
                    model.Add(markerForPV);
                }
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPVMarkersMobile(List<string> productUrls)
        {
            var model = new List<ProductViewMarker>();

            foreach (var productUrl in productUrls)
            {
                var markerForPV = MarkerService.GetLinks(productUrl);
                if (markerForPV.Markers.Count > 0)
                {
                    model.Add(markerForPV);
                }
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}
