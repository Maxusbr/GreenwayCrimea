using System;
using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Taxes;

namespace AdvantShop.Web.Admin.Handlers.Orders
{
    public class AddOrderItems
    {
        private readonly Order _order;
        private readonly List<int> _offerIds;

        public AddOrderItems(Order order, List<int> offerIds)
        {
            _order = order;
            _offerIds = offerIds;
        }

        public bool Execute()
        {
            var saveChanges = false;
            var couponCode = _order.Coupon != null ? _order.Coupon.Code : null;

            foreach (var offerId in _offerIds)
            {
                var offer = OfferService.GetOffer(offerId);
                if (offer == null)
                    continue;

                var product = offer.Product;

                var prodMinAmount = product.MinAmount == null
                            ? product.Multiplicity
                            : product.Multiplicity > product.MinAmount
                                ? product.Multiplicity
                                : product.MinAmount.Value;

                var item = new OrderItem
                {
                    ProductID = product.ProductId,
                    Name = product.Name,
                    ArtNo = offer.ArtNo,
                    Price = PriceService.GetFinalPrice(offer, new CustomerGroup() {GroupDiscount = _order.GroupDiscount}), //offer.RoundedPrice * (100 - Math.Max(_order.GroupDiscount, Math.Max(product.Discount.Percent, product.CalculableDiscount()))) / 100 - product.Discount.Amount,
                    Amount = prodMinAmount,
                    SupplyPrice = offer.SupplyPrice,
                    SelectedOptions = new List<EvaluatedCustomOptions>(),
                    Weight = product.Weight,
                    IsCouponApplied =
                        couponCode.IsNotEmpty() && CouponService.GetCouponByCode(couponCode) != null &&
                        CouponService.IsCouponAppliedToProduct(CouponService.GetCouponByCode(couponCode).CouponID, product.ProductId),
                    Color = offer.Color != null ? offer.Color.ColorName : null,
                    Size = offer.Size != null ? offer.Size.SizeName : null,
                    PhotoID = offer.Photo != null ? offer.Photo.PhotoId : default(int),
                    AccrueBonuses = offer.Product.AccrueBonuses,
                    Width = offer.Product.Width,
                    Length = offer.Product.Length,
                    Height = offer.Product.Height
                };

                var tax = product.TaxId != null ? TaxService.GetTax(product.TaxId.Value) : null;
                if (tax != null)
                {
                    item.TaxId = tax.TaxId;
                    item.TaxName = tax.Name;
                    item.TaxType = tax.TaxType;
                    item.TaxRate = tax.Rate;
                    item.TaxShowInPrice = tax.ShowInPrice;
                }

                var oItem = _order.OrderItems.Find(x => x == item);
                if (oItem != null)
                {
                    oItem.Amount += 1;
                    saveChanges = true;
                }
                else
                {
                    _order.OrderItems.Add(item);
                    saveChanges = true;
                }
            }

            return saveChanges;
        }
    }
}
