using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Web.Admin.Handlers.Orders
{
    public class AddOrderFromCart : AbstractCommandHandler<int>
    {
        private readonly Guid _customerId;
        private ShoppingCart _cart;

        public AddOrderFromCart(Guid customerId)
        {
            _customerId = customerId;
        }

        protected override void Load()
        {
            _cart = ShoppingCartService.GetShoppingCart(ShoppingCartType.ShoppingCart, _customerId, false);
        }
        protected override void Validate()
        {
            if (!_cart.HasItems)
                throw new BlException(T("Корзина не содержит товаров"));
        }

        protected override int Handle()
        {
            var customer = CustomerService.GetCustomer(_customerId) ?? new Customer(CustomerGroupService.DefaultCustomerGroup) { Id = _customerId };

            try
            {
                if (!customer.RegistredUser)
                {
                    customer.Password = StringHelper.GeneratePassword(8);
                    customer.SubscribedForNews = false;
                    customer.CustomerRole = Role.User;

                    customer.Id = CustomerService.InsertNewCustomer(customer);
                }

                var orderSource = OrderSourceService.GetOrderSource(OrderType.ShoppingCart);

                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    OrderCurrency = CurrencyService.CurrentCurrency,
                    OrderCustomer = new OrderCustomer
                    {
                        CustomerID = customer.Id,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Patronymic = customer.Patronymic,
                        Email = customer.EMail,
                        Phone = customer.Phone,
                        StandardPhone = customer.StandardPhone
                    },
                    OrderStatusId = OrderStatusService.DefaultOrderStatus,
                    GroupName = customer.CustomerGroup.GroupName,
                    GroupDiscount = customer.CustomerGroup.GroupDiscount,
                    OrderDiscount = _cart.DiscountPercentOnTotalPrice,
                    OrderSourceId = orderSource.Id,
                    IsFromAdminArea = true,
                    IsDraft = true
                };
                order.OrderItems.AddRange(_cart.Select(item => (OrderItem)item));
                if (_cart.Certificate != null)
                {
                    order.Certificate = new OrderCertificate()
                    {
                        Code = _cart.Certificate.CertificateCode,
                        Price = _cart.Certificate.Sum
                    };
                }
                if (_cart.Coupon != null && _cart.TotalPrice >= _cart.Coupon.MinimalOrderPrice)
                {
                    order.Coupon = new OrderCoupon()
                    {
                        Code = _cart.Coupon.Code,
                        Type = _cart.Coupon.Type,
                        Value = _cart.Coupon.Value
                    };
                }

                var changedBy = new OrderChangedBy(CustomerContext.CurrentCustomer);
                order.OrderID = OrderService.AddOrder(order, changedBy);

                if (order.OrderID != 0)
                    ShoppingCartService.ClearShoppingCart(ShoppingCartType.ShoppingCart, _customerId);

                return order.OrderID;
            }
            catch (Exception ex)
            {
                Debug.Log.Error("Add order from cart", ex);
            }

            return 0;
        }
    }
}
