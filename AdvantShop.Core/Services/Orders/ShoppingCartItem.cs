//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using AdvantShop.Catalog;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;
using AdvantShop.Repository.Currencies;
using Newtonsoft.Json;

namespace AdvantShop.Orders
{
    public class ShoppingCartItem : ICloneable
    {
        [JsonIgnore]
        public int ShoppingCartItemId { get; set; }

        [JsonIgnore]
        public ShoppingCartType ShoppingCartType { get; set; }

        [JsonIgnore]
        public Guid CustomerId { get; set; }

        [JsonIgnore]
        public string AttributesXml { get; set; }

        private bool _loadAmount;
        private float _amount;
        public float Amount
        {
            get
            {
                if (!_loadAmount)
                {
                    var multiplicity = (Offer.Product.Multiplicity > 0 ? Offer.Product.Multiplicity : 1f);
                    var minAmount = Offer.Product.MinAmount.HasValue ? Offer.Product.MinAmount.Value : multiplicity;

                    _amount = (float)(Math.Ceiling((decimal)_amount / (decimal)multiplicity) * (decimal)multiplicity);
                    if (_amount < minAmount)
                        _amount = minAmount;

                    _loadAmount = true;
                }
                return _amount;
            }
            set
            {
                _amount = value;
                _loadAmount = false;
            }
        }

        [JsonIgnore]
        public DateTime CreatedOn { get; set; }

        [JsonIgnore]
        public DateTime UpdatedOn { get; set; }

        [JsonIgnore]
        public bool AddedByRequest { get; set; }

        public int OfferId { get; set; }


        public bool IsGift { get; set; }

        public string ModuleKey { get; set; }

        public bool FrozenAmount
        {
            get { return AddedByRequest || IsGift; }
        }

        private Coupon _coupon;
        private bool? _isCouponApplied;

        [JsonIgnore]
        public bool IsCouponApplied
        {
            get
            {
                if (_isCouponApplied.HasValue)
                    return _isCouponApplied.Value;

                if (_coupon == null)
                    _coupon = CouponService.GetCustomerCoupon(CustomerId);

                //_isCouponApplied = _coupon != null && CouponService.IsCouponAppliedToProduct(_coupon.CouponID, Offer.ProductId)
                //    && ((_coupon.Type == CouponType.Percent && _coupon.Value > ProductDiscount.Percent) || _coupon.Type == CouponType.Fixed);

                if (_coupon != null && CouponService.IsCouponAppliedToProduct(_coupon.CouponID, Offer.ProductId))
                {
                    if (_coupon.Type == CouponType.Percent)
                    {
                        if ((ProductsDiscount.Type == DiscountType.Percent && _coupon.Value > ProductsDiscount.Percent) ||
                            (ProductsDiscount.Type == DiscountType.Amount && Price * _coupon.Value / 100 > ProductsDiscount.Amount))
                        {
                            return (_isCouponApplied = true).Value;
                        }
                    }
                    else if (_coupon.Type == CouponType.Fixed)
                    {
                        if ((ProductsDiscount.Type == DiscountType.Percent && _coupon.Value > Price * ProductsDiscount.Percent / 100) ||
                            (ProductsDiscount.Type == DiscountType.Amount && _coupon.Value > ProductsDiscount.Amount))
                        {
                            return (_isCouponApplied = true).Value;
                        }
                    }
                }

                return (_isCouponApplied = false).Value;
            }
        }

        private Offer _offer;
        [JsonIgnore]
        public Offer Offer
        {
            get
            {
                return _offer ?? (_offer = OfferService.GetOffer(OfferId));
            }
        }

        public override int GetHashCode()
        {
            return OfferId ^ Amount.GetHashCode() ^ IsGift.GetHashCode() ^ (AttributesXml ?? "").GetHashCode();
        }

        private CustomerGroup _customerGroup;
        private CustomerGroup CustomerGroup
        {
            get
            {
                return _customerGroup ?? (_customerGroup = CustomerContext.CurrentCustomer.CustomerGroup);
            }
        }

        private float? _customOptionsPrice;

        private float CustomOptionPrice
        {
            get
            {
                return _customOptionsPrice ??
                       (_customOptionsPrice =
                           CustomOptionsService.GetCustomOptionPrice(Offer.RoundedPrice, AttributesXml, Offer.Product.Currency.Rate)).Value;
            }
        }

        private float? _price;
        public float Price
        {
            get
            {
                return _price ?? (_price = PriceService.RoundPrice(Offer.RoundedPrice + CustomOptionPrice, null, CurrencyService.CurrentCurrency.Rate)).Value;
            }
        }


        private float? _priceWithDiscount;
        [JsonIgnore]
        public float PriceWithDiscount
        {
            get
            {
                if (IsGift) return 0;

                if (_priceWithDiscount.HasValue)
                    return _priceWithDiscount.Value;

                if (IsCouponApplied)
                    return Price;

                var discount = ShoppingCartService.GetShoppingCartItemDiscount(ShoppingCartItemId);
                if (discount != null)
                {
                    return (_priceWithDiscount = PriceService.GetFinalPrice(Price, discount)).Value;
                }

                _priceWithDiscount = PriceService.GetFinalPrice(Price, ProductsDiscount);

                return _priceWithDiscount.Value;
            }
        }

        private Discount _productsDiscount;
        private Discount ProductsDiscount
        {
            get
            {
                if (_productsDiscount != null)
                    return _productsDiscount;
                
                return _productsDiscount = PriceService.GetFinalDiscount(Price, Offer.Product.Discount, Offer.Product.Currency.Rate, CustomerGroup, Offer.ProductId);
            }
        }

        private Discount _discount;
        [JsonIgnore]
        public Discount Discount
        {
            get
            {
                if (_discount != null)
                    return _discount;

                var discount = ShoppingCartService.GetShoppingCartItemDiscount(ShoppingCartItemId);
                if (discount != null)
                    return _discount = discount;

                if (IsCouponApplied)
                    return _discount = new Discount();
                
                return _discount = new Discount(ProductsDiscount.Percent, ProductsDiscount.Amount, ProductsDiscount.Type);
            }
        }

        private Customer _customer;
        public Customer Customer
        {
            get { return _customer ?? (_customer = CustomerService.GetCustomer(CustomerId) ?? new Customer(CustomerGroupService.DefaultCustomerGroup) { Id = CustomerId, CustomerRole = Role.Guest }); }
        }

        public ShoppingCartItem()
        {
            ShoppingCartType = ShoppingCartType.ShoppingCart;
            AttributesXml = "";
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}