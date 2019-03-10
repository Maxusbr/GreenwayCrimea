using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;
using AdvantShop.Diagnostics;

namespace AdvantShop.Repository
{
    public class GeoIpService
    {
        private const string GeoServiceUrl = "https://geo.advsrvone.pw:8787/v1/geoip/";

        /// <summary>
        /// Get city name by ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static GeoIpData GetGeoIpData(string ip)
        {
            if (ip == "::1" || ip == "127.0.0.1")
                return null;

            try
            {
                var request = WebRequest.Create(GeoServiceUrl + ip);
                request.Timeout = 300;

                using (var dataStream = request.GetResponse().GetResponseStream())
                using (var reader = new StreamReader(dataStream, Encoding.UTF8))
                {
                    var responseFromServer = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(responseFromServer))
                    {
                        var ipElement = XElement.Parse(responseFromServer).Element("ip");
                        if (ipElement != null)
                        {
                            var countryIso = ipElement.Element("country");
                            var city = ipElement.Element("city");
                            var state = ipElement.Element("region");

                            if (countryIso != null)
                                return new GeoIpData()
                                {
                                    Country = countryIso.Value,
                                    City = city != null ? city.Value : string.Empty,
                                    State = state != null ? state.Value : string.Empty
                                };
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Debug.Log.Error("GeoIpService", ex);
            }

            return null;
        }
    }
}