using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Repository;
using System.Collections.Generic;
using System.Web.SessionState;

namespace AdvantShop.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public partial class LocationController : BaseClientController
    {
        public JsonResult GetCities(int countryId)
        {
            var cities = new object();
            var country = new Country();
            var zone = IpZoneContext.CurrentZone;

            if (countryId != 0)
            {
                cities = CityService.GetCitiesByCountryInPopup(countryId).Select(item => new { item.CityId, item.Name, item.RegionId }).ToList();
                country = CountryService.GetCountry(countryId);
            }
            else
            {
                cities = CityService.GetCitiesByCountryInPopup(zone.CountryId).Select(item => new { item.CityId, item.Name, item.RegionId }).ToList();
                country = CountryService.GetCountry(zone.CountryId);
            }

            return Json(new
            {
                CountryID = country.CountryId,
                CountryName = country.Name,
                CountryCities = cities,
            });
        }

        public JsonResult GetDataForPopup()
        {
            var result = new List<object>();
            var countries = CountryService.GetCountriesByDisplayInPopup().Select(item => new { item.CountryId, item.Name, item.Iso2 });

            foreach (var country in countries)
            {
                result.Add(new
                {
                    country.CountryId,
                    country.Name,
                    country.Iso2,
                    Cities = CityService.GetCitiesByCountryInPopup(country.CountryId).Select(item => new { item.CityId, item.Name, item.RegionId })
                });
            }

            return Json(result);
        }

        [HttpPost]
        public JsonResult GetCurrentZone()
        {
            var zone = IpZoneContext.CurrentZone;
            var city = CityService.GetCity(zone.CityId);
            var phoneNumber = city != null && city.PhoneNumber.IsNotEmpty() ? city.PhoneNumber: SettingsMain.Phone;
            var mobilePhoneNumber = city != null && city.MobilePhoneNumber.IsNotEmpty() ? city.MobilePhoneNumber : SettingsMain.MobilePhone;

            return Json(new
            {
                current = new IpZoneModel()
                {
                    CountryId = zone.CountryId,
                    CountryName = zone.CountryName,
                    RegionId = zone.RegionId,
                    Region = zone.Region,
                    CityId = zone.CityId,
                    City = zone.City,
                    Phone = phoneNumber,
                    MobilePhone = mobilePhoneNumber
                }
            });
        }

        public JsonResult SetZone(string city, int? cityId, int? countryId)
        {
            Country country = null;
            var zone = (cityId.HasValue
                ? IpZoneService.GetZoneByCityId(cityId.Value)
                : IpZoneService.GetZoneByCity(city.Trim().ToLower(), countryId))
                   ?? new IpZone()
                   {
                       CountryId = countryId.HasValue && (country = CountryService.GetCountry(countryId.Value)) != null ? country.CountryId : SettingsMain.SellerCountryId,
                       CountryName = countryId.HasValue && country != null ? country.Name : string.Empty,
                       Region = string.Empty,
                       City = HttpUtility.HtmlEncode(city.Trim())
                   };

            IpZoneContext.SetCustomerCookie(zone);

            var cityObj = cityId.HasValue ? CityService.GetCity(cityId.Value) : CityService.GetCityByName(zone.City);
            var phoneNumber = cityObj != null && cityObj.PhoneNumber.IsNotEmpty() ? cityObj.PhoneNumber : SettingsMain.Phone;
            var mobilePhoneNumber = cityObj != null && cityObj.MobilePhoneNumber.IsNotEmpty() ? cityObj.MobilePhoneNumber : SettingsMain.MobilePhone;

            return Json(new IpZoneModel()
            {
                CountryId = zone.CountryId,
                CountryName = zone.CountryName,
                RegionId = zone.RegionId,
                Region = zone.Region,
                CityId = zone.CityId,
                City = zone.City,
                Phone = phoneNumber,
                MobilePhone = mobilePhoneNumber
            });
        }

        [HttpGet]
        public JsonResult GetCitiesAutocomplete(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Json(null, JsonRequestBehavior.AllowGet);

            return Json(
                (from zone in IpZoneService.GetIpZonesByCity(q) 
                 select new
                 {
                     Name = zone.City,
                     CityId = zone.CityId,
                     RegionId = zone.RegionId,
                     Region = zone.Region,
                     CountryId = zone.CountryId,
                     Country = zone.CountryName,
                 }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetRegionsAutocomplete(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Json(null, JsonRequestBehavior.AllowGet);

            return Json((from item in RegionService.GetRegionsByName(q)
                         select new
                         {
                             Name = item
                         }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCountriesAutocomplete(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Json(null, JsonRequestBehavior.AllowGet);

            return Json((from item in CountryService.GetCountriesByName(q)
                         select new
                         {
                             Name = item
                         }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCountries()
        {
            return Json(CountryService.GetAllCountries(), JsonRequestBehavior.AllowGet);
        }
    }
}