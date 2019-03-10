using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Scheduler;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Mails;
using AdvantShop.Orders;
using Quartz;

namespace AdvantShop.Module.AbandonedCarts.Domain
{
    [DisallowConcurrentExecution]
    public class AbandonedCartsJob : IJob
    {
        private List<AbandonedCartLetter> _letters = new List<AbandonedCartLetter>(); 

        public void Execute(IJobExecutionContext context)
        {
            if (!context.CanStart())
            {
                Debug.Log.Info("AbandonedCartsJob !CanStart");
                return;
            }
            context.WriteLastRun();

            Debug.Log.Info("AbandonedCartsJob start");

            AbandonedCartsService.DeleteExpiredLetters();
            
            var templates = AbandonedCartsService.GetTemplates().Where(x => x.Active).ToList();

            if (!templates.Any())
                return;

            var cartsUnReg =
                AbandonedCartsService.GetAbondonedCartsUnReg()
                    .Where(
                        x =>
                            x.CheckoutData != null && x.CheckoutData.User != null &&
                            x.CheckoutData.User.Email.IsNotEmpty())
                    .ToList();

            var cartsWishUnReg =
                AbandonedCartsService.GetAbondonedCartsUnReg(ShoppingCartType.Wishlist)
                    .Where(
                        x =>
                            x.CheckoutData != null && x.CheckoutData.User != null &&
                            x.CheckoutData.User.Email.IsNotEmpty())
                    .ToList();

            var cartsReg = AbandonedCartsService.GetAbondonedCartsReg();

            var cartsWishReg = AbandonedCartsService.GetAbondonedCartsReg(ShoppingCartType.Wishlist);

            _letters = AbandonedCartsService.GetAllLetters();
            
            foreach (var template in templates)
            {
                var date = DateTime.Now.AddHours(-template.SendingTime);
                var upDate = DateTime.Now.AddDays(-5).AddHours(-template.SendingTime);

                
                foreach (var cart in cartsUnReg.Where(c => c.LastUpdate < date && c.LastUpdate > upDate &&
                                                        _letters.Find(x => x.TemplateId == template.Id && x.CustomerId == c.CustomerId) == null))
                {
                    try
                    {
                        var templ = template.DeepClone();
                        SendUnRegUsers(cart, templ);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }
                }

                foreach (var cart in cartsWishUnReg.Where(c => c.LastUpdate < date && c.LastUpdate > upDate &&
                                                        _letters.Find(x => x.TemplateId == template.Id && x.CustomerId == c.CustomerId) == null))
                {
                    try
                    {
                        var templ = template.DeepClone();
                        SendUnRegUsers(cart, templ, ShoppingCartType.Wishlist);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }
                }

                foreach (var cart in cartsReg.Where(c => c.LastUpdate < date && c.LastUpdate > upDate && 
                                                            _letters.Find(x => x.TemplateId == template.Id && x.CustomerId == c.CustomerId) == null))
                {
                    try
                    {
                        var templ = template.DeepClone();
                        SendRegUsers(cart, templ);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }
                }

                foreach (var cart in cartsWishReg.Where(c => c.LastUpdate < date && c.LastUpdate > upDate &&
                                                            _letters.Find(x => x.TemplateId == template.Id && x.CustomerId == c.CustomerId) == null))
                {
                    try
                    {
                        var templ = template.DeepClone();
                        SendRegUsers(cart, templ, ShoppingCartType.Wishlist);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(ex);
                    }
                }
            }

            Debug.Log.Info("AbandonedCartsJob done");
        }

        private void SendUnRegUsers(AbandonedCart cart, AbandonedCartTemplate template, ShoppingCartType cartType = ShoppingCartType.ShoppingCart)
        {
            var user = cart.CheckoutData.User;
            var customer = AbandonedCartsService.GetCustomer(user);

            if (_letters.Find(x => x.TemplateId == template.Id && x.Email == customer.EMail) != null || customer.IsAdmin)
                return;

            var shoppingCart = ShoppingCartService.GetShoppingCart(cartType, cart.CustomerId);

            template.Register(customer, shoppingCart);
            template.BuildMail();

            SendMail.SendMailNow(cart.CustomerId, customer.EMail, template.Subject, template.Body, true);

            var letter = new AbandonedCartLetter()
            {
                CustomerId = cart.CustomerId,
                TemplateId = template.Id,
                Email = customer.EMail,
                SendingDate = DateTime.Now
            };
            _letters.Add(letter);
            AbandonedCartsService.LogLetter(letter);
        }

        private void SendRegUsers(AbandonedCart cart, AbandonedCartTemplate template, ShoppingCartType cartType = ShoppingCartType.ShoppingCart)
        {
            var customer = CustomerService.GetCustomer(cart.CustomerId);
            if (customer == null || customer.IsAdmin || _letters.Find(x => x.TemplateId == template.Id && x.Email == customer.EMail) != null)
                return;

            var shoppingCart = ShoppingCartService.GetShoppingCart(cartType, cart.CustomerId);

            template.Register(customer, shoppingCart);
            template.BuildMail();

            SendMail.SendMailNow(customer.Id, customer.EMail, template.Subject, template.Body, true);

            var letter = new AbandonedCartLetter()
            {
                CustomerId = cart.CustomerId,
                TemplateId = template.Id,
                Email = customer.EMail,
                SendingDate = DateTime.Now
            };
            _letters.Add(letter);
            AbandonedCartsService.LogLetter(letter);
        }
    }
}