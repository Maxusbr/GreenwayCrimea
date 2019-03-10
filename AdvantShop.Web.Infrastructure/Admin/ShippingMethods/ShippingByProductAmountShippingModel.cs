using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping;
using AdvantShop.Shipping.ShippingByOrderPrice;
using AdvantShop.Shipping.ShippingByProductAmount;

namespace AdvantShop.Web.Infrastructure.Admin.ShippingMethods
{
    public class ShippingByProductAmountShippingAdminModel : ShippingMethodAdminModel, IValidatableObject
    {
        /// <summary>
        /// Example: 1500=100;3000=0;
        /// </summary>
        public string PriceRanges
        {
            get { return Params.ElementOrDefault(ShippingByProductAmountTemplate.PriceRanges); }
            set { Params.TryAddValue(ShippingByProductAmountTemplate.PriceRanges, value.DefaultOrEmpty()); }
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
