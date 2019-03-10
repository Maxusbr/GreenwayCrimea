using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Shipping;
using AdvantShop.Shipping.Sdek;

namespace AdvantShop.Web.Infrastructure.Admin.ShippingMethods
{
    public class SdekCallCourierModel : IValidatableObject
    {
        public int MethodId { get; set; }

        public string Date { get; set; }
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string DefaultCourierCity { get; set; }
        public string DefaultCourierStreet { get; set; }
        public string DefaultCourierHouse { get; set; }
        public string DefaultCourierFlat { get; set; }
        public string DefaultCourierNameContact { get; set; }
        public string DefaultCourierPhone { get; set; }
        public float Weight { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Date) ||
                string.IsNullOrWhiteSpace(TimeFrom) ||
                string.IsNullOrWhiteSpace(TimeTo) ||
                string.IsNullOrWhiteSpace(DefaultCourierCity) ||
                string.IsNullOrWhiteSpace(DefaultCourierStreet) ||
                string.IsNullOrWhiteSpace(DefaultCourierHouse) ||
                string.IsNullOrWhiteSpace(DefaultCourierFlat) ||
                string.IsNullOrWhiteSpace(DefaultCourierNameContact) ||
                string.IsNullOrWhiteSpace(DefaultCourierPhone))
            {
                yield return new ValidationResult("Заполните обязательные все поля: Дата и время вызова курьера, Город, Улица, Дом, Квартира, Имя контактного лица, Телефон контактного лица");
            }

            if (Weight <= 0)
            {
                yield return new ValidationResult("Вес заказа должен быть больше 0");
            }
            
            var method = ShippingMethodService.GetShippingMethod(MethodId);
            if (method == null || method.ShippingType != "Sdek")
            {
                yield return new ValidationResult("Не удалось получить метод доставки");
            }

            var cityIdSkek = SdekService.GetSdekCityId(DefaultCourierCity, string.Empty);
            if (cityIdSkek == 0)
            {
                yield return new ValidationResult("В системе СДЭК отсутствует данный город, проверьте написание");
            }


            var timeBegin = TimeFrom.Split(new[] { ':' });
            if (timeBegin.Length != 2)
            {
                yield return new ValidationResult("Неверно указано начальное время ожидания");
            }

            var timeEnd = TimeTo.Split(new[] { ':' });
            if (timeEnd.Length != 2)
            {
                yield return new ValidationResult("Неверно указано конечное время ожидания");
            }
            
            var resultTime = Math.Abs(timeBegin[0].TryParseInt() - timeEnd[0].TryParseInt());
            if (resultTime < 3)
            {
                yield return new ValidationResult("Минимальный промежуток ожидания 3 часа");
            }
        }
    }
}
