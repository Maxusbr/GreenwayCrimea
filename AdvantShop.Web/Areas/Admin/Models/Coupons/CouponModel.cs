using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Helpers;
using AdvantShop.Localization;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Coupons
{
    public class CouponsFilterModel : BaseFilterModel
    {
        public string Code { get; set; }
        public CouponType? Type { get; set; }
        public string Value { get; set; }
        public string AddingDateFrom { get; set; }
        public string AddingDateTo { get; set; }
        public string ExpirationDateFrom { get; set; }
        public string ExpirationDateTo { get; set; }
        public bool? Enabled { get; set; }
        public string MinimalOrderPrice { get; set; }
    }

    public class CouponModel : IValidatableObject
    {
        public int CouponId { get; set; }
        public string Code { get; set; }
        public CouponType Type { get; set; }

        public string TypeFormatted
        {
            get { return Type.Localize(); }
        }

        public float Value { get; set; }
        public string CurrencyIso3 { get; set; }
        public DateTime AddingDate { get; set; }

        public string AddingDateFormatted
        {
            get { return Culture.ConvertDate(AddingDate); }
        }

        public DateTime? ExpirationDate { get; set; }
        public string ExpirationDateFormatted
        {
            get
            {
                return ExpirationDate != null
                    ? Culture.ConvertDate(ExpirationDate.Value)
                    : LocalizationService.GetResource("Admin.Coupons.NoDate");
            }
        }

        public int PossibleUses { get; set; }
        public int ActualUses { get; set; }
        public bool Enabled { get; set; }
        public float MinimalOrderPrice { get; set; }

        public List<int> CategoryIds { get; set; }
        public List<int> ProductsIds { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Code))
                yield return new ValidationResult("Неверный код купона");

            if (CouponId <= 0)
            {
                var coupon = CouponService.GetCouponByCode(Code);
                if (coupon != null)
                    yield return new ValidationResult("Код купона уже занят");
            }

            if (Value <= 0)
                yield return new ValidationResult("Неверное значение купона");

            if (Type == CouponType.Percent && Value > 100)
                yield return new ValidationResult("Значение купона не может быть больше 100");

        }
    }
}
