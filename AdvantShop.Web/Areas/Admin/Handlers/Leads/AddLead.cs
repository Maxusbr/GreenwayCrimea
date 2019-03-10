using System;
using System.Collections.Generic;
using AdvantShop.Catalog;
using AdvantShop.Web.Admin.Models.Leads;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Trial;

namespace AdvantShop.Web.Admin.Handlers.Leads
{
    public class AddLead
    {
        private readonly AddLeadModel _model;

        public AddLead(AddLeadModel model)
        {
            _model = model;
        }

        public int Execute()
        {
            try
            {
                var orderSource = OrderSourceService.GetOrderSource(OrderType.None);

                var lead = new Lead()
                {
                    FirstName = _model.FirstName.DefaultOrEmpty(),
                    LastName = _model.LastName.DefaultOrEmpty(),
                    Patronymic = _model.Patronymic.DefaultOrEmpty(),
                    Phone = _model.Phone.DefaultOrEmpty(),
                    Email = _model.Email.DefaultOrEmpty(),
                    Description = _model.Description.DefaultOrEmpty(),
                    Sum = _model.Sum,
                    ManagerId = _model.ManagerId,
                    OrderSourceId = orderSource != null ? orderSource.Id : 0,
                    DealStatusId = _model.DealStatusId.TryParseInt(),
                    IsFromAdminArea = true
                };
                
                if (_model.CustomerId != null && _model.CustomerId != Guid.Empty)
                {
                    lead.CustomerId = _model.CustomerId.Value;

                    var customer = CustomerService.GetCustomer(_model.CustomerId.Value);
                    if (customer != null)
                    {
                        lead.Customer.FirstName = lead.FirstName;
                        lead.Customer.LastName = lead.LastName;
                        lead.Customer.Patronymic = lead.Patronymic;
                        lead.Customer.Phone = lead.Phone;
                        lead.Customer.StandardPhone = StringHelper.ConvertToStandardPhone(lead.Phone);

                        if (string.IsNullOrWhiteSpace(lead.Customer.EMail))
                            lead.Customer.EMail = lead.Email;
                    }
                    else
                    {
                        lead.Customer = new Customer(CustomerGroupService.DefaultCustomerGroup)
                        {
                            Id = lead.CustomerId.Value,
                            FirstName = lead.FirstName,
                            LastName = lead.LastName,
                            Patronymic = lead.Patronymic,
                            Phone = lead.Phone,
                            StandardPhone = StringHelper.ConvertToStandardPhone(lead.Phone),
                            EMail = lead.Email,
                            CustomerRole = Role.User
                        };
                    }
                }
                else
                {
                    lead.Customer = new Customer(CustomerGroupService.DefaultCustomerGroup)
                    {
                        FirstName = lead.FirstName,
                        LastName = lead.LastName,
                        Patronymic = lead.Patronymic,
                        Phone = lead.Phone,
                        StandardPhone = StringHelper.ConvertToStandardPhone(lead.Phone),
                        EMail = lead.Email,
                        CustomerRole = Role.User
                    };
                }

                if (_model.Products != null)
                {
                    var items = new List<LeadItem>();
                    
                    foreach (var productModel in _model.Products)
                    {
                        var offer = OfferService.GetOffer(productModel.OfferId);
                        if (offer == null)
                            continue;

                        items.Add(new LeadItem(offer, productModel.Amount));
                    }

                    lead.LeadItems = items;
                }

                LeadService.AddLead(lead, true);

                if (lead.CustomerId != null && _model.CustomerFields != null)
                {
                    foreach (var customerField in _model.CustomerFields)
                    {
                        CustomerFieldService.AddUpdateMap(lead.CustomerId.Value, customerField.Id, customerField.Value ?? "");
                    }
                }

                TrialService.TrackEvent(ETrackEvent.Trial_AddLead);

                return lead.Id;
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return 0;
        }
    }
}

