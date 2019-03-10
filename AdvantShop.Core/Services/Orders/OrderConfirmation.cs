using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Payment;
using AdvantShop.Repository;
using AdvantShop.Repository.Currencies;
using AdvantShop.Security;
using AdvantShop.Shipping;
using AdvantShop.Trial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.Orders
{
    public class MyCheckout
    {
        public CheckoutData Data { get; set; }

        public List<BaseShippingOption> AvailableShippingOptions(List<PreOrderItem> preorderList = null)
        {
            var preOrder = new PreOrder
            {
                Items = preorderList ?? ShoppingCartService.CurrentShoppingCart.Select(shpItem => new PreOrderItem(shpItem)).ToList(),
                CountryDest = this.Data.Contact.Country,
                RegionDest = this.Data.Contact.Region,
                CityDest = this.Data.Contact.City,
                ZipDest = this.Data.Contact.Zip,
                ShippingOption = this.Data.SelectShipping,
                PaymentOption = this.Data.SelectPayment,
                Currency = CurrencyService.CurrentCurrency,
                TotalDiscount = ShoppingCartService.CurrentShoppingCart.TotalDiscount + GetBonusDiscount(Data),
                BonusCardId = this.Data.User.BonusCardId,
                BonusUseIt = this.Data.Bonus.UseIt
            };
            var manager = new ShippingManager(preOrder);
            return manager.GetOptions();
        }

        public void UpdateSelectShipping(List<PreOrderItem> preorderList, BaseShippingOption shipping, List<BaseShippingOption> shippingOptions = null)
        {
            this.Data.SelectShipping = shipping;

            BaseShippingOption option = null;

            if (shippingOptions != null)
            {
                option = (shipping != null ? shippingOptions.SingleOrDefault(x => x.Id == shipping.Id) : null) ??
                         shippingOptions.FirstOrDefault();
            }
            else
            {
                var preOrder = new PreOrder
                {
                    Items = preorderList ?? ShoppingCartService.CurrentShoppingCart.Select(shpItem => new PreOrderItem(shpItem)).ToList(),
                    CountryDest = this.Data.Contact.Country,
                    RegionDest = this.Data.Contact.Region,
                    CityDest = this.Data.Contact.City,
                    ZipDest = this.Data.Contact.Zip,
                    ShippingOption = this.Data.SelectShipping,
                    PaymentOption = this.Data.SelectPayment,
                    Currency = CurrencyService.CurrentCurrency,
                    BonusUseIt = this.Data.Bonus.UseIt,
                    BonusCardId = this.Data.User.BonusCardId,
                    TotalDiscount = ShoppingCartService.CurrentShoppingCart.TotalDiscount + GetBonusDiscount(Data)
                };

                var manager = new ShippingManager(preOrder);
                var options = manager.GetOptions(false);

                option = (shipping != null ? options.SingleOrDefault(x => x.Id == shipping.Id) : null) ??
                         options.FirstOrDefault();
            }

            if (option != null)
                option.Update(shipping);

            this.Data.SelectShipping = option;
            this.Update();
        }

        public List<BasePaymentOption> AvailablePaymentOptions(List<PreOrderItem> preorderList = null)
        {
            var preOrder = new PreOrder()
            {
                Items = preorderList ?? ShoppingCartService.CurrentShoppingCart.Select(shpItem => new PreOrderItem(shpItem)).ToList(),
                CountryDest = this.Data.Contact.Country,
                CityDest = this.Data.Contact.City,
                RegionDest = this.Data.Contact.Region,
                BonusCardId = this.Data.User.BonusCardId,
                BonusUseIt = this.Data.Bonus.UseIt,
                ShippingOption = this.Data.SelectShipping,
                TotalDiscount = ShoppingCartService.CurrentShoppingCart.TotalDiscount + GetBonusDiscount(Data)
            };
            var manager = new PaymentManager(preOrder, preorderList == null ? ShoppingCartService.CurrentShoppingCart : null);
            var result = manager.GetOptions();
            return result;
        }

        public bool UpdateSelectPayment(List<PreOrderItem> preorderList, BasePaymentOption payment, List<BasePaymentOption> paymentOptions = null)
        {
            this.Data.SelectPayment = payment;
            var preOrder = new PreOrder
            {
                Items = preorderList ?? ShoppingCartService.CurrentShoppingCart.Select(shpItem => new PreOrderItem(shpItem)).ToList(),
                CountryDest = this.Data.Contact.Country,
                CityDest = this.Data.Contact.City,
                RegionDest = this.Data.Contact.Region,
                ShippingOption = this.Data.SelectShipping,
                PaymentOption = this.Data.SelectPayment,
                Currency = CurrencyService.CurrentCurrency,
                TotalDiscount = ShoppingCartService.CurrentShoppingCart.TotalDiscount + GetBonusDiscount(Data)
            };

            BasePaymentOption option = null;

            if (paymentOptions != null)
            {
                option = (payment != null ? paymentOptions.FirstOrDefault(x => x.Id == payment.Id) : null) ??
                         paymentOptions.FirstOrDefault();
            }
            else
            {
                var manager = new PaymentManager(preOrder, preorderList == null ? ShoppingCartService.CurrentShoppingCart : null);
                var options = manager.GetOptions(false);

                option = (payment != null ? options.SingleOrDefault(x => x.Id == payment.Id) : null) ??
                         options.FirstOrDefault();
            }

            if (option != null)
                option.Update(payment);

            this.Data.SelectPayment = option;

            if (this.Data.SelectShipping != null && this.Data.SelectShipping.ApplyPay(this.Data.SelectPayment))
            {
                var modules = AttachedModules.GetModules<IShippingCalculator>();
                var items = new List<BaseShippingOption> { this.Data.SelectShipping };
                foreach (var module in modules)
                {
                    if (module != null)
                    {
                        var classInstance = (IShippingCalculator)Activator.CreateInstance(module);
                        classInstance.ProcessOptions(items, preOrder.Items);
                    }
                }
                foreach (var item in items)
                {
                    item.Rate = item.Rate.RoundPrice(CurrencyService.CurrentCurrency);
                }
            }

            this.Update();
            return this.Data.SelectPayment == null;
        }

        public static MyCheckout Factory(Guid customerId)
        {
            var model = new MyCheckout();
            if (OrderConfirmationService.IsExist(customerId))
                model.Data = OrderConfirmationService.Get(customerId);
            else
            {
                model.Data = new CheckoutData
                {
                    ShopCartHash = ShoppingCartService.CurrentShoppingCart.GetHashCode(),
                    User = { Id = customerId }
                };
                OrderConfirmationService.Add(CustomerContext.CustomerId, model.Data);
            }
            return model;
        }

        public void Update()
        {
            OrderConfirmationService.Update(CustomerContext.CustomerId, Data);
        }

        public void ProcessUser()
        {
            var customer = CustomerContext.CurrentCustomer;

            if (customer.RegistredUser)
            {
                if (!customer.Contacts.Any())
                    ProcessRegisteredUser(customer);

                return;
            }

            var customerByEmail = CustomerService.GetCustomerByEmail(Data.User.Email);
            if (customerByEmail != null && customerByEmail.Id != Guid.Empty)
            {
                Data.User.Id = customerByEmail.Id;
                if (BonusSystem.IsActive)
                {
                    var bonusCard = BonusSystemService.GetCard(customerByEmail.Id);
                    if (bonusCard != null)
                        Data.User.BonusCardId = bonusCard.CardId;
                }
                return;
            }

            ProcessUnRegisteredUser();
        }

        private void ProcessRegisteredUser(Customer customer)
        {
            try
            {
                if (string.IsNullOrEmpty(Data.User.Email))
                {
                    Data.User.Email = customer.EMail;
                }

                customer.FirstName = Data.User.FirstName;
                customer.LastName = Data.User.LastName;
                customer.Patronymic = Data.User.Patronymic;
                customer.Phone = Data.User.Phone;
                customer.StandardPhone =
                    !string.IsNullOrEmpty(Data.User.Phone)
                        ? StringHelper.ConvertToStandardPhone(Data.User.Phone)
                        : null;

                // todo: remove
                if (customer.BonusCardNumber == null && Data.User.BonusCardId != null)
                {
                    var card = BonusSystemService.GetCard(Data.User.BonusCardId);
                    if (card != null && !card.Blocked)
                        customer.BonusCardNumber = card.CardNumber;
                }

                CustomerService.UpdateCustomer(customer);

                var country = !string.IsNullOrWhiteSpace(Data.Contact.Country)
                    ? CountryService.GetCountryByName(Data.Contact.Country)
                    : null;

                CustomerService.AddContact(new CustomerContact()
                {
                    Name = StringHelper.AggregateStrings(" ", Data.User.LastName, Data.User.FirstName, Data.User.Patronymic),
                    City = Data.Contact.City,
                    Country = Data.Contact.Country,
                    CountryId = country != null ? country.CountryId : 0,
                    Region = Data.Contact.Region,
                    Zip = Data.Contact.Zip,

                    Street = Data.Contact.Street,
                    House = Data.Contact.House,
                    Apartment = Data.Contact.Apartment,
                    Structure = Data.Contact.Structure,
                    Entrance = Data.Contact.Entrance,
                    Floor = Data.Contact.Floor

                }, customer.Id);

                if (Data.User.CustomerFields != null)
                {
                    foreach (var customerField in Data.User.CustomerFields)
                    {
                        CustomerFieldService.AddUpdateMap(customer.Id, customerField.Id, customerField.Value ?? "");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        private void ProcessUnRegisteredUser()
        {
            try
            {
                if (!Data.User.WantRegist)
                {
                    Data.User.Password = StringHelper.GeneratePassword(8);
                }

                var customer = new Customer(CustomerGroupService.DefaultCustomerGroup)
                {
                    Id = CustomerContext.CustomerId,
                    Password = Data.User.Password,
                    FirstName = Data.User.FirstName,
                    LastName = Data.User.LastName,
                    Patronymic = Data.User.Patronymic,
                    Phone = Data.User.Phone,
                    StandardPhone = StringHelper.ConvertToStandardPhone(Data.User.Phone),
                    SubscribedForNews = true,
                    EMail = Data.User.Email,
                    CustomerRole = Role.User
                };

                CustomerService.InsertNewCustomer(customer);
                if (customer.Id == Guid.Empty)
                    return;

                if (Data.User.WantRegist && BonusSystem.IsActive && (!Saas.SaasDataService.IsSaasEnabled || Saas.SaasDataService.CurrentSaasData.BonusSystem))
                {
                    CreateBonusCard(customer);
                }

                Data.User.Id = customer.Id;

                AuthorizeService.SignIn(customer.EMail, customer.Password, false, true);

                var country = !string.IsNullOrWhiteSpace(Data.Contact.Country)
                    ? CountryService.GetCountryByName(Data.Contact.Country)
                    : null;

                var contact = new CustomerContact()
                {
                    Name = customer.GetFullName(),
                    Country = Data.Contact.Country,
                    CountryId = country != null ? country.CountryId : 0,
                    Region = Data.Contact.Region,
                    City = Data.Contact.City,
                    Zip = Data.Contact.Zip,

                    Street = Data.Contact.Street,
                    House = Data.Contact.House,
                    Apartment = Data.Contact.Apartment,
                    Structure = Data.Contact.Structure,
                    Entrance = Data.Contact.Entrance,
                    Floor = Data.Contact.Floor
                };

                CustomerService.AddContact(contact, customer.Id);

                if (Data.User.CustomerFields != null)
                {
                    foreach (var customerField in Data.User.CustomerFields)
                    {
                        CustomerFieldService.AddUpdateMap(customer.Id, customerField.Id, customerField.Value ?? "");
                    }
                }

                if (Data.User.WantRegist)
                {
                    var registrationMail = new RegistrationMailTemplate(SettingsMain.SiteUrl, customer.FirstName,
                        customer.LastName, Localization.Culture.ConvertDate(DateTime.Now), customer.Password,
                        LocalizationService.GetResource("User.Registration.No"), customer.EMail, customer.Phone,
                        SettingsCheckout.IsShowPatronymic ? customer.Patronymic : string.Empty);
                    registrationMail.BuildMail();

                    SendMail.SendMailNow(CustomerContext.CustomerId, customer.EMail, registrationMail.Subject, registrationMail.Body, true);
                    SendMail.SendMailNow(Guid.Empty, SettingsMail.EmailForRegReport, registrationMail.Subject, registrationMail.Body, true, customer.EMail);
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        public Order ProcessOrder(string customData, OrderType orderType)
        {
            ProcessUser();

            var cart = ShoppingCartService.CurrentShoppingCart;

            var order = CreateOrder(cart, customData, orderType);
            
            var certificate = cart.Certificate;
            if (certificate != null)
            {
                certificate.ApplyOrderNumber = order.Number;
                certificate.Used = true;
                certificate.Enable = true;

                GiftCertificateService.DeleteCustomerCertificate(certificate.CertificateId);
                GiftCertificateService.UpdateCertificateById(certificate);
            }

            var coupon = cart.Coupon;
            if (coupon != null && cart.TotalPrice >= coupon.MinimalOrderPrice)
            {
                coupon.ActualUses += 1;
                CouponService.UpdateCoupon(coupon);
                CouponService.DeleteCustomerCoupon(coupon.CouponID);
            }

            ShoppingCartService.ClearShoppingCart(ShoppingCartType.ShoppingCart, Data.User.Id);
            ShoppingCartService.ClearShoppingCart(ShoppingCartType.ShoppingCart, CustomerContext.CustomerId);

            OrderConfirmationService.Delete(CustomerContext.CustomerId);

            return order;
        }
        
        private Order CreateOrder(ShoppingCart cart, string customData, OrderType orderType)
        {
            var currency = CurrencyService.CurrentCurrency;
            var orderSource = OrderSourceService.GetOrderSource(OrderType.ShoppingCart);

            if (orderType != OrderType.None)
                orderSource = OrderSourceService.GetOrderSource(orderType);
            else if (SettingsDesign.IsSocialTemplate)
                orderSource = OrderSourceService.GetOrderSource(OrderType.SocialNetworks);
            else if (SettingsDesign.IsMobileTemplate)
                orderSource = OrderSourceService.GetOrderSource(OrderType.Mobile);

            var customer = CustomerContext.CurrentCustomer;

            var order = new Order
            {
                OrderCustomer = new OrderCustomer
                {
                    CustomerIP = HttpContext.Current.Request.UserHostAddress,
                    CustomerID = Data.User.Id,
                    FirstName = Data.User.FirstName,
                    LastName = Data.User.LastName,
                    Patronymic = Data.User.Patronymic,
                    Email = Data.User.Email,
                    Phone = Data.User.Phone,
                    StandardPhone =
                        !string.IsNullOrWhiteSpace(Data.User.Phone)
                            ? StringHelper.ConvertToStandardPhone(Data.User.Phone)
                            : null,

                    Country = Data.Contact.Country,
                    Region = Data.Contact.Region,
                    City = Data.Contact.City,
                    Zip = Data.Contact.Zip,
                    CustomField1 = Data.Contact.CustomField1,
                    CustomField2 = Data.Contact.CustomField2,
                    CustomField3 = Data.Contact.CustomField3,

                    Street = Data.Contact.Street,
                    House = Data.Contact.House,
                    Apartment = Data.Contact.Apartment,
                    Structure = Data.Contact.Structure,
                    Entrance = Data.Contact.Entrance,
                    Floor = Data.Contact.Floor
                },
                OrderCurrency = currency,
                OrderStatusId = OrderStatusService.DefaultOrderStatus,
                AffiliateID = 0,
                OrderDate = DateTime.Now,
                CustomerComment = Data.CustomerComment,
                ManagerId = customer.ManagerId,

                GroupName = customer.CustomerGroup.GroupName,
                GroupDiscount = customer.CustomerGroup.GroupDiscount,
                OrderDiscount = cart.DiscountPercentOnTotalPrice,
                OrderSourceId = orderSource.Id,
                CustomData = customData
            };

            foreach (var orderItem in cart.Select(item => (OrderItem)item))
            {
                order.OrderItems.Add(orderItem);
            }

            order.ShippingMethodId = Data.SelectShipping.MethodId;
            order.PaymentMethodId = Data.SelectPayment.Id;

            order.ArchivedShippingName = Data.SelectShipping.Name;
            order.ArchivedPaymentName = Data.SelectPayment.Name;

            order.OrderPickPoint = Data.SelectShipping.GetOrderPickPoint();

            order.PaymentDetails = Data.SelectPayment.GetDetails();

            ProcessCertificate(order);
            ProcessCoupon(order);

            var shippingPrice = Data.SelectShipping.Rate;
            var paymentPrice = Data.SelectPayment.Rate;

            Card bonusCard = null;
            if (BonusSystem.IsActive)
            {
                bonusCard = BonusSystemService.GetCard(Data.User.BonusCardId);

                if (Data.Bonus.UseIt && bonusCard != null && bonusCard.BonusesTotalAmount > 0)
                {
                    order.BonusCost = BonusSystemService.GetBonusCost(bonusCard, cart, shippingPrice, Data.Bonus.UseIt).BonusPrice;
                }

                if (Data.User.WantBonusCard && bonusCard == null && customer.RegistredUser)
                {
                    CreateBonusCard(customer);
                    bonusCard = BonusSystemService.GetCard(customer.Id);
                }
            }

            order.BonusCardNumber = bonusCard != null && !bonusCard.Blocked ? bonusCard.CardNumber : default(long?);

            order.ShippingCost = shippingPrice;
            order.ShippingTaxType = Data.SelectShipping.TaxType;
            order.PaymentCost = paymentPrice;
            
            order.OrderID = OrderService.AddOrder(order, new OrderChangedBy(customer));

            OrderStatusService.ChangeOrderStatus(order.OrderID, OrderStatusService.DefaultOrderStatus, LocalizationService.GetResource("Core.OrderStatus.Created"));
            
            if (BonusSystem.IsActive && bonusCard != null && !bonusCard.Blocked)
            {
                BonusSystemService.MakeBonusPurchase(bonusCard.CardNumber, cart, shippingPrice, order);
            }

            PostProcessOrder(order);

            OrderService.SendOrderMail(order, cart.TotalDiscount, Data.Bonus.BonusPlus, Data.SelectShipping.ForMailTemplate(), Data.SelectPayment.Name);

            TrialService.TrackEvent(
                order.OrderItems.Any(item => item.Name.Contains("SM-G900F"))
                    ? TrialEvents.BuyTheProduct
                    : TrialEvents.CheckoutOrder, string.Empty);

            return order;
        }

        private void ProcessCertificate(Order order)
        {
            var certificate = ShoppingCartService.CurrentShoppingCart.Certificate;

            if (certificate != null)
            {
                order.Certificate = new OrderCertificate()
                {
                    Code = certificate.CertificateCode,
                    Price = certificate.Sum
                };
            }
        }

        private void ProcessCoupon(Order order)
        {
            var coupon = ShoppingCartService.CurrentShoppingCart.Coupon;
            if (coupon != null && ShoppingCartService.CurrentShoppingCart.TotalPrice >= coupon.MinimalOrderPrice)
            {
                order.Coupon = new OrderCoupon()
                {
                    Code = coupon.Code,
                    Type = coupon.Type,
                    Value = coupon.GetRate()
                };
            }
        }

        private void CreateBonusCard(Customer customer)
        {
            try
            {
                customer.BonusCardNumber = BonusSystemService.AddCard(new Card { CardId = customer.Id });
                CustomerService.UpdateCustomer(customer);

                if (customer.BonusCardNumber != null)
                {
                    Data.User.BonusCardId = customer.Id;

                    if (HttpContext.Current != null)
                        HttpContext.Current.Session["BonusesForNewCard"] = BonusSystem.BonusesForNewCard;
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
        }

        private void PostProcessOrder(Order order)
        {
            if (order.PaymentMethod.PaymentKey == "GiftCertificate" || order.Sum == 0)
                OrderService.PayOrder(order.OrderID, true);
        }

        private float GetBonusDiscount(CheckoutData data)
        {
            if(data.Bonus != null && data.Bonus.UseIt)
            {
                var bonusCard = BonusSystemService.GetCard(data.User.BonusCardId);
                return (float)bonusCard.BonusAmount;
            }

            return 0.0f;
        }
    }
}