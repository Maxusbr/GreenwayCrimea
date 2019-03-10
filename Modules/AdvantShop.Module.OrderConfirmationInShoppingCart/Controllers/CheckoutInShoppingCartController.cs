using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Module.CheckoutInShoppingCart.Models;
using AdvantShop.Orders;
using AdvantShop.Repository;
using AdvantShop.Repository.Currencies;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Shipping;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Bonuses.Model;

namespace AdvantShop.Module.OrderConfirmationInShoppingCart.Controllers
{
    public class OrderConfirmationInShoppingCartController : ModuleController
    {
        public ActionResult Checkout()
        {
            if (ShoppingCartService.CurrentShoppingCart.TotalItems == 0)
                return new EmptyResult();

            var model = new CheckoutInShoppingCartModel()
            {
                DisplayName = SettingsCheckout.IsShowBuyInOneClickName,
                DisplayEmail = SettingsCheckout.IsShowBuyInOneClickEmail,
                DisplayComment = SettingsCheckout.IsShowBuyInOneClickComment,
                DisplayPhone = SettingsCheckout.IsShowBuyInOneClickPhone,

                IsRequiredName = SettingsCheckout.IsRequiredBuyInOneClickName,
                IsRequiredEmail = SettingsCheckout.IsRequiredBuyInOneClickEmail,
                IsRequiredPhone = SettingsCheckout.IsRequiredBuyInOneClickPhone,
                IsRequiredComment = SettingsCheckout.IsRequiredBuyInOneClickComment,
                IsShowUserAgreementText = SettingsCheckout.IsShowUserAgreementText,
                UserAgreementText = SettingsCheckout.UserAgreementText,

                LabelName = SettingsCheckout.BuyInOneClickName,
                LabelPhone = SettingsCheckout.BuyInOneClickPhone,
                LabelEmail = SettingsCheckout.BuyInOneClickEmail,
                LabelComment = SettingsCheckout.BuyInOneClickComment,

                FirstText =
                    ModuleSettingsProvider.GetSettingValue<string>("FirstText", OrderConfirmationInShoppingCart.ModuleID),
                FinalText =
                    ModuleSettingsProvider.GetSettingValue<string>("FinalText", OrderConfirmationInShoppingCart.ModuleID)
            };

            var customer = CustomerContext.CurrentCustomer;
            if (customer.RegistredUser)
            {
                model.Name = customer.LastName + " " + customer.FirstName;
                model.Email = customer.EMail;
                model.Phone = customer.Phone;
            }

            return PartialView("~/Modules/OrderConfirmationInShoppingCart/Views/Checkout.cshtml", model);
        }

        [HttpPost]
        public ActionResult MakeOrder(CheckoutInShoppingCartModel model)
        {
            if (!ShoppingCartService.CurrentShoppingCart.CanOrder)
                return RedirectToRoute("Cart");

            model.Name = HttpUtility.HtmlEncode(model.Name);
            model.Email = HttpUtility.HtmlEncode(model.Email);
            model.Phone = HttpUtility.HtmlEncode(model.Phone);
            model.Comment = HttpUtility.HtmlEncode(model.Comment);

            if (!IsValid(model))
                return RedirectToRoute("Cart");

            var order = CreateOrder(model);
            if (order == null)
                return RedirectToRoute("Cart");

            TempData["orderid"] = order.OrderID.ToString();

            return RedirectToRoute("CheckoutSuccess", new { code = order.Code.ToString() });
        }
                

