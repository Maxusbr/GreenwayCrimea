using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Areas.Api.Attributes;
using AdvantShop.Areas.Api.Models.Leads;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Api;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Areas.Api.Controllers
{
    public class LeadsController : BaseApiController
    {
        [LogRequest, AuthApi]
        public JsonResult Add(AddLeadModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    OrderSource orderSource = null;
                    if (!string.IsNullOrWhiteSpace(model.Source))
                    {
                        orderSource = OrderSourceService.GetOrderSource(model.Source);
                        if (orderSource == null)
                        {
                            var orderSourceId = OrderSourceService.AddOrderSource(new OrderSource
                            {
                                Name = model.Source,
                                SortOrder = 0,
                                Type = OrderType.None
                            });
                            orderSource = OrderSourceService.GetOrderSource(orderSourceId);
                        }
                    }

                    if (orderSource == null)
                        orderSource = OrderSourceService.GetOrderSource(OrderType.None);

                    var lead = new Lead()
                    {
                        FirstName = model.FirstName.DefaultOrEmpty(),
                        LastName = model.LastName.DefaultOrEmpty(),
                        Patronymic = model.Patronymic.DefaultOrEmpty(),
                        Phone = model.Phone.DefaultOrEmpty(),
                        Email = model.Email.DefaultOrEmpty(),
                        Description = model.Description.DefaultOrEmpty(),
                        Sum = model.Sum,
                        OrderSourceId = orderSource != null ? orderSource.Id : 0,
                        DiscountValue = model.DiscountValue,
                        Discount = model.DiscountValue != 0 ? 0 : model.DiscountPercent,
                        LeadCurrency = CurrencyService.CurrentCurrency
                };

                    if (model.CustomerId != null && model.CustomerId != Guid.Empty)
                    {
                        lead.CustomerId = model.CustomerId.Value;

                        var customer = CustomerService.GetCustomer(model.CustomerId.Value);
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

                    if (model.Products != null)
                    {
                        var items = new List<LeadItem>();

                        foreach (var productModel in model.Products)
                        {
                            var offer = OfferService.GetOffer(productModel.ArtNo);
                            if (offer == null)
                            {
                                var p = ProductService.GetProduct(productModel.ArtNo);
                                if (p != null && p.Offers.Count == 1)
                                    offer = p.Offers[0];
                            }

                            if (offer != null)
                                items.Add(new LeadItem(offer, productModel.Amount));
                            else
                            {
                                items.Add(new LeadItem()
                                {
                                    Name = productModel.Name,
                                    ArtNo = productModel.ArtNo ?? "",
                                    Amount = productModel.Amount,
                                    Price = productModel.Price
                                });
                            }
                        }

                        lead.LeadItems = items;
                        lead.Sum = lead.LeadItems.Sum(x => x.Price*x.Amount) - lead.GetTotalDiscount(lead.LeadCurrency);
                    }

                    LeadService.AddLead(lead, true);

                    return Json(new AddLeadResponse("ok", "") { leadId = lead.Id});
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    ModelState.AddModelError("", ex.Message);
                }
            }

            var errors = new List<string>();

            foreach (var modelState in ViewData.ModelState.Values)
                foreach (var error in modelState.Errors)
                    errors.Add(error.ErrorMessage);

            return Json(new ApiResponse("error", String.Join("; ", errors)));
        }
    }
}