//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Orders
{
    public class ShoppingCart : List<ShoppingCartItem>
    {
        public ShoppingCart() { }

        public ShoppingCart(Guid customerId)
        {
            _customer = CustomerService.GetCustomer(customerId) ?? new Customer(CustomerGroupService.DefaultCustomerGroup) { Id = customerId, CustomerRole = Role.Guest };
        }

        private Customer _customer;
        public Customer Customer
        {
            get { return _customer ?? (_customer = CustomerContext.CurrentCustomer); }
        }

        private float _totalPrice = float.MinValue;
        public float TotalPrice
        {
            get
            {
                return _totalPrice != float.MinValue
                    ? _totalPrice
                    : (_totalPrice = this.Sum(x => x.PriceWithDiscount * x.Amount));
            }
        }

        /// <summary>
        /// сумма товаров, скидка на которые задается модулем
        /// </summary>
        private float? _totalPriceIgnoreDiscount;
        public float TotalPriceIgnoreDiscount
        {
            get
            {
                return _totalPriceIgnoreDiscount.HasValue
                    ? _totalPriceIgnoreDiscount.Value
                    : (_totalPriceIgnoreDiscount = this.Where(x => x.ModuleKey.IsNotEmpty() && x.Discount.HasValue).ToList().Sum(x => x.PriceWithDiscount * x.Amount)).Value;
            }
        }

        public float TotalShippingWeight
        {
            get { return this.Sum(x => x.Offer.Product.Weight * x.Amount); }
        }

        /// <summary>
        /// количество товаров, скидка на которые задается модулем
        /// </summary>
        public float TotalItems
        {
            get { return this.Sum(x => x.Amount); }
        }

        public float TotalItemsIgnoreDiscount
        {
            get { return this.Where(x => x.ModuleKey.IsNotEmpty() && x.Discount.HasValue).ToList().Sum(x => x.Amount); }
        }

        public bool HasItems
        {
            get { return this.Count > 0; }
        }

        public bool CanOrder
        {
            get
            {
                if (TotalPrice < CustomerGroupService.GetMinimumOrderPrice() || !HasItems)
                    return false;
                return
                    this.All(
                        item =>
                        item.Offer.Product.Enabled 
                        &&
                         (item.Amount <= item.Offer.Amount || !SettingsCheckout.AmountLimitation || item.IsGift
                             || (item.AddedByRequest && !SettingsCheckout.DenyToByPreorderedProductsWithZerroAmount)
                             || (item.Offer.Amount <= 0 && SettingsCheckout.OutOfStockAction == eOutOfStockAction.Cart))
                         &&
                         (!item.Offer.Product.MinAmount.HasValue || item.Amount >= item.Offer.Product.MinAmount) 
                         &&
                         (!item.Offer.Product.MaxAmount.HasValue || item.Amount <= item.Offer.Product.MaxAmount));
            }
        }

        private int _customerGroupId;

        public int CustomerGroupId
        {
            get
            {
                if (_customerGroupId == 0)
                {
                    _customerGroupId = Customer != null ? Customer.CustomerGroupId : CustomerGroupService.DefaultCustomerGroup;
                }

                return _customerGroupId;
            }
        }

        public float DiscountPercentOnTotalPrice
        {
            get
            {
                if (Coupon == null && CustomerGroupId == CustomerGroupService.DefaultCustomerGroup)
                {
                    var discountModule = AttachedModules.GetModules<IShoppingCartDiscount>().FirstOrDefault();
                    if (discountModule != null)
                    {
                        return ((IShoppingCartDiscount) Activator.CreateInstance(discountModule, null)).GetDiscount(this);
                    }

                    return OrderService.GetDiscount(TotalPrice - TotalPriceIgnoreDiscount);
                }

                return 0;
            }
        }

        public float TotalDiscount
        {
            get
            {
                if (CustomerGroupId != CustomerGroupService.DefaultCustomerGroup)
                    return 0;

                float discount = 0;
                float discountPercentOnTotalPrice = DiscountPercentOnTotalPrice;

                discount += discountPercentOnTotalPrice > 0 ? discountPercentOnTotalPrice * (TotalPrice - TotalPriceIgnoreDiscount) / 100 : 0;
                discount += Certificate != null ? Certificate.Sum : 0;

                if (Coupon != null && CouponCanBeApplied)
                {
                    switch (Coupon.Type)
                    {
                        case CouponType.Fixed:
                            var productsPrice = this.Where(x => x.IsCouponApplied).Sum(x => x.PriceWithDiscount * x.Amount);
                            var couponPrice = Coupon.GetRate();
                            discount += productsPrice >= couponPrice ? couponPrice : productsPrice;
                            break;
                        case CouponType.Percent:
                            discount += this.Where(x => x.IsCouponApplied).Sum(x => Coupon.Value * x.PriceWithDiscount / 100 * x.Amount);
                            break;
                    }
                }
                return discount.RoundPrice(CurrencyService.CurrentCurrency.Rate);
            }
        }

        private GiftCertificate _certificate;
        private bool _certificateLoaded;

        public GiftCertificate Certificate
        {
            get
            {
                if (_certificateLoaded)
                    return _certificate;

                if (_certificate == null)
                    _certificate = GiftCertificateService.GetCustomerCertificate(Customer.Id);

                if (_certificate != null && _coupon != null)
                    throw new Exception("Coupon and Certificate can't be used together");

                if (_certificate != null && (!_certificate.Paid || _certificate.Used || !_certificate.Enable))
                {
                    GiftCertificateService.DeleteCustomerCertificate(_certificate.CertificateId, Customer.Id);
                }

                _certificateLoaded = true;

                return _certificate;
            }
        }

        private Coupon _coupon;
        private bool _couponLoaded;

        public Coupon Coupon
        {
            get
            {
                if (_couponLoaded)
                    return _coupon;

                if (_coupon == null)
                    _coupon = CouponService.GetCustomerCoupon(Customer.Id);

                if (_coupon != null && _certificate != null)
                    throw new Exception("Coupon and Certificate can't be used together");

                if (_coupon != null &&
                    ((_coupon.ExpirationDate != null && _coupon.ExpirationDate < DateTime.Now) ||
                     (_coupon.PossibleUses != 0 && _coupon.PossibleUses <= _coupon.ActualUses) || !_coupon.Enabled))
                {
                    CouponService.DeleteCustomerCoupon(_coupon.CouponID, Customer.Id);
                }

                _couponLoaded = true;

                return _coupon;
            }
        }

        public bool CouponCanBeApplied
        {
            get
            {
                return this.Where(x => x.IsCouponApplied).Sum(x => x.PriceWithDiscount*x.Amount) >= Coupon.MinimalOrderPrice;
            }
        }

        public override int GetHashCode()
        {
            return this.Aggregate(0, (curr, val) => val.GetHashCode()) +
                   (Certificate != null ? Certificate.GetHashCode() : 0) + (Coupon != null ? Coupon.GetHashCode() : 0);
        }
    }
}