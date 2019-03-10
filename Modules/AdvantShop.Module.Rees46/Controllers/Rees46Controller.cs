using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Module.Rees46.Domain;
using AdvantShop.Orders;
using AdvantShop.Web.Infrastructure.Controllers;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using AdvantShop.Core.UrlRewriter;

namespace AdvantShop.Module.Rees46.Controllers
{
    public class Rees46Controller : ModuleController
    {
        public ActionResult GetScript()
        {
            var customer = CustomerContext.CurrentCustomer;

            var str = "<script type=\"text/javascript\" src=\"modules/rees46/js/lib.js\"></script>\n" +
                "<script type=\"text/javascript\"> \n" + 
                "  (function(r){ window.r46 = window.r46 || function(){ (r46.q = r46.q ||[]).push(arguments)};var s = document.getElementsByTagName(r)[0], rs = document.createElement(r); rs.async = 1; rs.src = '//cdn.rees46.com/v3.js';s.parentNode.insertBefore(rs, s); })('script');" +
                "  r46('init','" + Rees46Settings.ShopKey + "');\n" +
                
                (customer != null && customer.RegistredUser
                    ? "r46('profile', 'set', { id: '" + customer.Id + "', email: '" + customer.EMail + "' } );"
                    : "") +

                "</script>\n";

            return Content(str);
        }

        public ActionResult ShoppingcartAfter()
        {
            if (!Rees46Settings.DisplayInShoppingCart || CustomerContext.CurrentCustomer == null)
                return new EmptyResult();

            var cart = ShoppingCartService.CurrentShoppingCart;
            if (cart == null || !cart.HasItems)
                return new EmptyResult();

            var html = Rees46Service.GetRecomender(Recomender.see_also,
                cartIds: cart.Select(x => x.Offer.OfferId).ToList(), title: Rees46Settings.ShoppingCartTitle);

            return Content(html);
        }

        public ActionResult CheckoutFinalStep(IOrder order)
        {
            try
            {
                var productsTemp = order.OrderItems.Where(x => x.ProductID > 0);
                var productsList = new List<ProductRees46>();
                foreach (var item in productsTemp)
                {
                    if(item.ProductID == null)
                        continue;
                    var offer = OfferService.GetProductOffers(item.ProductID.Value).Find(x => x.ArtNo == item.ArtNo);
                    if(offer == null)
                        continue;
                    productsList.Add(new ProductRees46()
                    {
                        Id = offer.OfferId,
                        Price = offer.BasePrice.ToString().Replace(",","."),
                        Amount = item.Amount
                    });
                }
                var obj = new TrackEvents()
                {
                    Order = order.Number,
                    OrderPrice = order.Sum.ToString().Replace(",", "."),
                    Products = productsList.ToArray()
                };
                return Content(Rees46Service.TrackView(obj,TypeEvent.purchase));
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return new EmptyResult();
        }

        public ActionResult GetProductForEvent(int OfferId, int? ProductId, int? Amount, string type, string Recommend_by)
        {
            var types = TypeEvent.none;
            Enum.TryParse(type, out types);
            var recomends = Recomender.none;
            return Json(ParceForEvent(OfferId, ProductId, Amount, types, Enum.TryParse(Recommend_by, out recomends) ? Recommend_by : "none"));

        }

        private TrackEvents ParceForEvent(int OfferId, int? ProductId, int? Amount, TypeEvent type, string Recommend_by)
        {
            var product = ProductId != null ? ProductService.GetProduct(ProductId.Value) : null;
            var offer = OfferService.GetOffer(OfferId);
            var categoryId = offer != null || product != null
                ? ProductService.GetCategoriesIDsByProductId(product != null ? product.ProductId : offer.Product.ProductId)
                : null;

            switch (type)
            {
                case TypeEvent.cart:
                    {
                        var obj = new TrackEvents()
                        {
                            Amount = Amount,
                            Id = offer != null ? offer.OfferId : product != null ? product.ProductId : OfferId,
                            Stock = true,
                            Name = product != null ? product.Name : null,
                            Price = offer != null ? offer.BasePrice.ToString().Replace(",", ".") : null,
                            Categories = categoryId != null ? categoryId.ToArray() : null,
                            Image = offer != null && offer.Photo != null
                                ? offer.Photo.ImageSrcMiddle()
                                : product != null ? product.Photo : null,
                            Url = product != null ? 
                                HttpUtility.HtmlEncode(SettingsMain.SiteUrl.TrimEnd('/') + "/" + UrlService.GetLink(ParamType.Product, product.UrlPath, product.ProductId)) 
                                : null,
                            RecommendedBy = Recommend_by,
                            Type = TypeEvent.cart
                        };
                        return obj;
                    }
                case TypeEvent.remove_from_cart:
                    {
                        var obj = new TrackEvents()
                        {
                            Id = offer != null ? offer.OfferId : product != null ? product.ProductId : OfferId,
                            Type = TypeEvent.remove_from_cart
                        };
                        return obj;
                    }
                case TypeEvent.view:
                    {
                        var obj = new TrackEvents()
                        {
                            Id = offer != null ? offer.OfferId : product != null ? product.ProductId : OfferId,
                            Stock = true,
                            Price = offer.BasePrice.ToString().Replace(",", "."),
                            Name = product != null ? product.Name : null,
                            Categories = categoryId != null ? categoryId.ToArray() : null,
                            Image = offer != null && offer.Photo != null
                                ? offer.Photo.ImageSrcMiddle()
                                : product != null ? product.Photo : null,
                            Url = product != null ? HttpUtility.HtmlEncode(SettingsMain.SiteUrl.TrimEnd('/') + "/" + UrlService.GetLink(ParamType.Product, product.UrlPath, product.ProductId)) : null,
                            Type = TypeEvent.view
                        };
                        return obj;
                    }
            }
            return null;
        }

        public ActionResult ProductRight(Product product, Offer offer)
        {
            if (product == null || offer == null)
                return new EmptyResult();
            var obj = ParceForEvent(offer.OfferId, product.ProductId, null, TypeEvent.view, "none");
            return Content(Rees46Service.TrackView(obj,TypeEvent.view));
        }

        public ActionResult CategoryTop(Category category)
        {
            if (category == null)
                return new EmptyResult();

            var recommender = Recomender.none;
            Enum.TryParse(Rees46Settings.CatalogTopBlock, true, out recommender);

            var html = Rees46Service.GetRecomender(recommender, categoryId: category.CategoryId, title:Rees46Settings.CatalogTopBlockTitle);
            return Content(html);
        }

        public ActionResult CategoryBottom(Category category)
        {
            if (category == null)
                return new EmptyResult();

            var recommender = Recomender.none;
            Enum.TryParse(Rees46Settings.CatalogBottomBlock, true, out recommender);

            var html = Rees46Service.GetRecomender(recommender, categoryId: category.CategoryId, title: Rees46Settings.CatalogBottomBlockTitle);
            return Content(html);
        }

        public ActionResult MainPage()
        {
            var recommender = Recomender.none;
            Enum.TryParse(Rees46Settings.MainPageBlock, true, out recommender);

            var html = Rees46Service.GetRecomender(recommender, title: Rees46Settings.MainPageBlockTitle, productCount: SettingsDesign.CountMainPageProductInLine);
            return Content(html);
        }
    }
}
