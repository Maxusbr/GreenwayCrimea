using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping;
using AdvantShop.Shipping.ShippingByShippingCost;

namespace AdvantShop.Web.Infrastructure.Admin.ShippingMethods
{
    public class ShippingByShippingCostShippingAdminModel : ShippingMethodAdminModel, IValidatableObject
    {
        public bool ByMaxShippingCost
        {
            get { return Params.ElementOrDefault(ShippingByShippingCostTemplate.ByMaxShippingCost).TryParseBool(); }
            set { Params.TryAddValue(ShippingByShippingCostTemplate.ByMaxShippingCost, value.ToString()); }
        }

        public bool UseAmount
        {
            get { return Params.ElementOrDefault(ShippingByShippingCostTemplate.UseAmount).TryParseBool(); }
            set { Params.TryAddValue(ShippingByShippingCostTemplate.UseAmount, value.ToString()); }
        }
        

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
