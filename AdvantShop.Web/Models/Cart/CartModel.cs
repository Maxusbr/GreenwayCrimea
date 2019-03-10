using System.Collections.Generic;

namespace AdvantShop.Models.Cart
{
    public class CartCoupon
    {
        public string Price { get; set; }
        public string Percent { get; set; }
        public string Code { get; set; }
        public bool NotApplied { get; set; }
    }

    public class CartModel
    {
        public List<CartItemModel> CartProducts { get; set; }
        public string ColorHeader { get; set; }
        public string SizeHeader { get; set; }
        public string Count { get; set; }
        public float TotalItems { get; set; }
        public string BonusPlus { get; set; }
        public string Valid { get; set; }
        public bool CouponInputVisible { get; set; }
        public bool ShowConfirmButtons { get; set; }
        public bool ShowBuyInOneClick { get; set; }
        public string BuyInOneClickText { get; set; }
        public string TotalProductPrice { get; set; }
        public string TotalPrice { get; set; }
        public string DiscountPrice { get; set; }
        public string Certificate { get; set; }
        public CartCoupon Coupon { get; set; }
        public bool MobileIsFullCheckout { get; set; }
    }
}