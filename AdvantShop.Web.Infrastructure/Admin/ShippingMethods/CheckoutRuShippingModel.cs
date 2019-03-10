using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping;
using AdvantShop.Shipping.CheckoutRu;

namespace AdvantShop.Web.Infrastructure.Admin.ShippingMethods
{
    public class CheckoutRuShippingAdminModel : ShippingMethodAdminModel, IValidatableObject
    {
        public string ClientId
        {
            get { return Params.ElementOrDefault(CheckoutRuTemplate.ClientId); }
            set { Params.TryAddValue(CheckoutRuTemplate.ClientId, value.DefaultOrEmpty()); }
        }

        public bool Grouping
        {
            get { return Params.ElementOrDefault(CheckoutRuTemplate.Grouping).TryParseBool(); }
            set { Params.TryAddValue(CheckoutRuTemplate.Grouping, value.ToString()); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ClientId))
            {
                yield return new ValidationResult("Идентификационный ключ клиента", new[] { "ClientId" });
            }
        }
    }
}
