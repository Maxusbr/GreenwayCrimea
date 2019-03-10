using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Payment;
using AdvantShop.Payment;

namespace AdvantShop.Web.Infrastructure.Admin.PaymentMethods
{
    public class CloudPaymentsPaymentModel : PaymentMethodAdminModel, IValidatableObject
    {
        public string PublicId
        {
            get { return Parameters.ElementOrDefault(CloudPaymentsTemplate.PublicId); }
            set { Parameters.TryAddValue(CloudPaymentsTemplate.PublicId, value.DefaultOrEmpty()); }
        }

        public string APISecret
        {
            get { return Parameters.ElementOrDefault(CloudPaymentsTemplate.APISecret); }
            set { Parameters.TryAddValue(CloudPaymentsTemplate.APISecret, value.DefaultOrEmpty()); }
        }

        public string Site
        {
            get { return Parameters.ElementOrDefault(CloudPaymentsTemplate.Site) ?? "cloudpayments.ru"; }
            set { Parameters.TryAddValue(CloudPaymentsTemplate.Site, value.DefaultOrEmpty()); }
        }

        public bool SendReceiptData
        {
            get { return Parameters.ElementOrDefault(CloudPaymentsTemplate.SendReceiptData).TryParseBool(); }
            set { Parameters.TryAddValue(CloudPaymentsTemplate.SendReceiptData, value.ToString()); }
        }

        public string TaxationSystem
        {
            get { return Parameters.ElementOrDefault(CloudPaymentsTemplate.TaxationSystem); }
            set { Parameters.TryAddValue(CloudPaymentsTemplate.TaxationSystem, value.DefaultOrEmpty()); }
        }


        public override Tuple<string, string> Instruction
        {
            get { return new Tuple<string, string>("https://cloudpayments.ru/Docs/Connect", "Инструкция. Подключение к сервису Cloud Payments."); }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(PublicId) ||
                string.IsNullOrWhiteSpace(APISecret))
            {
                yield return new ValidationResult("Заполните обязательные поля");
            }
        }
    }
}
