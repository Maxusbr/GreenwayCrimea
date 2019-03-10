using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Bonuses.Service;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.SEO;
using AdvantShop.ViewModel.Checkout;
using AdvantShop.Helpers;

namespace AdvantShop.Handlers.Checkout
{
    public class CheckoutSuccessHandler
    {
        public CheckoutSuccess Get(Order order)
        {
            var model = new CheckoutSuccess { Order = order };

            var sb = StaticBlockService.GetPagePartByKeyWithCache("OrderSuccessTop");
            var sbM = StaticBlockService.GetPagePartByKeyWithCache("MobileOrderSuccessTop");
            var mob = MobileHelper.IsMobileEnabled();
            if ((!mob && sb == null || sbM == null && mob) || (!CustomerContext.CurrentCustomer.IsAdmin && (!mob && !sb.Enabled || !sbM.Enabled && mob)))
            {
                model.OrderSuccessTopText = string.Empty;
            }
            else
            {
                model.OrderSuccessTopText = mob ? sbM.Content ?? string.Empty : sb.Content ?? string.Empty;
                model.OrderSuccessTopText = model.OrderSuccessTopText.Replace("#ORDER_ID#", order.Number);
            }

            if (SettingsCheckout.SuccessOrderScript.IsNotEmpty())
            {
                var orderScript = SettingsCheckout.SuccessOrderScript
                                                              .Replace("#ORDER_ID#", order.OrderID.ToString())
                                                              .Replace("#ORDER_SUM#", order.Sum.ToString("0.##"))
                                                              .Replace("#SHIPPING_SUM#", order.ShippingCost.ToString("0.##"))
                                                              .Replace("#ORDER_SUM_RAW#", order.Sum.ToInvariantString())
                                                              .Replace("#ORDER_CURRENCY_CODE#", order.OrderCurrency.CurrencyCode)
                                                              .Replace("#CUSTOMER_EMAIL#", HttpUtility.HtmlEncode(order.OrderCustomer.Email))
                                                              .Replace("#CUSTOMER_FIRSTNAME#", HttpUtility.HtmlEncode(order.OrderCustomer.FirstName))
                                                              .Replace("#CUSTOMER_LASTNAME#", HttpUtility.HtmlEncode(order.OrderCustomer.LastName))
                                                              .Replace("#CUSTOMER_PHONE#", HttpUtility.HtmlEncode(order.OrderCustomer.Phone))
                                                              .Replace("#CUSTOMER_ID#", order.OrderCustomer.CustomerID.ToString());

                var regex = new Regex("<<(.*)>>");
                var match = regex.Match(orderScript);
                var products = new StringBuilder();

                if (match.Groups.Count > 0 && match.Groups[1].Value.IsNotEmpty())
                {
                    var productLine = match.Groups[1].Value;
                    foreach (var item in order.OrderItems)
                    {
                        products.Append(
                            productLine.Replace("#PRODUCT_ARTNO#", HttpUtility.HtmlEncode(item.ArtNo))
                                       .Replace("#PRODUCT_NAME#", HttpUtility.HtmlEncode(item.Name))
                                       .Replace("#PRODUCT_PRICE#", item.Price.ToString("0.##"))
                                       .Replace("#PRODUCT_AMOUNT#", item.Amount.ToString("0.##")));
                    }

                    orderScript = orderScript.Replace("<<" + productLine + ">>", products.ToString());
                }
                model.SuccessScript = orderScript;
            }

            model.GoogleAnalyticsString = GoogleAnalyticsString.GetForOrder(order);

            if (order.BonusCardNumber != null)
            {
                float newBonusAmount = 0;

                var purchase = PurchaseService.GetByOrderId(order.OrderID);
                if (purchase != null)
                    newBonusAmount = (float) purchase.NewBonusAmount;

                var context = HttpContext.Current;
                if (context != null && context.Session["BonusesForNewCard"] != null)
                { 
                    newBonusAmount += Convert.ToSingle(context.Session["BonusesForNewCard"]);
                    context.Session.Remove("BonusesForNewCard");
                }

                model.NewBonusAmount = newBonusAmount.FormatPriceInvariant();
            }

            return model;
        }
    }
}