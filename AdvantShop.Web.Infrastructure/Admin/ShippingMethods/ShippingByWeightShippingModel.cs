using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping;
using AdvantShop.Shipping.ShippingByWeight;

namespace AdvantShop.Web.Infrastructure.Admin.ShippingMethods
{
    public class ShippingByWeightShippingAdminModel : ShippingMethodAdminModel, IValidatableObject
    {
        private string _pricePerKg;
        public string PricePerKg
        {
            get { return Params.ElementOrDefault(ShippingByWeightTemplate.PricePerKg, "0"); }
            set
            {
                _pricePerKg = value;
                Params.TryAddValue(ShippingByWeightTemplate.PricePerKg, value.TryParseFloat().ToString());
            }
        }

        private string _extracharge;
        public string Extracharge
        {
            get { return Params.ElementOrDefault(ShippingByWeightTemplate.Extracharge, "0"); }
            set
            {
                _extracharge = value;
                Params.TryAddValue(ShippingByWeightTemplate.Extracharge, value.TryParseFloat().ToString());
            }
        }

        public string DeliveryTime
        {
            get { return Params.ElementOrDefault(ShippingByWeightTemplate.DeliveryTime); }
            set { Params.TryAddValue(ShippingByWeightTemplate.DeliveryTime, value.DefaultOrEmpty()); }
        }

        public string DefaultWeight
        {
            get { return Params.ElementOrDefault(ShippingByWeightTemplate.DefaultWeight, "1"); }
            set { Params.TryAddValue(ShippingByWeightTemplate.DefaultWeight, value.TryParseFloat().ToString()); }
        }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(_pricePerKg) || !_pricePerKg.IsDecimal())
            {
                yield return new ValidationResult("Укажите цену за килограмм", new[] {"PricePerKg"});
            }

            if (string.IsNullOrWhiteSpace(_extracharge) || !_extracharge.IsDecimal())
            {
                yield return new ValidationResult("Укажите наценку в базовой валюте", new[] {"Extracharge"});
            }
        }
    }
}
