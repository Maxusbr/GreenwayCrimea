using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Orders;

namespace AdvantShop.Web.Admin.Models.OrderSources
{
    public partial class OrderSourceModel : IValidatableObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }

        public bool Main { get; set; }

        public OrderType Type { get; set; }

        public string TypeFormatted { get { return Type.Localize(); } }

        public int OrdersCount { get; set; }

        public int LeadsCount { get; set; }

        public bool CanBeDeleted
        {
            get { return !(OrdersCount > 0 || LeadsCount > 0); }
        }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult("Укажите название", new[] { "Name" });
            }
        }
    }
}
