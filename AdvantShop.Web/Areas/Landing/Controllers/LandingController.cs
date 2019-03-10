using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using AdvantShop.App.Landing.Controllers.Domain;
using AdvantShop.App.Landing.Controllers.Domain.Settings;
using AdvantShop.Catalog;
using AdvantShop.Customers;
using AdvantShop.App.Landing.Domain;
using AdvantShop.App.Landing.Models;
using AdvantShop.Core.Services.SEO;
using AdvantShop.Orders;
using AdvantShop.SEO;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Extensions;

namespace AdvantShop.App.Landing.Controllers
{
    public partial class LandingController : LandingBaseController
    {
        private readonly LpService _lpService;
        private readonly LpBlockService _lpBlockService;
        private readonly LpBlockConfigService _lpBlockConfigService;

        public LandingController()
        {
            _lpService = new LpService();
            _lpBlockService = new LpBlockService();
            _lpBlockConfigService = new LpBlockConfigService();
        }

        public ActionResult Index(string url, bool? inplace)
        {
            var lp = _lpService.Get(url);

            if (lp == null || (!lp.Enabled && !CustomerContext.CurrentCustomer.IsAdmin))
                return Error404();

            LpService.CurrentLanding = lp;
            LpService.Inplace = inplace ?? false;

            var model = new IndexViewModel()
            {
                LandingPage = lp,
                Blocks = _lpBlockService.GetList(lp.Id)
            };

            SetMetaInformation(new MetaInfo()
            {
                Title = SeoSettings.PageTitle,
                MetaKeywords = SeoSettings.PageKeywords,
                MetaDescription = SeoSettings.PageDescription
            });

            WriteLog(LpConstants.EVENT_LANDING_VIEWPAGE, Url.AbsoluteRouteUrl("Landing", new {url}), ePageType.landing);
            
            return View(model);
        }


        [ChildActionOnly]
        public ActionResult Block(int id)
        {
            var block = _lpBlockService.Get(id);
            if (block == null)
                return new EmptyResult();

            var blockConfig = _lpBlockConfigService.Get(block.Name, LpService.CurrentLanding.Template);
            if (blockConfig == null)
                return new EmptyResult();

            var model = new BlockModel()
            {
                Block = block,
                Config = blockConfig,
                InPlace = LpService.Inplace,
            };

            return PartialView("_WrapBlock", model);
        }

        [ChildActionOnly]
        public ActionResult SubBlock(int blockId, string name)
        {
            var subBlock = _lpBlockService.GetSubBlock(blockId, name);

            if (subBlock == null)
                return new EmptyResult();

            var model = new SubBlockModel()
            {
                InPlace = LpService.Inplace,
                SubBlock = subBlock
            };
            
            if (string.IsNullOrEmpty(subBlock.ContentHtml))
            {
                var viewPath = string.Format("~/Areas/Landing/{0}/SubBlocks/{1}.cshtml", LpService.CurrentLanding.Template, subBlock.Name);
                if (ViewExist(viewPath))
                    model.ViewPath = viewPath;
                else
                {
                    viewPath = string.Format("~/Areas/Landing/Views/SubBlocks/{0}.cshtml", subBlock.Name);
                    if (ViewExist(viewPath))
                        model.ViewPath = viewPath;
                }
            }

            return PartialView("_WrapSubBlock", model);
        }


        /// <summary>
        /// Checkout from offerId, attributesXml
        /// </summary>
        /// <param name="offerId"></param>
        /// <param name="attributesXml"></param>
        /// <returns></returns>
        public ActionResult Checkout(int offerId, string attributesXml)
        {
            var cart = ShoppingCartService.CurrentShoppingCart;
            if (cart.HasItems)
            {
                cart.Clear();
                ShoppingCartService.ClearShoppingCart(ShoppingCartType.ShoppingCart);
            }

            var offer = OfferService.GetOffer(offerId);

            if (offer == null) //  || !offer.Product.Enabled || !offer.Product.CategoryEnabled
                return Json(new { status = "fail" });


            List<EvaluatedCustomOptions> listOptions = null;
            var selectedOptions = !String.IsNullOrWhiteSpace(attributesXml) && attributesXml != "null"
                                    ? HttpUtility.UrlDecode(attributesXml)
                                    : null;

            if (selectedOptions != null)
            {
                try
                {
                    listOptions = CustomOptionsService.DeserializeFromXml(selectedOptions, offer.Product.Currency.Rate);
                }
                catch (Exception)
                {
                    listOptions = null;
                }
            }

            /* Uncomment if need it
            if (CustomOptionsService.DoesProductHaveRequiredCustomOptions(offer.ProductId) && listOptions == null)
                return Json(new { status = "redirect" });
            */

            var prodMinAmount = offer.Product.MinAmount == null
                        ? offer.Product.Multiplicity
                        : offer.Product.Multiplicity > offer.Product.MinAmount
                            ? offer.Product.Multiplicity
                            : offer.Product.MinAmount.Value;

            var amount = prodMinAmount > 0 ? prodMinAmount : 1;

            var cartItem = new ShoppingCartItem()
            {
                OfferId = offer.OfferId,
                Amount = amount,
                AttributesXml = listOptions != null ? selectedOptions : string.Empty,
            };

            var cartId = ShoppingCartService.AddShoppingCartItem(cartItem);


            var customer = CustomerContext.CurrentCustomer;
            var current = MyCheckout.Factory(customer.Id);

            if (customer.RegistredUser)
            {
                current.Data.User.Id = customer.Id;
                current.Data.User.Email = customer.EMail;
                current.Data.User.FirstName = customer.FirstName;
                current.Data.User.LastName = customer.LastName;
                current.Data.User.Patronymic = customer.Patronymic;
                current.Data.User.Phone = customer.Phone;
                //current.Data.User.BonusCardNumber = customer.BonusCardNumber;
                current.Data.User.ManagerId = customer.ManagerId;
                current.Update();
            }

            LayoutExtensions.NgController = NgControllers.NgControllersTypes.CheckOutCtrl;
            SetMetaInformation(T("Checkout.Index.CheckoutTitle"));

            WriteLog(LpConstants.EVENT_LANDING_GOAL, "checkout", ePageType.landing);

            return View("~/Areas/Landing/Views/Checkout/Checkout.cshtml", "~/Areas/Landing/Views/Checkout/_Layout.cshtml");
        }
    }
}
