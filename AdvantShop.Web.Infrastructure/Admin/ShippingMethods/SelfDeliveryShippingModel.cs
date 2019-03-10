using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping;
using AdvantShop.Shipping.SelfDelivery;

namespace AdvantShop.Web.Infrastructure.Admin.ShippingMethods
{
    public class SelfDeliveryShippingAdminModel : ShippingMethodAdminModel, IValidatableObject
    {
        public string ShippingPrice
        {
            get { return Params.ElementOrDefault(SelfDeliveryTemplate.ShippingPrice, "0"); }
            set { Params.TryAddValue(SelfDeliveryTemplate.ShippingPrice, value.DefaultOrEmpty()); }
        }

        public string DeliveryTime
        {
            get { return Params.ElementOrDefault(SelfDeliveryTemplate.DeliveryTime); }
            set { Params.TryAddValue(SelfDeliveryTemplate.DeliveryTime, value.DefaultOrEmpty()); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ShippingPrice))
            {
                yield return new ValidationResult("Укажите стоимость доставки", new[] { "ShippingPrice" });
            }
            else if (!ShippingPrice.IsDecimal())
            {
                yield return new ValidationResult("Стоимость доставки дожна быть числом", new[] { "ShippingPrice" });
            }
        }
    }
}
