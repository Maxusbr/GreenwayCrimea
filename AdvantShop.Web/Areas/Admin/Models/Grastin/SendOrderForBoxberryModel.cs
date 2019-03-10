using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Helpers;

namespace AdvantShop.Web.Admin.Models.Grastin
{
    public class SendOrderForBoxberryModel : IValidatableObject
    {
        public string OrderPrefix { get; set; }
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string Comment { get; set; }
        public string Buyer { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string PointId { get; set; }
        public List<SelectListItem> AddressPoints { get; set; }
        public string PostcodeId { get; set; }
        public List<SelectListItem> Postcodes { get; set; }
        public string Address { get; set; }
        public float AssessedCost { get; set; }
        public bool CashOnDelivery { get; set; }
        public float Weight { get; set; }
        public string TakeWarehouse { get; set; }
        public List<SelectListItem> TakeWarehouses { get; set; }
        public string CargoType { get; set; }
        public string BarCode { get; set; }
        public EnBoxberryService? Service { get; set; }
        public List<SelectListItem> Services { get; set; }
        public int Seats { get; set; }

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

            //if (AssessedCost <= 0f)
            //    yield return new ValidationResult("Укажите оценочную стоимость");

            if (Seats <= 0f)
                yield return new ValidationResult("Укажите количество мест");

            if (Service == EnBoxberryService.Courier)
            {
                if (string.IsNullOrWhiteSpace(PostcodeId))
                    yield return new ValidationResult("Укажите индекс города доставки");

                if (string.IsNullOrWhiteSpace(Address))
                    yield return new ValidationResult("Укажите адрес");
            }

            if (Service == EnBoxberryService.PickPoint)
            {
                if (string.IsNullOrWhiteSpace(PointId))
                    yield return new ValidationResult("Укажите точку самовывоза");
            }
        }
    }


    public enum EnBoxberryService
    {
        /// <summary>
        /// Курьером
        /// </summary>
        [Localize("Курьером")]
        Courier = 1,

        /// <summary>
        /// Самовывоз
        /// </summary>
        [Localize("Самовывоз")]
        PickPoint = 2
    }

}
