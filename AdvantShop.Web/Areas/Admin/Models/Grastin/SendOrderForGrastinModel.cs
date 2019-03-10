using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AdvantShop.Core.Services.Shipping.Grastin.Api;
using AdvantShop.Helpers;

namespace AdvantShop.Web.Admin.Models.Grastin
{
    public class SendOrderForGrastinModel : IValidatableObject
    {
        public string OrderPrefix { get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string Comment { get; set; }
        public string Buyer { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string AddressCourier { get; set; }
        public string AddressPoint { get; set; }
        public List<SelectListItem> AddressPoints { get; set; }
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
        public string DeiveryTimeFrom { get; set; }
        public string DeiveryTimeTo { get; set; }
        public List<SelectListItem> Times { get; set; }
        public EnCourierService? Service { get; set; }
        public List<SelectListItem> Services { get; set; }
        public int Seats { get; set; }
        public string OfficeId { get; set; }
        public List<SelectListItem> Offices { get; set; }
        public EnTypeRecipient? TypeRecipient { get; set; }
        public List<SelectListItem> TypeRecipients { get; set; }
        public string Index { get; set; }
        public string Passport { get; set; }
        public string Organization { get; set; }
        public string Inn { get; set; }
        public string Kpp { get; set; }

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

            if (Service != EnCourierService.TransportCompany &&
                (string.IsNullOrWhiteSpace(DeiveryTimeFrom) || string.IsNullOrWhiteSpace(DeiveryTimeTo)))
                yield return new ValidationResult("Укажите время доставки");

            if (AssessedCost <= 0f)
                yield return new ValidationResult("Укажите оценочную стоимость");

            if (Seats <= 0f)
                yield return new ValidationResult("Укажите количество мест");

            if (Service == EnCourierService.TransportCompany)
            {
                if (string.IsNullOrWhiteSpace(Index))
                    yield return new ValidationResult("Укажите индекс");

                if (string.IsNullOrWhiteSpace(OfficeId))
                    yield return new ValidationResult("Укажите офис транспортной компании");

                if (!TypeRecipient.HasValue)
                    yield return new ValidationResult("Укажите вид получателя");

                if (TypeRecipient == EnTypeRecipient.Individual && string.IsNullOrWhiteSpace(Passport))
                    yield return new ValidationResult("Укажите паспортные данные");

                if (TypeRecipient == EnTypeRecipient.Organization && string.IsNullOrWhiteSpace(Organization))
                    yield return new ValidationResult("Укажите организацию покупателя");

                if (TypeRecipient == EnTypeRecipient.Organization && string.IsNullOrWhiteSpace(Inn))
                    yield return new ValidationResult("Укажите ИНН");

                if (TypeRecipient == EnTypeRecipient.Organization && string.IsNullOrWhiteSpace(Kpp))
                    yield return new ValidationResult("Укажите КПП");
            }

            if (Service == EnCourierService.DeliverWithoutPayment
                || Service == EnCourierService.DeliveryWithPayment
                || Service == EnCourierService.DeliveryWithCashServices
                || Service == EnCourierService.ExchangeReturnOfGoods
                || Service == EnCourierService.GreatDeliveryWithoutPayment
                || Service == EnCourierService.GreatDeliveryWithPayment
                || Service == EnCourierService.GreatDeliveryWithCashServices
                || Service == EnCourierService.TransportCompany
                || Service == EnCourierService.DeliverWithCreditCard)
            {
                if (string.IsNullOrWhiteSpace(AddressCourier))
                    yield return new ValidationResult("Укажите адрес");
            }

            if (Service == EnCourierService.PickupWithoutPaying
                || Service == EnCourierService.PickupWithPayment
                || Service == EnCourierService.PickupWithCashServices
                || Service == EnCourierService.PickupWithCreditCard
                || Service == EnCourierService.ExchangeReturnOfGoodsOnPickup)
            {
                if (string.IsNullOrWhiteSpace(AddressPoint))
                    yield return new ValidationResult("Укажите адрес точки самовывоза");
            }

        }
    }
}
