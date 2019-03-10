using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class PayOnlinePaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string MerchantId
        {
            get { return Parameters.ElementOrDefault(PayOnlineTemplate.MerchantId); }
            set { Parameters.TryAddValue(PayOnlineTemplate.MerchantId, value.DefaultOrEmpty()); }
        }

        public string SecretKey
        {
            get { return Parameters.ElementOrDefault(PayOnlineTemplate.SecretKey); }
            set { Parameters.TryAddValue(PayOnlineTemplate.SecretKey, value.DefaultOrEmpty()); }
        }

        public string Currency
        {
            get { return Parameters.ElementOrDefault(PayOnlineTemplate.Currency) ?? "RUB"; }
            set { Parameters.TryAddValue(PayOnlineTemplate.Currency, value.DefaultOrEmpty()); }
        }

        public string CurrencyValue
        {
            get { return Parameters.ElementOrDefault(PayOnlineTemplate.CurrencyValue); }
            set { Parameters.TryAddValue(PayOnlineTemplate.CurrencyValue, value.TryParseFloat().ToString()); }
        }

        public string PayType
        {
            get { return Parameters.ElementOrDefault(PayOnlineTemplate.PayType); }
            set { Parameters.TryAddValue(PayOnlineTemplate.PayType, value.DefaultOrEmpty()); }
        }

        public List<SelectListItem> PayTypes
        {
            get
            {
                var types = new List<SelectListItem>()
                {
                    new SelectListItem() {Text = "Select", Value = "0"},
                    new SelectListItem() {Text = "WebMoney", Value = "1"},
                    new SelectListItem() {Text = "QIWI", Value = "2"},
                    new SelectListItem() {Text = "YandexMoney", Value = "3"},
                    new SelectListItem() {Text = "CreditCard_EN", Value = "4"},
                    new SelectListItem() {Text = "CreditCard_RU", Value = "5"},
                };

                var type = types.Find(x => x.Value == PayType);
                if (type != null)
                    type.Selected = true;

                return types;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(MerchantId) ||
                string.IsNullOrWhiteSpace(SecretKey) ||
                string.IsNullOrWhiteSpace(Currency))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
