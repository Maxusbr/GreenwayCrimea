using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class IntellectMoneyMainProtocolPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string EshopId
        {
            get { return Parameters.ElementOrDefault(IntellectMoneyMainProtocolTemplate.EshopId); }
            set { Parameters.TryAddValue(IntellectMoneyMainProtocolTemplate.EshopId, value.DefaultOrEmpty()); }
        }

        public string SecretKey
        {
            get { return Parameters.ElementOrDefault(IntellectMoneyMainProtocolTemplate.SecretKey); }
            set { Parameters.TryAddValue(IntellectMoneyMainProtocolTemplate.SecretKey, value.DefaultOrEmpty()); }
        }

        public string Preference
        {
            get { return Parameters.ElementOrDefault(IntellectMoneyMainProtocolTemplate.Preference); }
            set { Parameters.TryAddValue(IntellectMoneyMainProtocolTemplate.Preference, value.DefaultOrEmpty()); }
        }

        public List<SelectListItem> Payments
        {
            get
            {
                var payments =
                    IntellectMoneyMainProtocol.PaymentSystems.Select(x => new SelectListItem() {Text = x.Value, Value = x.Key}).ToList();
                
                var payment = payments.Find(x => x.Value == Preference);
                if (payment != null)
                    payment.Selected = true;

                return payments;
            }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(EshopId) ||
                string.IsNullOrWhiteSpace(SecretKey))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
