using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Taxes;

namespace AdvantShop.Payment
{
    public class PaymentManager
    {
        private readonly List<PaymentMethod> _listMethod;
        private readonly PreOrder _preOrder;
        private readonly BaseShippingOption _shipping;
        private readonly ShoppingCart _cart;

        public PaymentManager(PreOrder preOrder, ShoppingCart cart)
        {
            _listMethod = PaymentService.GetAllPaymentMethods(true);
            _preOrder = preOrder;
            _shipping = preOrder.ShippingOption;
            _cart = cart;
        }

        public List<BasePaymentOption> GetOptions(bool getAll = true)
        {
            var options = new List<BasePaymentOption>();

            if (_shipping == null)
                return options;
            var currentItems = _listMethod;

            if (!getAll && this._preOrder.PaymentOption != null)
            {
                currentItems = currentItems.Where(x => x.PaymentMethodId == _preOrder.PaymentOption.Id).ToList();
            }

            var productsPrice = _cart != null
                ? _cart.TotalPrice - _cart.TotalDiscount
                : _preOrder.Items.Sum(x => x.Amount * x.Price) - _preOrder.TotalDiscount;

            var displayCertificateMetod = SettingsCheckout.EnableGiftCertificateService &&
                                          _cart != null && _cart.Certificate != null && (productsPrice + _shipping.Rate <= 0);
            if (displayCertificateMetod)
            {
                var certificateMethod = currentItems.FirstOrDefault(x => x is PaymentGiftCertificate);
                if (certificateMethod == null)
                {
                    certificateMethod = new PaymentGiftCertificate
                    {
                        Enabled = true,
                        Name = LocalizationService.GetResource("Core.Payment.GiftCertificate.PaymentTitle"),
                        Description = LocalizationService.GetResource("Core.Payment.GiftCertificate.PaymentDescription"),
                        SortOrder = 0
                    };
                    PaymentService.AddPaymentMethod(certificateMethod);
                }
                options.Add(certificateMethod.GetOption(null, 0));
                return options;
            }

            var availableMethod = PaymentService.UseGeoMapping(currentItems, _preOrder.CountryDest, _preOrder.CityDest);
            var notAvailableByShipping = ShippingMethodService.NotAvailablePayments(_shipping.MethodId);

            availableMethod = notAvailableByShipping.Any()
                ? availableMethod.Where(x => !notAvailableByShipping.Contains(x.PaymentMethodId)).ToList()
                : availableMethod;

            if (!availableMethod.Any() && !getAll)
                return GetOptions();

            var bonusCost = 0F;

            if (_preOrder.BonusUseIt && BonusSystem.IsActive)
            {
                var bonusCard = BonusSystemService.GetCard(_preOrder.BonusCardId);
                if (bonusCard != null && !bonusCard.Blocked && bonusCard.BonusesTotalAmount > 0)
                {
                    bonusCost = BonusSystemService.GetBonusCost(bonusCard, _cart, _shipping.Rate, _preOrder.BonusUseIt).BonusPrice;
                }
            }
            
            var preCoast = productsPrice + _shipping.Rate - bonusCost;

            foreach (var item in availableMethod)
            {
                if (item is PaymentGiftCertificate) continue;

                if (item is ICreditPaymentMethod && ((ICreditPaymentMethod)item).MinimumPrice >= productsPrice) continue;

                if (ShippingMethodService.IsPaymentNotUsed(_shipping.MethodId, item.PaymentMethodId))
                    continue;

                options.Add(item.GetOption(_shipping, preCoast));
            }
            options = options.Where(x => x != null).ToList();

            return options;
        }
    }
}
