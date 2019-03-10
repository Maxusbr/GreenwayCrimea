using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping;
using AdvantShop.Shipping.Sdek;

namespace AdvantShop.Web.Infrastructure.Admin.ShippingMethods
{
    public class SdekShippingAdminModel : ShippingMethodAdminModel, IValidatableObject
    {
        public string AuthLogin
        {
            get { return Params.ElementOrDefault(SdekTemplate.AuthLogin); }
            set { Params.TryAddValue(SdekTemplate.AuthLogin, value.DefaultOrEmpty()); }
        }
        public string AuthPassword
        {
            get { return Params.ElementOrDefault(SdekTemplate.AuthPassword); }
            set { Params.TryAddValue(SdekTemplate.AuthPassword, value.DefaultOrEmpty()); }
        }

        public string CityFrom
        {
            get { return Params.ElementOrDefault(SdekTemplate.CityFrom); }
            set { Params.TryAddValue(SdekTemplate.CityFrom, value.DefaultOrEmpty()); }
        }

        public string AdditionalPrice
        {
            get { return Params.ElementOrDefault(SdekTemplate.AdditionalPrice) ?? "0"; }
            set { Params.TryAddValue(SdekTemplate.AdditionalPrice, value.TryParseFloat().ToString()); }
        }

        public string Tariff
        {
            get { return Params.ElementOrDefault(SdekTemplate.Tariff); }
            set { Params.TryAddValue(SdekTemplate.Tariff, value.DefaultOrEmpty()); }
        }

        public string DeliveryNote
        {
            get { return Params.ElementOrDefault(SdekTemplate.DeliveryNote) ?? "1"; }
            set { Params.TryAddValue(SdekTemplate.DeliveryNote, value.TryParseInt().ToString()); }
        }

        public List<SelectListItem> Tariffs
        {
            get
            {
                var service = new SdekApiService();

                var tariffs = service.Tariffs.Select(x => new SelectListItem() { Text = x.Name, Value = x.TariffId.ToString() }).ToList();

                var selectedTariff = tariffs.Find(x => x.Value == Tariff);
                if (selectedTariff != null)
                {
                    selectedTariff.Selected = true;
                }
                else
                {
                    tariffs[0].Selected = true;
                    Tariff = tariffs[0].Value;
                }

                return tariffs;
            }
        }

        public string DefaultCourierCity
        {
            get { return Params.ElementOrDefault(SdekTemplate.DefaultCourierCity) ?? SettingsMain.City; }
            set { Params.TryAddValue(SdekTemplate.DefaultCourierCity, value.DefaultOrEmpty()); }
        }

        public string DefaultCourierStreet
        {
            get { return Params.ElementOrDefault(SdekTemplate.DefaultCourierStreet); }
            set { Params.TryAddValue(SdekTemplate.DefaultCourierStreet, value.DefaultOrEmpty()); }
        }

        public string DefaultCourierHouse
        {
            get { return Params.ElementOrDefault(SdekTemplate.DefaultCourierHouse); }
            set { Params.TryAddValue(SdekTemplate.DefaultCourierHouse, value.DefaultOrEmpty()); }
        }

        public string DefaultCourierFlat
        {
            get { return Params.ElementOrDefault(SdekTemplate.DefaultCourierFlat); }
            set { Params.TryAddValue(SdekTemplate.DefaultCourierFlat, value.DefaultOrEmpty()); }
        }

        public string DefaultCourierNameContact
        {
            get { return Params.ElementOrDefault(SdekTemplate.DefaultCourierNameContact); }
            set { Params.TryAddValue(SdekTemplate.DefaultCourierNameContact, value.DefaultOrEmpty()); }
        }

        public string DefaultCourierPhone
        {
            get { return Params.ElementOrDefault(SdekTemplate.DefaultCourierPhone); }
            set { Params.TryAddValue(SdekTemplate.DefaultCourierPhone, value.DefaultOrEmpty()); }
        }

        public string DefaultWeight
        {
            get { return Params.ElementOrDefault(SdekTemplate.DefaultWeight) ?? "1"; }
            set { Params.TryAddValue(SdekTemplate.DefaultWeight, value.TryParseFloat().ToString()); }
        }
        public string DefaultLength
        {
            get { return Params.ElementOrDefault(SdekTemplate.DefaultLength) ?? "100"; }
            set { Params.TryAddValue(SdekTemplate.DefaultLength, value.TryParseFloat().ToString()); }
        }
        public string DefaultWidth
        {
            get { return Params.ElementOrDefault(SdekTemplate.DefaultWidth) ?? "100"; }
            set { Params.TryAddValue(SdekTemplate.DefaultWidth, value.TryParseFloat().ToString()); }
        }
        public string DefaultHeight
        {
            get { return Params.ElementOrDefault(SdekTemplate.DefaultHeight) ?? "100"; }
            set { Params.TryAddValue(SdekTemplate.DefaultHeight, value.TryParseFloat().ToString()); }
        }

        public string TypeAdditionPrice
        {
            get { return Params.ElementOrDefault(SdekTemplate.TypeAdditionPrice) ?? "Fixed"; }
            set { Params.TryAddValue(SdekTemplate.TypeAdditionPrice, value); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
