using System;
using System.Collections.Generic;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Repository;
using AdvantShop.Web.Admin.Models.Leads;

namespace AdvantShop.Web.Admin.Handlers.Leads
{
    public class SaveLead
    {
        private readonly LeadModel _model;

        public SaveLead(LeadModel model)
        {
            _model = model;
        }

        public bool Execute()
        {
            var lead = LeadService.GetLead(_model.Id);

            if (lead == null || _model.Lead == null || LeadService.CheckAccess(lead) == false)
                return false;

            try
            {
                lead.Description = _model.Lead.Description.DefaultOrEmpty();
                lead.Sum = _model.Lead.Sum;
                lead.FirstName = _model.Lead.FirstName.DefaultOrEmpty();
                lead.LastName = _model.Lead.LastName.DefaultOrEmpty();
                lead.Patronymic = _model.Lead.Patronymic.DefaultOrEmpty();
                lead.Phone = _model.Lead.Phone.DefaultOrEmpty();
                lead.Email = _model.Lead.Email.DefaultOrEmpty();

                lead.CustomerId = _model.Lead.CustomerId;
                if (_model.Lead.CustomerId == null || _model.Lead.Customer == null)
                {
                    if (lead.Customer == null)
                        lead.Customer = new Customer(CustomerGroupService.DefaultCustomerGroup)
                        {
                            FirstName = _model.Lead.FirstName.DefaultOrEmpty(),
                            LastName = _model.Lead.LastName.DefaultOrEmpty(),
                            Patronymic = _model.Lead.Patronymic.DefaultOrEmpty(),
                            Phone = _model.Lead.Phone.DefaultOrEmpty(),
                            EMail = _model.Lead.Email.DefaultOrEmpty(),
                        };
                }
                else if (_model.Lead.Customer != null)
                {
                    if (lead.Customer == null)
                        lead.Customer = new Customer(CustomerGroupService.DefaultCustomerGroup);

                    lead.CustomerId = _model.Lead.CustomerId;
                    lead.Customer.FirstName = _model.Lead.Customer.FirstName.DefaultOrEmpty();
                    lead.Customer.LastName = _model.Lead.Customer.LastName.DefaultOrEmpty();
                    lead.Customer.Patronymic = _model.Lead.Customer.Patronymic.DefaultOrEmpty();
                    lead.Customer.Phone = _model.Lead.Customer.Phone.DefaultOrEmpty();
                    lead.Customer.EMail = _model.Lead.Customer.EMail.DefaultOrEmpty();
                }


                if (lead.Customer.Contacts == null || lead.Customer.Contacts.Count == 0)
                {
                    var country = CountryService.GetCountry(SettingsMain.SellerCountryId);

                    lead.Customer.Contacts = new List<CustomerContact>()
                    {
                        new CustomerContact()
                        {
                            CustomerGuid = lead.Customer.Id,
                            Country = country != null ? country.Name : "",
                            City = SettingsMain.City
                        }
                    };
                }

                if (_model.Lead.Customer != null && _model.Lead.Customer.Contacts != null && _model.Lead.Customer.Contacts.Count > 0)
                {
                    lead.Customer.Contacts[0].CustomerGuid = lead.Customer.Id;
                    lead.Customer.Contacts[0].City = _model.Lead.Customer.Contacts[0].City;
                }

                lead.ManagerId = _model.Lead.ManagerId;
                lead.OrderSourceId = _model.Lead.OrderSourceId;
                lead.DealStatusId = _model.Lead.DealStatusId;

                LeadService.UpdateLead(lead);

                if (_model.CustomerFields != null && lead.Customer != null)
                {
                    foreach (var customerField in _model.CustomerFields)
                    {
                        CustomerFieldService.AddUpdateMap(lead.Customer.Id, customerField.Id, customerField.Value ?? "");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return false;
        }
    }
}
