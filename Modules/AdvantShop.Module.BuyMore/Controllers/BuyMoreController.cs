using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;
using AdvantShop.Module.BuyMore.Domain;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Localization;
using System;
using AdvantShop.Shipping;

namespace AdvantShop.Module.BuyMore.Controllers
{
    public class BuyMoreController : ModuleController
    {
        public ActionResult MiniCartMessage()
        {
            var list = BuyMoreService.GetAll();
            var cart = ShoppingCartService.CurrentShoppingCart;
            var totalPrice = BuyMoreService.GetTotalPrice(cart);

            var selectedShippingMethodMass = ModuleSettingsProvider.GetSettingValue<string>("ExcludedShippingsIds", AdvantShop.Module.BuyMore.BuyMore.ModuleStringId);
            var selectedShippingMethod = selectedShippingMethodMass != null ? selectedShippingMethodMass.Split(",") : new string[0];
            var shippingsMethod = ShippingMethodService.GetAllShippingMethods(true).Where(x => !selectedShippingMethod.Contains(x.ShippingMethodId.ToString())).ToList();
            var availableShippingsMethod = BuyMoreService.GetShippingMethodsByGeoMapping(shippingsMethod);

            if (availableShippingsMethod.Count == 0)
                return Content(string.Empty);

            var currency = CurrencyService.CurrentCurrency;
            var discounts = OrderService.GetOrderPricesDiscounts();

            BuyMoreProductModel currentAction = null;
            BuyMoreProductModel nextAction = list.Count > 0 ? list[0] : null;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].OrderPriceFrom <= totalPrice)
                {
                    currentAction = list[i];
                    nextAction = i + 1 < list.Count ? list[i + 1] : null;
                }
            }

            var sb = new StringBuilder();
            
            var discount = GetDiscount(totalPrice, currency, discounts);
            sb.Append(GetCurrentAction(currentAction, discount, totalPrice));

            var discountNext = GetNextDiscount(totalPrice, currency, discounts);

            if (nextAction != null || discountNext != null)
            {
                sb.Append(GetNextAction(nextAction, discountNext, totalPrice));
            }

            return Content(sb.ToString());
        }

        public ActionResult FullCartMessage()
        {

            var list = BuyMoreService.GetAll();
            var cart = ShoppingCartService.CurrentShoppingCart;
            var totalPrice = BuyMoreService.GetTotalPrice(cart);
            var totalItems = BuyMoreService.GetTotalItems(cart);

            var selectedShippingMethodMass = ModuleSettingsProvider.GetSettingValue<string>("ExcludedShippingsIds", AdvantShop.Module.BuyMore.BuyMore.ModuleStringId);
            var selectedShippingMethod = selectedShippingMethodMass != null ? selectedShippingMethodMass.Split(",") : new string[0];
            var shippingsMethod = ShippingMethodService.GetAllShippingMethods(true).Where(x => !selectedShippingMethod.Contains(x.ShippingMethodId.ToString())).ToList();
            var availableShippingsMethod = BuyMoreService.GetShippingMethodsByGeoMapping(shippingsMethod);
            
            if (totalItems == 0 || availableShippingsMethod.Count == 0)
                return Content(string.Empty);

            var currency = CurrencyService.CurrentCurrency;
            var discounts = OrderService.GetOrderPricesDiscounts();

            BuyMoreProductModel currentAction = null;
            BuyMoreProductModel nextAction = list.Count > 0 ? list[0] : null;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].OrderPriceFrom <= totalPrice)
                {
                    currentAction = list[i];
                    nextAction = i + 1 < list.Count ? list[i + 1] : null;
                }
            }

            var sb = new StringBuilder();

            var discount = GetDiscount(totalPrice, currency, discounts);
            sb.Append(GetCurrentAction(currentAction, discount, totalPrice));

            var discountNext = GetNextDiscount(totalPrice, currency, discounts);
            if (nextAction != null || discountNext != null)
            {
                sb.Append(GetNextAction(nextAction, discountNext, totalPrice));
            }

            return Content(sb.ToString());
        }

        private string GetCurrentAction(BuyMoreProductModel currentAction, OrderPriceDiscount discount, float totalPrice)
        {
            if (currentAction == null && (discount == null || discount.PercentDiscount == 0))
                return string.Empty;

            float currentRange = 0;
            if (currentAction == null && discount != null)
                currentRange = discount.PriceRange;

            if (currentAction != null && discount == null)
                currentRange = currentAction.OrderPriceFrom;

            if (currentAction != null && discount != null)
                currentRange = Math.Max(currentAction.OrderPriceFrom, discount.PriceRange);


            var sb = new StringBuilder();
            sb.Append("<div>");
            sb.AppendFormat(
                "{1} <div class=\"module-buy-more-current\">{0}</div>, {2} ", PriceFormatService.FormatPrice(currentRange, false),
                LocalizationService.GetResource("BuyMore.CurrentAction.AmountIsMore"), LocalizationService.GetResource("BuyMore.CurrentAction.YouReceive"));

            var options = new List<string>();

            if (currentAction != null && currentAction.FreeShipping)
                options.Add(string.Format("<span class=\"module-buy-more-select\">{0}</span>", LocalizationService.GetResource("BuyMore.CurrentAction.FreeShipping")));


            if (discount != null)
                options.Add(string.Format("{1} <span class=\"module-buy-more-select\">{0}%</span>",
                    discount.PercentDiscount, LocalizationService.GetResource("BuyMore.CurrentAction.Discount")));


            if (currentAction != null)
            {
                foreach (var offerId in currentAction.GiftOffersIdsList)
                {
                    var offer = OfferService.GetOffer(offerId);
                    if (offer != null)
                    {
                        options.Add(string.Format("{1} - <span class=\"module-buy-more-select\">{0}</span>",
                            offer.Product.Name, LocalizationService.GetResource("BuyMore.CurrentAction.Gift")));
                    }
                }
            }

            sb.Append(options.AggregateString(", "));
            sb.Append("</div>");

            return sb.ToString();
        }

        private string GetNextAction(BuyMoreProductModel nextAction, OrderPriceDiscount discountNext, float totalPrice)
        {
            var sb = new StringBuilder();

            if (nextAction == null && discountNext == null)
                return string.Empty;

            float nextRange = 0;

            if (nextAction == null && discountNext != null)
                nextRange = discountNext.PriceRange;

            if (nextAction != null && discountNext == null)
                nextRange = nextAction.OrderPriceFrom;

            if (nextAction != null && discountNext != null)
                nextRange = Math.Min(nextAction.OrderPriceFrom, discountNext.PriceRange);

            float missingDiscount = ModuleSettingsProvider.GetSettingValue<float>("MissingDiscount", BuyMore.ModuleStringId);

            if (!ModuleSettingsProvider.GetSettingValue<bool>("DisplayAlways", BuyMore.ModuleStringId) && totalPrice < nextRange / 100 * (100 - missingDiscount))
                return string.Empty;

            sb.Append("<div>");

            sb.AppendFormat("{1} <div class=\"module-buy-more-need\">{0}</div>, {2} ",
                PriceFormatService.FormatPrice(nextRange - totalPrice, true), LocalizationService.GetResource("BuyMore.NextAction.NotHaveEnough"),
                LocalizationService.GetResource("BuyMore.NextAction.ToReceive"));

            var options = new List<string>();

            if (nextAction != null && nextAction.FreeShipping && (discountNext == null || nextAction.OrderPriceFrom <= discountNext.PriceRange))
                options.Add(string.Format("<span class=\"module-buy-more-select\">{0}</span>", LocalizationService.GetResource("BuyMore.NextAction.FreeShipping")));

            if (discountNext != null && (nextAction == null || discountNext.PriceRange <= nextAction.OrderPriceFrom))
                options.Add(string.Format("{1} <span class=\"module-buy-more-select\">{0}%</span>", discountNext.PercentDiscount,
                    LocalizationService.GetResource("BuyMore.NextAction.Discount")));


            if (nextAction != null && (discountNext == null || nextAction.OrderPriceFrom <= discountNext.PriceRange))
            {
                foreach (var offerId in nextAction.GiftOffersIdsList)
                {
                    var offer = OfferService.GetOffer(offerId);
                    if (offer != null)
                    {
                        options.Add(string.Format("подарок - <span class=\"module-buy-more-select\">{0}</span>", offer.Product.Name));
                    }
                }
            }

            sb.Append(options.AggregateString(", "));
            sb.Append("</div>");

            return sb.ToString();
        }


        private OrderPriceDiscount GetDiscount(float price, Currency currency, List<OrderPriceDiscount> discounts)
        {
            return discounts == null || discounts.Count == 0 ||
                   CustomerContext.CurrentCustomer.CustomerGroup.CustomerGroupId !=
                   CustomerGroupService.DefaultCustomerGroup
                ? null
                : discounts.Where(dr => dr.PriceRange <= price && dr.PriceRange != 0)
                    .OrderBy(dr => dr.PriceRange)
                    .LastOrDefault();
        }

        private OrderPriceDiscount GetNextDiscount(float price, Currency currency, List<OrderPriceDiscount> discounts)
        {
            return discounts == null || discounts.Count == 0 ||
                   CustomerContext.CurrentCustomer.CustomerGroup.CustomerGroupId !=
                   CustomerGroupService.DefaultCustomerGroup
                ? null
                : discounts.Where(dr => dr.PriceRange > price && dr.PriceRange != 0)
                    .OrderBy(dr => dr.PriceRange)
                    .FirstOrDefault();
        }
    }
}