        private Order CreateOrder(CheckoutInShoppingCartModel model)
        {
            OrderCertificate orderCertificate = null;
            OrderCoupon orderCoupon = null;

            var shoppingCart = ShoppingCartService.CurrentShoppingCart;
            var discountPercentOnTotalPrice = shoppingCart.DiscountPercentOnTotalPrice;
            var totalPrice = shoppingCart.TotalPrice;

            var minimumOrderPrice = CustomerGroupService.GetMinimumOrderPrice();

            if (totalPrice < minimumOrderPrice)
            {
                model.Error =
                    string.Format("Минимальная сумма заказа: {0}. Вам необходимо приобрести еще товаров на сумму: {1}",
                        minimumOrderPrice.RoundAndFormatPrice(CurrencyService.CurrentCurrency),
                        (minimumOrderPrice - totalPrice).RoundAndFormatPrice(CurrencyService.CurrentCurrency));
                return null;
            }

            ProcessUser(model);

            var orderItems = new List<OrderItem>();
            orderItems.AddRange(shoppingCart.Select(item => (OrderItem)item));

            var certificate = shoppingCart.Certificate;
            var coupon = shoppingCart.Coupon;

            if (certificate != null)
            {
                orderCertificate = new OrderCertificate
                {
                    Code = certificate.CertificateCode,
                    Price = certificate.Sum
                };
            }
            if (coupon != null && shoppingCart.TotalPrice >= coupon.MinimalOrderPrice)
            {
                orderCoupon = new OrderCoupon
                {
                    Code = coupon.Code,
                    Type = coupon.Type,
                    Value = coupon.Value
                };
            }

            var customerId = CustomerContext.CurrentCustomer.Id;
            var customer = CustomerService.GetCustomer(customerId);
            if (customer == null && !string.IsNullOrEmpty(model.Email))
            {
                customer = CustomerService.GetCustomerByEmail(model.Email);
            }
            
            var orderCustomer = new OrderCustomer();

            if (customer != null)
            {
                orderCustomer = (OrderCustomer)customer;

                if (!string.IsNullOrEmpty(model.Phone))
                {
                    orderCustomer.Phone = model.Phone;
                    orderCustomer.StandardPhone = StringHelper.ConvertToStandardPhone(model.Phone);
                }

                if (!string.IsNullOrEmpty(model.Name))
                {
                    orderCustomer.FirstName = model.Name;
                    orderCustomer.LastName = null;
                    orderCustomer.Patronymic = null;
                }
            }
            else
            {
                orderCustomer.CustomerID = CustomerContext.CurrentCustomer.Id;
                orderCustomer.Email = !string.IsNullOrEmpty(model.Email)
                            ? model.Email
                            : CustomerContext.CurrentCustomer.EMail;
                orderCustomer.FirstName = !string.IsNullOrEmpty(model.Name) ? model.Name : string.Empty;
                orderCustomer.LastName = string.Empty;
                orderCustomer.Phone = !string.IsNullOrEmpty(model.Phone) ? model.Phone : string.Empty;
                orderCustomer.StandardPhone = StringHelper.ConvertToStandardPhone(model.Phone);

                var zone = IpZoneContext.CurrentZone;

                orderCustomer.Country = zone.CountryName;
                orderCustomer.Region = zone.Region;
                orderCustomer.City = zone.City;
            }

            orderCustomer.CustomerIP = Request.UserHostAddress;

            var customerGroup = customer != null ? customer.CustomerGroup : CustomerContext.CurrentCustomer.CustomerGroup;

            var order = new Order
            {
                CustomerComment = model.Comment,
                OrderDate = DateTime.Now,
                ArchivedPaymentName = string.Empty,
                OrderCurrency = CurrencyService.CurrentCurrency,
                OrderCustomer = orderCustomer,
                OrderStatusId = OrderStatusService.DefaultOrderStatus,
                AffiliateID = 0,
                GroupName = customerGroup.GroupName,
                GroupDiscount = customerGroup.GroupDiscount,
                OrderItems = orderItems,
                OrderDiscount = discountPercentOnTotalPrice,
                Certificate = orderCertificate,
                Coupon = orderCoupon,
                AdminOrderComment = "Оформление заказа из корзины",
                OrderSourceId = OrderSourceService.GetOrderSource(OrderType.ShoppingCart).Id
            };


            var shipping = ShippingMethodService.GetShippingMethod(SettingsCheckout.BuyInOneClickDefaultShippingMethod);

            if (shipping != null)
            {

                var currentZone = IpZoneContext.CurrentZone;
                var preOrder = new PreOrder()
                {
                    CountryDest = currentZone.CountryName,
                    CityDest = currentZone.City,
                    RegionDest = currentZone.Region,
                    Items = order.OrderItems.Select(shpItem => new PreOrderItem(shpItem)).ToList(),
                    Currency = CurrencyService.CurrentCurrency
                };

                var shippingManager = new ShippingManager(preOrder, true);
                var shippingRate = shippingManager.GetOptions().FirstOrDefault(sh => sh.MethodId == shipping.ShippingMethodId);
                if (shippingRate != null)
                {
                    order.ShippingMethodId = shipping.ShippingMethodId;
                    order.ShippingCost = shippingRate.Rate;
                    order.ArchivedShippingName = shipping.Name;
                    order.ShippingTaxType = shipping.TaxType;
                }
            }


            var payment = Payment.PaymentService.GetPaymentMethod(SettingsCheckout.BuyInOneClickDefaultPaymentMethod);
            if (payment != null)
            {
                order.PaymentMethodId = payment.PaymentMethodId;
                order.ArchivedPaymentName = payment.Name;
                order.PaymentCost = payment.ExtrachargeType == Payment.ExtrachargeType.Fixed ? payment.Extracharge : totalPrice / 100 * payment.Extracharge;

            }

            var changedBy = new OrderChangedBy(CustomerContext.CurrentCustomer);

            Card bonusCard = null;
            if (BonusSystem.IsActive && customer != null)
            {
                bonusCard = BonusSystemService.GetCard(customer.Id);
                if (bonusCard != null)
                    order.BonusCardNumber = customer.BonusCardNumber;
            }

            order.OrderID = OrderService.AddOrder(order, changedBy);

            OrderStatusService.ChangeOrderStatus(order.OrderID, OrderStatusService.DefaultOrderStatus, "Оформление заказа");

            if (BonusSystem.IsActive && bonusCard != null)
            {
                var o = OrderService.GetOrder(order.OrderID);
                
                BonusSystemService.MakeBonusPurchase(bonusCard.CardNumber, o);
            }
            
            if (order.OrderID != 0)
            {
                var orderTable = OrderService.GenerateOrderItemsHtml(order.OrderItems,
                    CurrencyService.CurrentCurrency, totalPrice,
                    discountPercentOnTotalPrice, 0, orderCoupon,
                    orderCertificate,
                    order.TotalDiscount,
                    0, 0, 0, 0, 0);

                var mailTemplate = new BuyInOneClickMailTemplate(order.Number, model.Name, model.Phone,
                    model.Comment, orderTable, OrderService.GetBillingLinkHash(order), model.Email);
                mailTemplate.BuildMail();

                SendMail.SendMailNow(Guid.Empty, SettingsMail.EmailForOrders, mailTemplate.Subject, mailTemplate.Body, true);

                if (!string.IsNullOrEmpty(model.Email))
                    SendMail.SendMailNow(CustomerContext.CustomerId, model.Email, mailTemplate.Subject, mailTemplate.Body, true);

                if (certificate != null)
                {
                    certificate.ApplyOrderNumber = order.Number;
                    certificate.Used = true;
                    certificate.Enable = true;

                    GiftCertificateService.DeleteCustomerCertificate(certificate.CertificateId);
                    GiftCertificateService.UpdateCertificateById(certificate);
                }

                if (coupon != null && shoppingCart.TotalPrice >= coupon.MinimalOrderPrice)
                {
                    coupon.ActualUses += 1;
                    CouponService.UpdateCoupon(coupon);
                    CouponService.DeleteCustomerCoupon(coupon.CouponID);
                }

                ShoppingCartService.ClearShoppingCart(ShoppingCartType.ShoppingCart, CustomerContext.CustomerId);

                return order;
            }

            return null;
        }

