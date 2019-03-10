using AdvantShop.Core.Services.Catalog;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Orders;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Core.Services.Bonuses;

namespace AdvantShop.Controllers
{
    public class CouponController : BaseClientController
    {
        public JsonResult CouponJson()
        {
            var totalDiscount = ShoppingCartService.CurrentShoppingCart.TotalDiscount;
            var summary = new List<object>();
            if (ShoppingCartService.CurrentShoppingCart.Certificate != null)
            {
                summary.Add(
                    new
                    {
                        Key = T("Checkout.CheckoutCart.Certificate"),
                        Value = string.Format("-{0}<a class=\"cross\" data-cart-remove-cert=\"true\" title=\"{1}\"></a>",
                                  ShoppingCartService.CurrentShoppingCart.Certificate.Sum.FormatPrice(), 
                                  T("Checkout.CheckoutCart.DeleteCertificate"))
                    });
            }
            if (ShoppingCartService.CurrentShoppingCart.Coupon == null) return Json(summary);
            if (totalDiscount == 0)
            {
                summary.Add(
                    new
                    {
                        Key = T("Checkout.CheckoutCart.Coupon"),
                        Value = string.Format("-{0} ({1}) <img src='images/question_mark.png' title='{3}'> <a class=\"cross\"  data-cart-remove-cupon=\"true\" title=\"{2}\"></a>",
                            0F.FormatPrice(), ShoppingCartService.CurrentShoppingCart.Coupon.Code,
                            T("Checkout.CheckoutCart.DeleteCoupon"),
                            T("Checkout.CheckoutCart.CouponNotApplied"))
                    });
            }
            else
            {
                switch (ShoppingCartService.CurrentShoppingCart.Coupon.Type)
                {
                    case CouponType.Fixed:
                        summary.Add(new
                        {
                            Key = T("Checkout.CheckoutCart.Coupon"),
                            Value = string.Format("-{0} ({1}) <a class=\"cross\" data-cart-remove-cupon=\"true\" title=\"{2}\"></a>",
                                totalDiscount.FormatPrice(), ShoppingCartService.CurrentShoppingCart.Coupon.Code,
                                T("Checkout.CheckoutCart.DeleteCoupon"))
                        });
                        break;
                    case CouponType.Percent:
                        summary.Add(new
                        {
                            Key = T("Checkout.CheckoutCart.Coupon"),
                            Value = string.Format("-{0} ({1}%) ({2}) <a class=\"cross\"  data-cart-remove-cupon=\"true\" title=\"{3}\"></a>",
                                totalDiscount.FormatPrice(),
                                ShoppingCartService.CurrentShoppingCart.Coupon.Value.FormatPrice(),
                                ShoppingCartService.CurrentShoppingCart.Coupon.Code, T("Checkout.CheckoutCart.DeleteCoupon"))
                        });
                        break;
                }
            }
            return Json(summary);
        }

        [HttpPost]
        public JsonResult CouponPost(string code)
        {
            var customerGroup = CustomerContext.CurrentCustomer.CustomerGroup;
            if (customerGroup.CustomerGroupId != CustomerGroupService.DefaultCustomerGroup)
            {
                return Json(new {result = false});
            }
            //can't aplly coupon if bonus used
            var current = MyCheckout.Factory(CustomerContext.CustomerId);
            if (current!=null && current.Data.Bonus.UseIt && BonusSystem.ForbidOnCoupon)
            {
                return Json(new { result = false, msg = "Нельзя применить купон при использовании бонусов" });

            }

            var cert = GiftCertificateService.GetCertificateByCode(code);
            var coupon = CouponService.GetCouponByCode(code);
            if (SettingsCheckout.EnableGiftCertificateService && cert != null && cert.Paid && !cert.Used && cert.Enable)
            {
                GiftCertificateService.AddCustomerCertificate(cert.CertificateId);
                return Json(new {result = true});
            }

            if (coupon != null && (coupon.ExpirationDate == null || coupon.ExpirationDate > DateTime.Now) &&
                (coupon.PossibleUses == 0 || coupon.PossibleUses > coupon.ActualUses) && coupon.Enabled)
            {
                CouponService.AddCustomerCoupon(coupon.CouponID);

                var cart = ShoppingCartService.CurrentShoppingCart;

                var msg = cart.Coupon == null || !cart.CouponCanBeApplied
                    ? LocalizationService.GetResource("Checkout.CheckoutCart.CouponNotApplied")
                    : null;

                return Json(new { result = true, msg = msg });
            }

            return Json(new { result = false });
        }

        [HttpPost]
        public JsonResult DeleteCoupon()
        {
            var coupon = CouponService.GetCustomerCoupon();
            if (coupon != null)
                CouponService.DeleteCustomerCoupon(coupon.CouponID);
            
            return Json(true);
        }

        [HttpPost]
        public JsonResult DeleteCertificate()
        {
            var cer = GiftCertificateService.GetCustomerCertificate();
            if (cer != null)
                GiftCertificateService.DeleteCustomerCertificate(cer.CertificateId);
            
            return Json(true);
        }
    }
}