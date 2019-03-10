using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class PayPalExpressCheckoutPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string User
        {
            get { return Parameters.ElementOrDefault(PayPalExpressCheckoutTemplate.User); }
            set { Parameters.TryAddValue(PayPalExpressCheckoutTemplate.User, value.DefaultOrEmpty()); }
        }

        public string Password
        {
            get { return Parameters.ElementOrDefault(PayPalExpressCheckoutTemplate.Password); }
            set { Parameters.TryAddValue(PayPalExpressCheckoutTemplate.Password, value.DefaultOrEmpty()); }
        }

        public string Signature
        {
            get { return Parameters.ElementOrDefault(PayPalExpressCheckoutTemplate.Signature); }
            set { Parameters.TryAddValue(PayPalExpressCheckoutTemplate.Signature, value.DefaultOrEmpty()); }
        }

        public string CurrencyCode
        {
            get { return Parameters.ElementOrDefault(PayPalExpressCheckoutTemplate.CurrencyCode); }
            set { Parameters.TryAddValue(PayPalExpressCheckoutTemplate.CurrencyCode, value.DefaultOrEmpty()); }
        }

        public string CurrencyValue
        {
            get { return Parameters.ElementOrDefault(PayPalExpressCheckoutTemplate.CurrencyValue); }
            set { Parameters.TryAddValue(PayPalExpressCheckoutTemplate.CurrencyValue, value.TryParseFloat().ToString()); }
        }

        public List<SelectListItem> CurrenciesList
        {
            get
            {
                var currencies =
                    PayPalExpressCheckout.AvaliableCurrs.Select(x => new SelectListItem() {Text = x, Value = x})
                        .ToList();

                var currency = currencies.Find(x => x.Value == CurrencyCode);
                if (currency != null)
                {
                    currency.Selected = true;
                }
                else
                {
                    currencies[0].Selected = true;
                    CurrencyCode = currencies[0].Value;
                }

                return currencies;
            }
        }
        
        public bool Sandbox
        {
            get { return Parameters.ElementOrDefault(PayPalExpressCheckoutTemplate.Sandbox).TryParseBool(); }
            set { Parameters.TryAddValue(PayPalExpressCheckoutTemplate.Sandbox, value.ToString()); }
        }
        
        public override Tuple<string, string> Instruction
        {
            get { return new Tuple<string, string>("http://www.advantshop.net/help/pages/paypal", "Инструкция. Подключение платежного модуля \"Pay Pal Express Checkout\""); }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(User) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Signature) ||
                string.IsNullOrWhiteSpace(CurrencyCode))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
