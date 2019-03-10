using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping;
using AdvantShop.Shipping.ShippingByOrderPrice;

namespace AdvantShop.Web.Infrastructure.Admin.ShippingMethods
{
    public class ShippingByOrderPriceShippingAdminModel : ShippingMethodAdminModel, IValidatableObject
    {
        /// <summary>
        /// Example: 1500=100;3000=0;
        /// </summary>
        public string PriceRanges
        {
            get { return Params.ElementOrDefault(ShippingByOrderPriceTemplate.PriceRanges); }
            set { Params.TryAddValue(ShippingByOrderPriceTemplate.PriceRanges, value.DefaultOrEmpty()); }
        }

        public bool DependsOnCartPrice
        {
            get { return Params.ElementOrDefault(ShippingByOrderPriceTemplate.DependsOnCartPrice).TryParseBool(); }
            set { Params.TryAddValue(ShippingByOrderPriceTemplate.DependsOnCartPrice, value.ToString()); }
        }

        public string DeliveryTime
        {
            get { return Params.ElementOrDefault(ShippingByOrderPriceTemplate.DeliveryTime); }
            set { Params.TryAddValue(ShippingByOrderPriceTemplate.DeliveryTime, value.DefaultOrEmpty()); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(PriceRanges))
                yield return new ValidationResult("Укажите диапазон");
        }
    }
}
