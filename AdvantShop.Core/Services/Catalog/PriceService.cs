using System;
using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Customers;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Core.Services.Catalog
{
    public class PriceService
    {
        public static float GetFinalPrice(float price)
        {
            return RoundPrice(price, null, CurrencyService.CurrentCurrency.Rate);
        }

        public static float GetFinalPrice(float price, Discount discount)
        {
            return GetFinalPrice(price, discount, CurrencyService.CurrentCurrency.Rate, null);
        }

        public static float GetFinalPrice(float price, Discount discount, float baseCurrencyValue, Currency renderCurrency = null)
        {
            var discountPrice = discount.Type == DiscountType.Percent ? price*discount.Percent/100 : discount.Amount;

            return (RoundPrice(price, renderCurrency, baseCurrencyValue) - 
                        RoundPrice(discountPrice, renderCurrency, baseCurrencyValue)).RoundPrice(baseCurrencyValue, renderCurrency);
        }


        public static float GetFinalPrice(Offer offer, Customer customer, string selectedOptions = null)
        {
            var customOptionsPrice =
                selectedOptions != null
                    ? CustomOptionsService.GetCustomOptionPrice(offer.RoundedPrice, selectedOptions, offer.Product.Currency.Rate)
                    : 0;
            return GetFinalPrice(offer, customer.CustomerGroup, customOptionsPrice);
        }

        public static float GetFinalPrice(Offer offer, CustomerGroup customerGroup, float customOptionsPrice = 0)
        {
            var price = offer.RoundedPrice + customOptionsPrice;

            var discount = GetFinalDiscount(price, offer.Product.Discount.Percent, offer.Product.Discount.Amount, offer.Product.Currency.Rate, 
                                            customerGroup, offer.ProductId);

            return GetFinalPrice(price, discount);
        }


        public static float GetFinalDiscountPercentage(float productDiscountPercent, CustomerGroup group, float discountByTime, int productId, List<ProductDiscount> productDiscounts)
        {
            var finalDiscount = productDiscountPercent > discountByTime ? productDiscountPercent : discountByTime;

            if (group != null)
            {
                finalDiscount = finalDiscount > group.GroupDiscount ? finalDiscount : group.GroupDiscount;
            }

            if (productId != 0 && productDiscounts != null)
            {
                var prodDiscount = productDiscounts.Find(x => x.ProductId == productId);
                if (prodDiscount != null)
                {
                    finalDiscount = finalDiscount > prodDiscount.Discount ? finalDiscount : prodDiscount.Discount;
                }
            }

            return finalDiscount;
        }

        public static Discount GetFinalDiscount(float price, Discount discount, float currencyValue, 
                                                CustomerGroup customerGroup = null, int productId = 0, float discountByTime = -1,
                                                List<ProductDiscount> productDiscounts = null)
        {
            return GetFinalDiscount(price, discount.Percent, discount.Amount, currencyValue, customerGroup, productId, discountByTime, productDiscounts);
        }

        public static Discount GetFinalDiscount(float price, float discountPercent, float discountAmount, float currencyValue, 
                                                CustomerGroup customerGroup = null, int productId = 0, float discountByTime = -1, 
                                                List<ProductDiscount> productDiscounts = null)
        {
            discountAmount = RoundPrice(discountAmount, null, currencyValue);

            if (customerGroup == null)
                customerGroup = CustomerContext.CurrentCustomer.CustomerGroup;

            if (discountByTime == -1)
                discountByTime = DiscountByTimeService.GetDiscountByTime();

            if (productDiscounts == null)
                productDiscounts = productId != 0 ? ProductService.GetDiscountList() : null;

            var percent = GetFinalDiscountPercentage(discountPercent, customerGroup, discountByTime, productId, productDiscounts);

            var type = price * percent / 100 > discountAmount ? DiscountType.Percent : DiscountType.Amount;

            return new Discount(percent, discountAmount, type);
        }



        public static float GetBonusPrice(float bonusPercent, float productPrice)
        {
            return productPrice * bonusPercent / 100;
        }
        

        public static float RoundPrice(float value, Currency renderingCurrency = null, float baseCurrencyValue = 1)
        {
            var currency = renderingCurrency ?? CurrencyService.CurrentCurrency;

            if (!currency.EnablePriceRounding)
                return (float)Math.Round(value / currency.Rate * baseCurrencyValue, 4, MidpointRounding.AwayFromZero);

            return (float)(Math.Round(value / currency.RoundNumbers / currency.Rate * baseCurrencyValue, 0, MidpointRounding.AwayFromZero) * Math.Round(currency.RoundNumbers, 2));
        }
    }
}
