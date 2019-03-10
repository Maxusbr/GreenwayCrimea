using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdvantShop.Areas.Api.Models.Leads
{
    public class AddLeadModel : IValidatableObject
    {
        /// <summary>
        /// Описание лида
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Сумма сделки. Если есть товары, то берется по ним.
        /// </summary>
        public float Sum { get; set; }

        /// <summary>
        /// Имя контакта. Обязательное поле
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия контакта
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество контакта
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// Телефон контаутв
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Почта контакта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Id контакта
        /// </summary>
        public Guid? CustomerId { get; set; }


        /// <summary>
        /// Скидка числом
        /// </summary>
        public float DiscountValue { get; set; }

        /// <summary>
        /// Скидка процент. Если DiscountValue != 0, то игнорируется
        /// </summary>
        public float DiscountPercent { get; set; }


        /// <summary>
        /// Список товаров
        /// </summary>
        public List<AddLeadProductModel> Products { get; set; }

        /// <summary>
        /// Источник заказа
        /// </summary>
        public string Source { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(Phone))
            {
                yield return new ValidationResult("Заполните обязательное поле (имя, email или телефон)");
            }
        }
    }

    public class AddLeadProductModel
    {
        /// <summary>
        /// Артикул. Обязательное поле
        /// </summary>
        public string ArtNo { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }
        public float Price { get; set; }
    }
}