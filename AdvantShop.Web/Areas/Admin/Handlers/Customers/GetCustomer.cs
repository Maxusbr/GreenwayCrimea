using System;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.CustomerSegments;
using AdvantShop.Core.Services.Vk;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.Models.Customers;

namespace AdvantShop.Web.Admin.Handlers.Customers
{
    public class GetCustomer
    {
        private readonly int _orderId;
        private readonly Guid _customerId;
        private readonly string _clientCode;
        private readonly bool _isEditMode;

        public GetCustomer()
        {
        }

        public GetCustomer(Guid customerId, string clientCode = null)
        {
            _customerId = customerId;
            _clientCode = clientCode;
            _isEditMode = true;
        }

        public GetCustomer(AddCustomerModel addCustomerModel)
        {
            if (addCustomerModel.OrderId != null)
                _orderId = addCustomerModel.OrderId.Value;
        }

        public CustomersModel Execute()
        {
            var model = new CustomersModel()
            {
                CustomerId = _customerId,
                IsEditMode = _isEditMode,
            };

            if (_orderId != 0)
            {
                var orderCustomer = OrderService.GetOrderCustomer(_orderId);
                if (orderCustomer != null)
                {
                    model.CustomerId = orderCustomer.CustomerID;
                    model.Customer = (Customer)orderCustomer;

                    if (model.Customer.Contacts != null && model.Customer.Contacts.Count > 0)
                        model.CustomerContact = model.Customer.Contacts[0];
                }
            }

            if (!_isEditMode)
                return model;

            var customer = CustomerService.GetCustomer(_customerId);
            if (customer == null && _clientCode.IsNotEmpty())
            {
                customer = ClientCodeService.GetCustomerByCode(_clientCode, _customerId);
                customer.Code = _clientCode;
                model.IsEditMode = false;
            }

            if (customer == null)
                return null;

            model.Customer = customer;
            model.Customer.SubscribedForNews = customer.EMail.IsNotEmpty() ? SubscriptionService.IsSubscribe(customer.EMail) : false;

            if (customer.Contacts != null && customer.Contacts.Count > 0)
                model.CustomerContact = customer.Contacts[0];

            model.ShoppingCart = ShoppingCartService.GetShoppingCart(ShoppingCartType.ShoppingCart, customer.Id, false);

            if (BonusSystem.IsActive)
            {
                var card = BonusSystemService.GetCard(customer.Id);
                if (card != null)
                {
                    model.BonusCardNumber = card.CardNumber;
                    model.BonusAmount = card.BonusesTotalAmount;
                    model.BonusPercent = card.Grade.BonusPercent;
                    model.GradeName = card.Grade.Name;
                    model.BonusCardBlocked = card.Blocked;
                }
            }

            model.VkUser = VkService.GetUser(customer.Id);
            model.CustomerSegments = CustomerSegmentService.GetListByCustomerId(customer.Id);

            return model;
        }
    }
}
