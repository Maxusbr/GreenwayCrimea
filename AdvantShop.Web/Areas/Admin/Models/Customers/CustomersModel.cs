using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.CustomerSegments;
using AdvantShop.Core.Services.Vk;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Saas;

namespace AdvantShop.Web.Admin.Models.Customers
{
    public class CustomersModel : IValidatableObject
    {
        public CustomersModel()
        {
            CustomerContact = new CustomerContact();

            CustomerGroups =
                CustomerGroupService.GetCustomerGroupList().Select(x =>
                            new SelectListItem()
                            {
                                Text = string.Format("{0} - {1}%", x.GroupName, x.GroupDiscount),
                                Value = x.CustomerGroupId.ToString()
                            }).ToList();


            Managers = new List<SelectListItem>() { new SelectListItem() { Text = "-", Value = "" } };
            Managers.AddRange(ManagerService.GetManagersList().OrderBy(x => x.FullName).Select(x => new SelectListItem() { Text = x.FullName, Value = x.ManagerId.ToString() }));
        }

        public Guid CustomerId { get; set; }

        public bool IsEditMode { get; set; }

        public Customer Customer { get; set; }

        public CustomerContact CustomerContact { get; set; }

        public List<SelectListItem> CustomerGroups { get; set; }

        public List<SelectListItem> Managers { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

        public bool ShowTelelephony
        {
            get
            {
                return SettingsTelephony.PhonerLiteActive &&
                       (!SaasDataService.IsSaasEnabled ||
                        (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveTelephony));
            }
        }

        public List<IModuleSms> SmsModules
        {
            get
            {
                return AttachedModules.GetModules<IModuleSms>().Select(x => (IModuleSms)Activator.CreateInstance(x)).ToList();
            }
        }

        public bool ShowManager
        {
            get
            {
                var c = CustomerContext.CurrentCustomer;
                return c.IsAdmin || (c.IsModerator && c.HasRoleAction(RoleAction.Crm));
            }
        }


        public bool ShowCrm
        {
            get { return !SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveCrm; }
        }

        public bool ShowActivity
        {
            get { return !SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.HaveCustomerLog; }
        }

        public bool ShowVk
        {
            get { return new VkApiService().IsVkActive(); }
        }

        public bool ShowBonusSystem
        {
            get { return (!SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.BonusSystem) && BonusSystem.IsActive; }
        }

        public long BonusCardNumber { get; set; }
        public decimal BonusAmount { get; set; }
        public decimal BonusPercent { get; set; }
        public bool BonusCardBlocked { get; set; }
        public string GradeName { get; set; }

        //public Card BonusCard { get; set; }
        //public Grade Grade { get; set; }
        //public string Segment { get; set; }


        private List<CustomerFieldWithValue> _customerFields;

        public List<CustomerFieldWithValue> CustomerFields
        {
            get
            {
                if (_customerFields != null)
                    return _customerFields;

                _customerFields = CustomerFieldService.GetCustomerFieldsWithValue(CustomerId) ??
                                  new List<CustomerFieldWithValue>();

                return _customerFields;
            }
            set { _customerFields = value; }
        }

        public VkUser VkUser { get; set; }
        public List<CustomerSegment> CustomerSegments { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Customer == null)
            {
                yield return new ValidationResult("Пользователь не найден.");
            }

            if (IsEditMode && Customer != null)
            {
                var c = CustomerService.GetCustomer(CustomerId);
                if (c == null)
                    yield return new ValidationResult("Пользователь не найден");

                if (c != null)
                {
                    if (!string.IsNullOrEmpty(Customer.EMail) && Customer.EMail != c.EMail && CustomerService.ExistsEmail(Customer.EMail))
                        yield return new ValidationResult("Введенный Email занят");

                    if (Customer.BonusCardNumber.HasValue && BonusSystem.IsActive &&
                        Customer.BonusCardNumber != c.BonusCardNumber && BonusSystemService.GetCard(Customer.BonusCardNumber) == null)
                        yield return new ValidationResult("Бонусной карты с таким номером не существует");
                }
            }

            if (!IsEditMode && Customer != null)
            {
                if (!string.IsNullOrWhiteSpace(Customer.EMail) && CustomerService.ExistsEmail(Customer.EMail))
                {
                    yield return new ValidationResult("Пользователь с почтой \"" + Customer.EMail + "\" уже существует");
                }
            }
        }
    }
}
