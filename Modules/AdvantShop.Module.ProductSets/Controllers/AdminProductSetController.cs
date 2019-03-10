using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Module.ProductSets.Domain;
using AdvantShop.Module.ProductSets.Models;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Catalog;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Module.ProductSets.Controllers
{
    [AdminAuth]
    public class AdminProductSetController : ModuleController
    {
        #region Admin Product Tab
        
        public ActionResult AdminProductTab(int productId)
        {
            var model = new AdminProductTabModel()
            {
                ProductId = productId,
                ProductSetDiscount = ProductSetsService.GetDiscount(productId)
            };

            return PartialView("~/Modules/ProductSets/Views/AdminProduct/AdminProductTab.cshtml", model);
        }

        [HttpPost]
        public ActionResult AdminProductTab(AdminProductTabModel model)
        {
            ProductSetsService.SetDiscount(model.ProductId, model.ProductSetDiscount);

            return AdminProductTab(model.ProductId);
        }

        [HttpGet]
        public JsonResult GetLinkedOffers(int productId)
        {
            var items = ProductSetsService.GetLinkedOffers(productId).Select(x => new
            {
                x.ProductId,
                x.OfferId,
                x.Product.Name,
                x.ArtNo,
                ImageSrc = x.Photo.ImageSrcSmall(),
                Error = CheckOffer(x)
            });

            return Json(items);
        }

        private string CheckOffer(Offer o)
        {
            if (!o.Product.Enabled)
                return "Не активен";

            if (!o.Product.CategoryEnabled)
                return "Категория не активна";

            if (o.Amount <= 0)
                return "Кол-во меньше или равно 0";

            if (o.RoundedPrice <= 0)
                return "Цена меньше или равна 0";

            if (CustomOptionsService.DoesProductHaveRequiredCustomOptions(o.ProductId))
                return "У товара есть обязательные дополнительные опции";

            return null;
        }

        [HttpPost]
        public JsonResult DeleteLinkedOffer(int productId, int offerId)
        {
            ProductSetsService.DeleteProductLink(productId, offerId);
            return Json(new {result = true});
        }

        [HttpPost]
        public JsonResult AddLinkedOffers(int productId, List<int> offerIds)
        {
            var product = ProductService.GetProduct(productId);
            if (product == null || offerIds == null)
                return Json(new { result = false });
            
            foreach (var offer in offerIds.Select(x => OfferService.GetOffer(x)))
            {
                if (offer == null || offer.Product.ProductId == product.ProductId)
                    continue;

                ProductSetsService.AddProductLink(productId, offer.OfferId);
            }

            
            return Json(new { result = true });
        }

        #endregion
    }
}
