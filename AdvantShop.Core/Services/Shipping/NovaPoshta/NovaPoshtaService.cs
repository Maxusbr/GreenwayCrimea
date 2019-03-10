using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using Newtonsoft.Json;

namespace AdvantShop.Shipping.NovaPoshta
{
    public static class NovaPoshtaService
    {
        private const string Url = "https://api.novaposhta.ua/v2.0/json/";
        private const string Language = "ru";

        public static float GetDocumentPrice(string apiKey, Guid citySender, Guid cityRecipient, enNovaPoshtaDeliveryType serviceType, float weight, float coast)
        {
            var model = new NovaPoshtaRequest<NovaRequestPrice>
            {
                ApiKey = apiKey,
                Language = Language,
                ModelName = "InternetDocument",
                CalledMethod = "getDocumentPrice",
                MethodProperties = new NovaRequestPrice
                {
                    CitySender = citySender,
                    CityRecipient = cityRecipient,
                    ServiceType = serviceType.ToString(),
                    Weight = weight,
                    Cost = coast
                }
            };
            var str = MakeRequest(JsonConvert.SerializeObject(model));
            var obj = JsonConvert.DeserializeObject<NovaResponse<NovaResponsePrice>>(str);
            if (obj.Errors != null && obj.Errors.ToString() != "[]") throw new Exception(obj.Errors.ToString());
            return obj.Data.First().Cost;
        }

        public static DateTime GetDocumentDeliveryDate(string apiKey, Guid citySender, Guid cityRecipient, enNovaPoshtaDeliveryType serviceType, DateTime dateTime)
        {
            var model = new NovaPoshtaRequest<NovaRequestDelivery>
            {
                ApiKey = apiKey,
                Language = Language,
                ModelName = "InternetDocument",
                CalledMethod = "getDocumentDeliveryDate",
                MethodProperties = new NovaRequestDelivery
                {
                    CitySender = citySender,
                    CityRecipient = cityRecipient,
                    ServiceType = serviceType.ToString(),
                    DateTime = dateTime.ToString("dd.MM.yyyy")
                }
            };
            var str = MakeRequest(JsonConvert.SerializeObject(model));
            var obj = JsonConvert.DeserializeObject<NovaResponse<NovaResponseDeliveryDate>>(str);
            if (obj.Errors != null && obj.Errors.ToString() != "[]") throw new Exception(obj.Errors.ToString());
            return obj.Data.First().DeliveryDate.Date;
        }

        public static NovaResponseCity GetCity(string apiKey, string cityName, string area)
        {
            var citiesSt = GetCities(apiKey, 0, cityName);
            var cities = JsonConvert.DeserializeObject<NovaResponse<NovaResponseCity>>(citiesSt, new BoolConverter());
            if (cities.Errors != null && cities.Errors.ToString() != "[]") throw new Exception(cities.Errors.ToString());
            return cities.Data.Count > 1 ? FindCityByRegion(apiKey, cities.Data, area) : cities.Data.First();
        }

        public static NovaResponseCity FindCityByRegion(string apiKey, IEnumerable<NovaResponseCity> cities, string areaName)
        {
            var area = GetArea(apiKey, areaName);
            var city = area == null ? cities.FirstOrDefault() : cities.FirstOrDefault(x => x.Area == area.Ref);
            if (city == null)
                throw new Exception("not found city");
            return city;
        }

        public static NovaResponseArea GetArea(string apiKey, string areaName)
        {
            var areaSt = GetAreas(apiKey, 0);
            var areas = JsonConvert.DeserializeObject<NovaResponse<NovaResponseArea>>(areaSt);
            if (areas.Errors != null && areas.Errors.ToString() != "[]") throw new Exception(areas.Errors.ToString());
            return areas.Data.FirstOrDefault(x => x.Description == areaName);
        }

        public static string GetAreas(string apiKey, int page, string @ref = "")
        {
            var model = new NovaPoshtaRequest<NovaRequestArea>
            {
                ApiKey = apiKey,
                Language = Language,
                ModelName = "Address",
                CalledMethod = "getAreas",
                MethodProperties = new NovaRequestArea { Page = page, Ref = @ref }
            };
            return MakeRequest(JsonConvert.SerializeObject(model));
        }

        public static string GetCities(string apiKey, int page, string findByString, string @ref = "")
        {
            var model = new NovaPoshtaRequest<NovaRequestCity>
            {
                ApiKey = apiKey,
                Language = Language,
                ModelName = "Address",
                CalledMethod = "getCities",
                MethodProperties = new NovaRequestCity { Page = page, FindByString = findByString, Ref = @ref }
            };
            return MakeRequest(JsonConvert.SerializeObject(model));
        }

        private static string MakeRequest(string data)
        {
            try
            {
                var request = WebRequest.Create(Url) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 2000;

                if (data.IsNotEmpty())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    request.ContentLength = bytes.Length;

                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                    }
                }

                var responseContent = "";
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                            using (var reader = new StreamReader(stream))
                            {
                                responseContent = reader.ReadToEnd();
                            }
                    }
                }

                return responseContent;
            }
            catch (WebException ex)
            {
                using (var eResponse = ex.Response)
                {
                    if (eResponse != null)
                    {
                        using (var eStream = eResponse.GetResponseStream())
                            if (eStream != null)
                                using (var reader = new StreamReader(eStream))
                                {
                                    var error = reader.ReadToEnd();

                                }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return null;
        }
    }
}
