<%@ WebHandler Language="C#" Class="GetAdminCountryCityExcluded" %>

using System;
using System.Collections.Generic;
using System.Web;
using AdvantShop.Helpers;
using AdvantShop.Repository;

public class GetAdminCountryCityExcluded : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        AdvantShop.Localization.Culture.InitializeCulture();
        var methodId = SQLDataHelper.GetInt(context.Request["methodId"]);
        var subject = context.Request["subject"];

        List<City> listCity;
        if (subject == "shipping")
        {
            listCity = AdvantShop.ShippingPaymentGeoMaping.GetCityByShippingIdExcluded(methodId);
        }
        else
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write(string.Empty);
            return;
        }

        var sHtml = new System.Text.StringBuilder();
        foreach (var cityItem in listCity)
        {
            sHtml.Append("<span>" + cityItem.Name + "</span><img style='cursor:pointer;' src='images/remove.jpg' onclick=\"javascript:DelCityExcluded(" + cityItem.CityId + "," + methodId + ")\" />");
        }


        context.Response.ContentType = "text/plain";
        context.Response.Write(string.IsNullOrEmpty(sHtml.ToString()) ? "Нет исключений" : sHtml.ToString());
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}