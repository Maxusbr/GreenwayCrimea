using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Module.BuyInTime.Domain;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Module.BuyInTime.Controllers
{
    public class BuyInTimeController : ModuleController
    {
        private const string ModuleName = "BuyInTime";
        private const string CountdownScript = "<div data-countdown data-end-time=\"::'{0}'\"></div>";

        private string GetActionText(BuyInTimeProductModel action, bool isMobile = false)
        {
            if (action.IsRepeat && action.DateExpired < DateTime.Now)
            {
                action.DateExpired = BuyInTimeService.GetExpireDateTime(action.DateExpired, action.DaysRepeat);
            }

            var actionTitle = ModuleSettingsProvider.GetSettingValue<string>("BuyInTimeActionTitle", ModuleName);
            var countdown = string.Format(CountdownScript, action.DateExpired.ToString("u"));

            var actionText =
                (!isMobile ? action.ActionText : action.MobileActionText)
                    .Replace("#ActionTitle#", actionTitle)
                    .Replace("#Countdown#", countdown);

            var product = ProductService.GetProduct(action.ProductId);
            if (product == null)
                return string.Empty;

            var offer = product.Offers.FirstOrDefault(o => o.Main) ?? product.Offers.FirstOrDefault();
            if (offer == null)
                return string.Empty;

			var currency = CurrencyService.CurrentCurrency;
			actionText = actionText.Replace("#ProductPictureSrc#", action.Picture.IsNotEmpty()
					? string.Format("pictures/modules/{0}/{1}", ModuleName.ToLower(), action.Picture)
					: string.Empty);

            var oldPrice = PriceService.GetFinalPrice(offer.RoundedPrice, new Discount(), currency.Rate, currency);
            var newPrice = PriceService.GetFinalPrice(offer.RoundedPrice, new Discount(action.DiscountInTime, 0), currency.Rate, currency);

            actionText = actionText.Replace("#ProductLink#", Url.RouteUrl("Product", new { url = product.UrlPath }));
			actionText = actionText.Replace("#ProductName#", product.Name);
			actionText = actionText.Replace("#OldPrice#", oldPrice.ToString("##,##0.####"));
			actionText = actionText.Replace("#NewPrice#", newPrice.ToString("##,##0.####"));
			actionText = actionText.Replace("#DiscountPrice#",(oldPrice - newPrice).ToString("##,##0.####"));
			actionText = actionText.Replace("#DiscountPercent#", action.DiscountInTime.ToString("0.####"));
			actionText = actionText.Replace("#CurrencyCode#", currency.Symbol);

            return actionText;
        }

        public ActionResult MainPageBlock()
        {
			var action = BuyInTimeService.GetByShowMode((int)BuyInTimeService.eShowMode.Horizontal, DateTime.Now);
			return Content(action == null ? string.Empty : GetActionText(action));
			/* При смене валюты в шапке, валюта в модуле не меняется из-за кеша.
			 return Content(
                CacheManager.Get(BuyInTimeService.CacheKey + BuyInTimeService.eShowMode.Horizontal,
                    () =>
                    {
                        var action = BuyInTimeService.GetByShowMode((int)BuyInTimeService.eShowMode.Horizontal, DateTime.Now);
                        return action == null ? string.Empty : GetActionText(action);
                    }));*/
        }

        public ActionResult MainPageAfterCarouselBlock()
        {
			var action = BuyInTimeService.GetByShowMode((int)BuyInTimeService.eShowMode.Vertical, DateTime.Now);
			return Content(action == null ? string.Empty : GetActionText(action));
            /*return Content(
                CacheManager.Get(BuyInTimeService.CacheKey + BuyInTimeService.eShowMode.Vertical,
                    () =>
                    {
                        var action = BuyInTimeService.GetByShowMode((int)BuyInTimeService.eShowMode.Vertical, DateTime.Now);
                        return action == null ? string.Empty : GetActionText(action);
                    }));*/
        }

        public ActionResult MobileAfterCarousel()
        {
			var action = BuyInTimeService.GetInMobile(DateTime.Now);
			return Content(action == null ? string.Empty : GetActionText(action, true));
            /*return Content(
                CacheManager.Get(BuyInTimeService.CacheKey + "Mobile",
                    () =>
                    {
                        var action = BuyInTimeService.GetInMobile(DateTime.Now);
                        return action == null ? string.Empty : GetActionText(action, true);
                    }));*/
        }

        public ActionResult ProductInformation(Product product, Offer offer)
        {
            var discountModel = BuyInTimeService.GetByProduct(product.ProductId, DateTime.Now);
            if (discountModel == null)
                return new EmptyResult();

            var actionTitle = ModuleSettingsProvider.GetSettingValue<string>("BuyInTimeActionTitle", ModuleName);
            var countdown = discountModel.DateExpired != null
                                ? string.Format(CountdownScript, ((DateTime)discountModel.DateExpired).ToString("u"))
                                : string.Empty;

            return
                Content(
                    string.Format(
                        "<div class=\"details-buy-in-time-block\"> " +
	                        "<div class=\"buy-in-time-block\"> " +
		                        "<div class=\"buy-in-time-content\"> " +
			                        "<div class=\"buy-in-time-text\">{0}</div> {1}" +
		                        "</div> " +
	                        "</div> " +
                        "</div>",
                        actionTitle, countdown));
        }
    }
}