        public void ProcessUser(CheckoutInShoppingCartModel model)
        {
            var customer = CustomerContext.CurrentCustomer;

            if (customer.RegistredUser || string.IsNullOrWhiteSpace(model.Email))
                return;

            var customerByEmail = CustomerService.GetCustomerByEmail(model.Email);
            if (customerByEmail != null && customerByEmail.Id != Guid.Empty)
                return;

            var password = StringHelper.GeneratePassword(8);

            try
            {
                var id = CustomerService.InsertNewCustomer(new Customer(CustomerGroupService.DefaultCustomerGroup)
                {
                    Id = CustomerContext.CustomerId,
                    Password = password,
                    FirstName = model.Name ?? "",
                    LastName = "",
                    Phone = model.Phone ?? string.Empty,
                    StandardPhone = StringHelper.ConvertToStandardPhone(model.Phone),
                    SubscribedForNews = false,
                    EMail = model.Email,
                    CustomerRole = Role.User
                });

                if (id == Guid.Empty)
                    return;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private bool IsValid(CheckoutInShoppingCartModel model)
        {
            var valid = true;

            if (!(valid &= SettingsCheckout.IsShowUserAgreementText == model.Agreement))
            {
                model.Error = AdvantShop.Core.Services.Localization.LocalizationService.GetResource("Js.Subscribe.ErrorAgreement");
            }

            if (SettingsCheckout.IsShowBuyInOneClickName &&
                SettingsCheckout.IsRequiredBuyInOneClickName)
            {
                valid &= !string.IsNullOrEmpty(model.Name);
            }

            if (SettingsCheckout.IsShowBuyInOneClickEmail &&
                SettingsCheckout.IsRequiredBuyInOneClickEmail)
            {
                valid &= !string.IsNullOrEmpty(model.Email);
            }

            if (SettingsCheckout.IsShowBuyInOneClickPhone &&
                SettingsCheckout.IsRequiredBuyInOneClickPhone)
            {
                valid &= !string.IsNullOrEmpty(model.Phone);
            }

            if (SettingsCheckout.IsShowBuyInOneClickComment &&
                SettingsCheckout.IsRequiredBuyInOneClickComment)
            {
                valid &= !string.IsNullOrEmpty(model.Comment);
            }

            if (!valid)
            {
                model.Error = "";
            }

            return valid;
        }
    }
}
