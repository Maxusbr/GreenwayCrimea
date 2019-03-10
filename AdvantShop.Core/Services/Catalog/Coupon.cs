//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Catalog
{
    public enum CouponType
    {
        [Localize("Core.Coupon.CouponType.Fixed")]
        Fixed = 1,

        [Localize("Core.Coupon.CouponType.Percent")]
        Percent = 2
    }

    public class Coupon
    {
        public int CouponID { get; set; }
        public string Code { get; set; }
        public CouponType Type { get; set; }
        public float Value { get; set; }
        public string CurrencyIso3 { get; set; }
        public DateTime AddingDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int PossibleUses { get; set; }
        public int ActualUses { get; set; }
        public bool Enabled { get; set; }
        public float MinimalOrderPrice { get; set; }

        private IEnumerable<int> _categoryIds;
        public IEnumerable<int> CategoryIds
        {
            get { return _categoryIds ?? (_categoryIds = CouponService.GetCategoriesIDsByCoupon(CouponID)); }
        }

        private IEnumerable<int> _productsIds;
        public IEnumerable<int> ProductsIds
        {
            get { return _productsIds ?? (_productsIds = CouponService.GetProductsIDsByCoupon(CouponID)); }
        }

        private Currency _currency;
        public Currency Currency
        {
            get { return _currency ?? (_currency = CurrencyService.Currency(CurrencyIso3)); }
        }

        public float GetRate()
        {
            return Type == CouponType.Percent
                    ? Value
                    : Value * Currency.Rate / CurrencyService.CurrentCurrency.Rate;
        }

        public override int GetHashCode()
        {
            return CouponID.GetHashCode() ^ Code.GetHashCode() ^ PossibleUses.GetHashCode() ^ ActualUses.GetHashCode() ^
                   MinimalOrderPrice.GetHashCode() ^ ExpirationDate.GetHashCode() ^ Type.GetHashCode() ^
                   Value.GetHashCode();
        }
    }
}