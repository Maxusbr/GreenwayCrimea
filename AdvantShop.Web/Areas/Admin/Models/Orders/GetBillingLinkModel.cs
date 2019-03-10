using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Configuration;
using AdvantShop.Orders;

namespace AdvantShop.Web.Admin.Models.Orders
{
    public class GetBillingLinkModel : IValidatableObject
    {
        public int OrderId { get; set; }

        public Order Order { get; private set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Order = OrderService.GetOrder(OrderId);
            if (Order == null)
                yield return new ValidationResult("Заказ не найден");

            if (SettingsCheckout.ManagerConfirmed && Order != null && !Order.ManagerConfirmed)
                yield return new ValidationResult("Заказ не подтвержден, оплата запрещена");
        }
    }

    public class GetBillingLinkMailModel : GetBillingLinkModel
    {
        public new IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext))
                yield return result;

            if (Order.OrderCustomer == null || string.IsNullOrWhiteSpace(Order.OrderCustomer.Email))
                yield return new ValidationResult("Укажите email покупателя");
        }
    }

    public class SendBillingLinkMailModel : GetBillingLinkMailModel
    {
        public string Subject { get; set; }
        public string Text { get; set; }

        public new IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var result in base.Validate(validationContext))
                yield return result;
            
            if (string.IsNullOrWhiteSpace(Subject) || string.IsNullOrWhiteSpace(Text))
                yield return new ValidationResult("Укажите заголовок и текст письма");
        }
    }
}
