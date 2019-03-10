using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Shipping;
using AdvantShop.Shipping.EmsPost;

namespace AdvantShop.Web.Infrastructure.Admin.ShippingMethods
{
    public class EmsPostShippingAdminModel : ShippingMethodAdminModel, IValidatableObject
    {
        public EmsPostShippingAdminModel()
        {
            CheckService = EmsPostService.CheckService(true);
        }
        
        private bool CheckService { get; set; }

        private string _cityFrom;
        public string CityFrom
        {
            get { return Params.ElementOrDefault(EmsPostTemplate.CityFrom); }
            set
            {
                _cityFrom = value;
                Params.TryAddValue(EmsPostTemplate.CityFrom, value.DefaultOrEmpty());
            }
        }

        public List<SelectListItem> Cities
        {
            get
            {
                
                var cities = new List<SelectListItem>();
                if (!CheckService)
                {
                    if(CityFrom.IsNullOrEmpty())
                        return cities;
                    cities.Add(new SelectListItem() { Selected = true,  Value = CityFrom, Text = CityFrom });
                    return cities;
                }

                cities.AddRange(EmsPostService.GetCities(true).Select(x => new SelectListItem() {Text = x.name, Value = x.value}));
                cities.AddRange(EmsPostService.GetRegions(true).Select(x => new SelectListItem() {Text = x.name, Value = x.value}));


                var selectedCity = cities.Find(x => x.Value == CityFrom);
                if (selectedCity != null)
                {
                    selectedCity.Selected = true;
                }
                else
                {
                    cities[0].Selected = true;
                    CityFrom = cities[0].Value;
                }

                return cities;
            }
        }

        private string _extraPrice;
        public string ExtraPrice
        {
            get { return Params.ElementOrDefault(EmsPostTemplate.ExtraPrice, "0"); }
            set
            {
                _extraPrice = value;
                Params.TryAddValue(EmsPostTemplate.ExtraPrice, value.TryParseFloat().ToString());
            }
        }
        
        public string DefaultWeight
        {
            get { return Params.ElementOrDefault(EmsPostTemplate.DefaultWeight, "0"); }
            set { Params.TryAddValue(EmsPostTemplate.DefaultWeight, value.TryParseFloat().ToString()); }
        }

        public string MaxWeight
        {
            get
            {
                var maxWeight = Params.ElementOrDefault(EmsPostTemplate.MaxWeight);
                if (string.IsNullOrWhiteSpace(maxWeight))
                {
                    float weight = 0;
                    if (CheckService)
                    {
                        weight = EmsPostService.GetMaxWeight(true);
                    }
                    maxWeight = weight != 0 ? weight.ToString() : "31.5";
                }

                return maxWeight;
            }
            set { Params.TryAddValue(EmsPostTemplate.MaxWeight, value.DefaultOrEmpty()); }
        }

        public string DiscountType
        {
            get { return Params.ElementOrDefault(EmsPostTemplate.DiscountType, "0"); }
            set { Params.TryAddValue(EmsPostTemplate.DiscountType, value); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(_cityFrom))
            {
                yield return new ValidationResult("Укажите город РФ, из которого осуществляется доставка", new[] { "CityFrom" });
            }

            if (string.IsNullOrWhiteSpace(_extraPrice) || !_extraPrice.IsDecimal())
            {
                yield return new ValidationResult("Укажите наценку к цене доставки", new[] { "ExtraPrice" });
            }
        }
    }
}
