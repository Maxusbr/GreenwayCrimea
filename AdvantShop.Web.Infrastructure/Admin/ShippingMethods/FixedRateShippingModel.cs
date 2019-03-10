using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping;
using AdvantShop.Shipping.FixedRate;

namespace AdvantShop.Web.Infrastructure.Admin.ShippingMethods
{
    public class FixedRateShippingAdminModel : ShippingMethodAdminModel, IValidatableObject
    {
        public string ShippingPrice
        {
            get { return Params.ElementOrDefault(FixeRateShippingTemplate.ShippingPrice, "0"); }
            set { Params.TryAddValue(FixeRateShippingTemplate.ShippingPrice, value.TryParseFloat().ToString()); }
        }

        public string DeliveryTime
        {
            get { return Params.ElementOrDefault(FixeRateShippingTemplate.DeliveryTime); }
            set { Params.TryAddValue(FixeRateShippingTemplate.DeliveryTime, value.DefaultOrEmpty()); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ShippingPrice))
                yield return new ValidationResult("Укажите стоимость доставки");
            
            if (!ShippingPrice.IsDecimal())
                yield return new ValidationResult("Стоимость доставки дожна быть числом");
        }
    }
}
