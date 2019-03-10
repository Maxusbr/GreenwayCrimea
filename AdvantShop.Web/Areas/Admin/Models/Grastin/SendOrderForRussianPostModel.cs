using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AdvantShop.Core.Services.Shipping.Grastin.Api;
using AdvantShop.Helpers;

namespace AdvantShop.Web.Admin.Models.Grastin
{
    public class SendOrderForRussianPostModel : IValidatableObject
    {
        public string OrderPrefix { get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string Comment { get; set; }
        public string Buyer { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Index { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public float AssessedCost { get; set; }
        public bool CashOnDelivery { get; set; }
        public string TakeWarehouse { get; set; }
        public List<SelectListItem> TakeWarehouses { get; set; }
        public string CargoType { get; set; }
        public string BarCode { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string DeliveryDateString {
            get { return DeliveryDate.HasValue ? DeliveryDate.Value.ToString("yyyy-MM-dd") : string.Empty; }
        }
        public EnRussianPostService? Service { get; set; }
        public List<SelectListItem> Services { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(TakeWarehouse))
                yield return new ValidationResult("Укажите склад приемки");

            if (string.IsNullOrWhiteSpace(Buyer))
                yield return new ValidationResult("Укажите ФИО");

            if (string.IsNullOrWhiteSpace(Phone))
                yield return new ValidationResult("Укажите мобильный телефон");

            if (string.IsNullOrWhiteSpace(Email) || !ValidationHelper.IsValidEmail(Email))
                yield return new ValidationResult("Укажите email");

            if (!Service.HasValue)
                yield return new ValidationResult("Укажите тип доставки");

            if (!DeliveryDate.HasValue)
                yield return new ValidationResult("Укажите дату доставки");

            if (AssessedCost <= 0f)
                yield return new ValidationResult("Укажите оценочную стоимость");

            if (string.IsNullOrWhiteSpace(Index))
                yield return new ValidationResult("Укажите индекс");

            if (string.IsNullOrWhiteSpace(Region))
                yield return new ValidationResult("Укажите регион");

            if (string.IsNullOrWhiteSpace(City))
                yield return new ValidationResult("Укажите город");

            if (string.IsNullOrWhiteSpace(Address))
                yield return new ValidationResult("Укажите адрес");
        }
    }
}
