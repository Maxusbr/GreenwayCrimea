<%@ WebHandler Language="C#" Class="CreateLead" %>

using System;
using System.Web;
using System.Collections.Generic;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Module.AbandonedCarts.Domain;



public class CreateLead : IHttpHandler
{


    public void ProcessRequest(HttpContext context)
    {
        var customerId = context.Request["customerId"].TryParseGuid();
        var customer = CustomerService.GetCustomer(customerId);
        if (customer == null)
        {
            var confirmCart = AbandonedCartsService.GetAbondonedCart(customerId);
            if (confirmCart != null && confirmCart.CheckoutData != null && confirmCart.CheckoutData.User != null)
                customer = AbandonedCartsService.GetCustomer(confirmCart.CheckoutData.User);
        }

        if (customer == null)
        {
            context.Response.Write("error. invalid customer");
            return;
        }

        var shoppingCartType = context.Request["ShoppingCartType"].TryParseEnum<ShoppingCartType>();
        if (shoppingCartType == 0)
            shoppingCartType = ShoppingCartType.ShoppingCart;

        ShoppingCart shoppingCart = ShoppingCartService.GetShoppingCart(shoppingCartType, customerId);

        bool isValid = shoppingCart != null && shoppingCart.HasItems &&
            (customer.EMail.IsNotEmpty() || customer.FirstName.IsNotEmpty() || customer.Phone.IsNotEmpty());

        if (!isValid)
        {
            context.Response.Write("error. invalid data");
            return;
        }

        Currency cur = CurrencyService.CurrentCurrency;

        OrderSource orderSource = OrderSourceService.GetOrderSource(OrderType.AbandonedCart);

        Lead lead = new Lead();

        lead.FirstName = customer.FirstName;
        lead.LastName = customer.LastName;
        lead.Patronymic = customer.Patronymic;
        lead.Email = customer.EMail;
        lead.Phone = customer.Phone;
        lead.CustomerId = customer.Id != Guid.Empty ? customer.Id : default(Guid?);

        lead.LeadCurrency = new LeadCurrency();
        lead.LeadCurrency.CurrencyCode = cur.Iso3;
        lead.LeadCurrency.CurrencyNumCode = cur.NumIso3;
        lead.LeadCurrency.CurrencyValue = cur.Rate;
        lead.LeadCurrency.CurrencySymbol = cur.Symbol;
        lead.LeadCurrency.IsCodeBefore = cur.IsCodeBefore;

        lead.LeadItems = new List<LeadItem>();
        lead.OrderSourceId = orderSource.Id;

        lead.Customer = new Customer();
        lead.Customer.FirstName = customer.FirstName;
        lead.Customer.LastName = customer.LastName;
        lead.Customer.Patronymic = customer.Patronymic;
        lead.Customer.EMail = customer.EMail;
        lead.Customer.Phone = customer.Phone;
        lead.Customer.StandardPhone = customer.StandardPhone;

        foreach (ShoppingCartItem cartItem in shoppingCart)
        {
            lead.LeadItems.Add((LeadItem)((OrderItem)cartItem));
        }

        try
        {
            LeadService.AddLead(lead, true);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            context.Response.Write("error");
            return;
        }

        context.Response.Write(lead.Id);
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}