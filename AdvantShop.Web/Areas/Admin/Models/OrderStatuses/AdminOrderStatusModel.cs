using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Orders;

namespace AdvantShop.Web.Admin.Models.OrderStatuses
{
    public partial class OrderStatusModel : IValidatableObject
    {
        public int OrderStatusId { get; set; }

        public string StatusName { get; set; }

        public bool IsDefault { get; set; }

        public bool IsCanceled { get; set; }

        public bool IsCompleted { get; set; }

        public int CommandId { get; set; }

        public string CommandFormatted
        {
            get
            {
                var c = (OrderStatusCommand) CommandId;
                return c.Localize();
            }
        }

        public OrderStatusCommand Command
        {
            get { return (OrderStatusCommand) CommandId; }
        }

        public string Color { get; set; }

        public bool Hidden { get; set; }

        public int SortOrder { get; set; }

        public bool CanBeDeleted
        {
            get { return OrderStatusService.StatusCanBeDeleted(OrderStatusId); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(StatusName))
            {
                yield return new ValidationResult("Укажите название", new[] { "Name" });
            }
        }
    }
}
