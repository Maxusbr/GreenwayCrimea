<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.AdminSearch" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.FullSearch;
using AdvantShop.Core.Services.Security;
using AdvantShop.Customers;

namespace Admin.HttpHandlers
{
    [AuthorizeRole(RoleAction.Catalog, RoleAction.Orders)]
    public class AdminSearch : AdminHandler, IHttpHandler
    {

        public enum eAdminSearch
        {
            Product = 0,
            Order = 1,
            Customer = 2
        }

        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            context.Response.Cache.SetLastModified(DateTime.UtcNow);

            string type = context.Request["type"].ToString();
            string q = context.Request["q"].ToString();
            string result = string.Empty;

            eAdminSearch searchType;

            bool resultParse = Enum.TryParse(type, true, out searchType);

            if (resultParse == true)
            {
                switch (searchType)
                {
                    case eAdminSearch.Product:
                        result = GetProducts(q);
                        break;
                    case eAdminSearch.Order:
                        result = GetOrders(q);
                        break;
                    case eAdminSearch.Customer:
                        result = GetCustomers(q);
                        break;
                }
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(result);
            context.Response.End();
        }

        private string GetProducts(string q)
        {
            string result = string.Empty;

            //var productIds = AdvantShop.FullSearch.LuceneSearch.Search(q).AggregateString('/');

            var resProduct = ProductSeacher.Search(q);
            var productIds = resProduct.SearchResultItems.Take(10).Aggregate("", (current, item) => current + (item.Id + "/"));

            var productNames = ProductService.GetForAutoCompleteByIdsInAdmin(productIds);

            if (productNames.Count != 0)
            {
                productNames = productNames.Distinct().ToList();

                for (int i = 0; i < productNames.Count; i++)
                {
                    result += (productNames[i] + "\n");
                }
            }

            return result;
        }

        private string GetOrders(string q)
        {

            var result = new StringBuilder();
            var orders = AdvantShop.Orders.OrderService.GetOrdersForAutocomplete(q);

            for (int i = 0; i < orders.Count; i++)
            {
                result.AppendFormat("<a href=\"vieworder.aspx?orderid={0}\" data-orderid=\"{0}\">", orders[i].OrderID);

                result.Append("№" + orders[i].Number + " - ");

                if (orders[i].LastName.IsNotEmpty())
                {
                    result.Append(" " + orders[i].LastName);
                }

                if (orders[i].FirstName.IsNotEmpty())
                {
                    result.Append(" " + orders[i].FirstName);
                }

                if (orders[i].Email.IsNotEmpty())
                {
                    result.Append(", " + orders[i].Email);
                }

                if (orders[i].MobilePhone.IsNotEmpty())
                {
                    result.Append(", " + orders[i].MobilePhone);
                }

                result.Append("</a>");
                result.Append("\n");
            }

            return result.ToString();
        }

        private string GetCustomers(string q)
        {
            var result = new StringBuilder();

            foreach (var module in AttachedModules.GetModules<IAdminSearch>())
            {
                var instance = (IAdminSearch)Activator.CreateInstance(module);
                var customersByModule = instance.SearchCustomers(q);
                if (customersByModule != null)
                {
                    foreach (var c in customersByModule)
                    {
                        result.Append(c);
                    }
                }
            }

            var customersByClientCode = ClientCodeService.SearchCustomers(q);
            if (customersByClientCode != null)
            {
                foreach (var c in customersByClientCode)
                    result.Append(c);
            }

            var customers = CustomerService.GetCustomersForAutocomplete(q);
            foreach (var customer in customers)
            {
                result.AppendFormat("<a href=\"ViewCustomer.aspx?CustomerID={0}\">", customer.Id);

                if (customer.LastName.IsNotEmpty())
                    result.Append(customer.LastName);

                if (customer.FirstName.IsNotEmpty())
                    result.Append(" " + customer.FirstName);


                if (customer.Phone.IsNotEmpty())
                    result.Append(", " + customer.Phone);

                  if (customer.EMail.IsNotEmpty())
                    result.Append(", " + customer.EMail);

                result.Append("</a>\n");
            }

            return result.ToString();
        }

    }
}