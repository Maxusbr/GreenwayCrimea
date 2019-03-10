<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.GetAdminCountryCity" %>

using System;
using System.Collections.Generic;
using System.Web;
using AdvantShop.Helpers;
using AdvantShop.Repository;

namespace Admin.HttpHandlers
{
    public class GetAdminCountryCity : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            AdvantShop.Localization.Culture.InitializeCulture();
            var methodId = SQLDataHelper.GetInt(context.Request["methodId"]);
            var subject = context.Request["subject"];

            List<Country> listCountry;
            List<City> listCity;
            if (subject == "payment")
            {
                listCountry = AdvantShop.ShippingPaymentGeoMaping.GetCountryByPaymentId(methodId);
                listCity = AdvantShop.ShippingPaymentGeoMaping.GetCityByPaymentId(methodId);
            }
            else if (subject == "shipping")
            {
                listCountry = AdvantShop.ShippingPaymentGeoMaping.GetCountryByShippingId(methodId);
                listCity = AdvantShop.ShippingPaymentGeoMaping.GetCityByShippingId(methodId);
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(string.Empty);
                return;
            }

            var listCityCopy = new List<City>(listCity);
            var sHtml = new System.Text.StringBuilder();
            foreach (var itemCountry in listCountry)
            {
                //sHtml.Append("<span>" + itemCountry.Name + "</span><input type='image' src='images/remove.jpg' onlick=\"javascript:DelCountry(" + itemCountry.CountryID + "," + methodId + ") return false;\" />");
                sHtml.Append("<span>" + itemCountry.Name +
                             "</span><img style='cursor:pointer;' src='images/remove.jpg' onclick=\"javascript:DelCountry(" +
                             itemCountry.CountryId + "," + methodId + "); return false;\" />");
                var tempCity = new System.Text.StringBuilder();
                foreach (var cityItem in listCity)
                {
                    if (CityService.IsCityInCountry(cityItem.CityId, itemCountry.CountryId))
                    {
                        //tempCity.Append("<span>" + cityItem.Name + "</span><input type='image' src='images/remove.jpg' onlick=\"javascript:DelCity(" + cityItem.CityID + "," + methodId + ")\" />");
                        tempCity.Append("<span>" + cityItem.Name +
                                        "</span><img style='cursor:pointer;' src='images/remove.jpg' onclick=\"javascript:DelCity(" +
                                        cityItem.CityId + "," + methodId + ")\" />");
                        listCityCopy.Remove(cityItem);
                    }
                }

                if (tempCity.Length > 0)
                {
                    sHtml.Append("(" + tempCity + ")");
                }
                sHtml.Append("<br/>");
            }

            if (listCityCopy.Count > 0)
            {
                sHtml.Append("<span>No country</span>");
                var tempC = new System.Text.StringBuilder();
                foreach (var cityItem in listCityCopy)
                {
                    //tempC.Append("<span>" + cityItem.Name + "</span><input type='image' src='images/remove.jpg' onlick=\"javascript:DelCity(" + cityItem.CityID + "," + methodId + ")\" />");
                    tempC.Append("<span>" + cityItem.Name +
                                 "</span><img style='cursor:pointer;' src='images/remove.jpg' onclick=\"javascript:DelCity(" +
                                 cityItem.CityId + "," + methodId + ")\" />");
                }
                if (tempC.Length > 0)
                {
                    sHtml.Append("(" + tempC + ")");
                }
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(string.IsNullOrEmpty(sHtml.ToString())
                ? Resources.Resource.Admin_PaymentMethod_AllCountriesAllCities
                : sHtml.ToString());
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}