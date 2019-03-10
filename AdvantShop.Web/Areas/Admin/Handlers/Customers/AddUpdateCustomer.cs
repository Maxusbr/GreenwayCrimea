using System;
using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Repository;
using AdvantShop.Trial;
using AdvantShop.Web.Admin.Models.Customers;

namespace AdvantShop.Web.Admin.Handlers.Customers
{
    public class AddUpdateCustomer
    {
        private readonly CustomersModel _model;

        public AddUpdateCustomer(CustomersModel model)
        {
            _model = model;
        }

        public bool Execute()
        {
            var customer = _model.IsEditMode 
                                ? CustomerService.GetCustomer(_model.CustomerId) 
                                : new Customer();
            if (_model.CustomerId != Guid.Empty)
                customer.Id = _model.CustomerId;
            customer.EMail = _model.Customer.EMail.DefaultOrEmpty();
            customer.FirstName = _model.Customer.FirstName.DefaultOrEmpty();
            customer.LastName = _model.Customer.LastName.DefaultOrEmpty();
            customer.Patronymic = _model.Customer.Patronymic.DefaultOrEmpty();
            customer.Phone = _model.Customer.Phone.DefaultOrEmpty();
            customer.StandardPhone = StringHelper.ConvertToStandardPhone(customer.Phone);
            customer.SubscribedForNews = _model.Customer.SubscribedForNews;

            customer.CustomerGroupId = _model.Customer.CustomerGroupId;
            customer.ManagerId = _model.Customer.ManagerId;
            customer.AdminComment = _model.Customer.AdminComment.DefaultOrEmpty();
            customer.BonusCardNumber = _model.Customer.BonusCardNumber;
            

            if (!_model.IsEditMode)
            {
                customer.Password = _model.Customer.Password.DefaultOrEmpty();
                customer.RegistrationDateTime = DateTime.Now;

                _model.CustomerId = customer.Id = CustomerService.InsertNewCustomer(customer);
                TrialService.TrackEvent(ETrackEvent.Trial_AddCustomer);
            }
            else
            {
                CustomerService.UpdateCustomer(customer);
            }

            if (_model.CustomerContact != null && (_model.CustomerContact.City.IsNotEmpty() || _model.CustomerContact.Zip.IsNotEmpty() || _model.CustomerContact.Country.IsNotEmpty() ||
                                                   _model.CustomerContact.Region.IsNotEmpty() || _model.CustomerContact.Street.IsNotEmpty() ||  _model.CustomerContact.House.IsNotEmpty() ||
                                                   _model.CustomerContact.Apartment.IsNotEmpty() || _model.CustomerContact.Structure.IsNotEmpty() || _model.CustomerContact.Entrance.IsNotEmpty() ||
                                                   _model.CustomerContact.Floor.IsNotEmpty()))
            {
                var contact = CustomerService.GetCustomerContact(_model.CustomerContact.ContactId.ToString()) ??
                              new CustomerContact();
                
                contact.Name = (customer.FirstName + " " + customer.LastName).Trim();
                contact.City = _model.CustomerContact.City.DefaultOrEmpty();
                //contact.Address = _model.CustomerContact.Address.DefaultOrEmpty();
                contact.Zip = _model.CustomerContact.Zip.DefaultOrEmpty();

                contact.Country = _model.CustomerContact.Country.DefaultOrEmpty();
                var country = CountryService.GetCountryByName(contact.Country);
                contact.CountryId = country != null ? country.CountryId : 0;

                contact.Region = _model.CustomerContact.Region.DefaultOrEmpty();
                contact.RegionId = RegionService.GetRegionIdByName(contact.Region);

                contact.Street = _model.CustomerContact.Street.DefaultOrEmpty();
                contact.House = _model.CustomerContact.House.DefaultOrEmpty();
                contact.Apartment = _model.CustomerContact.Apartment.DefaultOrEmpty();
                contact.Structure = _model.CustomerContact.Structure.DefaultOrEmpty();
                contact.Entrance = _model.CustomerContact.Entrance.DefaultOrEmpty();
                contact.Floor = _model.CustomerContact.Floor.DefaultOrEmpty();

                if (contact.ContactId == Guid.Empty)
                {
                    CustomerService.AddContact(contact, customer.Id);
                }
                else
                {
                    CustomerService.UpdateContact(contact);
                }
            }

            if (_model.CustomerFields != null)
            {
                foreach (var customerField in _model.CustomerFields)
                {
                    CustomerFieldService.AddUpdateMap(customer.Id, customerField.Id, customerField.Value ?? "");
                }
            }

            return true;
        }
    }
}

