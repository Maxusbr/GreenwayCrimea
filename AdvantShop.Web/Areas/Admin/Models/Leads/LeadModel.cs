using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Crm.DealStatuses;
using AdvantShop.Core.Services.Loging.TrafficSource;
using AdvantShop.Core.Services.Vk;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Helpers;

namespace AdvantShop.Web.Admin.Models.Leads
{
    public class LeadModel : IValidatableObject
    {
        public LeadModel()
        {
            LeadSources =
                OrderSourceService.GetOrderSources()
                    .Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() })
                    .ToList();


            Statuses = new List<SelectListItem>() { new SelectListItem() { Text = "-", Value = "0" } };
            Statuses.AddRange(
                DealStatusService.GetList().Select(x => new SelectListItem() {Text = x.Name, Value = x.Id.ToString()}));


            Managers = new List<SelectListItem>() { new SelectListItem() { Text = "-", Value = "" } };
            Managers.AddRange(
                ManagerService.GetManagersList().OrderBy(x => x.FullName)
                    .Select(x => new SelectListItem() {Text = x.FullName, Value = x.ManagerId.ToString()}));
            

            LeadCurrency = CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3);
        }

        public int Id { get; set; }
        
        public Lead Lead { get; set; }

        public Currency LeadCurrency { get; set; }

        public List<SelectListItem> Statuses { get; set; }

        public List<SelectListItem> LeadSources { get; set; }

        public List<SelectListItem> Managers { get; set; }

        public long? StandardPhone
        {
            get
            {
                if (Lead != null)
                    return Lead.CustomerId != null && Lead.Customer != null
                        ? StringHelper.ConvertToStandardPhone(Lead.Customer.Phone)
                        : StringHelper.ConvertToStandardPhone(Lead.Phone);

                return null;
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

                _customerFields = new List<CustomerFieldWithValue>();

                if (Lead != null && Lead.CustomerId != null)
                {
                    _customerFields = CustomerFieldService.GetCustomerFieldsWithValue(Lead.CustomerId.Value) ??
                                      new List<CustomerFieldWithValue>();
                }

                return _customerFields;
            }
            set { _customerFields = value; }
        }

        public OrderTrafficSource TrafficSource { get; set; }

        public VkUser VkUser { get; set; }
        public Order Order { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Lead == null)
            {
                yield return new ValidationResult("Лид не найден");
            }
        }
    }
}
