using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class LiqPayPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string MerchantId
        {
            get { return Parameters.ElementOrDefault(LiqPayTemplate.MerchantId); }
            set { Parameters.TryAddValue(LiqPayTemplate.MerchantId, value.DefaultOrEmpty()); }
        }

        public string MerchantSig
        {
            get { return Parameters.ElementOrDefault(LiqPayTemplate.MerchantSig); }
            set { Parameters.TryAddValue(LiqPayTemplate.MerchantSig, value.DefaultOrEmpty()); }
        }

        public string MerchantIso
        {
            get { return Parameters.ElementOrDefault(LiqPayTemplate.MerchantISO); }
            set { Parameters.TryAddValue(LiqPayTemplate.MerchantISO, value.DefaultOrEmpty()); }
        }

        public List<SelectListItem> MerchantCurrencies
        {
            get
            {
                var currencies = new List<SelectListItem>()
                {
                    new SelectListItem() {Text = "Рубли", Value = "RUR"},
                    new SelectListItem() {Text = "Гривны", Value = "UAH"},
                    new SelectListItem() {Text = "Евро", Value = "EUR"},
                    new SelectListItem() {Text = "Доллар", Value = "USD"},
                };

                var currency = currencies.Find(x => x.Value == MerchantIso);
                if (currency != null)
                    currency.Selected = true;
                else
                {
                    MerchantIso = currencies[0].Value;
                    currencies[0].Selected = true;
                }

                return currencies;
            }
        }
        
        public override Tuple<string, string> Instruction
        {
            get { return new Tuple<string, string>("http://www.advantshop.net/help/pages/connect-liqpay", "Инструкция. Подключение платежного модуля LiqPay"); }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(MerchantId) ||
                string.IsNullOrWhiteSpace(MerchantSig) ||
                string.IsNullOrWhiteSpace(MerchantIso))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
