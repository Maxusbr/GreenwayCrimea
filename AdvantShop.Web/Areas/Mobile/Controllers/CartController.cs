using System.Web.Mvc;
using System;
using AdvantShop.Orders;
using AdvantShop.Customers;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Catalog;
using System.Linq;

namespace AdvantShop.Areas.Mobile.Controllers
{
    public class CartController : BaseMobileController
    {
        public ActionResult Index(string products)
        {
            if (!String.IsNullOrWhiteSpace(products))
            {
                foreach (var item in products.Split(';'))
                {
                    int offerId;
                    var newItem = new ShoppingCartItem() { CustomerId = CustomerContext.CustomerId };

                    var parts = item.Split('-');
                    if (parts.Length > 0 && (offerId = parts[0].TryParseInt(0)) != 0 && OfferService.GetOffer(offerId) != null)
                    {
                        newItem.OfferId = offerId;
                    }
                    else
                    {
                        continue;
                    }

                    newItem.Amount = parts.Length > 1 ? parts[1].TryParseFloat() : 1;

                    var currentItem = ShoppingCartService.CurrentShoppingCart.FirstOrDefault(shpCartitem => shpCartitem.OfferId == newItem.OfferId);
                    if (currentItem != null)
                    {
                        currentItem.Amount = newItem.Amount;
                        ShoppingCartService.UpdateShoppingCartItem(currentItem);
                    }
                    else
                    {
                        ShoppingCartService.AddShoppingCartItem(newItem);
                    }
                }
            }

            SetTitle(T("Mobile.Cart.Index.Title"));
            SetMetaInformation(T("Mobile.Cart.Index.Title"));

            return View();
        }
    }
}