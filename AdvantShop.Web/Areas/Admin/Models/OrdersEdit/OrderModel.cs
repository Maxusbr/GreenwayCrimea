using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Loging.TrafficSource;
using AdvantShop.Core.Services.Vk;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Saas;

namespace AdvantShop.Web.Admin.Models.OrdersEdit
{
    public class OrderModel : IValidatableObject
    {
        public OrderModel()
        {
            OrderStatuses =
                OrderStatusService.GetOrderStatuses()
                    .Select(x => new SelectListItem() { Text = x.StatusName, Value = x.StatusID.ToString() })
                    .ToList();

            OrderSources =
                OrderSourceService.GetOrderSources()
                    .Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() })
                    .ToList();

            Managers = new List<SelectListItem>() { new SelectListItem() { Text = "-", Value = "" } };
            Managers.AddRange(ManagerService.GetManagersList().OrderBy(x => x.FullName).Select(x => new SelectListItem() { Text = x.FullName, Value = x.ManagerId.ToString() }));

            OrderCurrency = CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3);
        }

        public int OrderId { get; set; }

        public bool IsPayed { get; set; }

        public Order Order { get; set; }

        public Currency OrderCurrency { get; set; }

        public Customer Customer { get; set; }

        public bool IsEditMode { get; set; }

        public OrderTrafficSource OrderTrafficSource { get; set; }


        public float ProductsCost { get; set; }
        public float ProductsDiscountPrice { get; set; }
        public float CouponPrice { get; set; }


        public List<SelectListItem> OrderStatuses { get; set; }

        public List<SelectListItem> OrderSources { get; set; }

        public List<SelectListItem> Managers { get; set; }

        public bool ShowManager
        {
            get
            {
                var c = CustomerContext.CurrentCustomer;
                return c.IsAdmin || (c.IsModerator && c.HasRoleAction(RoleAction.Crm));
            }
        }

        public Card BonusCard { get; set; }

        //public Purchase BonusCardPurchase { get; set; }
        //public bool UseBonuses { get; set; }



        public bool ShowCrm
        {
            get
            {
                return !SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveCrm;
            }
        }

        public bool ShowActivity
        {
            get
            {
                return !SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveCustomerLog;
            }
        }

        public bool ShowTelelephony
        {
            get
            {
                return SettingsTelephony.PhonerLiteActive &&
                       (!SaasDataService.IsSaasEnabled ||
                        (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveTelephony));
            }
        }

        public long? StandardPhone
        {
            get
            {
                return Order != null && Order.OrderCustomer != null ? Order.OrderCustomer.StandardPhone : null;
            }
        }

        public List<IModuleSms> SmsModules
        {
            get
            {
                return AttachedModules.GetModules<IModuleSms>().Select(x => (IModuleSms)Activator.CreateInstance(x)).ToList();
            }
        }

        private List<CustomerFieldWithValue> _customerFields;

        public List<CustomerFieldWithValue> CustomerFields
        {
            get
            {
                if (_customerFields != null)
                    return _customerFields;
                
                if (Order != null && Order.OrderCustomer != null)
                {
                    var customer = CustomerService.GetCustomer(Order.OrderCustomer.CustomerID);
                    if (customer != null)
                    {
                        _customerFields = CustomerFieldService.GetCustomerFieldsWithValue(customer.Id);
                    }
                }

                return _customerFields;
            }
            set { _customerFields = value; }
        }

        public bool ShowVk
        {
            get { return new VkApiService().IsVkActive(); }
        }

        public VkUser VkUser { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsEditMode && Order == null)
            {
                yield return new ValidationResult("Заказ не найден", new[] { "Order" });
            }
        }
    }
}