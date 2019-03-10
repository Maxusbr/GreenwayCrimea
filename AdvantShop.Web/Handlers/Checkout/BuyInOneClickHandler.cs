using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Models.Checkout;
using AdvantShop.Orders;
using AdvantShop.Repository;
using AdvantShop.Repository.Currencies;
using AdvantShop.Saas;
using AdvantShop.Security;
using AdvantShop.Shipping;
using AdvantShop.Taxes;

namespace AdvantShop.Handlers.Checkout
{
    public class BuyInOneClickHandler
    {
        private readonly BuyOneInClickJsonModel _model;
        
        public BuyInOneClickHandler(BuyOneInClickJsonModel jsonModel)
        {
            _model = jsonModel;
        }

        public BuyOneClickResult Create()
        {
            var amount = _model.Amount;

            if (!IsValidModel())
            {
                return new BuyOneClickResult() { error = LocalizationService.GetResource("Checkout.BuyInOneClick.WrongData") };
            }

            var orderItems = new List<OrderItem>();
            float discountPercentOnTotalPrice = 0;
            float totalPrice = 0;
            float minimumOrderPrice = CustomerGroupService.GetMinimumOrderPrice();

            OrderCertificate orderCertificate = null;
            OrderCoupon orderCoupon = null;

            var customer = CustomerContext.CurrentCustomer;

            if (_model.Page == BuyInOneclickPage.Product || _model.Page == BuyInOneclickPage.LandingPage || _model.Page == BuyInOneclickPage.PreOrder)
            {
                Offer offer;

                if (_model.OfferId == null || _model.OfferId == 0)
                {
                    var p = ProductService.GetProduct(Convert.ToInt32(_model.ProductId));
                    if (p == null || p.Offers.Count == 0)
                        return new BuyOneClickResult() { error = LocalizationService.GetResource("Checkout.BuyInOneClick.Error") };

                    offer = p.Offers.First();
                }
                else
                {
                    offer = OfferService.GetOffer((int)_model.OfferId);
                }

                List<EvaluatedCustomOptions> listOptions = null;
                var selectedOptions = HttpUtility.UrlDecode(_model.AttributesXml);
                if (selectedOptions.IsNotEmpty())
                    listOptions = CustomOptionsService.DeserializeFromXml(selectedOptions, offer.Product.Currency.Rate);


                totalPrice = PriceService.GetFinalPrice(offer, customer, selectedOptions); //(offer.RoundedPrice - (offer.RoundedPrice * offer.Product.Discount / 100)) * amount;
                discountPercentOnTotalPrice = OrderService.GetDiscount(totalPrice);

                if (_model.Page != BuyInOneclickPage.LandingPage && _model.Page != BuyInOneclickPage.PreOrder)
                {
                    if (totalPrice < minimumOrderPrice)
                    {
                        return new BuyOneClickResult()
                        {
                            error =
                                string.Format(LocalizationService.GetResource("Cart.Error.MinimalOrderPrice"),
                                    PriceFormatService.FormatPrice(minimumOrderPrice),
                                    PriceFormatService.FormatPrice(minimumOrderPrice - totalPrice))
                        };
                    }

                    var errorAvailable = GetAvalible(offer, amount);
                    if (!string.IsNullOrEmpty(errorAvailable))
                    {
                        return new BuyOneClickResult() { error = errorAvailable };
                    }
                }


                var orderItem = new OrderItem
                {
                    ProductID = offer.ProductId,
                    Name = offer.Product.Name,
                    ArtNo = offer.ArtNo,
                    Price = totalPrice, // PriceService.GetFinalPrice(offer.RoundedPrice + customOptionsPrice, discount),
                    Amount = amount,
                    SupplyPrice = offer.SupplyPrice,
                    SelectedOptions = listOptions,
                    Weight = offer.Product.Weight,
                    Color = offer.Color != null ? offer.Color.ColorName : string.Empty,
                    Size = offer.Size != null ? offer.Size.SizeName : string.Empty,
                    PhotoID = offer.Photo != null ? offer.Photo.PhotoId : (int?) null,
                    Width = offer.Product.Width,
                    Length = offer.Product.Length,
                    Height = offer.Product.Height
                };

                var tax = offer.Product.TaxId != null ? TaxService.GetTax(offer.Product.TaxId.Value) : null;
                if (tax != null)
                {
                    orderItem.TaxId = tax.TaxId;
                    orderItem.TaxName = tax.Name;
                    orderItem.TaxRate = tax.Rate;
                    orderItem.TaxShowInPrice = tax.ShowInPrice;
                    orderItem.TaxType = tax.TaxType;
                }

                orderItems = new List<OrderItem>() {orderItem};

                if (offer.Product.HasGifts())
                {
                    foreach (var gift in OfferService.GetProductGifts(offer.ProductId))
                    {
                        var item = new OrderItem
                        {
                            ProductID = gift.ProductId,
                            Name = gift.Product.Name,
                            ArtNo = gift.ArtNo,
                            Price = 0,
                            Amount = SettingsCheckout.MultiplyGiftsCount ? amount : 1,
                            Weight = gift.Product.Weight,
                            Color = gift.Color != null ? gift.Color.ColorName : string.Empty,
                            Size = gift.Size != null ? gift.Size.SizeName : string.Empty,
                            PhotoID = gift.Photo != null ? gift.Photo.PhotoId : (int?)null,
                            Width = gift.Product.Width,
                            Length = gift.Product.Length,
                            Height = gift.Product.Height
                        };

                        tax = offer.Product.TaxId != null ? TaxService.GetTax(gift.Product.TaxId.Value) : null;
                        if (tax != null)
                        {
                            item.TaxId = tax.TaxId;
                            item.TaxName = tax.Name;
                            item.TaxRate = tax.Rate;
                            item.TaxShowInPrice = tax.ShowInPrice;
                            item.TaxType = tax.TaxType;
                        }

                        orderItems.Add(item);
                    }
                }
            }
            else if (_model.Page == BuyInOneclickPage.Cart || _model.Page == BuyInOneclickPage.Checkout)
            {
                var shoppingCart = ShoppingCartService.CurrentShoppingCart;
                discountPercentOnTotalPrice = shoppingCart.DiscountPercentOnTotalPrice;
                totalPrice = shoppingCart.TotalPrice;

                if (totalPrice < minimumOrderPrice)
                {
                    return new BuyOneClickResult()
                    {
                        error = string.Format(LocalizationService.GetResource("Cart.Error.MinimalOrderPrice"),
                            PriceFormatService.FormatPrice(minimumOrderPrice),
                            PriceFormatService.FormatPrice(minimumOrderPrice - totalPrice))
                    };
                }

                foreach (var item in shoppingCart)
                {
                    var errorAvailable = GetAvalible(item.Offer, item.Amount);
                    if (!string.IsNullOrEmpty(errorAvailable))
                    {
                        return new BuyOneClickResult() { error = errorAvailable };
                    }
                }

                orderItems.AddRange(shoppingCart.Select(item => (OrderItem)item));

                var certificate = shoppingCart.Certificate;
                var coupon = shoppingCart.Coupon;

                if (certificate != null)
                {
                    orderCertificate = new OrderCertificate()
                    {
                        Code = certificate.CertificateCode,
                        Price = certificate.Sum
                    };
                }
                if (coupon != null && shoppingCart.TotalPrice >= coupon.MinimalOrderPrice)
                {
                    orderCoupon = new OrderCoupon()
                    {
                        Code = coupon.Code,
                        Type = coupon.Type,
                        Value = coupon.Value
                    };
                }
            }

            var currency = CurrencyService.CurrentCurrency;

            BuyOneClickResult result = null;

            var crmEnable = (!SaasDataService.IsSaasEnabled ||
                             (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveCrm));

            try
            {
                if (!customer.RegistredUser)
                {
                    var emailExist = false;
                    if (_model.Email.IsNotEmpty())
                    {
                        var email = HttpUtility.HtmlEncode(_model.Email);
                        var customerByEmail = CustomerService.GetCustomerByEmail(email);
                        if (customerByEmail != null && customerByEmail.Id != Guid.Empty)
                        {
                            customer.Id = customerByEmail.Id;
                            emailExist = true;
                        }
                    }

                    if (!emailExist)
                    {
                        var newcustomer = new Customer(CustomerGroupService.DefaultCustomerGroup)
                        {
                            Id = CustomerContext.CustomerId,
                            Password = StringHelper.GeneratePassword(8),
                            FirstName = _model.Name.IsNotEmpty() ? HttpUtility.HtmlEncode(_model.Name) : string.Empty,
                            LastName = string.Empty,
                            Phone = _model.Phone.IsNotEmpty() ? HttpUtility.HtmlEncode(_model.Phone) : string.Empty,
                            StandardPhone = StringHelper.ConvertToStandardPhone(_model.Phone),
                            SubscribedForNews = false,
                            EMail = _model.Email.IsNotEmpty() ? HttpUtility.HtmlEncode(_model.Email) : customer.EMail,
                            CustomerRole = Role.User,
                        };

                        newcustomer.Id = CustomerService.InsertNewCustomer(newcustomer);
                        AuthorizeService.SignIn(newcustomer.EMail, newcustomer.Password, false, true);

                        customer = newcustomer;
                    }
                }

                if ((!SettingsCheckout.BuyInOneClickCreateOrder && crmEnable) || (_model.OrderType == OrderType.PreOrder && SettingsCheckout.OutOfStockAction == eOutOfStockAction.Lead))
                {
                    var orderSource = OrderSourceService.GetOrderSource(_model.OrderType);

                    var lead = CreateLead(customer, currency, orderItems, orderCertificate, orderCoupon,
                                            totalPrice, discountPercentOnTotalPrice, orderSource.Id);

                    result = new BuyOneClickResult()
                    {
                        orderId = lead.Id,
                        url = "."
                    };
                }
                else
                {
                    var order = CreateOrder(customer, currency, orderItems, orderCertificate, orderCoupon,
                                            orderItems.Sum(item => item.Price * item.Amount), discountPercentOnTotalPrice, _model.OrderType);

                    result = new BuyOneClickResult()
                    {
                        orderNumber = order.Number,
                        code = order.Code.ToString(),
                        url = UrlService.GetUrl("checkout/success/" + order.Code),
                        doGo = true,
                        orderId = order.OrderID
                    };

                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            if (result != null && (_model.Page == BuyInOneclickPage.Cart || _model.Page == BuyInOneclickPage.Checkout))
            {
                ShoppingCartService.ClearShoppingCart(ShoppingCartType.ShoppingCart, CustomerContext.CustomerId);
            }

            return result;
        }

        #region Help methods

        private bool IsValidModel()
        {
            var valid = true;
            valid &= _model.Page != BuyInOneclickPage.None;

            if (SettingsCheckout.IsShowBuyInOneClickName &&
                SettingsCheckout.IsRequiredBuyInOneClickName)
            {
                valid &= _model.Name.IsNotEmpty();
            }

            if (SettingsCheckout.IsShowBuyInOneClickEmail &&
                SettingsCheckout.IsRequiredBuyInOneClickEmail)
            {
                valid &= _model.Email.IsNotEmpty();
            }

            if (SettingsCheckout.IsShowBuyInOneClickPhone &&
                SettingsCheckout.IsRequiredBuyInOneClickPhone)
            {
                valid &= _model.Phone.IsNotEmpty();
            }

            return valid;
        }

        private string GetAvalible(Offer offer, float amount)
        {
            if (!offer.Product.Enabled || !offer.Product.CategoryEnabled)
                return LocalizationService.GetResource("Cart.Error.NotAvailable") + " 0 " + offer.Product.Unit;

            if ((SettingsCheckout.AmountLimitation) && (amount > offer.Amount))
                return LocalizationService.GetResource("Cart.Error.NotAvailable") + " " + offer.Amount + " " + offer.Product.Unit;

            if (amount > offer.Product.MaxAmount)
                return LocalizationService.GetResource("Cart.Error.MaximumOrder") + " " + offer.Product.MaxAmount + " " + offer.Product.Unit;

            if (amount < offer.Product.MinAmount)
                return LocalizationService.GetResource("Cart.Error.MinimumOrder") + " " + offer.Product.MinAmount + " " + offer.Product.Unit;

            return string.Empty;
        }

        private Order CreateOrder(Customer customer, Currency currency, List<OrderItem> orderItems,
                                    OrderCertificate orderCertificate, OrderCoupon orderCoupon, float totalPrice,
                                    float discountPercentOnTotalPrice, OrderType orderType)
        {
            var orderSource = OrderSourceService.GetOrderSource(_model.OrderType);

            var order = new Order
            {
                CustomerComment = HttpUtility.HtmlEncode(_model.Comment),
                OrderDate = DateTime.Now,
                OrderCurrency = currency,
                OrderCustomer = new OrderCustomer
                {
                    CustomerID = customer.Id,
                    Email = _model.Email.IsNotEmpty() ? HttpUtility.HtmlEncode(_model.Email) : customer.EMail,
                    FirstName = _model.Name.IsNotEmpty() ? HttpUtility.HtmlEncode(_model.Name) : string.Empty,
                    LastName = string.Empty,
                    Phone = _model.Phone.IsNotEmpty() ? HttpUtility.HtmlEncode(_model.Phone) : string.Empty,
                    StandardPhone = StringHelper.ConvertToStandardPhone(_model.Phone),
                    CustomerIP = HttpContext.Current.Request.UserHostAddress,

                    Country = IpZoneContext.CurrentZone.CountryName,
                    Region = IpZoneContext.CurrentZone.Region,
                    City = IpZoneContext.CurrentZone.City,
                },

                OrderStatusId = OrderStatusService.DefaultOrderStatus,
                AffiliateID = 0,
                GroupName = customer.CustomerGroup.GroupName,
                GroupDiscount = customer.CustomerGroup.GroupDiscount,
                OrderItems = orderItems,
                OrderDiscount = discountPercentOnTotalPrice,
                Certificate = orderCertificate,
                Coupon = orderCoupon,
                OrderSourceId = orderSource.Id
            };

            var shipping = ShippingMethodService.GetShippingMethod(SettingsCheckout.BuyInOneClickDefaultShippingMethod);

            if (shipping != null)
            {

                var currentZone = IpZoneContext.CurrentZone;
                var mainCountry = CountryService.GetCountry(SettingsMain.SellerCountryId);
                var preOrder = new PreOrder()
                {
                    CountryDest = currentZone.CountryName ?? (mainCountry != null ? mainCountry.Name : string.Empty),
                    RegionDest = currentZone.Region,
                    CityDest = currentZone.City ?? SettingsMain.City,
                    Items = order.OrderItems.Select(shpItem => new PreOrderItem(shpItem)).ToList(),
                    Currency = CurrencyService.CurrentCurrency
                };

                var shippingManager = new ShippingManager(preOrder, true);
                var shippingRate = shippingManager.GetOptions().FirstOrDefault(sh => sh.MethodId == shipping.ShippingMethodId);
                if (shippingRate != null)
                    order.ShippingCost = shippingRate.Rate;

                order.ShippingMethodId = shipping.ShippingMethodId;
                order.ArchivedShippingName = shipping.Name;
                order.ShippingTaxType = shipping.TaxType;
            }


            var payment = Payment.PaymentService.GetPaymentMethod(SettingsCheckout.BuyInOneClickDefaultPaymentMethod);
            if (payment != null)
            {
                order.PaymentMethodId = payment.PaymentMethodId;
                order.ArchivedPaymentName = payment.Name;
                order.PaymentCost = payment.ExtrachargeType == Payment.ExtrachargeType.Fixed ? payment.Extracharge : totalPrice / 100 * payment.Extracharge;

            }

            Card bonusCard = null;
            if (BonusSystem.IsActive)
            {
                bonusCard = BonusSystemService.GetCard(customer.Id);
                if (bonusCard != null && !bonusCard.Blocked)
                    order.BonusCardNumber = bonusCard.CardNumber;
            }

            var changedBy = new OrderChangedBy(CustomerContext.CurrentCustomer);

            order.OrderID = OrderService.AddOrder(order, changedBy);
            OrderStatusService.ChangeOrderStatus(order.OrderID, OrderStatusService.DefaultOrderStatus, LocalizationService.GetResource("Core.OrderStatus.Created"));

            if (BonusSystem.IsActive && bonusCard != null && !bonusCard.Blocked)
            {
                BonusSystemService.MakeBonusPurchase(bonusCard.CardNumber, OrderService.GetOrder(order.OrderID));
            }

            if (order.OrderID != 0)
            {
                var orderTable = OrderService.GenerateOrderItemsHtml(order.OrderItems, currency, totalPrice,
                    discountPercentOnTotalPrice, 0, orderCoupon, orderCertificate,
                    order.TotalDiscount, 0, 0, order.TaxCost, 0, 0);

                var mailTemplate = new BuyInOneClickMailTemplate(order.Number, order.OrderCustomer.FirstName,
                    order.OrderCustomer.Phone, order.CustomerComment, orderTable, OrderService.GetBillingLinkHash(order), order.OrderCustomer.Email);

                mailTemplate.BuildMail();

                if (_model.Email.IsNotEmpty())
                {
                    SendMail.SendMailNow(CustomerContext.CustomerId, _model.Email, mailTemplate.Subject, mailTemplate.Body, true);
                }

                SendMail.SendMailNow(Guid.Empty, SettingsMail.EmailForOrders, mailTemplate.Subject, mailTemplate.Body, true, _model.Email);

                if (orderCoupon != null)
                {
                    var coupon = CouponService.GetCouponByCode(orderCoupon.Code);
                    if (coupon != null)
                    {
                        coupon.ActualUses += 1;
                        CouponService.UpdateCoupon(coupon);
                        CouponService.DeleteCustomerCoupon(coupon.CouponID);
                    }
                }

                if (orderCertificate != null)
                {
                    var certificate = GiftCertificateService.GetCertificateByCode(orderCertificate.Code);
                    if (certificate != null)
                    {
                        certificate.ApplyOrderNumber = order.Number;
                        certificate.Used = true;
                        certificate.Enable = true;

                        GiftCertificateService.DeleteCustomerCertificate(certificate.CertificateId);
                        GiftCertificateService.UpdateCertificateById(certificate);
                    }
                }
            }

            return order;
        }

        private Lead CreateLead(Customer customer, Currency currency, List<OrderItem> orderItems,
                                OrderCertificate orderCertificate, OrderCoupon orderCoupon, float totalPrice,
                                float discountPercentOnTotalPrice, int orderSourceId)
        {
            var lead = new Lead()
            {
                Email = _model.Email.IsNotEmpty() ? HttpUtility.HtmlEncode(_model.Email) : customer.EMail,
                FirstName = _model.Name.IsNotEmpty() ? HttpUtility.HtmlEncode(_model.Name) : string.Empty,
                Phone = _model.Phone.IsNotEmpty() ? HttpUtility.HtmlEncode(_model.Phone) : string.Empty,

                CustomerId = customer.Id,
                LeadItems = new List<LeadItem>(),
                LeadCurrency = new LeadCurrency()
                {
                    CurrencyCode = currency.Iso3,
                    CurrencyNumCode = currency.NumIso3,
                    CurrencyValue = currency.Rate,
                    CurrencySymbol = currency.Symbol,
                    IsCodeBefore = currency.IsCodeBefore
                },
                OrderSourceId = orderSourceId,
                Comment = _model.Comment.IsNotEmpty() ? HttpUtility.HtmlEncode(_model.Comment).Replace("\n", "<br/>") : string.Empty
            };

            if (customer.RegistredUser)
                lead.Comment =
                    (string.IsNullOrEmpty(lead.Email) ? "" : "Email: " + lead.Email + " <br/>\n") +
                    (string.IsNullOrEmpty(lead.FirstName) ? "" : "Имя: " + lead.FirstName + " <br/>\n") +
                    (string.IsNullOrEmpty(lead.Phone) ? "" : "Телефон: " + lead.Phone + " <br/>\n") +
                    (string.IsNullOrEmpty(lead.Comment) ? "" : "Комментарий:<br/>\n" + lead.Comment);

            foreach (var orderItem in orderItems)
                lead.LeadItems.Add((LeadItem)orderItem);
            
            lead.Sum = lead.LeadItems.Sum(x => x.Price * x.Amount) - lead.GetTotalDiscount(lead.LeadCurrency);

            LeadService.AddLead(lead);
            return lead;
        }

        #endregion
    }
}