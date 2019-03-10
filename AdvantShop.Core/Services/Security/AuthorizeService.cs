//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web;
using System.Web.Security;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Core.Caching;
using System.Collections.Generic;
using System.Linq;

namespace AdvantShop.Security
{
    public class AuthorizeService
    {
        private const string Spliter = ":";

        public static Customer GetAuthenticatedCustomer()
        {
            if (HttpContext.Current == null) return null;

            var formsCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (formsCookie != null)
            {
                try
                {
                    var formsAuthenticationTicket = FormsAuthentication.Decrypt(formsCookie.Value);
                    if (formsAuthenticationTicket != null)
                    {
                        var token = formsAuthenticationTicket.Name;
                        var words = token.Split(new[] { Spliter }, StringSplitOptions.RemoveEmptyEntries);
                        if (words.Length != 2) return null;
                        var email = words[0];
                        var passHash = words[1];
                        return string.IsNullOrEmpty(email)
                            ? null
                            : CustomerService.GetCustomerByEmailAndPassword(email, passHash, true);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                }
            }
            return null;
        }

        public static bool SignIn(string email, string password, bool isHash, bool createPersistentCookie)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return false;
            
            var isDebug = Secure.IsDebugAccount(email, password);
            if (isDebug)
            {
                //var isDebugList = CacheManager.Get<List<Guid>>(CacheNames.IsDebug) ?? new List<Guid>();

                //var debugSessionId = Guid.NewGuid();
                //if (!isDebugList.Any(item => item == debugSessionId))
                //{
                //    isDebugList.Add(debugSessionId);
                //    Helpers.CommonHelper.SetCookie("isDebug", debugSessionId.ToString(), new TimeSpan(0, 0, 20, 0), true);
                //}
                //CacheManager.Insert<List<Guid>>(CacheNames.IsDebug, isDebugList);
                CustomerContext.IsDebug = true;

                var virtualCustomer = new Customer()
                {
                    CustomerRole = Role.Administrator,
                    IsVirtual = true,
                    Enabled = true
                };
                HttpContext.Current.Items["CustomerContext"] = virtualCustomer;

                Secure.AddUserLog("sa", true, true);
                return true;
            }

            var oldCustomerId = CustomerContext.CurrentCustomer.Id;
            var customer = CustomerService.GetCustomerByEmailAndPassword(email, password, isHash);
            if (customer == null) return false;

            Secure.AddUserLog(customer.EMail, true, customer.IsAdmin);
            ShoppingCartService.MergeShoppingCarts(oldCustomerId, customer.Id);
            CustomerContext.SetCustomerCookie(customer.Id);
            FormsAuthentication.SetAuthCookie(email + Spliter + customer.Password, createPersistentCookie);
            return true;
        }

        public static void SignOut()
        {
            //if (CustomerContext.IsDebug)
            //{
            //    var isdebuglist = cachemanager.get<list<guid>>(cachenames.isdebug) ?? new list<guid>();
            //    var customerid = customercontext.customerid;
            //    if (isdebuglist.any(item => item == customerid))
            //    {
            //        isdebuglist.remove(customerid);
            //    }
            //    cachemanager.remove(cachenames.isdebug);
            //    cachemanager.insert<list<guid>>(cachenames.isdebug, isdebuglist);
            //}
            CustomerContext.IsDebug = false;
            CustomerContext.DeleteCustomerCookie();
            FormsAuthentication.SignOut();
        }

        //private static void AuthIsDebug()
        //{
        //    var isDebugList = CacheManager.Get<List<Guid>>(CacheNames.IsDebug) ?? new List<Guid>();

        //    var debugSessionId = Guid.NewGuid();
        //    if (!isDebugList.Any(item => item == debugSessionId))
        //    {
        //        isDebugList.Add(debugSessionId);
        //        if (Helpers.CommonHelper.GetCookie("isDebug") != null)
        //        {
        //            Helpers.CommonHelper.DeleteCookie("isDebug");
        //        }

        //        Helpers.CommonHelper.SetCookie("isDebug", debugSessionId.ToString(), new TimeSpan(0, 0, 20, 0), true);
        //    }
        //    CacheManager.Insert<List<Guid>>(CacheNames.IsDebug, isDebugList);

        //    var virtualCustomer = new Customer
        //    {
        //        CustomerRole = Role.Administrator,
        //        IsVirtual = true,
        //        Enabled = true
        //    };
        //    HttpContext.Current.Items["CustomerContext"] = virtualCustomer;
        //}
    }
}