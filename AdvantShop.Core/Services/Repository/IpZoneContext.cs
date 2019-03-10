using System;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Helpers;

namespace AdvantShop.Repository
{
    public static class IpZoneContext
    {
        private const string IpZoneContextKey = "IpZoneContext";
        private const string IpZoneCookie = "ipzone";

        public static IpZone CurrentZone
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    var contextZone = HttpContext.Current.Items[IpZoneContextKey] as IpZone;
                    if (contextZone != null) return contextZone;

                    var zone = GetZoneFromCookie();
                    if (zone == null)
                    {
                        if (SettingsDesign.DisplayCityInTopPanel)
                        {
                            zone = GetZoneByIp();
                            SetCustomerCookie(zone);
                        }
                        else
                        {
                            return new IpZone();
                        }
                    }

                    HttpContext.Current.Items[IpZoneContextKey] = zone;
                    return zone;
                }

                return GetDefaultZone();
            }
        }

        private static IpZone GetZoneFromCookie()
        {
            var cookie = HttpContext.Current.Request.Cookies[IpZoneCookie];

            if (cookie == null || cookie.Value.IsNullOrEmpty())
                return null;

            var cookieValues = HttpUtility.UrlDecode(cookie.Value).Split(new[] { ';' }, StringSplitOptions.None);
            if (cookieValues.Length < 5)
                return null;

            var countryId = cookieValues[0].TryParseInt();
            var regionId = cookieValues[1].TryParseInt();
            var cityId = cookieValues[2].TryParseInt();

            var region = cookieValues[3].Trim();
            var city = cookieValues[4].Trim();

            var dialCode = cookieValues.Length >= 6 ? cookieValues[5].TryParseInt(true) : null;

            var country = CountryService.GetCountry(countryId);
            if (country == null || country.CountryId == 0)
                return null;

            if (cityId == 0 && city.IsNullOrEmpty())
                return null;

            if (dialCode == null)
                dialCode = country.DialCode;

            if (regionId != 0)
            {
                var regionTemp = RegionService.GetRegion(regionId);
                regionId = regionTemp == null || regionTemp.RegionId == 0 ? 0 : regionTemp.RegionId;
                region = regionTemp == null || regionTemp.RegionId == 0 ? string.Empty : regionTemp.Name;
            }

            var zone = new IpZone()
            {
                CountryId = country.CountryId,
                CountryName = country.Name,
                RegionId = regionId,
                Region = region,
                CityId = cityId,
                City = city,
                DialCode = dialCode
            };

            return zone;
        }

        private static IpZone GetZoneByIp()
        {
            var zone = new IpZone();

            if (SettingsDesign.DisplayCityInTopPanel)
            {
                var geoIpData = GeoIpService.GetGeoIpData(HttpContext.Current.Request.Headers["X-1Gb-Client-IP"] ?? HttpContext.Current.Request.UserHostAddress);
                if (geoIpData != null)
                {
                    if (geoIpData.Country.IsNotEmpty())
                    {
                        var country = CountryService.GetCountryByIso2(geoIpData.Country);
                        if (country != null)
                        {
                            zone.CountryId = country.CountryId;
                            zone.CountryName = country.Name;
                            zone.DialCode = country.DialCode;
                        }

                        zone.Region = geoIpData.State;
                        zone.City = geoIpData.City;
                    }
                }
            }

            if (zone.CountryId == 0)
            {
                zone.CountryId = SettingsMain.SellerCountryId;
                var country = CountryService.GetCountry(zone.CountryId);
                if (country != null)
                    zone.CountryName = country.Name;
            }

            if (zone.City.IsNullOrEmpty())
            {
                var state = RegionService.GetRegion(SettingsMain.SellerRegionId);
                if (state != null)
                {
                    var country = CountryService.GetCountry(state.CountryId);
                    if (country != null)
                    {
                        zone.CountryId = state.CountryId;
                        zone.CountryName = country.Name;
                        zone.DialCode = country.DialCode;
                    }
                    zone.Region = state.Name;
                    zone.RegionId = state.RegionId;
                }

                zone.City = SettingsMain.City;
            }
            else
            {
                var city = CityService.GetCityByName(zone.City);

                if (city != null)
                {
                    zone.CityId = city.CityId;

                    if (!CityService.IsCityInCountry(city.CityId, zone.CountryId))
                    {
                        var region = RegionService.GetRegion(city.RegionId);
                        if (region != null)
                        {
                            zone.RegionId = region.RegionId;
                            zone.Region = region.Name;

                            var country = CountryService.GetCountry(region.CountryId);
                            if (country != null)
                            {
                                zone.CountryId = country.CountryId;
                                zone.CountryName = country.Name;
                                zone.DialCode = country.DialCode;
                            }
                        }
                    }
                }
            }

            if (zone.Region.IsNullOrEmpty())
            {
                var state = RegionService.GetRegion(SettingsMain.SellerRegionId);
                if (state != null)
                {
                    zone.Region = state.Name;
                    zone.RegionId = state.RegionId;
                }
            }

            return zone;
        }

        private static IpZone GetDefaultZone()
        {
            return new IpZone()
            {
                CountryId = SettingsMain.SellerCountryId,
                CountryName = CountryService.GetCountry(SettingsMain.SellerCountryId).Name,
                RegionId = SettingsMain.SellerRegionId,
                Region = RegionService.GetRegion(SettingsMain.SellerRegionId).Name,
                City = SettingsMain.City
            };
        }

        public static void SetCustomerCookie(IpZone zone)
        {
            CommonHelper.SetCookie(IpZoneCookie,
                            string.Format("{0};{1};{2};{3};{4};{5}", zone.CountryId, zone.RegionId, zone.CityId, zone.Region, zone.City, zone.DialCode),
                            new TimeSpan(365, 24, 0),
                            true);
        }

        public static bool ShowBubble()
        {
            return SettingsDesign.DisplayCityInTopPanel && SettingsDesign.DisplayCityBubble;
        }
    }
}